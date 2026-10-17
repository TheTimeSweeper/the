using ModdedEntityStates.Joe;

namespace LfPMod.Survivors.Mamluk.SkillStates
{
    public class Spear_M1 : Primary1Swing
    {
        protected override void SetSwingValues()
        {
            base.SetSwingValues();

            hitboxName = "dash";

        }
        protected override void PlayAttackAnimation()
        {
            PlayAnimation("Fullbody, overried", "meleeDashAttackShort", "dash.playbackRate", duration);
        }
    }
    public class Spear_M4Enter : Axe_M2Enter
    {
        protected override void SetSwingValues()
        {
            base.SetSwingValues();

            baseDuration = 0.8f;
            hitboxName = "dash";
        }

        public override void OnEnter()
        {
            base.OnEnter();
            characterMotor.Motor.ForceUnground();
            SmallHop(characterMotor, 2);
        }
        protected override void PlayAttackAnimation()
        {
            PlayAnimation("Fullbody, overried", "meleeDashAttack", "dash.playbackRate", duration);
        }
    }
}
