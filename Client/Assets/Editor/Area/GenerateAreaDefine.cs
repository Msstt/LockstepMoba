using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.Area {
    public static class ExportDefine {
        private static string ScriptPath = Application.dataPath + "/Scripts/Combat/Area/";
        private static string TemplatePath = Application.dataPath + "/Editor/Area/Template/";
        
        private static bool GenerateEffectDefine(List<string> name) {
            if (!File.Exists(TemplatePath + "EffectDefine.txt")) {
                return false;
            }
            string template = File.ReadAllText(TemplatePath + "EffectDefine.txt");
            string enumList = "";
            string factoryList = "";
            int index = 0;
            foreach (string effect in name) {
                enumList += "        " + effect + " = " + ++index + ",\n";
                factoryList += "                case EffectType." + effect + ":\n                    return new " +
                               effect + "(area, config.Params);\n";
            }

            template = template.Replace("{EffectEnumList}", enumList);
            template = template.Replace("{EffectFactoryList}", factoryList);
            
            File.WriteAllText(ScriptPath + "EffectDefine.cs", template);
            return true;
        }
        
        [MenuItem("工具/技能/生成 Area 配置文件")]
        public static void Execute() {
            if (!Directory.Exists(ScriptPath + "Effect/")) {
                return;
            }

            List<string> name = new List<string>();
            foreach (var file in Directory.GetFiles(ScriptPath + "Effect/", "*.cs", SearchOption.AllDirectories)) {
                name.Add(Path.GetFileNameWithoutExtension(file));
            }

            if (!GenerateEffectDefine(name)) {
                return;
            }
            
            AssetDatabase.Refresh();
            Debug.Log("已生成所有配置代码");
        }
    }
}