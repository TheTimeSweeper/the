using LfPMod.Survivors.Mamluk.MamlukContent;
using ModdedEntityStates.Joe;
using RoR2;

namespace LfPMod.Survivors.Mamluk.SkillStates
{
    public class Shield_M1: Primary1Swing
    {

    }
    public class Shield_M3Enter : Utility1MeleeDashAttack
    {
        protected override string hitbox => "jumpswing";
        protected override void InitMeleeDashStuff()
        {
            speedCoefficient = CharacterConfig.Shield_Primary_DashSpeed;
            duration = 0.3f;
            _travelEndPercentTime = 1f;// 17 / 40f;//todo testvaluemanager
        }
        protected override void PlayAnimation()
        {
            base.PlayAnimation("Arms, Override",
                               "heavySwing",
                               "swing.playbackRate",
                               this.duration);
        }
    }
}
