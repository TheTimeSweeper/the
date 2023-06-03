using EntityStates;

namespace ModdedEntityStates.Joe {
    public class PrimaryScepter1JumpSwingFall : Primary1JumpSwingFall {

        protected override EntityState GetLandState() {
            return new PrimaryScepter1JumpSwingLand();
        }
    }
}