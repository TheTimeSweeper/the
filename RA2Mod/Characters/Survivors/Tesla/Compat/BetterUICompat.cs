using ModdedEntityStates.TeslaTrooper;
using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace RA2Mod.Survivors.Tesla.Compat
{
    public class BetterUICompat
    {
        public static void init()
        {
            //tesla trooper
            AddSkill("Tesla_Primary_Zap", "TESLA_BOLTS", Zap.ProcCoefficient);
            AddSkill("Tesla_Secondary_BigZap", "TESLA_BLAST", BigZap.ProcCoefficient);
            AddSkill("Tesla_Utility_ShieldZap", "TESLA_BLAST", 1);

            //tesla trooper alts
            AddSkill("Tesla_Primary_Punch", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "TESLA_FIST",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "TESLA_BOLTS",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "TESLA_PROJECTILES",
                    procCoefficient = 1
                }
            });

            AddSkill("Tesla_Secondary_BigZapPunch", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "TESLA_FIST",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "TESLA_BOLTS",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "TESLA_BEAM",
                    procCoefficient = 1
                }
            });

            AddSkill("Tesla_Utility_BlinkZap", "TESLA_BOLT", 1);
        }

    }
}
