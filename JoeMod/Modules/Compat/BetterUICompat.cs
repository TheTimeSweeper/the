using ModdedEntityStates.Desolator;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace Modules {

    public class BetterUICompat {

        public static void init() {
            //tesla trooper
            AddSkill("Tesla_Primary_Zap", "Bolts", Zap.ProcCoefficient);
            AddSkill("Tesla_Secondary_BigZap", "Blast", BigZap.ProcCoefficient);
            AddSkill("Tesla_Utility_ShieldZap", "Blast", 1);

            //tesla trooper alts
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

            AddSkill("Tesla_Secondary_BigZapPunch", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "Fist",
                    procCoefficient = ZapPunch.ProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "Bolts",
                    procCoefficient = ZapPunch.OrbProcCoefficient
                },
                new ProcCoefficientInfo {
                    name = "Charged Beam",
                    procCoefficient = 1
                }
            });

            AddSkill("Tesla_Utility_BlinkZap", "Bolt", 1);

            //desolator
            AddSkill("Desolator_Primary_Beam", "Beam", 1);
            AddSkill("Desolator_Secondary_BigBeam", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "Initial Blast",
                    procCoefficient = 1
                },
                new ProcCoefficientInfo {
                    name = "Ticks",
                    procCoefficient = 0.7f
                }
            });
            AddSkill("Desolator_Special_Deploy", "Ticks", 0.7f);
            AddSkill("Desolator_Special_Deploy_Scepter", "Ticks", 0.7f);
            AddSkill("Desolator_Special_Tower", "Ticks", 0.7f);
            AddSkill("Desolator_Special_Tower_Scepter", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "Ticks",
                    procCoefficient = 0.7f
                },
                new ProcCoefficientInfo {
                    name = "Ending Blast",
                    procCoefficient = 1
                }
            });
        }
    }
}