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
                if (!behaviour.Evaluate()) {
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
            curBehaviour?.Execute(frame);
        }

        public void Abort() {
            curBehaviour?.OnAbort();
            curBehaviour = null;
        }
    }
}