using Combat;
using UnityEngine.UI;

namespace UnityEngine {
    public class ActorRaycasterCom : MonoBehaviour {
        private int? uid;
        private static bool hasRay = false;
        private cakeslice.Outline lastHighlight;

        public int Uid {
            get => uid ?? -1;
            set => uid ??= value;
        }

        public void Update() {
            if (hasRay) {
                return;
            }
            hasRay = true;

            HighlightActor();
          
        }
        
        public void LateUpdate() {
            hasRay = false;
        }

        private void HighlightActor() {
            if (lastHighlight != null) {
                lastHighlight.eraseRenderer = true;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Actor"))) {
                return;
            }
            int uid = hitInfo.collider?.transform.GetComponent<ActorRaycasterCom>()?.Uid ?? -1;
            if (ActorUtils.IsSameCamp(uid)) {
                return;
            }
            lastHighlight = hitInfo.collider?.transform.parent.Find("Meshes")?.EnsureComponent<cakeslice.Outline>();
            if (lastHighlight != null) {
                lastHighlight.eraseRenderer = false;
            }
        }
    }
}