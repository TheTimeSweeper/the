using EntityStates;
using JetBrains.Annotations;
using PlagueMod.Survivors.Plague.Components;
using RoR2;
using RoR2.Skills;

namespace PlagueMod.Survivors.Plague.SkillDefs
{
    public class PlagueBombSelectionSkillDef : SkillDef
    {
        public interface IPlagueBombSetSelector
        {
            void SetPlagueComponent(PlagueBombSelectorController component);
        }

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                selectorComponent = skillSlot.GetComponent<PlagueBombSelectorController>()
            };
        }

        public class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public PlagueBombSelectorController selectorComponent;
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IPlagueBombSetSelector throwSkill;
            if ((throwSkill = (entityState as IPlagueBombSetSelector)) != null)
            {
                throwSkill.SetPlagueComponent(instanceData.selectorComponent);
            }
            return entityState;
        }
    }
}
