using System;
using System.Collections.Generic;
using Combat.Actor;

namespace Combat.BehaviourMachine {
    public static class Define {
        public static Dictionary<int, Action<Machine>> createFunc = new Dictionary<int, Action<Machine>>() {
            { 1001, machine => {
                NormalMinionCreateFunc(machine, 1001);
            } },
            
            { 1101, machine => {
                machine.AddBehaviour(new RangedAttackBehaviour(machine, Config.Turret[1101].attackWindupRatio));
            } },
        };
        
        private static void NormalMinionCreateFunc(Machine machine, int actorId) {
            machine.AddBehaviour(new MeleeAttackBehaviour(machine, Config.Minion[actorId].attackWindupRatio));
            machine.AddBehaviour(new ChaseBehaviour(machine, Config.Minion[actorId].patrolDistance, Config.Minion[actorId].chaseDistance));
            
            if (ActorUtils.GetActor(machine.Uid) is Minion minion) {
                machine.AddBehaviour(new MinionWaveBehaviour(machine, minion.WaveIndex));
            }
        }
    }
}