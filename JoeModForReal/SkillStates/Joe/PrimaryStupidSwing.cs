namespace ModdedEntityStates.Joe {
    public class PrimaryStupidSwing : Primary1Swing {

        protected override void SetSwingValues() {

            base.SetSwingValues();

            base.attackStartTime = 0.0f;
            base.baseEarlyExitTime = 0.0f;
            base.keypress = true;
        }
    }
}