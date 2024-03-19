namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerSpawnStateScepter : TowerSpawnState
    {

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {

                outer.SetNextState(new TowerIdleSearchScepter
                {
                    justSpawned = true
                });
            }
        }
    }

}
