using System.Collections.Generic;
using Framework;
using Framework.Res;
using Framework.UI;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class SelectChampionPanel : UIPanel {
        private int championId;
        private bool isSelecting = true;
        private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();
        private GameObject btnConfirm;
        private GameObject waitingTip;
        
        public override void OnAwake() {
            btnConfirm = Transform.GetGameObject("BtnConfirm");
            waitingTip = Transform.GetGameObject("WaitingTip");
            btnConfirm.GetComponent<Button>().onClick.AddListener(() => {
                if (!isSelecting) {
                    return;
                }
                NetworkUtils.Send(MessageDef.select_champion_c2s, new select_champion_c2s() {
                    ChampionId = championId,
                });
            });
            
            // TODO 获取所有 ChampionId 
            GameObject itemRoot = Transform.GetGameObject("Scroll View/Viewport/Content");
            GameObject itemPrefab = Transform.GetGameObject("Prefabs/Champion");
            for (int i = 1; i <= TempConfig.ChampionCount; i++) {
                GameObject item = GameObject.Instantiate(itemPrefab, itemRoot.transform);
                int championId = i;
                item.GetComponent<Button>("Icon").onClick.AddListener(() => {
                    Select(championId);
                });
                item.GetComponent<Image>("Icon").sprite = ResUtils.Load<Sprite>(Config.Champion[i].icon);
                items[i] = item;
            }

            championId = 1;
            GoUtils.SetGoActive(items[championId], "Select", true);
        }

        public override void OnShow(IUIParam param) {
            GoUtils.SetGoActive(btnConfirm, "", true);
            GoUtils.SetGoActive(waitingTip, "", false);
            
            EventMgr.Instance.Register<EventType.OnGameStart>(OnGameStart);
            NetworkUtils.RegisterMsgHandler<select_champion_s2c>(MessageDef.select_champion_s2c, select_champion_s2c);
        }

        public override void OnHide() {
            NetworkUtils.UnRegisterMsgHandler<select_champion_s2c>(MessageDef.select_champion_s2c, select_champion_s2c);
            EventMgr.Instance.UnRegister<EventType.OnGameStart>(OnGameStart);
            isSelecting = false;
        }

        private void OnGameStart() {
            Close();
        }

        private void select_champion_s2c(select_champion_s2c msg) {
            Select(msg.ChampionId);
            isSelecting = false;
            GoUtils.SetGoActive(btnConfirm, "", false);
            GoUtils.SetGoActive(waitingTip, "", true);
        }

        private void Select(int id) {
            if (!isSelecting) {
                return;
            }
            GoUtils.SetGoActive(items[championId], "Select", false);
            championId = id;
            GoUtils.SetGoActive(items[championId], "Select", true);
        }
    }
}