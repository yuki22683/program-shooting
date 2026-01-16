#!/usr/bin/env node
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { CallToolRequestSchema, ListToolsRequestSchema, ListResourcesRequestSchema, ReadResourceRequestSchema, } from "@modelcontextprotocol/sdk/types.js";
import * as fs from "fs";
import * as path from "path";
import { execSync } from "child_process";
// Unity project root path (can be configured via environment variable)
const UNITY_PROJECT_PATH = process.env.UNITY_PROJECT_PATH || process.cwd();
const server = new Server({
    name: "unity-mcp-server",
    version: "1.0.0",
}, {
    capabilities: {
        tools: {},
        resources: {},
    },
});
// Helper function to find Unity Editor path
function findUnityEditorPath() {
    const possiblePaths = [
        "C:/Program Files/Unity/Hub/Editor",
        "C:/Program Files (x86)/Unity/Hub/Editor",
        process.env.UNITY_EDITOR_PATH,
    ];
    for (const basePath of possiblePaths) {
        if (basePath && fs.existsSync(basePath)) {
            const versions = fs.readdirSync(basePath);
            if (versions.length > 0) {
                const latestVersion = versions.sort().reverse()[0];
                const editorPath = path.join(basePath, latestVersion, "Editor", "Unity.exe");
                if (fs.existsSync(editorPath)) {
                    return editorPath;
                }
            }
        }
    }
    return null;
}
// Helper function to recursively get files
function getFilesRecursively(dir, pattern) {
    const files = [];
    if (!fs.existsSync(dir)) {
        return files;
    }
    const items = fs.readdirSync(dir, { withFileTypes: true });
    for (const item of items) {
        const fullPath = path.join(dir, item.name);
        if (item.isDirectory()) {
            // Skip certain directories
            if (!["Library", "Temp", "Logs", "obj", ".svn", ".git"].includes(item.name)) {
                files.push(...getFilesRecursively(fullPath, pattern));
            }
        }
        else if (pattern.test(item.name)) {
            files.push(fullPath);
        }
    }
    return files;
}
// Helper function to parse Unity YAML files (scenes, prefabs)
function parseUnityYaml(content) {
    const objects = [];
    const sections = content.split(/^--- !u!/m);
    for (const section of sections) {
        if (section.trim()) {
            const lines = section.split("\n");
            const headerMatch = lines[0]?.match(/(\d+) &(\d+)/);
            if (headerMatch) {
                objects.push({
                    classId: headerMatch[1],
                    fileId: headerMatch[2],
                    content: lines.slice(1).join("\n"),
                });
            }
        }
    }
    return objects;
}
// List available tools
server.setRequestHandler(ListToolsRequestSchema, async () => {
    return {
        tools: [
            {
                name: "list_scripts",
                description: "List all C# scripts in the Unity project Assets folder",
                inputSchema: {
                    type: "object",
                    properties: {
                        searchPattern: {
                            type: "string",
                            description: "Optional filename pattern to filter scripts (e.g., 'Player' to find scripts containing 'Player')",
                        },
                    },
                },
            },
            {
                name: "read_script",
                description: "Read the contents of a C# script file",
                inputSchema: {
                    type: "object",
                    properties: {
                        scriptPath: {
                            type: "string",
                            description: "Path to the script file (relative to project root or absolute)",
                        },
                    },
                    required: ["scriptPath"],
                },
            },
            {
                name: "list_scenes",
                description: "List all Unity scenes (.unity files) in the project",
                inputSchema: {
                    type: "object",
                    properties: {},
                },
            },
            {
                name: "list_prefabs",
                description: "List all prefabs (.prefab files) in the project",
                inputSchema: {
                    type: "object",
                    properties: {},
                },
            },
            {
                name: "get_project_settings",
                description: "Get Unity project settings (ProjectSettings folder contents)",
                inputSchema: {
                    type: "object",
                    properties: {
                        settingFile: {
                            type: "string",
                            description: "Specific setting file to read (e.g., 'ProjectSettings.asset', 'InputManager.asset')",
                        },
                    },
                },
            },
            {
                name: "search_in_scripts",
                description: "Search for a pattern in all C# scripts",
                inputSchema: {
                    type: "object",
                    properties: {
                        pattern: {
                            type: "string",
                            description: "Text pattern to search for in scripts",
                        },
                        caseSensitive: {
                            type: "boolean",
                            description: "Whether the search should be case-sensitive (default: false)",
                        },
                    },
                    required: ["pattern"],
                },
            },
            {
                name: "get_scene_hierarchy",
                description: "Get the GameObject hierarchy from a Unity scene file",
                inputSchema: {
                    type: "object",
                    properties: {
                        scenePath: {
                            type: "string",
                            description: "Path to the scene file",
                        },
                    },
                    required: ["scenePath"],
                },
            },
            {
                name: "list_packages",
                description: "List installed Unity packages from manifest.json",
                inputSchema: {
                    type: "object",
                    properties: {},
                },
            },
            {
                name: "get_unity_version",
                description: "Get the Unity version used by this project",
                inputSchema: {
                    type: "object",
                    properties: {},
                },
            },
            {
                name: "list_assets",
                description: "List assets in the project by type",
                inputSchema: {
                    type: "object",
                    properties: {
                        assetType: {
                            type: "string",
                            description: "Asset type to list: 'materials', 'textures', 'models', 'audio', 'animations', or 'all'",
                        },
                    },
                    required: ["assetType"],
                },
            },
            {
                name: "run_unity_command",
                description: "Run a Unity Editor command line operation (requires Unity to be installed)",
                inputSchema: {
                    type: "object",
                    properties: {
                        command: {
                            type: "string",
                            description: "Unity command to execute (e.g., '-buildTarget Android -executeMethod BuildScript.Build')",
                        },
                    },
                    required: ["command"],
                },
            },
        ],
    };
});
// Handle tool calls
server.setRequestHandler(CallToolRequestSchema, async (request) => {
    const { name, arguments: args } = request.params;
    switch (name) {
        case "list_scripts": {
            const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
            const scripts = getFilesRecursively(assetsPath, /\.cs$/);
            const searchPattern = args?.searchPattern;
            let filteredScripts = scripts.map((s) => path.relative(UNITY_PROJECT_PATH, s));
            if (searchPattern) {
                const pattern = new RegExp(searchPattern, "i");
                filteredScripts = filteredScripts.filter((s) => pattern.test(s));
            }
            return {
                content: [
                    {
                        type: "text",
                        text: `Found ${filteredScripts.length} scripts:\n${filteredScripts.join("\n")}`,
                    },
                ],
            };
        }
        case "read_script": {
            const scriptPath = args.scriptPath;
            let fullPath = scriptPath;
            if (!path.isAbsolute(scriptPath)) {
                fullPath = path.join(UNITY_PROJECT_PATH, scriptPath);
            }
            if (!fs.existsSync(fullPath)) {
                return {
                    content: [{ type: "text", text: `Script not found: ${scriptPath}` }],
                    isError: true,
                };
            }
            const content = fs.readFileSync(fullPath, "utf-8");
            return {
                content: [{ type: "text", text: content }],
            };
        }
        case "list_scenes": {
            const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
            const scenes = getFilesRecursively(assetsPath, /\.unity$/);
            const relativeScenes = scenes.map((s) => path.relative(UNITY_PROJECT_PATH, s));
            return {
                content: [
                    {
                        type: "text",
                        text: `Found ${relativeScenes.length} scenes:\n${relativeScenes.join("\n")}`,
                    },
                ],
            };
        }
        case "list_prefabs": {
            const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
            const prefabs = getFilesRecursively(assetsPath, /\.prefab$/);
            const relativePrefabs = prefabs.map((s) => path.relative(UNITY_PROJECT_PATH, s));
            return {
                content: [
                    {
                        type: "text",
                        text: `Found ${relativePrefabs.length} prefabs:\n${relativePrefabs.join("\n")}`,
                    },
                ],
            };
        }
        case "get_project_settings": {
            const settingsPath = path.join(UNITY_PROJECT_PATH, "ProjectSettings");
            const settingFile = args?.settingFile;
            if (settingFile) {
                const filePath = path.join(settingsPath, settingFile);
                if (fs.existsSync(filePath)) {
                    const content = fs.readFileSync(filePath, "utf-8");
                    return {
                        content: [{ type: "text", text: content }],
                    };
                }
                else {
                    return {
                        content: [{ type: "text", text: `Setting file not found: ${settingFile}` }],
                        isError: true,
                    };
                }
            }
            const files = fs.readdirSync(settingsPath);
            return {
                content: [
                    {
                        type: "text",
                        text: `Project settings files:\n${files.join("\n")}`,
                    },
                ],
            };
        }
        case "search_in_scripts": {
            const pattern = args.pattern;
            const caseSensitive = args.caseSensitive ?? false;
            const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
            const scripts = getFilesRecursively(assetsPath, /\.cs$/);
            const results = [];
            const regex = new RegExp(pattern, caseSensitive ? "g" : "gi");
            for (const script of scripts) {
                const content = fs.readFileSync(script, "utf-8");
                const lines = content.split("\n");
                lines.forEach((line, index) => {
                    if (regex.test(line)) {
                        results.push({
                            file: path.relative(UNITY_PROJECT_PATH, script),
                            line: index + 1,
                            content: line.trim(),
                        });
                    }
                });
            }
            const resultText = results
                .map((r) => `${r.file}:${r.line}: ${r.content}`)
                .join("\n");
            return {
                content: [
                    {
                        type: "text",
                        text: `Found ${results.length} matches:\n${resultText}`,
                    },
                ],
            };
        }
        case "get_scene_hierarchy": {
            const scenePath = args.scenePath;
            let fullPath = scenePath;
            if (!path.isAbsolute(scenePath)) {
                fullPath = path.join(UNITY_PROJECT_PATH, scenePath);
            }
            if (!fs.existsSync(fullPath)) {
                return {
                    content: [{ type: "text", text: `Scene not found: ${scenePath}` }],
                    isError: true,
                };
            }
            const content = fs.readFileSync(fullPath, "utf-8");
            const objects = parseUnityYaml(content);
            // Extract GameObjects (classId 1 is GameObject)
            const gameObjects = objects
                .filter((obj) => obj.classId === "1")
                .map((obj) => {
                const nameMatch = obj.content.match(/m_Name: (.+)/);
                return nameMatch ? nameMatch[1] : "Unknown";
            });
            return {
                content: [
                    {
                        type: "text",
                        text: `GameObjects in scene (${gameObjects.length}):\n${gameObjects.join("\n")}`,
                    },
                ],
            };
        }
        case "list_packages": {
            const manifestPath = path.join(UNITY_PROJECT_PATH, "Packages", "manifest.json");
            if (!fs.existsSync(manifestPath)) {
                return {
                    content: [{ type: "text", text: "manifest.json not found" }],
                    isError: true,
                };
            }
            const manifest = JSON.parse(fs.readFileSync(manifestPath, "utf-8"));
            const packages = Object.entries(manifest.dependencies || {})
                .map(([name, version]) => `${name}: ${version}`)
                .join("\n");
            return {
                content: [
                    {
                        type: "text",
                        text: `Installed packages:\n${packages}`,
                    },
                ],
            };
        }
        case "get_unity_version": {
            const versionPath = path.join(UNITY_PROJECT_PATH, "ProjectSettings", "ProjectVersion.txt");
            if (!fs.existsSync(versionPath)) {
                return {
                    content: [{ type: "text", text: "ProjectVersion.txt not found" }],
                    isError: true,
                };
            }
            const content = fs.readFileSync(versionPath, "utf-8");
            return {
                content: [{ type: "text", text: content }],
            };
        }
        case "list_assets": {
            const assetType = args.assetType;
            const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
            const typePatterns = {
                materials: /\.mat$/,
                textures: /\.(png|jpg|jpeg|tga|psd|tiff|gif|bmp|exr|hdr)$/i,
                models: /\.(fbx|obj|dae|3ds|blend|max|mb|ma)$/i,
                audio: /\.(wav|mp3|ogg|aiff|aif|flac|m4a)$/i,
                animations: /\.(anim|controller)$/,
                all: /\.(mat|png|jpg|jpeg|tga|psd|tiff|gif|bmp|exr|hdr|fbx|obj|dae|3ds|blend|max|mb|ma|wav|mp3|ogg|aiff|aif|flac|m4a|anim|controller)$/i,
            };
            const pattern = typePatterns[assetType];
            if (!pattern) {
                return {
                    content: [
                        {
                            type: "text",
                            text: `Invalid asset type. Valid types: ${Object.keys(typePatterns).join(", ")}`,
                        },
                    ],
                    isError: true,
                };
            }
            const assets = getFilesRecursively(assetsPath, pattern);
            const relativeAssets = assets.map((a) => path.relative(UNITY_PROJECT_PATH, a));
            return {
                content: [
                    {
                        type: "text",
                        text: `Found ${relativeAssets.length} ${assetType} assets:\n${relativeAssets.join("\n")}`,
                    },
                ],
            };
        }
        case "run_unity_command": {
            const command = args.command;
            const unityPath = findUnityEditorPath();
            if (!unityPath) {
                return {
                    content: [
                        {
                            type: "text",
                            text: "Unity Editor not found. Set UNITY_EDITOR_PATH environment variable.",
                        },
                    ],
                    isError: true,
                };
            }
            try {
                const fullCommand = `"${unityPath}" -projectPath "${UNITY_PROJECT_PATH}" -batchmode -quit ${command}`;
                const output = execSync(fullCommand, { encoding: "utf-8", timeout: 300000 });
                return {
                    content: [{ type: "text", text: output || "Command executed successfully" }],
                };
            }
            catch (error) {
                return {
                    content: [{ type: "text", text: `Error: ${error.message}` }],
                    isError: true,
                };
            }
        }
        default:
            return {
                content: [{ type: "text", text: `Unknown tool: ${name}` }],
                isError: true,
            };
    }
});
// List available resources
server.setRequestHandler(ListResourcesRequestSchema, async () => {
    return {
        resources: [
            {
                uri: "unity://project/info",
                name: "Project Info",
                description: "Unity project information",
                mimeType: "application/json",
            },
        ],
    };
});
// Read resources
server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
    const { uri } = request.params;
    if (uri === "unity://project/info") {
        const versionPath = path.join(UNITY_PROJECT_PATH, "ProjectSettings", "ProjectVersion.txt");
        const manifestPath = path.join(UNITY_PROJECT_PATH, "Packages", "manifest.json");
        let unityVersion = "Unknown";
        if (fs.existsSync(versionPath)) {
            const content = fs.readFileSync(versionPath, "utf-8");
            const match = content.match(/m_EditorVersion: (.+)/);
            if (match)
                unityVersion = match[1];
        }
        let packageCount = 0;
        if (fs.existsSync(manifestPath)) {
            const manifest = JSON.parse(fs.readFileSync(manifestPath, "utf-8"));
            packageCount = Object.keys(manifest.dependencies || {}).length;
        }
        const assetsPath = path.join(UNITY_PROJECT_PATH, "Assets");
        const scriptCount = getFilesRecursively(assetsPath, /\.cs$/).length;
        const sceneCount = getFilesRecursively(assetsPath, /\.unity$/).length;
        const prefabCount = getFilesRecursively(assetsPath, /\.prefab$/).length;
        const info = {
            projectPath: UNITY_PROJECT_PATH,
            unityVersion,
            packageCount,
            scriptCount,
            sceneCount,
            prefabCount,
        };
        return {
            contents: [
                {
                    uri,
                    mimeType: "application/json",
                    text: JSON.stringify(info, null, 2),
                },
            ],
        };
    }
    throw new Error(`Unknown resource: ${uri}`);
});
// Start the server
async function main() {
    const transport = new StdioServerTransport();
    await server.connect(transport);
    console.error("Unity MCP Server started");
}
main().catch(console.error);
//# sourceMappingURL=index.js.map