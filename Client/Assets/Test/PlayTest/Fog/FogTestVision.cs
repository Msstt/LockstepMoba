using System;
using UnityEngine;

public class FogTestVision : MonoBehaviour {
    private Action removeFogHandle;

    [Header("半径")]
    public FloatF radius;
    
    public void Update() {
        removeFogHandle?.Invoke();
        removeFogHandle = FogUtils.AddVision(transform.position.ToVector3F(), radius);
    }
}
