using Combat.Fog;
using UnityEngine;

public class FogTestVision : MonoBehaviour {
    private IVisionHandle handle;

    [Header("半径")]
    public FloatF radius;
    
    public void Update() {
        if (handle == null) {
            handle = FogUtils.AddVision(VisionType.Self, transform.position.ToVector3F(), radius);
        }
        handle.UpdatePos(transform.position.ToVector3F());
    }
    
    public void OnDestroy() {
        handle.Dispose();
    }
}
