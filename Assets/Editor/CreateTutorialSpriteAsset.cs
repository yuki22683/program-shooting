using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class CreateTutorialSpriteAsset : MonoBehaviour
{
    [MenuItem("Tools/Create Tutorial Sprite Asset")]
    public static void CreateSpriteAsset()
    {
        string[] texturePaths = new string[] {
            "Assets/Sprites/TutorialSlides/snake.png",
            "Assets/Sprites/TutorialSlides/monitor.png"
        };

        // First, make textures readable
        foreach (string path in texturePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.isReadable = true;
                importer.SaveAndReimport();
                Debug.Log($"Set texture readable: {path}");
            }
        }

        // Wait a frame for reimport
        EditorApplication.delayCall += () => CreateSpriteAssetInternal(texturePaths);
    }

    private static void CreateSpriteAssetInternal(string[] texturePaths)
    {
        // Load the textures
        Texture2D snakeTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePaths[0]);
        Texture2D monitorTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePaths[1]);

        if (snakeTexture == null || monitorTexture == null)
        {
            Debug.LogError("Could not find tutorial slide textures!");
            return;
        }

        Debug.Log($"Snake texture: {snakeTexture.width}x{snakeTexture.height}, readable={snakeTexture.isReadable}");
        Debug.Log($"Monitor texture: {monitorTexture.width}x{monitorTexture.height}, readable={monitorTexture.isReadable}");

        // Create a combined texture atlas
        int atlasWidth = snakeTexture.width + monitorTexture.width;
        int atlasHeight = Mathf.Max(snakeTexture.height, monitorTexture.height);
        Texture2D atlasTexture = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBA32, false);
        
        // Clear to transparent
        Color[] clearPixels = new Color[atlasWidth * atlasHeight];
        for (int i = 0; i < clearPixels.Length; i++)
            clearPixels[i] = Color.clear;
        atlasTexture.SetPixels(clearPixels);

        // Copy snake texture
        atlasTexture.SetPixels(0, atlasHeight - snakeTexture.height, snakeTexture.width, snakeTexture.height, snakeTexture.GetPixels());
        // Copy monitor texture
        atlasTexture.SetPixels(snakeTexture.width, atlasHeight - monitorTexture.height, monitorTexture.width, monitorTexture.height, monitorTexture.GetPixels());
        atlasTexture.Apply();

        // Save the atlas texture
        byte[] pngData = atlasTexture.EncodeToPNG();
        string atlasPath = "Assets/Sprites/TutorialSlides/TutorialSlidesAtlas.png";
        System.IO.File.WriteAllBytes(atlasPath, pngData);
        AssetDatabase.Refresh();

        // Set atlas import settings
        TextureImporter atlasImporter = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
        if (atlasImporter != null)
        {
            atlasImporter.textureType = TextureImporterType.Sprite;
            atlasImporter.spriteImportMode = SpriteImportMode.Single;
            atlasImporter.isReadable = true;
            atlasImporter.SaveAndReimport();
        }

        // Create Sprite Asset after atlas is ready
        EditorApplication.delayCall += () => FinalizeSpriteAsset(snakeTexture.width, snakeTexture.height, monitorTexture.width, monitorTexture.height, atlasPath);
    }

    private static void FinalizeSpriteAsset(int snakeW, int snakeH, int monitorW, int monitorH, string atlasPath)
    {
        Texture2D savedAtlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);
        if (savedAtlas == null)
        {
            Debug.LogError("Could not load atlas texture!");
            return;
        }

        int atlasHeight = savedAtlas.height;

        // Create a new Sprite Asset
        TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        spriteAsset.name = "TutorialSlides";
        spriteAsset.spriteSheet = savedAtlas;

        // Create sprite character table
        var spriteCharacterTable = new List<TMP_SpriteCharacter>();
        var spriteGlyphTable = new List<TMP_SpriteGlyph>();

        // Add snake sprite (index 0)
        TMP_SpriteGlyph snakeGlyph = new TMP_SpriteGlyph();
        snakeGlyph.index = 0;
        snakeGlyph.metrics = new UnityEngine.TextCore.GlyphMetrics(snakeW, snakeH, 0, snakeH * 0.8f, snakeW);
        snakeGlyph.glyphRect = new UnityEngine.TextCore.GlyphRect(0, atlasHeight - snakeH, snakeW, snakeH);
        snakeGlyph.scale = 1.0f;
        spriteGlyphTable.Add(snakeGlyph);

        TMP_SpriteCharacter snakeChar = new TMP_SpriteCharacter(0, snakeGlyph);
        snakeChar.name = "snake";
        snakeChar.scale = 1.0f;
        spriteCharacterTable.Add(snakeChar);

        // Add monitor sprite (index 1)
        TMP_SpriteGlyph monitorGlyph = new TMP_SpriteGlyph();
        monitorGlyph.index = 1;
        monitorGlyph.metrics = new UnityEngine.TextCore.GlyphMetrics(monitorW, monitorH, 0, monitorH * 0.8f, monitorW);
        monitorGlyph.glyphRect = new UnityEngine.TextCore.GlyphRect(snakeW, atlasHeight - monitorH, monitorW, monitorH);
        monitorGlyph.scale = 1.0f;
        spriteGlyphTable.Add(monitorGlyph);

        TMP_SpriteCharacter monitorChar = new TMP_SpriteCharacter(1, monitorGlyph);
        monitorChar.name = "monitor";
        monitorChar.scale = 1.0f;
        spriteCharacterTable.Add(monitorChar);

        // Assign tables using reflection
        var spriteCharTableField = typeof(TMP_SpriteAsset).GetField("m_SpriteCharacterTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var spriteGlyphTableField = typeof(TMP_SpriteAsset).GetField("m_SpriteGlyphTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        spriteCharTableField?.SetValue(spriteAsset, spriteCharacterTable);
        spriteGlyphTableField?.SetValue(spriteAsset, spriteGlyphTable);

        // Save the sprite asset
        string assetPath = "Assets/Sprites/TutorialSlides/TutorialSlides.asset";
        AssetDatabase.CreateAsset(spriteAsset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Tutorial Sprite Asset created at: {assetPath}");
    }
}