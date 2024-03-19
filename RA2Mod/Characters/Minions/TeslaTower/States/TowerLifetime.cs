using EntityStates;

namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerLifetime : BaseSkillState
    {
        public static float LifeDuration = 12;
        public static float skillsPlusSeconds = 0;

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge > LifeDuration + skillsPlusSeconds)
            {
                outer.SetNextState(new TowerUndeploy());
                return;
            }
        }
    }

}
