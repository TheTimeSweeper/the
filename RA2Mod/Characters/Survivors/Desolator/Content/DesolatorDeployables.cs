using R2API;
using RoR2;
using RoR2.Projectile;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorDeployables
    {
        public static DeployableSlot irradiatorDeployableSlot;

        public static void Init()
        {
            irradiatorDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetIrradiatorSlotLimit);
            DesolatorAssets.DesolatorIrradiatorProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot = irradiatorDeployableSlot;
            DesolatorAssets.DesolatorIrradiatorProjectileScepter.GetComponent<ProjectileDeployToOwner>().deployableSlot = irradiatorDeployableSlot;
        }

        private static int onGetIrradiatorSlotLimit(CharacterMaster self, int deployableCountMultiplier)
        {
            int result = 1;
            if (self.bodyInstanceObject)
            {
                //would this guy need a limit too?
                //if (Modules.Config.LysateLimit == -1) {
                return self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
                //}

                //int lysateCount = self.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                //result += Mathf.Min(lysateCount, Modules.Config.LysateLimit);

                //result += SkillsPlusCompat.SkillsPlusAdditionalTowers;

                //result += Modules.Compat.TryGetScepterCount(self.inventory);
            }

            return result;
        }
    }
}