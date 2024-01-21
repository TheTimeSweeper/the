using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class PhaseIndicatorController : MonoBehaviour
    {
        private PhaseIndicator indicator;
        private CharacterBody characterBody;

        void Awake()
        {
            indicator = new PhaseIndicator(base.gameObject, ChronoAssets.chronoIndicatorPhase);
            characterBody = GetComponent<CharacterBody>();
            indicator.targetTransform = characterBody.coreTransform;
        }

        public void UpdateIndicatorActive(bool active)
        {
            indicator.active = active;
        }

        public void UpdateIndicatorFill(float fill)
        {
            indicator.fill = fill;
        }

        public class PhaseIndicator : Indicator
        {
            public float fill;

            public PhaseIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

            public override void UpdateVisualizer()
            {
                base.UpdateVisualizer();

                if (visualizerTransform)
                {
                    visualizerTransform.GetComponent<CooldownTimerView>().SetFill(fill);
                }
            }
        }
    }
}