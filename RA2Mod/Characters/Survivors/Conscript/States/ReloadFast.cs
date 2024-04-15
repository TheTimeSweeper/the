namespace RA2Mod.Survivors.Conscript.States
{
    public class ReloadFast : Reload
    {
        public override float TimedBaseDuration => 0.5f;

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            characterBody.RemoveBuff(ConscriptBuffs.magazineBuff);
        }
    }
}
