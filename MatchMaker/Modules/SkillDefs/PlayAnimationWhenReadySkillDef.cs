using RoR2;
using RoR2.Skills;
using JetBrains.Annotations;
using UnityEngine;

namespace MatcherMod.Modules.SkillDefs
{
    public abstract class PlayAnimationWhenReadySkillDef : SkillDef
    {
        public string animatorBoolParameterName;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData { animator = skillSlot.characterBody.modelLocator.modelTransform.GetComponent<Animator>()};
        }

        public class InstanceData : BaseSkillInstanceData
        {
            public Animator animator;
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            if (!skillSlot.CanExecute())
            {
                ((InstanceData)skillSlot.skillInstanceData).animator.SetBool(animatorBoolParameterName, false);
            }
        }
    }
}