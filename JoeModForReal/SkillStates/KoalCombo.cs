using JoeModForReal.Content;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Joe {
    public class KoalCombo : PrimaryStupidSwing, SteppedSkillDef.IStepSetter, CombinedSteppedSkillDef.ICombinedStepSetter {

        public int ComboStep;

        public virtual void SetCombinedStep(int i) {
            //Helpers.LogWarning("primary combinedstep: " + i);
            ComboStep = i;
            
        }

        public override void SetStep(int i) {
            base.SetStep(i);
            //Helpers.LogWarning("primary step: " + i);
            swingIndex = Mathf.FloorToInt(i / 2);
        }

        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            writer.Write(this.ComboStep);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            this.ComboStep = reader.ReadInt32();
        }
    }

    public class KoalCombo2 : PrimaryStupidSwing, SteppedSkillDef.IStepSetter, CombinedSteppedSkillDef.ICombinedStepSetter {

        public int ComboStep;

        public virtual void SetCombinedStep(int i) {
            //Helpers.LogWarning("secondary combinedstep: " + i);
            ComboStep = i;
        }

        public override void SetStep(int i) {
            base.SetStep(i);
            //Helpers.LogWarning("secondary step: " + i);
            swingIndex = Mathf.FloorToInt(i / 2);
        }

        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            writer.Write(this.ComboStep);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            this.ComboStep = reader.ReadInt32();
        }

        protected override void PlayAttackAnimation() {
            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);
        }
    }
}