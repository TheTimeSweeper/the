using ModdedEntityStates.TeslaTrooper;

namespace ModdedEntityStates.Desolator {
    public class DesolatorVoiceLines : TeslaVoiceLines {
        protected override void playRandomvoiceLine(string prefix = "Play_") {
            base.playRandomvoiceLine("Play_Desolator_");
        }
    }
}