# Repository Guidelines

## 项目总览

这是一个 MOBA/类 MOBA 战斗原型仓库，核心由 Unity 客户端、.NET 服务端和 protobuf 协议组成。主 Unity 工程在 `Client/`，Unity 版本为 `2022.3.41f1`。服务端在 `Server/`，目标框架是 `net9.0`。协议定义在 `Proto/Define/`，通过 Unity 编辑器工具生成到客户端和服务端。`Client_Temp/` 是 `工具/Unity多开` 创建的临时多开目录，不作为主工程修改目标。

## 协作偏好

用户是 Unity 客户端开发者，主要使用 C#。无论用户用什么语言或方式提问，后续回答都必须使用中文。解释代码时优先站在 Unity 客户端开发视角，关注客户端运行时、编辑器工具、资源配置、C# 类型和 Unity 生命周期；涉及服务端或协议时，说明其与客户端改动的关系即可。

## 目录地图

- `Client/Assets/Scripts/`: 客户端运行时代码。
- `Client/Assets/Scripts/Combat/`: 战斗域，包括 Actor、技能、Buff、Area、Fog、等级、生成系统。
- `Client/Assets/Scripts/Framework/`: 通用框架，包括网络、事件、UI、定点数、集合、资源和工具。
- `Client/Assets/Scripts/Network/`: 客户端协议枚举、消息分发、lockstep 输入收集/派发。
- `Client/Assets/Scripts/Input/`: 输入命令系统，把鼠标/键盘输入转换为 `skill_input`。
- `Client/Assets/Scripts/Navmesh/`: 导航网格、寻路、射线检测、单位范围查询、高度图。
- `Client/Assets/Scripts/UI/`: 具体 UI 面板和 `UIDef` 注册。
- `Client/Assets/Resources/Config/`: 运行时配置资源，英雄为 `.asset`，Skill/Buff/Area 为导出的 JSON。
- `Client/Assets/Editor/`: 编辑器工具、技能/Area/Buff 图编辑器、协议生成器、Navmesh/Fog 生成工具。
- `Client/Assets/Test/`: Unity Test Framework 测试，分为 `EditTest` 和 `PlayTest`。
- `Server/`: lockstep relay 服务端、网络框架、Match、配置和命令循环。
- `Proto/`: `.proto` 文件、`proto_msg_map.json`、本地 `protoc`。

## 客户端生命周期

入口是 `Client/Assets/Scripts/StartUp/Main.cs`。`Awake()` 获取 `GMTool` 并调用 `GameMgr.Instance.RegisterSystem()`，`Start()` 调用 `GameMgr.Init()`，本地调试模式会走 `StartLocalDebug()`。`Update()` 每帧调用 `GameMgr.Update()`，退出时调用 `GameMgr.Quit()`。

`GameMgr` 是系统容器和生命周期调度器。系统注册顺序也是更新顺序：`IDataSystem`、`IInputSystem`、`INetwork`、`ILockStep`、`INavmesh`、`ICombatSystem`、`IActorSystem`、`ISkillSystem`、`IAreaSystem`、`ISpawnSystem`、`ILevelSystem`、`IFogSystem`、`IUISystem`。通过 `GameMgr.Instance.GetSystem<T>()` 获取系统。帧同步逻辑走 `IFrameDriver.FrameReady()` 后调用各系统 `FrameUpdate(frame)`，表现层逻辑走普通 `Update()`。

## 常用客户端 API

- `CombatUtils`: 访问自身 uid、英雄 id、阵营等战斗基础信息。
- `ActorUtils`: 获取 Actor、组件、持久组件，判断阵营。
- `SkillUtils`: 技能槽数量等技能工具。
- `AreaUtils`: 创建/销毁 Area，访问 Area 根节点。
- `FogUtils`: 添加视野、判断位置或 Actor 是否可见。
- `NavmeshUtils`: 寻路、可达性、表面射线、单位注册和圆形范围查询。
- `NetworkUtils`: 发送普通消息，注册 lockstep collector/handler，校验消息类型。
- `UIUtils`: `ShowUI`、`CloseUI`、`BindingUI`、`UnBindingUI`。
- `DataUtils`: 访问运行时 `IData` 容器。
- `EventUtils`: 注册、注销、发送全局事件。

同步逻辑优先使用 `FloatF` 和 `Vector3F`，不要在会影响战斗结果的代码里直接使用 `float`/`Vector3`。遍历时可能增删的集合优先用 `SafeDictionary`、`SafeList`。

## 战斗系统结构

`ActorSystem` 管理 Actor 创建、删除、组件更新和持久组件。Actor 是非 MonoBehaviour 的领域对象，绑定 Unity `GameObject`，通过 `Com` 组件扩展能力。常见组件包括 `MoveCom`、`SkillCom`、`BuffCom`、`LevelCom`、`EquipmentCom`、`NormalUICom`、`VisionCom`。`PersistentCom` 会登记到 `ActorSystem`，用于死亡后仍保留的状态。

技能由 `SkillCom` 发起，实际执行由 `ISkillSystem` 管理。技能配置为 `SkillConfig`，节点配置为 `NodeConfig`，运行时构造 `Tree` 并逐帧执行 `Node`。新增技能节点通常需要同时修改：

- 运行时节点：`Client/Assets/Scripts/Combat/Skill/Node/...`
- `NodeType` 与 `NodeFactory`: `Client/Assets/Scripts/Combat/Skill/NodeDefine.cs`
- 编辑器节点：`Client/Assets/Editor/Skill/Node/...`
- 技能图/JSON：通过 `工具/技能编辑器` 导出到 `Client/Assets/Resources/Config/Skill/Json/`

