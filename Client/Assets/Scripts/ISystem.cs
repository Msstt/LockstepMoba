public interface ISystem {
    public void Start();
    public void FrameStart();
    
    public void Update();
    public void FrameUpdate();
}

public interface IFrameDriver {
    public bool FrameReady();
}
