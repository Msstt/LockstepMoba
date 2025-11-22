using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Framework;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.Network {
    public class GenerateMessageCode {
        private static string protoPath = Application.dataPath + "/../../Proto";
        private static string clientCodePath = Application.dataPath + "/Scripts/Network/Message";
        private static string serverCodePath = Application.dataPath + "/../../Server/Network/Message";
        private static string defineTemplatePath = Application.dataPath + "/Editor/Network/Template/MessageDef.txt";
        
        private static bool GenerateProto(string outputPath) {
            if (Directory.Exists(outputPath)) {
                Directory.Delete(outputPath, true);
            }
            Directory.CreateDirectory(outputPath);
            var protoFiles = Directory.GetFiles(protoPath, "*.proto", SearchOption.AllDirectories);
            foreach (var file in protoFiles) {
                string args = $"--csharp_out={outputPath} --proto_path={protoPath} {file}";

                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = protoPath + "/protoc",
                    Arguments = args,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(psi)) {
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0) {
                        Debug.LogError($"生成失败: {file}\n{error}");
                        return false;
                    }
                }
            }

            return true;
        }
        
        public static bool GenerateDefine(string outputPath) {
            if (File.Exists(outputPath)) {
                File.Delete(outputPath);
            }
            
            if (!JsonHelper.LoadFromFile(protoPath + "/proto_msg_map.json", out Dictionary<string, int> protoMap)) {
                Debug.LogError("proto_msg_map.json 解析失败");
                return false;
            }

            string messageEnumList = "\n";
            string messageMapping = "\n";
            string messageParserList = "\n";
            foreach (var value in protoMap) {
                messageEnumList += "        " + value.Key + " = " + value.Value + ",\n";
                messageMapping += "            { MessageDef." + value.Key + ", typeof(" + value.Key + ") },\n";
                messageParserList += "            { MessageDef." + value.Key + ", " + value.Key + ".Parser },\n";
            }
            string template = File.ReadAllText(defineTemplatePath);
            template = template.Replace("{MessageEnumList}", messageEnumList);
            template = template.Replace("{MessageMapping}", messageMapping);
            template = template.Replace("{MessageParserList}", messageParserList);
            EditorApplication.LockReloadAssemblies();
            File.WriteAllText(outputPath, template);
            EditorApplication.UnlockReloadAssemblies();

            return true;
        }   
        
        [MenuItem("工具/网络/生成协议代码")]
        public static void Execute() {
            if (!GenerateProto(clientCodePath + "/Proto")) {
                return;
            }
            if (!GenerateProto(serverCodePath + "/Proto")) {
                return;
            }
            
            if (!GenerateDefine(clientCodePath + "/MessageDef.cs")) {
                return;
            }
            if (!GenerateDefine(serverCodePath + "/MessageDef.cs")) {
                return;
            }
            
            AssetDatabase.Refresh();
            Debug.Log("已生成所有协议代码");
        }
    }
}