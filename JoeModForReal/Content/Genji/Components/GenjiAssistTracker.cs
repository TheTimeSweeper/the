using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {

    //credit in part to moffein for riskymod's assist manager
    public class GenjiAssistTracker : MonoBehaviour, IOnDamageDealtServerReceiver, IOnKilledOtherServerReceiver {

        public static List<GenjiAssistTracker> AllTrackers = new List<GenjiAssistTracker>();

        public static void CheckAllTrackersForDeath(CharacterBody body) {
            for (int i = 0; i < AllTrackers.Count; i++) {
                AllTrackers[i].CheckDeath(body);
            }
        }

        public SkillLocator skillLocator;

        private List<CharacterBody> _trackedBodies = new List<CharacterBody>();

        public void Awake() {
            skillLocator = GetComponent<SkillLocator>();
            AllTrackers.Add(this);
        }
        public void OnDestroy() {
            AllTrackers.Remove(this);
        }

        public void OnDamageDealtServer(DamageReport damageReport) {
            _trackedBodies.Add(damageReport.victimBody);
        }

        public void OnKilledOtherServer(DamageReport damageReport) {
            CheckDeath(damageReport.victimBody);
        }

        private void CheckDeath(CharacterBody body) {
            if (_trackedBodies.Contains(body)) {
                _trackedBodies.Remove(body);

                for (int i = 0; i < skillLocator.allSkills.Length; i++) {
                    GenericSkill genericSkill = skillLocator.allSkills[i];
                    //todo i thought this was gonna be done in the skilldef
                    if (genericSkill.skillDef is AssistResetSkillDef) {
                        //todo definitely not networked
                        skillLocator.allSkills[i].Reset();
                    }
                }
            }
        }
    }
}