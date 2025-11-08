using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Framework {
    public static class JsonHelper {
        public static T LoadFromFile<T>(string path) {
            try {
                if (!File.Exists(path)) {
                    Debug.LogError($"[JsonHelper.LoadFromFile] {path} not exists");
                    return default;
                }
                
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.LoadFromFile] parse failed, {e}");
                return default;
            }
        }
        
        public static bool SaveToFile<T>(T data, string path) {
            try {
                File.Delete(path);
                string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
                File.WriteAllText(path, json);
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.LoadFromFile] parse failed, {e}");
                return false;
            }
            return true;
        }
    }
}