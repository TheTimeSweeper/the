using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace JoeMod {
    public class DesolatorDeploySkillDef : SkillDef {
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot) {
            //don't recharge during these states
            //base.OnFixedUpdate(skillSlot);
        }
    }
}
