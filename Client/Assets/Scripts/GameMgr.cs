using System;
using System.Collections.Generic;
using Combat;
using Combat.Actor;
using Framework;
using Framework.Network;
using Network;
using UnityEngine;

public class GameMgr : Singleton<GameMgr> {
    private Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();
    private List<ISystem> systemList = new List<ISystem>();
    
    private IFrameDriver driver = null;
    private bool frameHasStarted = false;
    
    public bool IsRunning => frameHasStarted;

    public int Frame => GetSystem<ILockStep>().Frame;
    public FloatF DeltaTime => 1 / new FloatF(30);

    public void Init(HashSet<Type> system = null) {
        void Register<T1, T2>() where T2 : T1, new() where T1 : ISystem {
            if (system == null || system.Contains(typeof(T1))) {
                RegisterSystem<T1>(new T2());
            }
        }
        
        // 注册顺序即为更新顺序
        Register<INetwork, Framework.Network.Network>();
        Register<ILockStep, LockStep>();
        Register<Navmesh.INavmesh, Navmesh.Navmesh>();
        Register<ICombatSystem, CombatSystem>();
        Register<IActorSystem, ActorSystem>();
    }
    
    public void Start() {
        foreach (var system in systemList) {
            system.Start();
        }
    }

    public void StartFrame() {
        foreach (var system in systemList) {
            system.FrameStart();
        }
        frameHasStarted = true;
    }
    
    public void Update() {
        if (frameHasStarted && driver != null && driver.FrameReady()) {
            FrameUpdate();
        }
        
        foreach (var system in systemList) {
            system.Update();
        }
        UpdateLocalDebug();
    }
    
    public void FrameUpdate() {
        foreach (var system in systemList) {
            system.FrameUpdate();
        }
    }
    
    private void RegisterSystem<T>(T system) where T : ISystem {
        var type = typeof(T);
        if (systems.ContainsKey(type)) {
            Debug.LogError($"[GameMgr!!!] System {type} already registered");
            return;
        }
        systems[type] = system;
        systemList.Add(system);
        if (system is IFrameDriver) {
            if (driver != null) {
                Debug.LogError("[GameMgr!!!] Frame driver already registered");
                return;
            }
            driver = system as IFrameDriver;
        }
    }
    
    public T GetSystem<T>() where T : class, ISystem {
        var type = typeof(T);
        if (systems.ContainsKey(type)) {
            return systems[type] as T;
        }
        Debug.LogError($"[GameMgr!!!] System {type} not found");
        return null;
    }

    #region 本地调试模式

    private bool isLocalDebug = false;
    private float lastTick = 0;
    private int frame = 0;
    
    public void StartLocalDebug() {
        isLocalDebug = true;
        lastTick = Time.time;

        var msg = new battle_start_s2c {
            SelfUid = 1,
        };
        msg.Players.Add(new battle_start_s2c.Types.player_info {
            Uid = 1,
            ChampionId = 1,
        });
        GetSystem<ICombatSystem>().Init(msg);
        StartFrame();
    }
    
    public void UpdateLocalDebug() {
        if (!isLocalDebug) return;
        
        if (Time.time - lastTick < 0.033f) return;
        lastTick = Time.time;
        frame++;

        var inputMsg = GetSystem<ILockStep>().GetInputMsg();
        var msg = new frame_input_s2c {
            Frame = frame,
        };
        msg.Inputs.Add(
            new frame_input_s2c.Types.input_info {
                Uid = 1, 
                Input = inputMsg.Input,
            });
        GetSystem<ILockStep>().PushInputMsg(msg);
    }

    #endregion
}
