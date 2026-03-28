using Combat.Actor;

public static class LevelUtils {
    private static LevelCom GetCom(int uid) {
        return ActorUtils.GetActor(uid)?.GetComponent<LevelCom>();
    }
    
    public static void AddExp(int uid, int exp) {
        LevelCom com = GetCom(uid);
        if (com == null) {
        } else {
            
        }
    }
}
