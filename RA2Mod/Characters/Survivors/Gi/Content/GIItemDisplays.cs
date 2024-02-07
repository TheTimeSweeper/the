using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

/* for custom copy format in keb's helper
{childName},
                    {localPos}, 
                    {localAngles},
                    {localScale})
*/

namespace RA2Mod.Survivors.GI
{
    public class GIItemDisplays : ItemDisplaysBase
    {
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["AlienHead"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAlienHead"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ArmorPlate"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ArmorReductionOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWarhammer"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayCoffee"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["AttackSpeedOnCrit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["AutoCastEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFossil"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Bandolier"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBandolier"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BarrierOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBrooch"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BarrierOnOverHeal"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAegis"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Bear"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBear"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BearVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBearVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BeetleGland"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Behemoth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBehemoth"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BleedOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTriTip"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BleedOnHitAndExplode"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BleedOnHitVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BonusGoldPackOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTome"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BossDamageBonus"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAPRound"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BounceNearby"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayHook"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ChainLightning"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayUkulele"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ChainLightningVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Clover"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayClover"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CloverVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CooldownOnCrit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySkull"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CritDamage"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLaserSight"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CritGlasses"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGlasses"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CritGlassesVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Crowbar"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayCrowbar"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Dagger"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDagger"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["DeathMark"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDeathMark"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ElementalRingVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayVoidRing"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EmpowerAlways"],
                Modules.ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWarHorn"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EquipmentMagazine"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBattery"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EquipmentMagazineVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ExecuteLowHealthElite"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGuillotine"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ExplodeOnDeath"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ExplodeOnDeathVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ExtraLife"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayHippo"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ExtraLifeVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FallBoots"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Feather"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFeather"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FireballsOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FireRing"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFireRing"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Firework"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFirework"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FlatHealth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FocusConvergence"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FragileDamageBonus"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FreeChest"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GhostOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMask"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GoldOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GoldOnHurt"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HeadHunter"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySkullcrown"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HealingPotion"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HealOnCrit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayScythe"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["HealWhileSafe"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySnail"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Hoof"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayHoof"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["IceRing"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayIceRing"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Icicle"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["IgniteOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGasoline"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ImmuneToDebuff"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["IncreaseHealing"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Incubator"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Infusion"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayInfusion"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["JumpBoost"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWaxBird"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["KillEliteFrenzy"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Knurl"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayKnurl"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LaserTurbine"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LightningStrikeOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarDagger"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarPrimaryReplacement"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBirdEye"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarSecondaryReplacement"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBirdClaw"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarSpecialReplacement"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarTrinket"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBeads"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarUtilityReplacement"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Medkit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMedkit"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MinorConstructOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Missile"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MissileVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MonstersOnShrineUse"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MoreMissile"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayICBM"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MoveSpeedOnKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Mushroom"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMushroom"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MushroomVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["NearbyDamageBonus"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDiamond"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["NovaOnHeal"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["NovaOnLowHealth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["OutOfCombatArmor"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ParentEgg"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayParentEgg"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Pearl"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayPearl"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["PermanentDebuffOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayScorpion"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["PersonalShield"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Phasing"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayStealthkit"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Plant"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["PrimarySkillShuriken"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShuriken"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["RandomDamageZone"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["RandomEquipmentTrigger"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["RandomlyLunar"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDomino"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["RegeneratingScrap"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["RepeatHeal"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayCorpseflower"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SecondarySkillMagazine"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Seed"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySeed"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ShieldOnly"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ShinyPearl"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ShockNearby"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SiphonOnLowHealth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SlowOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBauble"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SlowOnHitVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SprintArmor"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBuckler"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SprintBonus"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySoda"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SprintOutOfCombat"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWhip"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["SprintWisp"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Squid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["StickyBomb"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["StrengthenBurn"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGasTank"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["StunChanceOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Syringe"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Talisman"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTalisman"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Thorns"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["TitanGoldDuringTP"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Tooth"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothNecklaceDecal"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["TPHealingNova"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["TreasureCache"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayKey"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["TreasureCacheVoid"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["VoidMegaCrabItem"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["WarCryOnMultiKill"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayPauldron"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["WardOnLevel"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWarbanner"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BFG"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBFG"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Blackhole"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGravCube"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BossHunter"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTricornGhost"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BossHunterConsumed"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["BurnNearby"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayPotion"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Cleanse"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayWaterPack"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CommandMissile"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMissileRack"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CrippleWard"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEffigy"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["CritOnUse"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["DeathProjectile"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["DroneBackup"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRadio"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteEarthEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteFireEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteHauntedEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteIceEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteLightningEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteLunarEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["ElitePoisonEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["EliteVoidEquipment"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["FireBallDash"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayEgg"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Fruit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayFruit"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GainArmor"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Gateway"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayVase"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GoldGat"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGoldGat"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["GummyClone"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayGummyClone"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["IrradiatingLaser"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Jetpack"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBugWings"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LifestealOnHit"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Lightning"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                Modules.ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["LunarPortalOnUse"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Meteor"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMeteor"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Molotov"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayMolotov"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["MultiShopCard"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["QuestVolatileBattery"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Recycle"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayRecycler"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Saw"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Scanner"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayScanner"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["TeamWarCry"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["Tonic"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayTonic"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
            itemDisplayRules.Add(Modules.ItemDisplays.CreateDisplayRuleGroupWithRules(Modules.ItemDisplays.KeyAssets["VendingMachine"],
                Modules.ItemDisplays.CreateDisplayRule(Modules.ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                    "Chest",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    )
                ));
        }
    }
}