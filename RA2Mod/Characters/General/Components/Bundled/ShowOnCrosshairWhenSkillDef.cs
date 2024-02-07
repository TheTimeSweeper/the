using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.General.Components
{
    public class ShowOnCrosshairWhenSkillDef : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] objectsToHide;

        [SerializeField]
        private HudElement hudElement;

        [SerializeField]
        private SkillSlot skillSlot;

        [SerializeField]
        private List<SkillDef> includedSkillDefs;
        public List<SkillDef> IncludedSkillDefs { get => includedSkillDefs; set => includedSkillDefs = value; }

        private CharacterBody _currentBody;
        private GenericSkill _currentSkill;

        private bool _subscribedToEvent;

        void LateUpdate()
        {
            if (_currentBody != hudElement.targetCharacterBody)
            {
                _currentBody = hudElement.targetCharacterBody;
                SubscribeToEvent(false);
                if (_currentBody != null)
                {
                    _currentSkill = _currentBody.skillLocator.GetSkill(skillSlot);
                } 
                else
                {
                    _currentSkill = null;
                }
                SubscribeToEvent(true);
            }
        }

        void OnEnable()
        {
            SubscribeToEvent(true);
        }

        void OnDisable()
        {
            SubscribeToEvent(false);
        }

        private void SubscribeToEvent(bool shouldSubscribe)
        {
            if (_subscribedToEvent == shouldSubscribe)
                return;
            _subscribedToEvent = shouldSubscribe;

            if (_currentSkill == null)
                return;

            if (shouldSubscribe)
            {
                _currentSkill.onSkillChanged += LastSkill_onSkillChanged;
                CheckSkillChanged();
            }
            else
            {
                _currentSkill.onSkillChanged -= LastSkill_onSkillChanged;
            }
        }

        private void LastSkill_onSkillChanged(GenericSkill obj)
        {
            CheckSkillChanged();
        }

        private void CheckSkillChanged()
        {
            for (int j = 0; j < objectsToHide.Length; j++)
            {
                objectsToHide[j].SetActive(ShouldShow());
            }
        }

        private bool ShouldShow()
        {
            if (_currentSkill == null)
                return false;

            for (int i = 0; i < includedSkillDefs.Count; i++)
            {
                if (_currentSkill.skillDef == includedSkillDefs[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}