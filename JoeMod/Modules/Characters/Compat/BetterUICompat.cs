using ModdedEntityStates.Desolator;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace Modules {

    public class BetterUICompat {

        public static void init() {
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

            //desolator
            AddSkill("Desolator_Primary_Beam", "DESOLATOR_BEAM", 1);
            AddSkill("Desolator_Secondary_BigBeam", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "DESOLATOR_INITIAL_BLAST",
                    procCoefficient = 1
                },
                new ProcCoefficientInfo {
                    name = "DESOLATOR_TICKS",
                    procCoefficient = 0.7f
                }
            });
            AddSkill("Desolator_Special_Deploy", "DESOLATOR_TICKS", 0.7f);
            AddSkill("Desolator_Special_Deploy_Scepter", "DESOLATOR_TICKS", 0.7f);
            AddSkill("Desolator_Special_Tower", "DESOLATOR_TICKS", 0.7f);
            AddSkill("Desolator_Special_Tower_Scepter", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "DESOLATOR_TICKS",
                    procCoefficient = 0.7f
                },
                new ProcCoefficientInfo {
                    name = "DESOLATOR_ENDING_BLAST",
                    procCoefficient = 1
                }
            });
        }
    }
}