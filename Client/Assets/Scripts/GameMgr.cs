using System;
using System.Collections.Generic;
using Framework;
using Network;
using UnityEngine;
using UnityEngine.Profiling;

public class GameMgr : Singleton<GameMgr> {
    private Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();
    private List<ISystem> systemList = new List<ISystem>();
    
    private IFrameDriver driver = null;
    private bool frameHasStarted = false;
    
    public bool IsRunning => frameHasStarted;
    
    public GMTool GMTool { get; set; }

    public int Frame {
        get {
            if (driver == null) {
                Debug.LogError("[GameMgr!!!] No frame driver registered");
                return -1;
            }
            return driver.Frame;
        }
    }
    
    public readonly int FramePerSecond = 30;
    public FloatF DeltaTime => 1 / new FloatF(FramePerSecond);

    public void RegisterSystem(HashSet<Type> system = null) {
        void Register<T1, T2>() where T2 : T1, new() where T1 : ISystem {
            if (system == null || system.Contains(typeof(T1))) {
                RegisterSystem<T1>(new T2());
            }
        }
        
        // 注册顺序即为更新顺序
        Register<Data.IDataSystem, Data.DataSystem>();
        
        Register<InputSystem.IInputSystem, InputSystem.InputSystem>();
        
        Register<Framework.Network.INetwork, Framework.Network.Network>();
        Register<ILockStep, LockStep>();
        
        Register<Navmesh.INavmesh, Navmesh.Navmesh>();
        
        Register<Combat.ICombatSystem, Combat.CombatSystem>();
        Register<Combat.Actor.IActorSystem, Combat.Actor.ActorSystem>();
        Register<Combat.Skill.ISkillSystem, Combat.Skill.SkillSystem>();
        Register<Combat.Area.IAreaSystem, Combat.Area.AreaSystem>();
        
        Register<Combat.Actor.ISpawnSystem, Combat.Actor.SpawnSystem>();
        
        Register<Framework.UI.IUISystem, Framework.UI.UISystem>();

        if (driver == null) {
            RegisterSystem<IFrameDriver>(new MockLockStep());
        }
    }
    
    public void Init() {
        foreach (var system in systemList) {
            (system as IInitSystem)?.Init();
        }
    }

    public void Start() {
        foreach (var system in systemList) {
            (system as IStartSystem)?.Start();
        }
        frameHasStarted = true;
    }
    
    public void Update() {
        if (frameHasStarted && driver != null && driver.FrameReady()) {
            FrameUpdate();
        }
        Profiler.BeginSample("GameMgr.Update");
        foreach (var system in systemList) {
            (system as IUpdateSystem)?.Update();
        }
        Profiler.EndSample();
        UpdateLocalDebug();
    }
    
    public void FrameUpdate() {
        Profiler.BeginSample("GameMgr.FrameUpdate");
        foreach (var system in systemList) {
            (system as IFrameUpdateSystem)?.FrameUpdate(Frame);
        }
        Profiler.EndSample();
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
            Camp = 0,
            Skill = { 5, 4 },
        });
        msg.Players.Add(new battle_start_s2c.Types.player_info {
            Uid = 2,
            ChampionId = 1,
            Camp = 1,
            Skill = { 3, 3 },
        });
        msg.Players.Add(new battle_start_s2c.Types.player_info {
            Uid = 3,
            ChampionId = 1,
            Camp = 0,
            Skill = { 3, 3 },
        });
        GetSystem<Combat.ICombatSystem>().SetStartInfo(msg);
        Start();
    }
    
    public void UpdateLocalDebug() {
        if (!isLocalDebug) return;
        
        if (Time.time - lastTick < 0.016f) return;
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
