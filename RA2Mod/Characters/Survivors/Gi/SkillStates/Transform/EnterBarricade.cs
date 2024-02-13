using RA2Mod.Modules.BaseStates;
using RoR2;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class EnterBarricade : BaseTimedSkillState
    {

        public override float TimedBaseDuration => GIConfig.M4_Transform_InDuration.Value;
        public override float TimedBaseCastStartPercentTime => 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            base.PlayAnimation("Fullbody, overried", "charge", "dash.playbackRate", duration);
            Util.PlaySound("Play_GIBarricade", gameObject);
        }
    }
}