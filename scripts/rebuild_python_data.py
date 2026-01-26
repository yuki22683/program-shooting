import os
import re

def esc(s):
    if not s: return ""
    b = chr(92)
    q = chr(34)
    s = s.replace(b, b+b)
    s = s.replace(q, b+q)
    return s

def get_ex(c):
    start_match = re.search(r'exercises\s*:\s*\[', c)
    if not start_match:
        start_match = re.search(r'["\']exercises["\']\s*:\s*\[', c)
    if not start_match: return []
    
    start_pos = start_match.end()
    txt = c[start_pos:]
    
    res = []
    depth = 0
    in_dq = False
    in_sq = False
    in_bt = False
    bs = chr(92)
    dq = chr(34)
    sq = chr(39)
    bt = chr(96)
    current_ex_start = -1
    i = 0
    length = len(txt)
    while i < length:
        char = txt[i]
        escaped = False
        if i > 0 and txt[i-1] == bs:
            bc = 0
            k = i - 1
            while k >= 0 and txt[k] == bs:
                bc += 1
                k -= 1
            if bc % 2 == 1: escaped = True
        if not escaped:
            if in_dq:
                if char == dq: in_dq = False
            elif in_sq:
                if char == sq: in_sq = False
            elif in_bt:
                if char == bt: in_bt = False
            else:
                if char == dq: in_dq = True
                elif char == sq: in_sq = True
                elif char == bt: in_bt = True
                elif char == "{":
                    if depth == 0: current_ex_start = i
                    depth += 1
                elif char == "}":
                    depth -= 1
                    if depth == 0 and current_ex_start != -1:
                        res.append(txt[current_ex_start:i+1])
                        current_ex_start = -1
                elif char == "[": depth += 1
                elif char == "]":
                    if depth == 0: break
                    depth -= 1
        i += 1
    return res

def parse_it():
    base = "C:/Work/git/senkou-code/data/lessons"
    all_data = []
    for i in range(1, 6):
        fn = "python" + (str(i) if i > 1 else "") + ".ts"
        p = os.path.join(base, fn)
        if not os.path.exists(p): continue
        print("File: " + fn)
        with open(p, "r", encoding="utf-8") as f:
            exs = get_ex(f.read())
            print("  Items: " + str(len(exs)))
            ls = []
            for e in exs:
                d = {}
                bs = chr(92)
                
                mt = re.search(r'title\s*:\s*(["\'])(.*?)\1', e)
                if mt: d["title"] = mt.group(2)
                
                ml = re.search(r'correctLines\s*:\s*\[(.*?)\]', e, re.DOTALL)
                if ml:
                    content = ml.group(1)
                    pat = r'(["\''"])(\1)'
                    matches = re.findall(pat, content, re.DOTALL)
                    d["lines"] = [m[1] for m in matches]
                
                if "lines" not in d:
                    mc = re.search(r'code\s*:\s*`([^`]*)`', e, re.DOTALL)
                    if not mc: mc = re.search(r'code\s*:\s*(["\'])(.*?)\1', e, re.DOTALL)
                    if mc:
                        content = mc.group(1) if len(mc.groups()) == 1 else mc.group(2)
                        d["lines"] = content.replace(chr(13), "").split(chr(10))
                
                mc = re.search(r'comments\s*:\s*\[(.*?)\]', e, re.DOTALL)
                if mc:
                    d["comments"] = []
                    parts = re.findall(r'line\s*:\s*(\d+).*?text\s*:\s*(["\'])(.*?)\2', mc.group(1), re.DOTALL)
                    for ln, q, txt in parts:
                        d["comments"].append({"line": int(ln), "text": txt})
                
                mt = re.search(r'expected_output\s*:\s*(["\'])(.*?)\1', e, re.DOTALL)
                if mt:
                    d["out"] = mt.group(2).replace(bs + "n", chr(10)).split(chr(10))
                
                ls.append(d)
            all_data.append(ls)
    return all_data

def make_cs(data):
    lines = ["    private void InitializePythonLessons()", "    {"]
    for i in range(1, 6):
        lines.append("        Lesson lesson" + str(i) + " = new Lesson { titleKey = " + chr(34) + "python_lesson" + str(i) + "_title" + chr(34) + " };")
    lines.append("")
    
    for l_idx, exs in enumerate(data):
        ln = l_idx + 1
        lines.append("        // Lesson " + str(ln))
        for e_idx, ex in enumerate(exs):
            en = e_idx + 1
            pre = "python_lesson" + str(ln) + "_ex" + str(en)
            
            raw_lines = ex.get("lines", [])
            cs_lines = []
            for l in raw_lines:
                cs_lines.append(chr(34) + esc(l) + chr(34))
            cl = ", ".join(cs_lines)
            
            cms = "            comments = new List<LocalizedComment>(),"
            if ex.get("comments"):
                clist = []
                for i, c in enumerate(ex["comments"]):
                    l_idx = max(0, c["line"] - 1)
                    clist.append("new LocalizedComment { lineIndex = " + str(l_idx) + ", commentPrefix = " + chr(34) + "#" + chr(34) + ", localizationKey = " + chr(34) + pre + "_comment" + str(i+1) + chr(34) + " }")
                cms = "            comments = new List<LocalizedComment> { " + ", ".join(clist) + " },"
            
            out_str = "            expectedOutput = new List<string>()"
            if ex.get("out"):
                plist = [chr(34) + esc(p) + chr(34) for p in ex["out"] if p]
                if plist: out_str = "            expectedOutput = new List<string> { " + ", ".join(plist) + " }"
            
            title = esc(ex.get("title", ""))
            
            lines.append("        // Ex" + str(en) + ": " + title)
            lines.append("        lesson" + str(ln) + ".exercises.Add(new Exercise {")
            lines.append("            titleKey = " + chr(34) + pre + "_title" + chr(34) + ",")
            lines.append("            slideKeyPrefix = " + chr(34) + pre + "" + chr(34) + ",")
            lines.append("            slideCount = " + str(len(ex.get("comments", [])) + 1) + ",")
            lines.append("            correctLines = new List<string> { " + cl + " },")
            lines.append(cms)
            lines.append(out_str)
            lines.append("        });")
    
    for i in range(1, 6): lines.append("        lessons.Add(lesson" + str(i) + ");")
    lines.append("    }")
    return lines

if __name__ == "__main__":
    d = parse_it()
    c = make_cs(d)
    with open("python_snippet.txt", "w", encoding="utf-8") as f: f.write(chr(10).join(c))
    print("Done")