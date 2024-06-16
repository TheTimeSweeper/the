using EntityStates;
using JetBrains.Annotations;
using ModdedEntityStates.Aliem;
using RoR2;
using RoR2.Skills;

namespace AliemMod.Content.SkillDefs
{
    public class OffHandSkillDef : SkillDef
    {
        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState state = base.InstantiateNextState(skillSlot);
            IOffHandable OffHandState;
            if((OffHandState = (state as IOffHandable)) != null)
            {
                OffHandState.isOffHanded = true;
            }
            return state;
        }
    }
}