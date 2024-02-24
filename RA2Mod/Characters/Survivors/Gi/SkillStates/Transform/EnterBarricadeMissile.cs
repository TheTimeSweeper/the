using RoR2;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class EnterBarricadeMissile : BaseEnterBarricade
    {
        protected override void OnEnterbarricade()
        {
            base.PlayAnimation("Fullbody, overried", "charge", "dash.playbackRate", duration);
            Util.PlaySound("Play_GGIBarricade", gameObject);
            //spawn barricade or somethin
        }
    }
}