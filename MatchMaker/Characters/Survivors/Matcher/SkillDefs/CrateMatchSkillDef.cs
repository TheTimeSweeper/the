using MatcherMod.Survivors.Matcher.Components;
using RoR2;
using MatcherMod.Survivors.Matcher.MatcherContent;
using JetBrains.Annotations;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.SkillDefs
{
    public class CrateMatchSkillDef : MatchBoostedSkillDef
    {
        public static BasicPickupDropTable dtTier1Item => MatcherContent.Assets.dtTier1Item;

        public class CrateInstanceData : InstanceData
        {
            public Xoroshiro128Plus rng;
        }
        
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData =  (InstanceData)base.OnAssigned(skillSlot);
            CrateInstanceData crateInstanceData = new CrateInstanceData
            {
                componentFromSkillDef1 = instanceData.componentFromSkillDef1,
                rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong)
            };

            return crateInstanceData;
        }

        public static GameObject CrateMatchAction(MatcherGridController controller, GenericSkill skillSlot, int matches)
        {
            if (RoR2.Util.CheckRoll(Config.M4_Crate_PercentChance.Value, controller.CharacterBody.master))
            {
                PickupIndex pickup = dtTier1Item.GenerateDrop(((CrateInstanceData)skillSlot.skillInstanceData).rng);
                controller.CharacterBody.master.inventory.GiveItem(PickupCatalog.GetPickupDef(pickup).itemIndex);
                return controller.gameObject;
            }
            return null;
        }
    }
}
