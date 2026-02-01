using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Debug {
    public class FPSDrawer : MonoBehaviour {
        private readonly float UPDATE_INTERVAL = 1f;
        private readonly float RECORD_INTERVAL = 10f;
        
        private LinkedList<Tuple<float, int>> record = new LinkedList<Tuple<float, int>>();

        private Text text;

        public void Awake() {
            text = GetComponent<Text>();
        }

        public void Update() {
            record.AddLast(Tuple.Create(Time.realtimeSinceStartup, GameMgr.Instance.Frame));
            while (record.Any() && record.First.Value.Item1 < Time.realtimeSinceStartup - RECORD_INTERVAL) {
                record.RemoveFirst();
            }
            if (record.Any() && record.Last.Value.Item1 - record.First.Value.Item1 > 0) {
                var First = record.First.Value;
                var Last = record.Last.Value;
                text.text = "FPS: " + Mathf.RoundToInt((Last.Item2 - First.Item2) / (Last.Item1 - First.Item1));
            } else {
                text.text = "FPS: 0";
            }
        }
    }
}