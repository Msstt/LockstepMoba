public interface ISystem {
}

public interface IInitSystem : ISystem {
    public void Init();
}

public interface IStartSystem : ISystem {
    public void Start();
}

public interface IUpdateSystem : ISystem {
    public void Update();
}

public interface IFrameUpdateSystem : ISystem {
    public void FrameUpdate(int frame);
}

public interface IFrameDriver : ISystem {
    public int Frame { get; }
    
    public bool FrameReady();
}
