using RA2Mod.Modules.BaseStates;

namespace RA2Mod.General.States
{
    public class Rest : BaseEmote
    {
        public override void OnEnter()
        {
            animString = "Sit";
            duration = 0;
            base.OnEnter();
            PlayAnimation("RadCannonSpin", "CannonSpin");
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayAnimation("RadCannonSpin", "DesolatorIdlePose");
        }
    }
}
