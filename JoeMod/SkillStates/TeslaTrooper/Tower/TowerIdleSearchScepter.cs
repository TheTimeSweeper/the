namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerIdleSearchScepter : TowerIdleSearch {
        protected override TowerZap GetNextStateType() {
            return new TowerZapMulti();
        }
    }

}
