using UnityEngine;
using RoR2;
using R2API;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaDeployables
    {
        public static DeployableSlot teslaTowerDeployableSlot;

        public static void Init()
        {
            teslaTowerDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetTeslaTowerSlotLimit);
        }

        private static int onGetTeslaTowerSlotLimit(CharacterMaster self, int deployableCountMultiplier)
        {
            int result = 1;
            if (self.bodyInstanceObject)
            {

                if (TeslaConfig.M4_Tower_LysateLimit.Value == -1)
                {
                    return self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
                }

                int lysateCount = self.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                result += Mathf.Min(lysateCount, TeslaConfig.M4_Tower_LysateLimit.Value);

                //result += Compat.SkillsPlusCompat.SkillsPlusAdditionalTowers;

                result += General.GeneralCompat.TryGetScepterCount(self.inventory);
            }

            return result;
        }
    }
    }
