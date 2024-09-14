using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using EntityStates;
using System;

namespace JoeModForReal.Content {

    //didn't work
    public class ComboInputSkillDef : ConditionalSkillDef {

        protected override bool CheckCondition(GenericSkill skillSlot) {

            return skillSlot.characterBody.inputBank.skill1.down;
        }
    }

    public class LookingDownSkillDef : ConditionalSkillDef {

        public float LookingDownAngle;

        protected override bool CheckCondition(GenericSkill skillSlot) {

            Vector3 dir = skillSlot.characterBody.inputBank.GetAimRay().direction.normalized;
            bool looking = Vector3.Angle(dir, Vector3.down) <= LookingDownAngle;

            return !skillSlot.characterBody.characterMotor.isGrounded && looking;
        }
    }

    //wtf there's a conditionalskilldef in the base game?
    public abstract class ConditionalSkillDef : SteppedSkillDef {

        public SerializableEntityStateType ConditionalState;
        public Sprite ConditionalIcon = null;
        public int ConditionalRequriedStock = 1;

        protected bool conditionMet;

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime) {
            base.OnFixedUpdate(skillSlot, deltaTime);

            conditionMet = CheckCondition(skillSlot);
        }

        protected virtual bool CanExecuteCondition(GenericSkill skillSlot) {
            return conditionMet && skillSlot.stock >= ConditionalRequriedStock;
        }

        protected abstract bool CheckCondition(GenericSkill skillSlot);

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {

            return ConditionalIcon != null && CanExecuteCondition(skillSlot) ? ConditionalIcon : icon;
        }

        //didn't work in mp. moving back to skillstate onenter
        //public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot) {

        //    SerializableEntityStateType state = this.activationState;

        //    if (CanExecuteCondition(skillSlot)) {
        //        state = ConditionalState;
        //        skillSlot.DeductStock(ConditionalRequriedStock);
        //    }

        //    EntityState entityState = EntityStateCatalog.InstantiateState(state);
        //    ISkillState skillState;
        //    if ((skillState = (entityState as ISkillState)) != null) {
        //        skillState.activatorSkillSlot = skillSlot;
        //    }
            
        //    SteppedSkillDef.InstanceData instanceData = (SteppedSkillDef.InstanceData)skillSlot.skillInstanceData;
        //    SteppedSkillDef.IStepSetter stepSetter;
        //    if ((stepSetter = (entityState as SteppedSkillDef.IStepSetter)) != null) {
        //        stepSetter.SetStep(instanceData.step);
        //    }
        //    return entityState;
        //}
    }
}
