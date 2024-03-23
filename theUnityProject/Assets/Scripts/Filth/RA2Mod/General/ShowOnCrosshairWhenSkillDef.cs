using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.General.Components {
    public class ShowOnCrosshairWhenSkillDef : MonoBehaviour {
        [SerializeField]
        private GameObject[] objectsToHide;

        [SerializeField]
        private HudElement hudElement;

        [SerializeField]
        private SkillSlot skillSlot;

        [SerializeField]
        private List<SkillDef> includedSkillDefs;
        public List<SkillDef> IncludedSkillDefs { get => includedSkillDefs; set => includedSkillDefs = value; }

        private CharacterBody currentBody;
        private GenericSkill currentSkill;

        void LateUpdate() {
            if (currentBody != hudElement.targetCharacterBody) {
                SubscribeToSkill(false);
                currentBody = hudElement.targetCharacterBody;
                if (currentBody != null) {
                    currentSkill = currentBody.skillLocator.GetSkill(skillSlot);
                } else {
                    currentSkill = null;
                }
                SubscribeToSkill(true);
                CheckSkillChanged();
            }
        }

        private void SubscribeToSkill(bool shouldSubscribe) {
            if (currentSkill == null)
                return;

            if (shouldSubscribe) {
                currentSkill.onSkillChanged += LastSkill_onSkillChanged;
            } else {
                currentSkill.onSkillChanged -= LastSkill_onSkillChanged;
            }
        }

        private void LastSkill_onSkillChanged(GenericSkill obj) {
            CheckSkillChanged();
        }

        private void CheckSkillChanged() {
            for (int j = 0; j < objectsToHide.Length; j++) {
                objectsToHide[j].SetActive(ShouldShow());
            }
        }

        private bool ShouldShow() {
            for (int i = 0; i < includedSkillDefs.Count; i++) {
                if (currentSkill.skillDef == includedSkillDefs[i]) {
                    return true;
                }
            }
            return false;
        }
    }
}
