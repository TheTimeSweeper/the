﻿using MatcherMod.Survivors.Matcher.Components;
using RoR2;
using MatcherMod.Survivors.Matcher.Content;
using JetBrains.Annotations;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.SkillDefs
{
    public class CrateMatchSkillDef : MatchBoostedSkillDef
    {
        public static BasicPickupDropTable dtTier1Item => Content.CharacterAssets.dropTableTier1Item;

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
            if (RoR2.Util.CheckRoll(CharacterConfig.M4_Crate_PercentChance.Value, controller.CharacterBody.master))
            {
                PickupIndex pickup = dtTier1Item.GenerateDrop(((CrateInstanceData)skillSlot.skillInstanceData).rng);
                controller.CharacterBody.master.inventory.GiveItem(PickupCatalog.GetPickupDef(pickup).itemIndex);
                GenericPickupController.SendPickupMessage(controller.CharacterBody.master, pickup);
                return controller.gameObject;
            }
            return null;
        }
    }
}
