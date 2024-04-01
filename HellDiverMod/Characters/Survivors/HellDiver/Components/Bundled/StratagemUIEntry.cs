using RoR2;
using RoR2.UI;
using System;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components.UI
{
    public class StratagemUIEntry : MonoBehaviour
    {
        [SerializeField]
        public Transform skillAnchor;
        [SerializeField]
        private StratagemUIEntryInput[] inputs;

        [HideInInspector] public SkillIcon skillIcon;


        internal void Init(StratagemInputSequence sequence)
        {
            Log.Warning("init");
            Log.WarningNull("skillicon", skillIcon);
            Log.WarningNull("sequence", sequence);
            skillIcon.targetSkill = sequence.stratagemGenericSkill;

            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].gameObject.SetActive(i < sequence.sequence.Length);
                if(i < sequence.sequence.Length)
                {
                    inputs[i].InitArrow(sequence.sequence[i]);
                }
            }

            Show(true);
        }

        internal void Reset()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].Reset();
            }
        }

        internal void Show(bool shouldShow)
        {
            gameObject.SetActive(shouldShow);
        }

        internal void UpdateComplete(bool chosen)
        {
            if (chosen)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i].Hide();
                }
            }
        }

        internal void UpdateInput(bool inputSuccess, int progress)
        {
            if (inputSuccess)
            {
                inputs[progress - 1].Darken();
                if (progress < inputs.Length)
                {
                    inputs[progress].Lighten();
                }
            }
            else
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i].Darken();
                }
            }
        }
    }
}
