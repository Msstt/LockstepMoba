using System;
using System.Collections.Generic;

namespace Combat.BehaviourMachine {
    public class Machine {
        // 行为机掌控的 actor
        public int Uid { get; private set; }
        private Behaviour curBehaviour;
        private List<Behaviour> behaviours = new List<Behaviour>();
        private List<Func<Behaviour, bool>> customEvaluateFunc = new List<Func<Behaviour, bool>>();
        
        public Behaviour CurBehaviour => curBehaviour;

        public Machine(int Uid) {
            this.Uid = Uid;
        }

        // Add 顺序等于优先级
        public void AddBehaviour(Behaviour behaviour) {
            if (behaviours.Contains(behaviour)) {
                return;
            }
            behaviours.Add(behaviour);
        }

        public void AddCustomEvaluateFunc(Func<Behaviour, bool> func) {
            customEvaluateFunc.Add(func);
        }
        
        public void Update(int frame) {
            Behaviour nextBehaviour = null;
            foreach (var behaviour in behaviours) {
                bool noPass = false;
                foreach (var func in customEvaluateFunc) {
                    if (!func(behaviour)) {
                        noPass = true;
                    }
                }
                if (noPass) {
                    continue;
                }
#if ENABLE_PROFILER
                Type behaviourType = behaviour.GetType();
                Framework.Profiler.Instance.BeginBehaviourEvaluate(behaviourType);
                bool evaluateResult;
                try {
                    evaluateResult = behaviour.Evaluate();
                } finally {
                    Framework.Profiler.Instance.EndBehaviourEvaluate(behaviourType);
                }
#else
                bool evaluateResult = behaviour.Evaluate();
#endif
                if (!evaluateResult) {
                    continue;
                }
                nextBehaviour = behaviour;
                break;
            }

            if (nextBehaviour != curBehaviour) {
                curBehaviour?.OnAbort();
                nextBehaviour?.OnStart();
                curBehaviour = nextBehaviour;
            }
            if (curBehaviour != null) {
#if ENABLE_PROFILER
                Type behaviourType = curBehaviour.GetType();
                Framework.Profiler.Instance.BeginBehaviourExecute(behaviourType);
                try {
                    curBehaviour.Execute(frame);
                } finally {
                    Framework.Profiler.Instance.EndBehaviourExecute(behaviourType);
                }
#else
                curBehaviour.Execute(frame);
#endif
            }
        }

        public void Abort() {
            curBehaviour?.OnAbort();
            curBehaviour = null;
        }
    }
}
