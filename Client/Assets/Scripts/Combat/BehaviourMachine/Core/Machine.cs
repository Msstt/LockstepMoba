using System;
using System.Collections.Generic;

namespace Combat.BehaviourMachine {
    public class Machine {
        private Behaviour curBehaviour;
        private List<Behaviour> behaviours = new List<Behaviour>();
        private List<Func<Behaviour, bool>> customEvaluateFunc = new List<Func<Behaviour, bool>>();
        
        public Behaviour CurBehaviour => curBehaviour;

        // Add 顺序等于优先级
        public void AddBehaviour<T>() where T : Behaviour, new() {
            behaviours.Add(new T());
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