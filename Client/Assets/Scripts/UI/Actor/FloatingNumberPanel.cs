using Combat.Actor;
using Framework;
using Framework.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace UI.Actor {
    public struct FloatingNumberPanelParam : IUIParam {
        public Combat.Actor.Actor actor;
    }
    
    public class FloatingNumberPanel : UIPanel {
        private GameObjectPool numberPool;
        private Transform[] numberPos;

        private Combat.Actor.Actor actor;
        
        public override void OnAwake() {
            GameObject number = Transform.GetGameObject("Prefab/Number");
            numberPool = new GameObjectPool(number, Transform.gameObject);
            
            Transform pos = Transform.GetComponent<Transform>("Pos");
            numberPos = new Transform[pos.childCount];
            for (int i = 0; i < numberPos.Length; i++) {
                numberPos[i] = pos.GetChild(i);
            }
        }

        public override void OnShow(IUIParam uiParam) {
            if (uiParam is not FloatingNumberPanelParam param || param.actor == null) {
                return;
            }

            actor = param.actor;
            actor.Event.OnHit.Register(OnHit);
        }

        public override void OnHide() {
            actor.Event.OnHit.UnRegister(OnHit);
        }

        public override void OnDestroy() {
            numberPool.Dispose();
        }

        private void OnHit(Damage damage) {
            if (damage.physical > 0) {
                Floating(damage.physical, Color.red);
            }
            if (damage.magic > 0) {
                Floating(damage.magic, Color.blue);
            }
            if (damage.@true > 0) {
                Floating(damage.@true, Color.grey);
            }
        }

        private void Floating(FloatF number, Color color) => Floating(FloatF.FloorInt(number), color);

        private int index = 0;
        private void Floating(int number, Color color) {
            color.a = 0;
            GameObject go = numberPool.Get();
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
            text.color = color;
            text.text = number.ToString();
            text.transform.localScale = Vector3.one;
            
            Transform pos = numberPos[index];
            index = (index + 1) % numberPos.Length;
            text.transform.position = pos.position;

            Sequence seq = DOTween.Sequence();

            seq.Append(text.transform.DOScale(1.5f, 0.2f))
                .Join(text.DOFade(1f, 0.2f))
                .Append(text.transform.DOScale(0f, 0.2f))
                .Join(text.DOFade(0f, 0.2f))
                .OnComplete(() => {
                    numberPool.Recycle(go);
                });
        }
    }
}