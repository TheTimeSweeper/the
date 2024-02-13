using EntityStates.Engi.Mine;

namespace RA2Mod.Survivors.GI.SkillStates.Mine
{
    public class MineArmingMutiny : MineArmingFull
    {
        public override void OnEnter()
        {
            pathToChildToEnable = "StrongIndicator";
            base.OnEnter();
            triggerRadius = GIConfig.M2_Mine_TriggerRadius.Value;
            blastRadiusScale = GIConfig.M2_Mine_BlastRadius.Value / Detonate.blastRadius;
            forceScale = 1;
            damageScale = 1;
        }
    }
}
