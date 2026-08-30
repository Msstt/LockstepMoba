using Sirenix.OdinInspector;
using UnityEngine;

public class GMTool : MonoBehaviour {
    [LabelText("本地调试模式")]
    public bool IsLocalDebug = false;
    
    [LabelText("显示调试辅助线")]
    public bool ShowDebugMode = false;
    
    [LabelText("打印技能树")]
    public bool PrintSkillTree = false;
    
    [LabelText("关闭战争迷雾")]
    public bool DisableFog = false;
    
    [LabelText("关闭小兵")]
    public bool DisableMinion = false;
    
    [LabelText("关闭防御塔")]
    public bool DisableTurret = false;
    
    [LabelText("服务器地址")]
    public string ServerIP = "";
    [LabelText("服务器端口")]
    public int ServerPort = 9980;
}
