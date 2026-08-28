using Battle;
using Framework;
using Framework.Network;

namespace Network {
    public class LockStep : Singleton<LockStep> {
        public int Frame {
            get {
                if (isCollectingInput) {
                    return frame - 1;
                }
                return frame;
            }
        }
        private int frame = 0;
        public bool IsRunning { get; private set; } = false;
        
        private Dictionary<Uid, battle_input> inputs = new Dictionary<Uid, battle_input>();
        private List<Dictionary<Uid, battle_input>> historyInputs = new List<Dictionary<Uid, battle_input>>();
        
        private long lastFrameTime = 0;

        private float frameTime = 0;
        private float collectTime = 0;
        private float outTime = 0;
        private bool isCollectingInput = false;
        private int frameMaxDelay = 0;

        private bool hasDiffStatusCode = false;
        private Dictionary<int, int> statusCode = new Dictionary<int, int>();
        
        private void Clear() {
            frame = 0;
            inputs = new Dictionary<Uid, battle_input>();
            historyInputs = new List<Dictionary<Uid, battle_input>>();
            hasDiffStatusCode = false;
            statusCode = new Dictionary<int, int>();
        }
        
        public void Start() {
            Clear();
            IsRunning = true;

            frameTime = 1000f / Config.instance.Network.frame_per_second;
            collectTime = frameTime * Config.instance.Network.input_collect_window;
            outTime = Config.instance.Network.frame_timeout * 1000;
            frameMaxDelay = Config.instance.Network.frame_max_delay;
            lastFrameTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void Update() {
            if (!IsRunning) {
                return;
            }
            
            long nowTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (!isCollectingInput && nowTime - lastFrameTime >= frameTime) {
                NextFrame();
            }
            
            if (isCollectingInput) {
                List<Uid> uids = NetworkUtils.GetAllClientUid();
                if (inputs.Count >= uids.Count) {
                    FinishCollectInput();
                } else if (nowTime - lastFrameTime >= outTime) {
                    // 断线重连时可能会有玩家丢 start_frame_s2c,先这样处理一下
                    frame--;
                    NextFrame();
                }
            }
            // if (isCollectingInput && nowTime - lastFrameTime >= collectTime) {
            //     FinishCollectInput();
            // }
        }

        public void AddInput(int frame, Uid uid, battle_input input) {
            if (frame < this.frame - frameMaxDelay) {
                return;
            }

            if (!inputs.ContainsKey(uid)) {
                inputs.Add(uid, input);
            } else {
                inputs[uid] = Battle.InputMerge.Merge(inputs[uid], input);
            }
        }

        private void NextFrame() {
            frame++;
            lastFrameTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            // 广播收集玩家输入
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            foreach (var uid in uids) {
                if (!inputs.ContainsKey(uid)) {
                    NetworkUtils.Send(uid, MessageDef.frame_start_s2c, new frame_start_s2c {
                        Frame = frame,
                    });
                }
            }
            isCollectingInput = true;
        }

        private void FinishCollectInput() {
            isCollectingInput = false;
            
            // 广播收集到的指令
            historyInputs.Add(inputs);
            NetworkUtils.Broadcast(MessageDef.frame_input_s2c, GetInputMsg(frame));
            
            inputs = new Dictionary<Uid, battle_input>();
        }

        public frame_input_s2c GetInputMsg(int frame) {
            if (historyInputs.Count < frame) {
                return null;
            }
            
            frame_input_s2c msg = new frame_input_s2c() {
                Frame = frame,
            };
            foreach (var (uid, battleInput) in historyInputs[frame - 1]) {
                msg.Inputs.Add(new frame_input_s2c.Types.input_info {
                    Uid = uid,
                    Input = battleInput,
                });
            }

            return msg;
        }
        
        public void AddStatusCode(int frame, int code) {
            if (!statusCode.TryAdd(frame, code)) {
                if (statusCode[frame] != code && !hasDiffStatusCode) {
                    hasDiffStatusCode = true;
                    string logPath = FileLog.Instance.RecordInputData(Match.Instance.GetStartMsg(1), historyInputs);
                    Log.Error("Frame {0} has different status code: {1} vs {2}, log saved to {3}", frame, statusCode[frame], code, logPath);
                }
            }
        }
    }
}
