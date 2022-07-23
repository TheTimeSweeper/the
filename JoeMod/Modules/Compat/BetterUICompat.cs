using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using static BetterUI.ProcCoefficientCatalog;

namespace Modules {
    public class BetterUICompat {

        public static void init() {
            
            AddSkill("Tesla_Primary_Zap", "Bolts", Zap.ProcCoefficient);
            AddSkill("Tesla_Secondary_BigZap", "Blast", BigZap.ProcCoefficient);
            AddSkill("Tesla_Utility_ShieldZap", "Blast", 1);
        }
    }
}