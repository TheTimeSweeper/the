using RA2Mod.Minions.TeslaTower.States;
using RA2Mod.Modules;

namespace RA2Mod.Minions.TeslaTower
{
    public class TeslaTowerStates
    {
        public static void Init()
        {
            Content.AddEntityState(typeof(TowerSpawnState));
            Content.AddEntityState(typeof(TowerIdleSearch));

            Content.AddEntityState(typeof(TowerLifetime));
            Content.AddEntityState(typeof(TowerUndeploy));
            Content.AddEntityState(typeof(TowerSell));

            Content.AddEntityState(typeof(TowerZap));
            Content.AddEntityState(typeof(TowerBigZap));
            Content.AddEntityState(typeof(TowerBigZapGauntlet));

            if (General.GeneralCompat.ScepterInstalled)
            {
                //scepter
                Content.AddEntityState(typeof(TowerSpawnStateScepter));
                Content.AddEntityState(typeof(TowerIdleSearchScepter));

                Content.AddEntityState(typeof(TowerZapMulti));
            }
        }
    }
}
