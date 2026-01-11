// TODO: 缓存、堆栈

using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Framework.UI {
    public partial class UISystem : IUISystem {
        private Transform backCanvas;
        private Transform topCanvas;
        private Transform worldCanvas;
        private Camera mainCamera;
        
        Dictionary<UIDef, UIPanel> panels = new Dictionary<UIDef, UIPanel>();
        
        public void Init() {
            backCanvas = GameObject.Find("UIRoot/Back").transform;
            topCanvas = GameObject.Find("UIRoot/Top").transform;
            worldCanvas = GameObject.Find("UIRoot/World").transform;
            mainCamera = Camera.main;

            if (backCanvas == null || topCanvas == null || worldCanvas == null) {
                throw new UIException("UISystem canvas not found");
            }
        }

        public void ShowUI(UIDef def, IUIParam param = null) {
            UIPanel panel;
            if (!panels.TryGetValue(def, out panel)) {
                if (!UIConfig.config.TryGetValue(def, out UIConfig.Info config)) {
                    throw new UIException("UISystem panel config not found: " + def);
                }
                Transform parent;
                if (config.layer == UILayer.Back) {
                    parent = backCanvas;
                }
                else if (config.layer == UILayer.Top) {
                    parent = topCanvas;
                }
                else if (config.layer == UILayer.World) {
                    Log.Error("World Layer should use BindingUI: " + def);
                    return;
                } else {
                    throw new UIException("UISystem panel layer invalid: " + config.layer);
                }
                panel = CreatePanel(def, config, parent);
                if (panel == null) {
                    return;
                }
                ExecuteOnAwake(panel);
            }
            panels[def] = panel;
            ExecuteOnShow(panel, param);
        }
        
        public void CloseUI(UIDef def) {
            if (!panels.TryGetValue(def, out UIPanel panel)) {
                return;
            }
            ExecuteOnHide(panel);
            ExecuteOnDestroy(panel);
            GameObject.Destroy(panel.Transform.gameObject);
            panels.Remove(def);
        }

        private UIPanel CreatePanel(UIDef def, UIConfig.Info config, Transform parent) {
            GameObject panelGo = GoUtils.NewGo(config.prefab, parent);
            if (panelGo == null) {
                Log.Error("UISystem panel prefab not found: " + config.prefab);
                return null;
            }
            UIPanel panel = config.creator?.Invoke();
            if (panel == null) {
                Log.Error("UISystem panel creator failed: " + def);
                return null;
            }

            panel.Transform = panelGo.transform;
            panel.Def = def;
            panel.Layer = config.layer;

            return panel;
        }
        
        private void ExecuteOnAwake(UIPanel panel) {
            try {
                panel.OnAwake();
            } catch (Exception e) {
                Log.Error("UISystem panel OnAwake error: " + panel.Def + "\n" + e);
            }
        }
        
        private void ExecuteOnShow(UIPanel panel, IUIParam param) {
            try {
                panel.OnShow(param);
            } catch (Exception e) {
                Log.Error("UISystem panel OnShow error: " + panel.Def + "\n" + e);
            }
        }
        
        private void ExecuteOnHide(UIPanel panel) {
            try {
                panel.OnHide();
            } catch (Exception e) {
                Log.Error("UISystem panel OnHide error: " + panel.Def + "\n" + e);
            }
        }
        
        private void ExecuteOnDestroy(UIPanel panel) {
            try {
                panel.OnDestroy();
            } catch (Exception e) {
                Log.Error("UISystem panel OnDestroy error: " + panel.Def + "\n" + e);
            }
        }
    }
}