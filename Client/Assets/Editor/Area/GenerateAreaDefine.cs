using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Editor.Area {
    public static class ExportDefine {
        private static string ScriptPath = Application.dataPath + "/Scripts/Combat/Area/";
        private static string TemplatePath = Application.dataPath + "/Editor/Area/Template/";

        private static string BuildEnumList(string definePath, List<string> name) {
            Dictionary<string, int> exists = new Dictionary<string, int>();
            List<string> orderedName = new List<string>();
            int index = 0;
            if (File.Exists(definePath)) {
                string define = File.ReadAllText(definePath);
                MatchCollection matches = Regex.Matches(
                    define,
                    @"(?m)^\s*(\w+)\s*=\s*(\d+),");
                foreach (Match match in matches) {
                    string enumName = match.Groups[1].Value;
                    int enumIndex = int.Parse(match.Groups[2].Value);
                    exists[enumName] = enumIndex;
                    orderedName.Add(enumName);
                    if (enumIndex > index) {
                        index = enumIndex;
                    }
                }
            }

            foreach (string item in name) {
                if (!exists.ContainsKey(item)) {
                    exists[item] = ++index;
                    orderedName.Add(item);
                }
            }

            string enumList = "";
            foreach (string item in orderedName) {
                enumList += "        " + item + " = " + exists[item] + ",\n";
            }

            return enumList;
        }

        private static string BuildFactoryList(string definePath, string enumType, List<string> name, string createExpression) {
            HashSet<string> exists = new HashSet<string>();
            string factoryList = "";
            if (File.Exists(definePath)) {
                string define = File.ReadAllText(definePath);
                MatchCollection matches = Regex.Matches(
                    define,
                    @"(?ms)^\s*case\s+" + enumType + @"\.(\w+):\s*\n\s*return\s+([^;]+);");
                foreach (Match match in matches) {
                    string caseName = match.Groups[1].Value;
                    exists.Add(caseName);
                    factoryList += "                case " + enumType + "." + caseName + ":\n                    return " +
                                   match.Groups[2].Value.Trim() + ";\n";
                }
            }

            foreach (string item in name) {
                if (exists.Contains(item)) {
                    continue;
                }
                factoryList += "                case " + enumType + "." + item + ":\n                    return " +
                               createExpression.Replace("{Name}", item) + ";\n";
            }

            return factoryList;
        }
        
        private static bool GenerateEffectDefine(List<string> name) {
            if (!File.Exists(TemplatePath + "EffectDefine.txt")) {
                return false;
            }
            string template = File.ReadAllText(TemplatePath + "EffectDefine.txt");
            string definePath = ScriptPath + "EffectDefine.cs";
            string enumList = BuildEnumList(definePath, name);
            string factoryList = BuildFactoryList(
                definePath,
                "EffectType",
                name,
                "new {Name}(area, config.RaycastId, config.Params)");

            template = template.Replace("{EffectEnumList}", enumList);
            template = template.Replace("{EffectFactoryList}", factoryList);
            
            File.WriteAllText(definePath, template);
            return true;
        }

        private static bool GenerateRaycastDefine(List<string> name) {
            if (!File.Exists(TemplatePath + "RaycastDefine.txt")) {
                return false;
            }
            string template = File.ReadAllText(TemplatePath + "RaycastDefine.txt");
            string definePath = ScriptPath + "RaycastDefine.cs";
            string enumList = BuildEnumList(definePath, name);
            string factoryList = BuildFactoryList(
                definePath,
                "RaycastType",
                name,
                "new {Name}(area, config.Params)");

            template = template.Replace("{RaycastEnumList}", enumList);
            template = template.Replace("{RaycastFactoryList}", factoryList);
            
            File.WriteAllText(definePath, template);
            return true;
        }
        
        [MenuItem("工具/技能/生成 Area 配置文件")]
        public static void Execute() {
            if (!Directory.Exists(ScriptPath + "Effect/") || !Directory.Exists(ScriptPath + "Raycast/")) {
                return;
            }

            List<string> effectName = new List<string>();
            foreach (var file in Directory.GetFiles(ScriptPath + "Effect/", "*.cs", SearchOption.AllDirectories)) {
                string effect = Path.GetFileNameWithoutExtension(file);
                if (effect[0] != '_') {
                    effectName.Add(effect);
                }
            }

            List<string> raycastName = new List<string>();
            foreach (var file in Directory.GetFiles(ScriptPath + "Raycast/", "*.cs", SearchOption.AllDirectories)) {
                string raycast = Path.GetFileNameWithoutExtension(file);
                if (raycast[0] != '_') {
                    raycastName.Add(raycast);
                }
            }

            if (!GenerateEffectDefine(effectName)) {
                return;
            }
            
            if (!GenerateRaycastDefine(raycastName)) {
                return;
            }
            
            AssetDatabase.Refresh();
            Debug.Log("已生成所有配置代码");
        }
    }
}
