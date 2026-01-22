using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextBasedScaler : MonoBehaviour
{
    // JetBrains Darcula Colors (from syntax-highlight.ts)
    public static class SyntaxColors
    {
        public static readonly Color Background = HexToColor("#2B2B2B");
        public static readonly Color Foreground = HexToColor("#D0D0D0");
        public static readonly Color Keyword = HexToColor("#CC7832");
        public static readonly Color String = HexToColor("#6A8759");
        public static readonly Color Number = HexToColor("#6897BB");
        public static readonly Color Comment = HexToColor("#D0D0D0");
        public static readonly Color DocComment = HexToColor("#629755");
        public static readonly Color Constant = HexToColor("#9876AA");
        public static readonly Color FunctionDef = HexToColor("#FFC66D");
        public static readonly Color Decorator = HexToColor("#BBB529");
        public static readonly Color BuiltIn = HexToColor("#8888C6");
        public static readonly Color Variable = HexToColor("#D0D0D0");
        public static readonly Color Bracket = HexToColor("#D0D0D0");
        public static readonly Color Error = HexToColor("#BC3F3C");

        public static Color HexToColor(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            return color;
        }
    }

    // Language configuration
    public class LanguageConfig
    {
        public HashSet<string> Keywords { get; set; }
        public HashSet<string> Builtins { get; set; }
        public string CommentPrefix { get; set; }
    }

    // String prefixes (Python f-string, r-string, etc.)
    private static readonly HashSet<string> StringPrefixes = new HashSet<string>
    {
        "f", "r", "b", "u", "fr", "rf", "br", "rb",
        "F", "R", "B", "U", "FR", "RF", "BR", "RB"
    };

    // All language configurations from syntax-highlight.ts
    public static readonly Dictionary<string, LanguageConfig> LanguageConfigs = new Dictionary<string, LanguageConfig>
    {
        ["python"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "def", "class", "return", "import", "from", "if", "elif", "else", "for", "in", "while", "with", "as", "try", "except", "finally", "pass", "break", "continue", "lambda", "global", "nonlocal", "yield", "raise", "assert", "del", "async", "await", "and", "or", "not", "is" },
            Builtins = new HashSet<string> { "print", "len", "range", "int", "str", "list", "dict", "set", "tuple", "bool", "True", "False", "None", "self", "cls", "super", "open", "type", "id", "input" },
            CommentPrefix = "#"
        },
        ["javascript"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "function", "const", "let", "var", "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return", "import", "export", "from", "default", "class", "extends", "super", "this", "new", "try", "catch", "finally", "throw", "async", "await", "yield", "typeof", "instanceof", "in", "of", "delete", "void", "debugger" },
            Builtins = new HashSet<string> { "console", "Math", "parseInt", "parseFloat", "JSON", "Promise", "Object", "Array", "String", "Number", "Boolean", "true", "false", "null", "undefined" },
            CommentPrefix = "//"
        },
        ["typescript"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "function", "const", "let", "var", "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return", "import", "export", "from", "default", "class", "extends", "super", "this", "new", "try", "catch", "finally", "throw", "async", "await", "yield", "type", "interface", "enum", "readonly", "public", "private", "protected", "abstract", "static", "namespace", "module", "declare", "implements", "as", "is", "keyof", "infer", "typeof", "instanceof", "in", "of", "delete", "void", "debugger" },
            Builtins = new HashSet<string> { "console", "Math", "parseInt", "parseFloat", "JSON", "Promise", "Object", "Array", "String", "Number", "Boolean", "true", "false", "null", "undefined", "string", "number", "boolean", "any", "void", "never", "unknown" },
            CommentPrefix = "//"
        },
        ["java"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "public", "private", "protected", "static", "final", "class", "interface", "extends", "implements", "package", "import", "new", "this", "super", "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return", "try", "catch", "finally", "throw", "throws", "synchronized", "volatile", "transient", "native", "strictfp", "enum", "void", "abstract", "default", "instanceof", "assert" },
            Builtins = new HashSet<string> { "String", "System", "Math", "Integer", "Long", "Double", "Float", "Boolean", "Byte", "Short", "Character", "Object", "true", "false", "null", "int", "long", "double", "float", "boolean", "byte", "short", "char" },
            CommentPrefix = "//"
        },
        ["c"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return", "struct", "enum", "union", "typedef", "sizeof", "static", "extern", "register", "volatile", "const", "auto", "void", "goto", "inline", "restrict", "default" },
            Builtins = new HashSet<string> { "int", "long", "short", "char", "float", "double", "printf", "scanf", "malloc", "free", "NULL", "size_t" },
            CommentPrefix = "//"
        },
        ["cpp"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return", "struct", "enum", "union", "typedef", "sizeof", "static", "extern", "volatile", "const", "auto", "void", "public", "private", "protected", "class", "template", "typename", "using", "namespace", "inline", "virtual", "override", "final", "constexpr", "new", "delete", "try", "catch", "throw", "noexcept", "decltype", "mutable", "explicit", "friend", "operator", "this", "default", "nullptr", "static_cast", "dynamic_cast", "const_cast", "reinterpret_cast", "register" },
            Builtins = new HashSet<string> { "int", "long", "short", "char", "float", "double", "bool", "std", "cout", "cin", "endl", "vector", "string", "map", "set", "true", "false", "NULL", "size_t" },
            CommentPrefix = "//"
        },
        ["csharp"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "public", "private", "protected", "internal", "static", "class", "interface", "struct", "enum", "if", "else", "for", "foreach", "while", "do", "switch", "case", "break", "continue", "return", "try", "catch", "finally", "throw", "new", "this", "base", "using", "namespace", "var", "partial", "async", "await", "yield", "lock", "delegate", "event", "readonly", "override", "virtual", "abstract", "sealed", "in", "out", "ref", "params", "default", "typeof", "sizeof", "checked", "unchecked", "fixed", "unsafe", "is", "as", "const", "extern", "implicit", "explicit", "operator", "where", "get", "set", "value" },
            Builtins = new HashSet<string> { "Console", "Math", "String", "Int32", "Int64", "Double", "Single", "Boolean", "Object", "true", "false", "null", "int", "long", "double", "float", "bool", "string", "void", "decimal", "byte", "char", "short", "uint", "ulong", "ushort", "sbyte", "object", "dynamic" },
            CommentPrefix = "//"
        },
        ["go"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "package", "import", "func", "var", "const", "type", "struct", "interface", "if", "else", "for", "range", "switch", "case", "default", "select", "chan", "go", "defer", "return", "break", "continue", "goto", "fallthrough", "map" },
            Builtins = new HashSet<string> { "fmt", "Println", "Printf", "Print", "len", "cap", "append", "copy", "close", "panic", "recover", "make", "new", "bool", "string", "int", "float64", "true", "false", "nil", "error", "byte", "rune", "int8", "int16", "int32", "int64", "uint", "uint8", "uint16", "uint32", "uint64", "float32", "complex64", "complex128" },
            CommentPrefix = "//"
        },
        ["rust"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "fn", "let", "mut", "const", "static", "if", "else", "for", "while", "loop", "match", "return", "break", "continue", "use", "mod", "pub", "crate", "self", "Self", "trait", "impl", "struct", "enum", "type", "where", "unsafe", "async", "await", "dyn", "ref", "move", "box", "as", "in", "extern", "super" },
            Builtins = new HashSet<string> { "println", "vec", "String", "Option", "Result", "Some", "None", "Ok", "Err", "bool", "char", "i8", "i16", "i32", "i64", "i128", "isize", "u8", "u16", "u32", "u64", "u128", "usize", "f32", "f64", "true", "false", "str", "Vec", "Box", "Rc", "Arc", "Cell", "RefCell" },
            CommentPrefix = "//"
        },
        ["ruby"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "def", "class", "module", "if", "else", "elsif", "unless", "case", "when", "while", "until", "for", "in", "do", "end", "begin", "rescue", "ensure", "raise", "next", "break", "return", "yield", "self", "alias", "undef", "defined?", "super", "then", "redo", "retry", "and", "or", "not", "lambda", "proc", "attr_reader", "attr_writer", "attr_accessor", "private", "protected", "public" },
            Builtins = new HashSet<string> { "puts", "print", "gets", "require", "require_relative", "include", "extend", "nil", "true", "false", "Array", "Hash", "String", "Integer", "Float", "Symbol", "Range", "Regexp", "Time", "File", "IO" },
            CommentPrefix = "#"
        },
        ["php"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "function", "class", "interface", "trait", "extends", "implements", "public", "private", "protected", "static", "final", "abstract", "const", "if", "else", "elseif", "foreach", "as", "while", "do", "switch", "case", "break", "continue", "return", "try", "catch", "finally", "throw", "new", "clone", "instanceof", "global", "namespace", "use", "var", "match", "fn", "declare", "goto", "default", "yield", "insteadof" },
            Builtins = new HashSet<string> { "echo", "print", "list", "array", "include", "include_once", "require", "require_once", "isset", "empty", "unset", "null", "true", "false", "bool", "int", "string", "float", "mixed", "void", "object", "callable", "iterable", "self", "parent" },
            CommentPrefix = "//"
        },
        ["bash"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "if", "then", "else", "elif", "fi", "for", "in", "do", "done", "while", "until", "case", "esac", "function", "select", "time", "[[", "]]", "declare", "export", "local", "readonly", "set", "unset", "shift", "trap", "exec", "eval", "source", "alias", "unalias", "return", "exit", "break", "continue" },
            Builtins = new HashSet<string> { "echo", "printf", "read", "cd", "pwd", "ls", "grep", "cat", "true", "false", "test", "expr", "let", "getopts" },
            CommentPrefix = "#"
        },
        ["haskell"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "module", "import", "where", "let", "in", "do", "if", "then", "else", "case", "of", "data", "type", "newtype", "class", "instance", "deriving", "default", "infix", "infixl", "infixr", "foreign", "forall", "qualified", "as", "hiding" },
            Builtins = new HashSet<string> { "putStrLn", "print", "show", "read", "fst", "snd", "Int", "Integer", "Float", "Double", "Bool", "String", "Char", "Maybe", "Just", "Nothing", "Either", "Right", "Left", "IO", "return", "True", "False", "map", "filter", "foldl", "foldr", "head", "tail", "null", "length" },
            CommentPrefix = "--"
        },
        ["elixir"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "def", "defmodule", "defp", "defmacro", "defmacrop", "defstruct", "defprotocol", "defimpl", "if", "else", "unless", "case", "cond", "with", "do", "end", "fn", "quote", "unquote", "unquote_splicing", "receive", "after", "try", "catch", "rescue", "raise", "throw", "alias", "import", "require", "use", "when", "and", "or", "not", "in" },
            Builtins = new HashSet<string> { "IO", "puts", "inspect", "Enum", "Map", "List", "String", "Integer", "Float", "Atom", "nil", "true", "false", "Kernel", "Agent", "Task", "GenServer", "Supervisor" },
            CommentPrefix = "#"
        },
        ["lua"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "and", "break", "do", "else", "elseif", "end", "false", "for", "function", "goto", "if", "in", "local", "nil", "not", "or", "repeat", "return", "then", "true", "until", "while" },
            Builtins = new HashSet<string> { "print", "pairs", "ipairs", "table", "string", "math", "io", "os", "debug", "assert", "error", "type", "tonumber", "tostring", "require", "pcall", "xpcall", "setmetatable", "getmetatable", "rawget", "rawset", "next", "select", "collectgarbage", "coroutine", "self" },
            CommentPrefix = "--"
        },
        ["assembly"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "section", "global", "extern", "default", "mov", "add", "sub", "mul", "div", "imul", "idiv", "jmp", "je", "jne", "jz", "jnz", "jl", "jle", "jg", "jge", "ja", "jae", "jb", "jbe", "call", "ret", "push", "pop", "inc", "dec", "cmp", "test", "and", "or", "xor", "not", "shl", "shr", "sal", "sar", "rol", "ror", "lea", "nop", "syscall", "int", "iret", "loop", "loope", "loopne" },
            Builtins = new HashSet<string> { "db", "dw", "dd", "dq", "resb", "resw", "resd", "resq", "rax", "rbx", "rcx", "rdx", "rsi", "rdi", "rbp", "rsp", "r8", "r9", "r10", "r11", "r12", "r13", "r14", "r15", "eax", "ebx", "ecx", "edx", "esi", "edi", "ebp", "esp", "ax", "bx", "cx", "dx", "al", "ah", "bl", "bh", "cl", "ch", "dl", "dh", "cs", "ds", "es", "fs", "gs", "ss" },
            CommentPrefix = ";"
        },
        ["sql"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "SELECT", "FROM", "WHERE", "INSERT", "UPDATE", "DELETE", "CREATE", "DROP", "ALTER", "TABLE", "INDEX", "VIEW", "VALUES", "AND", "OR", "NOT", "NULL", "IS", "IN", "LIKE", "BETWEEN", "JOIN", "INNER", "LEFT", "RIGHT", "FULL", "OUTER", "CROSS", "ON", "GROUP", "BY", "HAVING", "ORDER", "ASC", "DESC", "LIMIT", "OFFSET", "AS", "INTO", "DISTINCT", "UNIQUE", "PRIMARY", "KEY", "FOREIGN", "REFERENCES", "CONSTRAINT", "DEFAULT", "CHECK", "SET", "CASE", "WHEN", "THEN", "ELSE", "END", "UNION", "ALL", "EXISTS", "TRUNCATE", "BEGIN", "COMMIT", "ROLLBACK", "TRANSACTION" },
            Builtins = new HashSet<string> { "COUNT", "SUM", "AVG", "MAX", "MIN", "UPPER", "LOWER", "COALESCE", "CONCAT", "SUBSTRING", "LENGTH", "TRIM", "CAST", "CONVERT", "NOW", "DATE", "TIME", "DATETIME", "YEAR", "MONTH", "DAY", "ROUND", "FLOOR", "CEIL", "ABS", "IFNULL", "NULLIF", "ROW_NUMBER", "RANK", "DENSE_RANK" },
            CommentPrefix = "--"
        },
        ["kotlin"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "fun", "val", "var", "class", "object", "interface", "if", "else", "when", "for", "while", "do", "return", "break", "continue", "package", "import", "super", "this", "null", "true", "false", "try", "catch", "finally", "throw", "is", "in", "as", "typealias", "companion", "init", "constructor", "open", "data", "sealed", "inline", "noinline", "crossinline", "infix", "operator", "by", "suspend", "abstract", "final", "override", "private", "public", "internal", "protected", "lateinit", "const", "vararg", "out", "reified", "annotation", "enum", "inner", "tailrec", "external", "expect", "actual" },
            Builtins = new HashSet<string> { "println", "print", "readLine", "Array", "List", "MutableList", "Map", "MutableMap", "Set", "MutableSet", "Int", "String", "Boolean", "Double", "Float", "Long", "Short", "Byte", "Char", "Unit", "Any", "Nothing", "Pair", "Triple", "listOf", "mutableListOf", "mapOf", "mutableMapOf", "setOf", "mutableSetOf", "arrayOf", "intArrayOf", "to", "lazy", "also", "apply", "let", "run", "with", "takeIf", "takeUnless" },
            CommentPrefix = "//"
        },
        ["swift"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "func", "var", "let", "if", "else", "switch", "case", "default", "for", "while", "repeat", "return", "break", "continue", "fallthrough", "class", "struct", "enum", "extension", "protocol", "init", "deinit", "import", "guard", "defer", "in", "do", "try", "catch", "throw", "throws", "rethrows", "public", "private", "fileprivate", "internal", "open", "static", "self", "Self", "super", "true", "false", "nil", "where", "associatedtype", "typealias", "subscript", "mutating", "nonmutating", "indirect", "required", "convenience", "final", "weak", "unowned", "lazy", "override", "dynamic", "inout", "some", "any", "async", "await", "actor", "nonisolated", "isolated", "get", "set", "willSet", "didSet", "infix", "prefix", "postfix", "operator", "precedencegroup", "as", "is" },
            Builtins = new HashSet<string> { "print", "debugPrint", "Array", "Dictionary", "Set", "String", "Int", "Double", "Float", "Bool", "Optional", "Result", "Error", "Codable", "Equatable", "Hashable", "Comparable", "CustomStringConvertible", "OptionSet", "Sequence", "Collection", "IteratorProtocol", "Range", "ClosedRange", "Void", "Never", "Any", "AnyObject", "Type" },
            CommentPrefix = "//"
        },
        ["perl"] = new LanguageConfig
        {
            Keywords = new HashSet<string> { "my", "our", "local", "state", "sub", "if", "unless", "while", "until", "foreach", "for", "do", "next", "last", "redo", "goto", "return", "package", "use", "no", "require", "else", "elsif", "given", "when", "default", "BEGIN", "END", "CHECK", "INIT", "UNITCHECK", "and", "or", "not", "xor", "eq", "ne", "lt", "gt", "le", "ge", "cmp" },
            Builtins = new HashSet<string> { "print", "say", "chomp", "chop", "split", "join", "map", "grep", "sort", "reverse", "length", "substr", "index", "rindex", "push", "pop", "shift", "unshift", "splice", "keys", "values", "each", "exists", "delete", "defined", "undef", "ref", "bless", "tie", "untie", "open", "close", "read", "write", "seek", "tell", "eof", "binmode", "die", "warn", "exit", "eval", "exec", "system", "qw", "wantarray", "caller", "scalar", "chr", "ord", "hex", "oct", "sprintf", "printf", "pack", "unpack", "localtime", "gmtime", "time", "sleep" },
            CommentPrefix = "#"
        }
    };

    // Definition keywords for function/class coloring
    private static readonly HashSet<string> DefKeywords = new HashSet<string>
    {
        "def", "function", "fn", "func", "fun", "sub",
        "defp", "defmodule", "defmacro",
        "class", "interface", "struct", "enum", "trait", "type",
        "object", "data", "newtype"
    };

    public enum TokenType
    {
        Keyword,
        String,
        StringPrefix,
        Number,
        Comment,
        DocComment,
        Constant,
        FunctionDef,
        Decorator,
        BuiltIn,
        Variable,
        Bracket,
        Foreground
    }

    [Header("References")]
    [SerializeField] private Transform rayInteraction;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform textRect;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform cube1;
    [SerializeField] private Transform cube2;
    [SerializeField] private Transform cube3;
    [SerializeField] private BoxCollider colliderBox;

    [Header("Materials")]
    [SerializeField] private Material keywordMaterial;
    [SerializeField] private Material stringMaterial;
    [SerializeField] private Material numberMaterial;
    [SerializeField] private Material docCommentMaterial;
    [SerializeField] private Material constantMaterial;
    [SerializeField] private Material functionDefMaterial;
    [SerializeField] private Material builtInMaterial;
    [SerializeField] private Material variableMaterial;
    [SerializeField] private Material decoratorMaterial;

    [Header("Settings")]
    [SerializeField] private bool updateEveryFrame = false;
    [SerializeField] private string language = "csharp";

    private TextMeshProUGUI tmpText;
    private float lastCharacterCount = -1f;
    private string lastText = "";
    private Renderer cubeRenderer;
    private Renderer cube1Renderer;
    private Renderer cube2Renderer;
    private Renderer cube3Renderer;

    private void Awake()
    {
        if (textRect != null)
        {
            tmpText = textRect.GetComponent<TextMeshProUGUI>();
        }

        if (cube != null) cubeRenderer = cube.GetComponent<Renderer>();
        if (cube1 != null) cube1Renderer = cube1.GetComponent<Renderer>();
        if (cube2 != null) cube2Renderer = cube2.GetComponent<Renderer>();
        if (cube3 != null) cube3Renderer = cube3.GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateScales();
        UpdateColors();
    }

    private void Update()
    {
        if (updateEveryFrame)
        {
            UpdateScales();
            UpdateColors();
        }
    }

    // 大文字は2文字分としてカウント
    private int GetWeightedCharCount(string text)
    {
        int count = 0;
        foreach (char c in text)
        {
            if (char.IsUpper(c))
            {
                count += 2;
            }
            else
            {
                count += 1;
            }
        }
        return count;
    }

    public void UpdateScales()
    {
        if (tmpText == null) return;

        int charCount = GetWeightedCharCount(tmpText.text);

        // Minimum 4 characters for size calculation
        charCount = Mathf.Max(charCount, 4);

        // Scale width: reduce to 3/4 (make it shorter)
        float scaledCharCount = charCount * (3f / 4f);

        if (scaledCharCount == lastCharacterCount) return;
        lastCharacterCount = scaledCharCount;

        if (rayInteraction != null)
        {
            Vector3 scale = rayInteraction.localScale;
            scale.x = 0.05f * scaledCharCount;
            rayInteraction.localScale = scale;
        }

        if (canvasRect != null)
        {
            Vector2 size = canvasRect.sizeDelta;
            size.x = scaledCharCount * 5f;
            canvasRect.sizeDelta = size;
        }

        if (textRect != null)
        {
            Vector2 size = textRect.sizeDelta;
            size.x = scaledCharCount * 50f;
            textRect.sizeDelta = size;
        }

        float cubeScaleX = scaledCharCount * 0.05f + 0.02f;
        if (cube != null)
        {
            Vector3 scale = cube.localScale;
            scale.x = cubeScaleX;
            cube.localScale = scale;
        }
        if (cube1 != null)
        {
            Vector3 scale = cube1.localScale;
            scale.x = cubeScaleX;
            cube1.localScale = scale;
        }

        float cube2PosX = scaledCharCount * 0.025f + 0.005f;
        if (cube2 != null)
        {
            Vector3 pos = cube2.localPosition;
            pos.x = cube2PosX;
            cube2.localPosition = pos;
        }

        if (cube3 != null)
        {
            Vector3 pos = cube3.localPosition;
            pos.x = -cube2PosX;
            cube3.localPosition = pos;
        }

        if (colliderBox != null && rayInteraction != null)
        {
            // x-size = 1 + (0.1 / (rayInteraction's x-scale × 10))
            float rayScaleX = rayInteraction.localScale.x;
            if (rayScaleX > 0)
            {
                Vector3 size = colliderBox.size;
                size.x = 1f + (0.1f / (rayScaleX * 10f));
                colliderBox.size = size;
            }
        }
    }

    public void UpdateColors()
    {
        if (tmpText == null) return;

        string text = tmpText.text.Trim();
        if (text == lastText) return;
        lastText = text;

        TokenType tokenType = GetTokenStyle(text, language, null, null);
        Color color = GetColorForTokenType(tokenType);

        tmpText.color = color;

        Material material = GetMaterialForTokenType(tokenType);
        if (material != null)
        {
            if (cubeRenderer != null) cubeRenderer.material = material;
            if (cube1Renderer != null) cube1Renderer.material = material;
            if (cube2Renderer != null) cube2Renderer.material = material;
            if (cube3Renderer != null) cube3Renderer.material = material;
        }
        else
        {
            SetCubeColors(color);
        }
    }

    private LanguageConfig GetLanguageConfig(string lang)
    {
        if (LanguageConfigs.TryGetValue(lang, out var config))
        {
            return config;
        }
        return LanguageConfigs["javascript"];
    }

    // Exact port of getTokenStyle from syntax-highlight.ts
    private TokenType GetTokenStyle(string token, string lang, string prevToken, string nextToken)
    {
        if (string.IsNullOrEmpty(token)) return TokenType.Foreground;

        var config = GetLanguageConfig(lang);

        // String prefixes (f, r, b, etc.) - string color in JetBrains
        if (StringPrefixes.Contains(token) && nextToken != null &&
            (nextToken.StartsWith("\"") || nextToken.StartsWith("'") || nextToken.StartsWith("`")))
        {
            return TokenType.StringPrefix;
        }

        // Interpolation brackets: { } ${ in f-strings and template literals
        // These are keyword color (orange) in JetBrains
        if (token == "{" || token == "}" || token == "${")
        {
            return TokenType.Keyword;
        }

        // String (including partial strings from interpolation split)
        bool startsWithQuote = token.StartsWith("\"") || token.StartsWith("'") || token.StartsWith("`");
        bool endsWithQuote = token.EndsWith("\"") || token.EndsWith("'") || token.EndsWith("`");
        if (startsWithQuote || endsWithQuote)
        {
            // Python Docstrings (""" or ''')
            if (lang == "python" && (token.StartsWith("\"\"\"") || token.StartsWith("'''")))
            {
                return TokenType.DocComment;
            }
            return TokenType.String;
        }

        // Middle string parts between interpolations
        bool prevIsClosingBrace = prevToken == "}";
        bool nextIsOpeningBrace = nextToken == "{" || nextToken == "${";
        if (prevIsClosingBrace && nextIsOpeningBrace)
        {
            return TokenType.String;
        }

        // Comment
        bool isComment = token.StartsWith(config.CommentPrefix) ||
                        token.StartsWith("//") ||
                        token.StartsWith("/*") ||
                        token.StartsWith("--") ||
                        token.StartsWith("{-") ||
                        token.StartsWith(";");

        if (isComment)
        {
            // Doc Comment (starts with /**)
            if (token.StartsWith("/**"))
            {
                return TokenType.DocComment;
            }
            return TokenType.Comment;
        }

        // Decorator
        if (token.StartsWith("@")) return TokenType.Decorator;

        // Number
        if (Regex.IsMatch(token, @"^\d+$")) return TokenType.Number;

        // Keywords & Built-ins
        if (config.Keywords.Contains(token)) return TokenType.Keyword;
        if (config.Builtins.Contains(token)) return TokenType.BuiltIn;

        // Constant (All uppercase, at least 2 chars)
        if (Regex.IsMatch(token, @"^[A-Z][A-Z0-9_]+$") && token.Length > 1)
        {
            return TokenType.Constant;
        }

        // Other Brackets (not interpolation related)
        if (Regex.IsMatch(token, @"^[()[\]]$") || token == "#{" || token == "{$" || token == "\\(")
        {
            return TokenType.Bracket;
        }

        // Function/Class Definition (prev token was 'def', 'function', 'class', etc.)
        if (!string.IsNullOrEmpty(prevToken) && DefKeywords.Contains(prevToken))
        {
            return TokenType.FunctionDef;
        }

        // Type annotation (PascalCase identifier after ':', '->', or ')')
        if (Regex.IsMatch(token, @"^[A-Z][a-zA-Z0-9_]*$") &&
            (prevToken == ":" || prevToken == "->" || prevToken == ")"))
        {
            return TokenType.FunctionDef;
        }

        // Identifiers (variables)
        if (Regex.IsMatch(token, @"^[a-zA-Z_][a-zA-Z0-9_]*$") ||
            (lang == "php" && token.StartsWith("$")) ||
            (lang == "kotlin" && token.StartsWith("$")))
        {
            return TokenType.Variable;
        }

        // Default
        return TokenType.Foreground;
    }

    private Color GetColorForTokenType(TokenType type)
    {
        switch (type)
        {
            case TokenType.Keyword: return SyntaxColors.Keyword;
            case TokenType.String:
            case TokenType.StringPrefix: return SyntaxColors.String;
            case TokenType.Number: return SyntaxColors.Number;
            case TokenType.Comment: return SyntaxColors.Comment;
            case TokenType.DocComment: return SyntaxColors.DocComment;
            case TokenType.Constant: return SyntaxColors.Constant;
            case TokenType.FunctionDef: return SyntaxColors.FunctionDef;
            case TokenType.Decorator: return SyntaxColors.Decorator;
            case TokenType.BuiltIn: return SyntaxColors.BuiltIn;
            case TokenType.Variable: return SyntaxColors.Variable;
            case TokenType.Bracket: return SyntaxColors.Bracket;
            case TokenType.Foreground:
            default: return SyntaxColors.Foreground;
        }
    }

    private Material GetMaterialForTokenType(TokenType type)
    {
        switch (type)
        {
            case TokenType.Keyword: return keywordMaterial;
            case TokenType.String:
            case TokenType.StringPrefix: return stringMaterial;
            case TokenType.Number: return numberMaterial;
            case TokenType.DocComment: return docCommentMaterial;
            case TokenType.Constant: return constantMaterial;
            case TokenType.FunctionDef: return functionDefMaterial;
            case TokenType.BuiltIn: return builtInMaterial;
            case TokenType.Decorator: return decoratorMaterial;
            case TokenType.Variable:
            case TokenType.Bracket:
            case TokenType.Comment:
            case TokenType.Foreground:
            default: return variableMaterial;
        }
    }

    private void SetCubeColors(Color color)
    {
        if (cubeRenderer != null) cubeRenderer.material.color = color;
        if (cube1Renderer != null) cube1Renderer.material.color = color;
        if (cube2Renderer != null) cube2Renderer.material.color = color;
        if (cube3Renderer != null) cube3Renderer.material.color = color;
    }
}
