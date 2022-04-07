using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using static BetterUI.ProcCoefficientCatalog;

namespace Modules {
    public class BetterUICompat {

        public static void init() {
            
            AddSkill(Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_ZAP_NAME", "Bolts", Zap.ProcCoefficient);
            AddSkill(Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "SECONDARY_BIGZAP_NAME", "Blast", BigZap.ProcCoefficient);
            AddSkill(Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "UTILITY_BARRIER_NAME", "Blast", 1);
        }
    }
}