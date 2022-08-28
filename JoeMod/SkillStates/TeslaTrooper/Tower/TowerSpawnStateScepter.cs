namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerSpawnStateScepter : TowerSpawnState {

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority) {

                this.outer.SetNextState(new TowerIdleSearchScepter {
                    justSpawned = true
                });
            }
        }
    }

}
