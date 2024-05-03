using System.Collections.Generic;
using static BetterUI.ProcCoefficientCatalog;

namespace RA2Mod.Survivors.Desolator.Compat
{
    public class DesolatorBetterUICompat
    {
        private static string prefix = DesolatorSurvivor.TOKEN_PREFIX;

        public static void init()
        {
            //desolator
            AddSkill("Desolator_Primary_Beam", prefix + "PROC_BEAM", 1);
            AddSkill("Desolator_Secondary_BigBeam", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = "PROC_INITIAL_BLAST",
                    procCoefficient = 1
                },
                new ProcCoefficientInfo {
                    name = "PROC_TICKS",
                    procCoefficient = 0.7f
                }
            });
            AddSkill("Desolator_Special_Deploy", prefix + "PROC_TICKS", 0.7f);
            AddSkill("Desolator_Special_Deploy_Scepter", prefix + "PROC_TICKS", 0.7f);
            AddSkill("Desolator_Special_Tower", prefix + "PROC_TICKS", 0.7f);
            AddSkill("Desolator_Special_Tower_Scepter", new List<ProcCoefficientInfo>() {
                new ProcCoefficientInfo {
                    name = prefix + "PROC_TICKS",
                    procCoefficient = 0.7f
                },
                new ProcCoefficientInfo {
                    name = prefix + "PROC_ENDING_BLAST",
                    procCoefficient = 1
                }
            });
        }

    }
}