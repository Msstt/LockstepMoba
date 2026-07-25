using System;
using Combat;
using Combat.Actor;
using Combat.Skill;
using Framework.UI;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main {
    public class SkillPanel : UIPanel {
        private GameObject skillRoot;
        private GameObject skill;
        private GameObject dot;
        private SkillInfo[] config;
        private level_input proto = new level_input();
        
        public override void OnAwake() {
            skillRoot = Transform.GetGameObject("SkillRoot");
            skill = Transform.GetGameObject("Prefabs/Skill");
            dot = Transform.GetGameObject("Prefabs/Dot");
            
            NetworkUtils.RegisterCollector(MessageDef.level_input, () => {
                level_input result = proto;
                proto = new level_input();
                return result;
            });
        }
        
        public override void OnShow(IUIParam uiParam) {
            config = Config.Champion[CombatUtils.GetChampionId()].skill;
            
            skillRoot.DestroyAllChildren();
            for (int i = (int)SkillSlot.SkillQ; i <= (int)SkillSlot.SkillR; i++) {
                var node = GameObject.Instantiate(skill.transform, skillRoot.transform);

                int slot = i;
                node.GetComponent<Button>("LevelUp/Icon").onClick.AddListener(() => {
                    proto.LevelUp.Add(new skill_level_up_info {
                        Slot = slot,
                    });
                });
            }
            RefreshLevel();
            EventUtils.Register<EventType.ChampionLevelUp>(OnLevelUp);
            EventUtils.Register<EventType.ChampionSkillLevelUp>(OnLevelUp);
        }

        public override void OnHide() {
            EventUtils.UnRegister<EventType.ChampionLevelUp>(OnLevelUp);
            EventUtils.UnRegister<EventType.ChampionSkillLevelUp>(OnLevelUp);
        }

        private void ForEach(Action<SkillSlot, GameObject> func) {
            for (int i = (int)SkillSlot.SkillQ; i <= (int)SkillSlot.SkillR; i++) {
                func((SkillSlot)i, skillRoot.transform.GetChild(i - (int)SkillSlot.SkillQ).gameObject);
            }
        }
        
        private void RefreshLevel() {
            SkillCom com = ActorUtils.GetCom<SkillCom>();
            if (com == null) {
                return;
            }
            bool CanLevelUp = com.SkillCanLevelUp();
            ForEach((slot, node) => {
                GoUtils.SetGoActive(node, "LevelUp", CanLevelUp);
                GoUtils.SetGoActive(node, "LevelUp/Icon/Mask", !com.SkillCanLevelUp(slot));

                int level = com.GetSkillLevel(slot);
                UIUtils.InitChildCount(node.GetGameObject("DotList"), dot, config[(int)slot].maxLevel, (i, trans) => {
                    GoUtils.SetGoActive(trans, "Active", i + 1 <= level);
                });
            });
        }

        private void OnLevelUp(EventType.ChampionSkillLevelUp param) {
            if (param.Uid == CombatUtils.SelfUid) {
                RefreshLevel();
            }
        }
        
        private void OnLevelUp(EventType.ChampionLevelUp param) {
            if (param.Uid == CombatUtils.SelfUid) {
                RefreshLevel();
            }
        }
    }
}