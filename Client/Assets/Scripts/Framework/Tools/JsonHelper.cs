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
    }
}