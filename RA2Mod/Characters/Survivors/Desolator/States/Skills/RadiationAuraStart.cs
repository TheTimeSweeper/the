using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.Desolator.States
{
    public class RadiationAuraStart : BaseTimedSkillState {

        public override float TimedBaseDuration => 1;

        public override float TimedBaseCastStartPercentTime => 0;

        public override void OnEnter() {
            base.OnEnter();

            //todo: lingering gesture, interruptible legs
            //he's running in place what
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }
    }
}
