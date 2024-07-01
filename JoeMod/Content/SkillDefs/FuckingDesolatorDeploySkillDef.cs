using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace TeslaTrooper
{
    //have to do this song and dance because replacing a skilldef with a cancel skilldef gets fucked up by other mods
    public class FuckingDesolatorDeploySkillDef : SkillDef
    {
        // Token: 0x04004450 RID: 17488
        [Tooltip("The state to enter when this skill is activated.")]
        public SerializableEntityStateType actualActivationState;
        public Sprite cancelIcon;

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            if(skillSlot.stateMachine.GetType() == activationState.GetType())
            {
                return cancelIcon;
            }
            return base.GetCurrentIcon(skillSlot);
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = EntityStateCatalog.InstantiateState(this.actualActivationState);
            ISkillState skillState;
            if ((skillState = (entityState as ISkillState)) != null)
            {
                skillState.activatorSkillSlot = skillSlot;
            }
            return entityState;
        }
    }
}
