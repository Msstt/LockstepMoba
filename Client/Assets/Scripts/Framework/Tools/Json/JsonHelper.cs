using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Framework {
    public static class JsonHelper {
        public static bool LoadFromFile<T>(string path, out T ret) {
            ret = default;
            try {
                if (!File.Exists(path)) {
                    Debug.LogError($"[JsonHelper.LoadFromFile] {path} not exists");
                    return false;
                }
                
                string json = File.ReadAllText(path);
                ret = JsonConvert.DeserializeObject<T>(json);
                return true;
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.LoadFromFile] parse failed, {e}");
                return false;
            }
        }
        
        public static bool LoadFromString<T>(string json, out T ret) {
            ret = default;
            try {
                ret = JsonConvert.DeserializeObject<T>(json);
                return true;
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.LoadFromFile] parse failed, {e}");
                return false;
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