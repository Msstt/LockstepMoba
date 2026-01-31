using System.Collections.Generic;
using System.IO;
using Combat.Skill;
using Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Skill {
    public class SkillData {
        public int Id;
        public string Name;
        public SkillType Type;
        
        public SkillData(int id) {
            Id = id;
        }

        public SkillData(SkillConfig config) {
            Id = config.Id;
            Name = config.Name;
            Type = (SkillType)config.SkillType;
        }
    }
    
    public static class SkillDefineUtils {
        private static string DefinePath = Application.dataPath + "/Editor/Skill/Data/SkillDefine.json";
        
        public static void Export(List<SkillData> define) {
            JsonHelper.SaveToFile(define, DefinePath);
            AssetDatabase.Refresh();
        }
        
        public static List<SkillData> Import() {
            if (File.Exists(DefinePath) && JsonHelper.LoadFromFile<List<SkillData>>(DefinePath, out var define)) {
                return define;
            }
            return Refresh();
        }

        public static List<SkillData> Refresh() {
            var define = new List<SkillData>();
            var skillFiles = Directory.GetFiles(SkillGraph.ExportPath, "*.json", SearchOption.AllDirectories);
            foreach (var file in skillFiles) {
                if (JsonHelper.LoadFromFile<SkillConfig>(file, out var config)) {
                    define.Add(new SkillData(config));
                }
            }
            define.Sort((a, b) => a.Id.CompareTo(b.Id));
            Export(define);
            return define;
        }
    }
}