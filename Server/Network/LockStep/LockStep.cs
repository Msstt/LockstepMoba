using Framework;
using Framework.Network;

namespace Network {
    public class LockStep : Singleton<LockStep> {
        public int Frame { get; private set; }
        public bool IsRunning { get; private set; } = false;
        
        private Dictionary<Uid, battle_input> inputs = new Dictionary<Uid, battle_input>();
        private List<Dictionary<Uid, battle_input>> historyInputs = new List<Dictionary<Uid, battle_input>>();
        
        private long lastFrameTime = 0;

        private float frameTime = 0;
        private float collectTime = 0;
        private bool isCollectingInput = false;
        private int frameMaxDelay = 0;
        
        private void Clear() {
            Frame = 0;
            inputs = new Dictionary<Uid, battle_input>();
            historyInputs = new List<Dictionary<Uid, battle_input>>();
        }
        
        public void Start() {
            Clear();
            IsRunning = true;

            frameTime = 1000f / Config.instance.Network.frame_per_second;
            collectTime = frameTime * Config.instance.Network.input_collect_window;
            frameMaxDelay = Config.instance.Network.frame_max_delay;
            lastFrameTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void Update() {
            if (!IsRunning) {
                return;
            }
            
            long nowTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (nowTime - lastFrameTime >= frameTime) {
                NextFrame();
            }
            if (isCollectingInput && nowTime - lastFrameTime >= collectTime) {
                FinishCollectInput();
            }
        }

        public void AddInput(int frame, Uid uid, battle_input input) {
            if (frame < Frame - frameMaxDelay) {
                return;
            }

            if (!inputs.ContainsKey(uid)) {
                inputs.Add(uid, input);
            } else {
                inputs[uid] = Battle.InputMerge.Merge(inputs[uid], input);
            }
        }

        private void NextFrame() {
            Frame++;
            lastFrameTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            // 广播收集玩家输入
            NetworkUtils.Broadcast(MessageDef.frame_start_s2c, new frame_start_s2c() {
                Frame = Frame,
            });
            isCollectingInput = true;
        }

        private void FinishCollectInput() {
            isCollectingInput = false;
            
            // 广播收集到的指令
            frame_input_s2c msg = new frame_input_s2c() {
                Frame = Frame,
            };
            foreach (var (uid, battleInput) in inputs) {
                msg.Inputs.Add(new frame_input_s2c.Types.input_info() {
                    Uid = uid,
                    Input = battleInput,
                });
            }
            NetworkUtils.Broadcast(MessageDef.frame_input_s2c, msg);
            
            historyInputs.Add(inputs);
            inputs = new Dictionary<Uid, battle_input>();
        }
    }
}
