using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Debug {
    public class FPSDrawer : MonoBehaviour {
        private readonly float UPDATE_INTERVAL = 1f;

        private float lastTime = 0f;
        private int lastFrame = 0;

        private Text text;

        public void Awake() {
            text = GetComponent<Text>();
        }

        public void Update() {
            if (Time.realtimeSinceStartup - lastTime > UPDATE_INTERVAL) {
                text.text = "FPS: " + Mathf.RoundToInt((GameMgr.Instance.Frame - lastFrame) / (Time.realtimeSinceStartup - lastTime));
                lastTime = Time.realtimeSinceStartup;
                lastFrame = GameMgr.Instance.Frame;
            }
        }
    }
}