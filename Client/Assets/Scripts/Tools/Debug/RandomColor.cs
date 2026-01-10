using UnityEngine;

public class RandomColor : MonoBehaviour {
    private void Start() {
        GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
    
}
