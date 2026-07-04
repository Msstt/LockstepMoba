using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Editor.Buff {
    public static class ExportDefine {
        private static string ScriptPath = Application.dataPath + "/Scripts/Combat/Buff/";
        private static string TemplatePath = Application.dataPath + "/Editor/Buff/Template/";

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

        private static string BuildFactoryList(string definePath, List<string> name) {
            HashSet<string> exists = new HashSet<string>();
            string factoryList = "";
            if (File.Exists(definePath)) {
                string define = File.ReadAllText(definePath);
                MatchCollection matches = Regex.Matches(
                    define,
                    @"(?ms)^\s*case\s+EffectType\.(\w+):\s*\n\s*return\s+([^;]+);");
                foreach (Match match in matches) {
                    string caseName = match.Groups[1].Value;
                    exists.Add(caseName);
                    factoryList += "                case EffectType." + caseName + ":\n                    return " +
                                   match.Groups[2].Value.Trim() + ";\n";
                }
            }

            foreach (string item in name) {
                if (exists.Contains(item)) {
                    continue;
                }
                factoryList += "                case EffectType." + item + ":\n                    return new " +
                               item + "(buff, config.Params);\n";
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
            string factoryList = BuildFactoryList(definePath, name);

            template = template.Replace("{EffectEnumList}", enumList);
            template = template.Replace("{EffectFactoryList}", factoryList);
            
            File.WriteAllText(definePath, template);
            return true;
        }
        
        [MenuItem("工具/技能/生成 Buff 配置文件")]
        public static void Execute() {
            if (!Directory.Exists(ScriptPath + "Effect/")) {
                return;
            }

            List<string> name = new List<string>();
            foreach (var file in Directory.GetFiles(ScriptPath + "Effect/", "*.cs", SearchOption.AllDirectories)) {
                string effect = Path.GetFileNameWithoutExtension(file);
                if (effect[0] != '_') {
                    name.Add(effect);
                }
            }

            if (!GenerateEffectDefine(name)) {
                return;
            }
            
            AssetDatabase.Refresh();
            Debug.Log("已生成所有配置代码");
        }
    }
}
