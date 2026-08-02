using System;
using System.Collections.Generic;

namespace Combat.BehaviourMachine {
    public static class Define {
        public static Dictionary<int, Action<Machine>> create = new Dictionary<int, Action<Machine>>() {
            { 1, machine => {
                machine.AddBehaviour(new AttackBehaviour(machine, Config.Minion[1].attackDistance));
            } },
        };
    }
}