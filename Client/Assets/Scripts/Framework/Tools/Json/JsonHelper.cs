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
                Debug.LogError($"[JsonHelper.LoadFromFile] serialize failed, {e}");
                return false;
            }
            return true;
        }
        
        public static bool LoadFromRes<T>(string path, out T ret) {
            ret = default;
            try {
                TextAsset asset = Resources.Load<TextAsset>(path);
                if (asset == null) {
                    Debug.LogError($"[JsonHelper.LoadFromRes] {path} not exists");
                    return false;
                }
                
                ret = JsonConvert.DeserializeObject<T>(asset.text);
                return true;
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.LoadFromRes] parse failed, {e}");
                return false;
            }
        }
        
        public static string GetString<T>(T data) {
            try {
                string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
                return json;
            } catch (Exception e) {
                Debug.LogError($"[JsonHelper.GetString] serialize failed, {e}");
                return "";
            }
        }
    }
}