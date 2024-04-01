using HellDiverMod.General.Components.UI;
using HellDiverMod.Survivors.HellDiver.Components.UI;
using HellDiverMod.Survivors.HellDiver.SkillDefs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components
{
    public enum StratagemInput
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class StratagemInputSequence
    {
        public GenericSkill stratagemGenericSkill;
        public StratagemInput[] sequence;
        public int sequenceProgress;
        public bool sequenceComplete => sequenceProgress >= sequence.Length;

        private bool _failed;

        public StratagemInputSequence(GenericSkill cooldownGenericSkill, StratagemInput[] sequence)
        {
            this.stratagemGenericSkill = cooldownGenericSkill;
            this.sequence = sequence;
        }

        public bool CheckSequenceInput(StratagemInput input)
        {
            if (_failed)
            {
                return false;
            }
            if (sequence[sequenceProgress] == input)
            {
                sequenceProgress++;
                return true;
            } 
            else
            {
                _failed = true;
                return false;
            }
        }

        public void Reset()
        {
            sequenceProgress = 0;
            _failed = false;
        }
    }

    public class StratagemInputController : MonoBehaviour, IHasCompanionUI<HellDiverUI>
    {
        public List<StratagemInputSequence> sequences = new List<StratagemInputSequence>();

        public Queue<GenericSkill> thrownStratagemQueue = new Queue<GenericSkill>();

        private GenericSkill _nextStratagem;

        public bool allowUIUpdate { get; set; }
        public HellDiverUI CompanionUI { get; set; }

        void Start()
        {
            //todo add resupply n stuff
            GetAvailableStrategems();
            allowUIUpdate = true;
        }

        private void GetAvailableStrategems()
        {
            GenericSkill[] skills = GetComponents<GenericSkill>();
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].skillDef is StratagemSkillDef)
                {
                    StratagemSkillDef skillDef = skills[i].skillDef as StratagemSkillDef;
                    sequences.Add(new StratagemInputSequence(skills[i], skillDef.sequence));
                }
            }
        }

        public bool TryStratagemInput(StratagemInput input)
        {
            bool anySucceeded = false;
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].stratagemGenericSkill.IsReady())
                {
                    bool inputSuccess = sequences[i].CheckSequenceInput(input);
                    anySucceeded |= inputSuccess;

                    CompanionUI.UpdateSequence(i, inputSuccess, sequences[i].sequenceProgress);                    
                }
            }

            bool complete = false;
            int completed = -1;
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i].stratagemGenericSkill.IsReady() && sequences[i].sequenceComplete)
                {
                    _nextStratagem = sequences[i].stratagemGenericSkill;
                    complete = true;
                    completed = i;
                }
            }

            if (!anySucceeded || complete)
            {
                for (int i = 0; i < sequences.Count; i++)
                {
                    sequences[i].Reset();
                }
                if (complete)
                {
                    CompanionUI.UpdateComplete(completed);
                }
            }
            return complete;
        }

        public void QueueStratagem()
        {
            if(_nextStratagem == null)
            {
                Log.Error("no stratagem to enqueue");
                return;
            }
            thrownStratagemQueue.Enqueue(_nextStratagem);
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                sequences[i].Reset();
            }
            _nextStratagem = null;
            CompanionUI.Reset();
        }

        public void ShowUI(bool shoudlShow)
        {
            CompanionUI.Show(shoudlShow);
        }
    }
}
