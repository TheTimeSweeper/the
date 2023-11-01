using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {
    //handles tracking assists and handles building up ultimate
    //digusting violation of SOC but to avoid a getcomponent in takedamage
    public class GenjiDamageTracker : MonoBehaviour {

        public SkillLocator skillLocator;

        public AssistTracker assistTracker;
        public UltimateBuildup ultimateBuildup;
        public List<GenjiSubComponent> subcomponents = new List<GenjiSubComponent>();

        void Awake() {

            assistTracker = new AssistTracker();
            subcomponents.Add(assistTracker);
            ultimateBuildup = new UltimateBuildup();
            subcomponents.Add(ultimateBuildup);

            for (int i = 0; i < subcomponents.Count; i++) {
                subcomponents[i].Awake(this);
            }
        }

        void OnDestroy() {

            for (int i = 0; i < subcomponents.Count; i++) {
                subcomponents[i].OnDestroy(this);
            }
        }

        //all this hullaballoo to avoid one getcomponent
        #region SubComponents
        public abstract class GenjiSubComponent {
            public virtual void Awake(GenjiDamageTracker tracker) { }
            public virtual void OnDestroy(GenjiDamageTracker tracker) { }
        }

        #region Assist Tracker
        public class AssistTracker : GenjiSubComponent {

            #region static
            public static List<AssistTracker> AllTrackers = new List<AssistTracker>();

            public static void CheckAllTrackersForDeath(GameObject body) {
                for (int i = 0; i < AllTrackers.Count; i++) {
                    AllTrackers[i].CheckDeath(body);
                }
            }
            #endregion

            public SkillLocator skillLocator;
            private List<GameObject> _trackedBodies = new List<GameObject>();

            public override void Awake(GenjiDamageTracker tracker) {
                skillLocator = tracker.skillLocator;
                AllTrackers.Add(this);
            }
            public override void OnDestroy(GenjiDamageTracker tracker) {
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
                            skillLocator.allSkills[i].Reset();
                        }
                    }
                }
            }
        }
        #endregion Assist Tracker

        #region Ultimate Buildup

        public class UltimateBuildup : GenjiSubComponent {

            public delegate void DamageForUltimateEvent(float damage);
            public DamageForUltimateEvent OnDamageForUltimate;

            public CharacterBody body;

            public void trackDamage(float damage) {
                if (body.damage == 0)
                    return;
                OnDamageForUltimate?.Invoke(damage/body.damage);
            }

            public override void Awake(GenjiDamageTracker tracker) {
                body = tracker.GetComponent<CharacterBody>();
            }
        }

        #endregion Ultimate Buildup
        #endregion SubComponents
    }
}