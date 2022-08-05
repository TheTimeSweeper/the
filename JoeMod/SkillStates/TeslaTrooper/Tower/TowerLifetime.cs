using EntityStates;
using RoR2;

namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerLifetime : BaseSkillState {
        public static float LifeDuration = 12;
        public static float skillsPlusSeconds = 0;

        public override void FixedUpdate() {
            base.FixedUpdate();

            if(base.fixedAge > LifeDuration + skillsPlusSeconds) {
                base.outer.SetNextState(new TowerUndeploy());
                return;
            }
        }
    }

}
