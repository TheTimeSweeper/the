using RA2Mod.Survivors.Tesla.States;
using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace RA2Mod.Survivors.Tesla.Compat
{
    public class TeslaBetterUICompat
    {
        private static string prefix = TeslaTrooperSurvivor.TOKEN_PREFIX;

        public static void init()
        {
            //tesla trooper
            AddSkill("Tesla_Primary_Zap", prefix + "PROC_BOLTS", Zap.ProcCoefficient);
            AddSkill("Tesla_Secondary_BigZap", prefix + "PROC_BLAST", BigZap.ProcCoefficient);
            AddSkill("Tesla_Utility_ShieldZap", prefix + "PROC_BLAST", 1);

            //tesla trooper alts
            AddSkill("Tesla_Primary_Punch", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = prefix + "PROC_FIST",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = prefix + "PROC_BOLTS",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                }
            });

            AddSkill("Tesla_Secondary_BigZapPunch", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = prefix + "PROC_FIST",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = prefix + "PROC_BOLTS",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = prefix + "PROC_BEAM",
                    procCoefficient = 1
                }
            });

            AddSkill("Tesla_Utility_BlinkZap", prefix + "PROC_BOLT", 1);
        }

    }
}
