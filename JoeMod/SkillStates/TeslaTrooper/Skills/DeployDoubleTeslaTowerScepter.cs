namespace ModdedEntityStates.TeslaTrooper {

    public class DeployDoubleTeslaTowerScepter : DeployTeslaTower {
        private int towersLeft = 2;

        protected override void HandleConstructCoil() {

            //doesn't work
            if (towersLeft >= 2) {
                if (characterBody) {

                    constructCoil();

                    PlayCrossfade("Gesture, Override", "DoPlaceScepter", 0.1f);

                    towersLeft--;
                }
            } else {
                base.HandleConstructCoil();
            }
        }
    }
}