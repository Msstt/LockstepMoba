using UI;
using UnityEngine;

namespace Framework.UI {
    public interface IUISystem : ISystem, IInitSystem, IUpdateSystem {
        public void ShowUI(UIDef def, IUIParam param = null);
        public void CloseUI(UIDef def);

        public void BindingUI(UIDef def, Transform transform, IUIParam param = null);
        public void UnBindingUI(UIDef def, Transform transform);
        
        public Camera UICamera { get; }
    }
}