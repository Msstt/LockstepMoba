using Framework;
using Network;

public class GameMgr : Singleton<GameMgr> {
    public void Start() {
        NetworkUtils.Start();
    }
    
    // public void Update() {
    //     NetworkUtils.Update();
    // }
}
