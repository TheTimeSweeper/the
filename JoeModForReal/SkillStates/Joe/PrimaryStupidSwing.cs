namespace ModdedEntityStates.Joe {
    public class PrimaryStupidSwing : Primary1Swing {

        protected override void SetSwingValues() {

            base.SetSwingValues();

            base.attackStartTime = 0.0f;
            base.baseEarlyExitTime = 0.0f;
            base.keypress = true;
        }

        protected override void SetNextState() {
            //int index = this.swingIndex;
            //if (index == 0) index = 1;
            //else index = 0;

            this.outer.SetNextState(new PrimaryStupidSwing {
                swingIndex = this.swingIndex + 1
            });
        }
    }
}