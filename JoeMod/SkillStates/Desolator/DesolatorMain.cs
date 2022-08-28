using ModdedEntityStates.TeslaTrooper;

namespace ModdedEntityStates.Desolator {
    public class DesolatorMain : TeslaTrooperMain {
        protected override void playRandomvoiceLine(string prefix = "Play_") {
            base.playRandomvoiceLine("Play_Desolator");
        }
    }
}