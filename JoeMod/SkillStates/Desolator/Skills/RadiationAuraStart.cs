using ModdedEntityStates.BaseStates;

namespace ModdedEntityStates.Desolator {
    public class RadiationAuraStart : BaseTimedSkillState {

        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.0f;

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastStartTime);

            //todo: lingering gesture, interruptible legs
            //he's running in place what
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }

    }
}
