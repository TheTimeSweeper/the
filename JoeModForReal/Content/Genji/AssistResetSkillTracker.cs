using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {
    public class AssistResetSkillTracker : MonoBehaviour {

        #region static
        public static List<AssistResetSkillTracker> AllTrackers = new List<AssistResetSkillTracker>();

        public static void CheckAllTrackersForDeath(GameObject body) {
            for (int i = 0; i < AllTrackers.Count; i++) {
                AllTrackers[i].CheckDeath(body);
            }
        }
        #endregion

        public SkillLocator skillLocator;
        private List<GameObject> _trackedBodies = new List<GameObject>();

        void Awake() {
            AllTrackers.Add(this);
        }
        void OnDestroy() {
            AllTrackers.Remove(this);
        }

        public void addTrackedBody(GameObject body) {
            _trackedBodies.Add(body);
        }

        private void CheckDeath(GameObject body) {
            if (_trackedBodies.Contains(body)) {
                _trackedBodies.Remove(body);

                for (int i = 0; i < skillLocator.allSkills.Length; i++) {
                    GenericSkill genericSkill = skillLocator.allSkills[i];
                    if (genericSkill.skillDef is AssistResetSkillDef) {
                        //todo probably not networked
                        //todo i thought this was gonna be done in the skilldef
                        skillLocator.allSkills[i].RestockSteplike();
                    }
                }
            }
        }
    }
}