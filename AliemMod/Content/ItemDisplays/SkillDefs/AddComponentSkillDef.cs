using RoR2;
using System.Diagnostics.CodeAnalysis;

namespace AliemMod.Content.SkillDefs
{
    public abstract class AddComponentSkillDef<T> : HasComponentSkillDef<T> where T : UnityEngine.Component
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            T component = skillSlot.GetComponent<T>();
            if (component == null)
            {
                component = skillSlot.gameObject.AddComponent<T>();
            }
            return new InstanceData
            {
                componentFromSkillDef1 = component
            };
        }
    }
}