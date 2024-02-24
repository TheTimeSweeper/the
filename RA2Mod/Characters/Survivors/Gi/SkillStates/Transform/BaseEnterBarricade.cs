using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public abstract class BaseEnterBarricade : BaseTimedSkillState
    {
        public override float TimedBaseDuration => GIConfig.M4_Transform_InDuration.Value;
        public override float TimedBaseCastStartPercentTime => 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            OnEnterbarricade();
        }

        protected abstract void OnEnterbarricade();
    }
}