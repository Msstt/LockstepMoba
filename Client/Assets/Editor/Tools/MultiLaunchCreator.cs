using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor {
    public class MultiLaunchCreator {
        private static string clientPath = Application.dataPath + "/../../Client";
        private static string[] folders = {
            "/Assets",
            "/Packages",
            "/ProjectSettings",
            "/Library",
            "/UserSettings",
        };
        
        [MenuItem("工具/Unity多开")]
        public static void CreateLaunchDir() {
            string targetPath = clientPath + "_Temp";

            if (Directory.Exists(targetPath)) {
                Debug.Log("多开目录已存在 " + targetPath);
                return;
            }
            Directory.CreateDirectory(targetPath);
            
            try {
                foreach (string folder in folders) {
                    if (!CreateSymlink(targetPath + folder, clientPath + folder)) {
                        return;
                    }
                }
                Debug.Log("多开构建成功 " + targetPath);
            } catch (Exception e) {
                Debug.LogError($"多开构建失败 {e.Message}");
            }
        }

        private static bool CreateSymlink(string linkPath, string targetPath) {
            if (Directory.Exists(linkPath) || File.Exists(linkPath))
                return true;
            
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "/bin/bash",
                Arguments = $"-c \"ln -s '{targetPath}' '{linkPath}'\"",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            
            using (var process = Process.Start(psi)) {
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0) {
                    Debug.LogError($"多开构建失败: {error}");
                    return false;
                }
            }

            return true;
        }
    }
}