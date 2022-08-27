using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerZapMulti : TowerZap {

        int extraZaps = 10;

        protected override void fireOrb() {
            base.fireOrb();

            for (int i = 0; i < extraZaps; i++) {

                lightningOrb.target = lightningOrb.PickNextTarget(transform.position);
                if (lightningOrb.target == null)
                    return;
                base.fireOrb();
            }
        }
    }

}
