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
}
