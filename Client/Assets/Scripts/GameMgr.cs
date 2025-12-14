using Battle;
using Framework;
using Google.Protobuf.Collections;
using Network;
using PlasticGui.WorkspaceWindow.Merge;
using UnityEngine;

public class GameMgr : Singleton<GameMgr> {
    public void Start() {
        NavmeshUtils.Start();
        NetworkUtils.Start();
    }
    
    public void Update() {
        UpdateLocalDebug();
    }
    
    public void FrameUpdate() {
        NavmeshUtils.Update();
    }

    #region 本地调试模式

    private bool isLocalDebug = false;
    private float lastTick = 0;
    private int frame = 0;
    
    public void StartLocalDebug() {
        isLocalDebug = true;
        lastTick = Time.time;
        
        BattleMgr.Instance.Start(new battle_start_s2c {
            SelfUid = 1,
            Uids = { 1 },
        });
        LockStep.Instance.Start();
    }
    
    public void UpdateLocalDebug() {
        if (!isLocalDebug) return;
        
        if (Time.time - lastTick < 0.033f) return;
        lastTick = Time.time;
        frame++;

        var inputMsg = LockStep.Instance.GetInputMsg();
        var msg = new frame_input_s2c {
            Frame = frame,
        };
        msg.Inputs.Add(
            new frame_input_s2c.Types.input_info {
                Uid = 1, 
                Input = inputMsg.Input,
            });
        LockStep.Instance.PushInputMsg(msg);
    }

    #endregion
}
