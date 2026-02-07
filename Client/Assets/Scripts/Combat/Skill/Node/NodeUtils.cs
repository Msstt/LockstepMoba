using Combat.Actor;

namespace Combat.Skill {
    public abstract partial class Node {
        protected static T GetCom<T>(Context context) where T : Com => ActorUtils.GetCom<T>(context.ActorUid);
        protected static Stats GetStats(Context context) => ActorUtils.GetActor(context.ActorUid)?.Stats;
        
        protected static T GetLevelNumber<T>(Context context, LevelNumber<T> levelNumber) => levelNumber[context.Level];
    }
}