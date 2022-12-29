using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using EntityStates;

namespace JoeModForReal.Content {
    public class LookingDownSteppedSkillDef : SteppedSkillDef {

        public Sprite LookingDownIcon;
        public SerializableEntityStateType LookingDownState;
        public float LookingDownAngle;

        private bool _lookingDown;

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot) {
            base.OnFixedUpdate(skillSlot);
            
            Vector3 dir = skillSlot.characterBody.inputBank.GetAimRay().direction.normalized;
            bool looking = Vector3.Angle(dir, Vector3.down) <= LookingDownAngle;

            _lookingDown = !skillSlot.characterBody.characterMotor.isGrounded && looking;
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {
            return _lookingDown ? LookingDownIcon : icon;
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot) {

            EntityState entityState = EntityStateCatalog.InstantiateState(_lookingDown ? LookingDownState : this.activationState);
            ISkillState skillState;
            if ((skillState = (entityState as ISkillState)) != null) {
                skillState.activatorSkillSlot = skillSlot;
            }

            SteppedSkillDef.InstanceData instanceData = (SteppedSkillDef.InstanceData)skillSlot.skillInstanceData;
            SteppedSkillDef.IStepSetter stepSetter;
            if ((stepSetter = (entityState as SteppedSkillDef.IStepSetter)) != null) {
                stepSetter.SetStep(instanceData.step);
            }
            return entityState;
        }
    }
}
