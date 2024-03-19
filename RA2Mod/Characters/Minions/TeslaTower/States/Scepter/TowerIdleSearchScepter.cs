namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerIdleSearchScepter : TowerIdleSearch
    {
        protected override TowerZap GetNextStateType()
        {
            return new TowerZapMulti();
        }
    }

}