Buff 由 `BuffCom` 挂载，`BuffConfig` 创建 `Combat.Buff.Buff`，效果通过 `Combat.Buff.EffectFactory` 分发。新增 Buff 效果放到 `Client/Assets/Scripts/Combat/Buff/Effect/`，然后运行 `工具/技能/生成 Buff 配置文件` 更新 `EffectDefine.cs`。Area 类似，核心在 `Combat/Area/`，新增 Area 效果后运行 `工具/技能/生成 Area 配置文件`。

## 网络与协议

底层协议包格式为 `int32 payloadLength + int32 msgId + protobuf payload`。客户端底层网络在 `Framework.Network.Network`，负责连接、读写线程、普通消息分发；服务端对应实现是 `Server/Framework/Network/Network.cs` 和 `Client.cs`。

lockstep 消息流：

1. 服务端 `Match` 达到 `auto_start_count` 后广播 `battle_start_s2c`。
2. 客户端 `BattleMsgDispatcher` 写入 `ICombatSystem.SetStartInfo()` 并启动 `GameMgr.Start()`。
3. 服务端每帧发 `frame_start_s2c` 请求输入。
4. 客户端 `LockStep.GetInputMsg()` 收集 `battle_input`，发送 `frame_input_c2s`。
5. 服务端合并输入并广播 `frame_input_s2c`。
6. 客户端 `LockStep.PushInputMsg()` 后，`FrameReady()` 推进本地帧并派发输入给玩法系统。

协议源文件只改 `Proto/Define/*.proto` 和 `Proto/proto_msg_map.json`。生成物位于 `Client/Assets/Scripts/Network/Message/` 和 `Server/Network/Message/`，不要手改。使用 Unity 菜单 `工具/网络/生成协议代码` 重新生成 protobuf C#、`MessageDef`、`MessageMapping` 和 parser 表。新增 lockstep 输入时，还要维护 `NetworkDef.InputMsgDef`、客户端 collector/handler、服务端 `InputMerge`。

## 服务端结构

服务端入口是 `Server/Start.cs`：解析 `Server/Config/Config/network.json`，启动网络，后台读取命令，然后循环 `NetworkUtils.Update()` 与 `AsyncUtils.Update()`。默认配置端口 `9980`、30 FPS、自动开始人数 `2`。

`Server/Network/NetworkUtils.cs` 注册 dispatcher、监听玩家连接并启动 TCP 服务。`Server/Battle/Match.cs` 负责玩家加入和开局信息。`Server/Network/LockStep/LockStep.cs` 负责按帧收集输入、历史帧缓存和断线重连补帧。服务端新增普通消息处理器放在 `Server/Network/Dispatcher/`，通过 `NetworkDef.RegisterDispatcher()` 注册。

## 配置与资源

客户端配置入口是 `Client/Assets/Scripts/Config/Define.cs`。`Config.Champion`、`Config.Equipment` 读取 Resources 下的 ScriptableObject；`Config.Skill`、`Config.Buff`、`Config.Area` 读取 JSON；`Config.Time`、`Config.Map`、`Config.Exp`、`Config.Vision` 读取 `Config/Other` 下的全局资源。资源路径必须与 `Resources.Load` 路径一致。

## 编辑器工具

常用 Unity 菜单：

- `工具/技能编辑器`: 编辑并导出技能、Buff、Area 图配置。
- `工具/网络/生成协议代码`: 同步生成客户端/服务端协议代码。
- `工具/Navmesh/导出 Navmesh 网格数据`: 导出寻路表面 JSON。
- `工具/Navmesh/生成高度图`: 生成高度图。
- `工具/战争迷雾/生成视野遮罩图`: 从 Navmesh 与 `VisionBlocker` 层生成遮罩。
- `工具/战争迷雾/替换地图材质`、`同步材质属性`: Fog 材质批处理。
- `工具/Unity多开`: 创建 `Client_Temp/` 多开目录。

## 构建、运行与测试

- `dotnet restore Server/Server.sln`: 恢复服务端依赖。
- `dotnet build Server/Server.sln`: 构建服务端。
- `dotnet run --project Server/Server.csproj`: 启动服务端。
- Unity Editor: 打开 `Client/`，版本使用 `2022.3.41f1`。
- EditMode 测试：`Unity -batchmode -projectPath Client -runTests -testPlatform EditMode -quit`。
- PlayMode 测试：`Unity -batchmode -projectPath Client -runTests -testPlatform PlayMode -quit`。

现有测试覆盖 Fixed、集合、Geo、Fog、Navmesh、Network/Lockstep 等。纯逻辑测试放 `Client/Assets/Test/EditTest/`，依赖场景或 Unity 播放环境的测试放 `Client/Assets/Test/PlayTest/`。

## 编码与提交约定

C# 使用四空格缩进、同一行左大括号、PascalCase 类型/方法、camelCase 私有字段和局部变量、接口以 `I` 开头。保留 Unity `.meta` 文件。不要提交 `Client_Temp/`、`Client/Library/`、`Server/bin/`、`Server/obj/`、`Server/Log/`、`.DS_Store` 或 IDE 本地状态。

近期提交使用中文方括号范围，例如 `[战争迷雾]单位可见性`、`[寻路]缓存高度图`、`[技能]技能等级`。提交应聚焦单一变更；PR 说明需包含影响范围、测试结果、协议/配置兼容性说明，UI 或场景表现变化需附截图或录屏。

## 后续代理工作注意事项

优先读自有代码，避免把 `Client/Assets/Plugins/`、生成的 protobuf 文件、Unity 缓存目录当作修改目标。修改战斗同步逻辑时，确认是否需要服务端、协议、输入合并、客户端 handler 同步变更。修改配置驱动能力时，同时检查运行时工厂、编辑器节点/生成器和 Resources 输出路径。运行 Unity 生成工具后，检查生成物是否同时覆盖客户端和服务端。
