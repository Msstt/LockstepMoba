using UnityEngine;

namespace Framework.UI {
    public partial class UISystem : IUISystem {
        private Camera camera;

        public Camera UICamera {
            get {
                if (camera == null) {
                    camera = GameObject.Find("UIRoot/UICamera")?.GetComponent<Camera>();
                    if (camera == null) {
                        throw new UIException("UICamera not found");
                    }
                }

                return camera;
            }
        }

        public void Update() {
            UICamera.transform.position = mainCamera.transform.position;
            UICamera.transform.rotation = mainCamera.transform.rotation;
        }
    }
}