using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace Modules {

    public class BetterUICompat {

        public static void init() {
            
            AddSkill("Tesla_Primary_Zap", "Bolts", Zap.ProcCoefficient);
            AddSkill("Tesla_Secondary_BigZap", "Blast", BigZap.ProcCoefficient);
            AddSkill("Tesla_Utility_ShieldZap", "Blast", 1);

            AddSkill("Tesla_Primary_Punch", new List<ProcCoefficientInfo>() { 
                new ProcCoefficientInfo {
                    name = "Fist",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "Bolts",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "Deflected Projectiles",
                    procCoefficient = 1
                }
            });
        }
    }
}