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

namespace RA2Mod.Survivors.Conscript
{
    public class ConscriptItemDisplays : ItemDisplaysBase
    {
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AlienHead"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAlienHead"),
                    "Pelvis",
                    new Vector3(-0.07167F, 0F, -0.15889F),
                    new Vector3(355.8723F, 281.9293F, 285.0362F),
                    new Vector3(0.62547F, 0.62547F, 0.62547F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorPlate"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "ThighL",
                    new Vector3(-0.2456F, -0.06842F, 0.04034F),
                    new Vector3(4.45319F, 86.15548F, 330.5126F),
                    new Vector3(0.15696F, 0.12821F, 0.15696F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorReductionOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarhammer"),
                    "Weapon1",
                    new Vector3(-0.00012F, 5.17776F, -0.00004F),
                    new Vector3(270F, 0F, 0F),
                    new Vector3(2.06381F, 1.36113F, 1.39924F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCoffee"),
                    "Pelvis",
                    new Vector3(-0.06448F, -0.07469F, 0.13122F),
                    new Vector3(281.8889F, 347.3669F, 102.9009F),
                    new Vector3(0.13321F, 0.13321F, 0.13321F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                    "UpperArmL",
                    new Vector3(0.03016F, -0.0205F, -0.01138F),
                    new Vector3(309.8974F, 157.9219F, 143.2774F),
                    new Vector3(0.18777F, 0.18777F, 0.18777F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AutoCastEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFossil"),
                    "Pelvis",
                    new Vector3(-0.03655F, -0.10662F, 0.04217F),
                    new Vector3(5.5841F, 44.09656F, 264.2653F),
                    new Vector3(0.322F, 0.30277F, 0.30277F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bandolier"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBandolier"),
                    "Chest",
                    new Vector3(0.08412F, -0.01455F, -0.02638F),
                    new Vector3(0F, 39.04367F, 194.1263F),
                    new Vector3(0.47939F, 0.47939F, 0.47939F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrooch"),
                    "Chest",
                    new Vector3(0.05822F, 0.16803F, -0.00098F),
                    new Vector3(350.1134F, 90F, 0F),
                    new Vector3(0.27433F, 0.27433F, 0.27433F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnOverHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAegis"),
                    "LowerArmR",
                    new Vector3(-0.05688F, -0.00569F, 0.05118F),
                    new Vector3(274.3724F, 132.5731F, 223.8972F),
                    new Vector3(0.11747F, 0.11747F, 0.11747F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bear"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBear"),
                    "Chest",
                    new Vector3(-0.02325F, 0.10862F, 0.07514F),
                    new Vector3(296.7887F, 316.6584F, 122.6229F),
                    new Vector3(0.10307F, 0.10307F, 0.11428F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BearVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBearVoid"),
                    "Chest",
                    new Vector3(-0.02325F, 0.10862F, 0.07514F),
                    new Vector3(296.7887F, 316.6584F, 122.6229F),
                    new Vector3(0.10307F, 0.10307F, 0.11428F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BeetleGland"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                    "Pelvis",
                    new Vector3(-0.0481F, -0.1037F, -0.15948F),
                    new Vector3(349.5289F, 182.0354F, 292.7573F),
                    new Vector3(0.05924F, 0.05924F, 0.05924F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Behemoth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBehemoth"),
                    "Weapon2",
                    new Vector3(0F, 0.00034F, 0.10767F),
                    new Vector3(82.11126F, 83.17943F, 83.10132F),
                    new Vector3(0.02961F, 0.02961F, 0.02961F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTip"),
                    "Weapon1",
                    new Vector3(-0.00009F, -0.57855F, 0.22691F),
                    new Vector3(72.8263F, 0F, 0F),
                    new Vector3(1.79249F, 1.79249F, 1.79249F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitAndExplode"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "ThighR",
                    new Vector3(-0.15005F, 0.08155F, -0.02917F),
                    new Vector3(69.44172F, 0F, 0F),
                    new Vector3(0.04481F, 0.04481F, 0.04481F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                    "Weapon1",
                    new Vector3(-0.00018F, -1.18566F, 0.43736F),
                    new Vector3(285.4396F, 180F, 180F),
                    new Vector3(1.79249F, 1.79249F, 1.79249F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BonusGoldPackOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTome"),
                    "ThighL",
                    new Vector3(-0.1581F, 0.08403F, 0.01791F),
                    new Vector3(299.5951F, 355.1984F, 278.3699F),
                    new Vector3(0.0316F, 0.0316F, 0.0316F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAPRound"),
                    "ThighL",
                    new Vector3(-0.26202F, 0F, 0.08571F),
                    new Vector3(357.2135F, 84.54404F, 251.2906F),
                    new Vector3(0.32524F, 0.32524F, 0.32524F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BounceNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHook"),
                    "Chest",
                    new Vector3(-0.05567F, -0.09924F, 0.00255F),
                    new Vector3(279.8761F, 116.5238F, 336.6657F),
                    new Vector3(0.27631F, 0.27631F, 0.27631F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkulele"),
                    "HandL",
                    new Vector3(-0.18007F, -0.07466F, 0.18858F),
                    new Vector3(281.9821F, 0.39167F, 325.6704F),
                    new Vector3(0.41622F, 0.41622F, 0.41622F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightningVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                    "HandL",
                    new Vector3(-0.18007F, -0.07466F, 0.18858F),
                    new Vector3(281.9821F, 0.39167F, 325.6704F),
                    new Vector3(0.41622F, 0.41622F, 0.41622F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Clover"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayClover"),
                    "Head",
                    new Vector3(-0.08423F, 0.01018F, 0.04501F),
                    new Vector3(351.557F, 244.5463F, 293.6258F),
                    new Vector3(0.26342F, 0.26342F, 0.26342F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CloverVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                    "Head",
                    new Vector3(-0.08423F, 0.01018F, 0.04501F),
                    new Vector3(351.557F, 244.5463F, 293.6258F),
                    new Vector3(0.26342F, 0.26342F, 0.26342F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CooldownOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkull"),
                    "HandR",
                    new Vector3(-0.08704F, -0.01058F, -0.02446F),
                    new Vector3(15.33806F, 254.7079F, 182.6586F),
                    new Vector3(0.13104F, 0.13104F, 0.13104F)
                    )
                ));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserSight"),
                    "Weapon1",
                    new Vector3(-0.01772F, 4.66133F, -0.04503F),
                    new Vector3(0F, 90F, 270F),
                    new Vector3(0.25716F, 0.25716F, 0.25716F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlasses"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlasses"),
                    "Head",
                    new Vector3(-0.01328F, 0.06849F, 0F),
                    new Vector3(270F, 90F, 0F),
                    new Vector3(0.17551F, 0.19277F, 0.17605F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlassesVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                    "Head",
                    new Vector3(-0.01328F, 0.06849F, 0F),
                    new Vector3(270F, 90F, 0F),
                    new Vector3(0.17551F, 0.19277F, 0.17605F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Crowbar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCrowbar"),
                    "Chest",
                    new Vector3(0.03188F, -0.09072F, -0.10085F),
                    new Vector3(56.13412F, 168.892F, 292.1468F),
                    new Vector3(0.26093F, 0.26093F, 0.26093F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Dagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDagger"),
                    "UpperArmR",
                    new Vector3(0.03378F, 0.0043F, 0F),
                    new Vector3(332.2854F, 356.1611F, 278.2102F),
                    new Vector3(0.47252F, 0.47252F, 0.47252F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathMark"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathMark"),
                    "HandL",
                    new Vector3(-0.04322F, 0.00447F, 0.00442F),
                    new Vector3(11.44537F, 262.8808F, 179.7555F),
                    new Vector3(0.02015F, 0.02015F, 0.02015F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElementalRingVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVoidRing"),
                    "Weapon1",
                    new Vector3(-0.00003F, -0.43532F, -0.00229F),
                    new Vector3(88.36877F, 180F, 180F),
                    new Vector3(1.05934F, 1.05934F, 1.05934F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EmpowerAlways"],
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Chest",
                    new Vector3(-0.04936F, 0.00867F, -0.00285F),
                    new Vector3(52.65432F, 203.9559F, 302.8301F),
                    new Vector3(0.79016F, 0.79016F, 0.79016F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.70481F, 0.50603F, 0.50603F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarHorn"),
                    "Pelvis",
                    new Vector3(-0.14987F, 0.0274F, 0.11035F),
                    new Vector3(333.7772F, 338.812F, 74.20507F),
                    new Vector3(0.25998F, 0.25998F, 0.25998F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBattery"),
                    "Stomach",
                    new Vector3(-0.04171F, -0.07711F, -0.0332F),
                    new Vector3(0F, 0F, 255.3181F),
                    new Vector3(0.12321F, 0.12321F, 0.12321F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazineVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                    "Stomach",
                    new Vector3(-0.04171F, -0.07711F, -0.0332F),
                    new Vector3(0F, 0F, 255.3181F),
                    new Vector3(0.12321F, 0.12321F, 0.12321F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExecuteLowHealthElite"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGuillotine"),
                    "LowerArmL",
                    new Vector3(-0.09193F, -0.05309F, -0.02129F),
                    new Vector3(309.7166F, 307.1917F, 230.2828F),
                    new Vector3(0.10572F, 0.10572F, 0.10572F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeath"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                    "Stomach",
                    new Vector3(-0.02417F, -0.00628F, -0.15903F),
                    new Vector3(0F, 0F, 104.5543F),
                    new Vector3(0.04662F, 0.04662F, 0.04662F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeathVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                    "Stomach",
                    new Vector3(-0.02417F, -0.00628F, -0.15903F),
                    new Vector3(0F, 0F, 104.5543F),
                    new Vector3(0.04662F, 0.04662F, 0.04662F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLife"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippo"),
                    "Chest",
                    new Vector3(0.00396F, -0.07764F, 0.12539F),
                    new Vector3(37.31347F, 327.5696F, 58.20817F),
                    new Vector3(0.17848F, 0.17848F, 0.17848F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLifeVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                    "Chest",
                    new Vector3(0.00396F, -0.07779F, 0.12553F),
                    new Vector3(46.95388F, 2.37236F, 82.03741F),
                    new Vector3(0.22451F, 0.22451F, 0.22451F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FallBoots"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfL",
                    new Vector3(-0.31609F, 0.00715F, -0.00001F),
                    new Vector3(359.3953F, 358.7579F, 274.0115F),
                    new Vector3(0.19852F, 0.19852F, 0.19852F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfR",
                    new Vector3(-0.31609F, 0.00715F, -0.00001F),
                    new Vector3(359.3953F, 358.7579F, 274.0115F),
                    new Vector3(0.19852F, 0.19852F, 0.19852F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Feather"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFeather"),
                    "UpperArmR", 
                    new Vector3(0.0621F, -0.01618F, -0.03316F),
                    new Vector3(285.8112F, 229.8166F, 338.1937F),
                    new Vector3(0.03056F, 0.03056F, 0.03056F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireRing"),
                    "Weapon1",
                    new Vector3(-0.00004F, -0.54562F, -0.00581F),
                    new Vector3(88.36865F, 180F, 180F),
                    new Vector3(1.05934F, 1.05934F, 1.05934F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireballsOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                    "Weapon1",
                    new Vector3(-0.71949F, 4.32422F, 0.07579F),
                    new Vector3(0F, 276.0154F, 0F),
                    new Vector3(0.33858F, 0.33858F, 0.33858F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Firework"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFirework"),
                    "Stomach",
                    new Vector3(-0.07563F, 0.03375F, 0.11637F),
                    new Vector3(27.8276F, 289.5561F, 307.2693F),
                    new Vector3(0.15366F, 0.15366F, 0.15366F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FlatHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                    "Chest",
                    new Vector3(0.10056F, 0.08564F, 0.1476F),
                    new Vector3(348.3777F, 339.0652F, 153.2118F),
                    new Vector3(0.06602F, 0.06602F, 0.06602F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FocusConvergence"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                    "Base",
                    new Vector3(-0.95123F, -0.40321F, 1.04859F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.15501F, 0.15501F, 0.15501F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FragileDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                   "LowerArmL",
                    new Vector3(-0.19533F, -0.00369F, -0.00877F),
                    new Vector3(4.40062F, 275.5829F, 81.03358F),
                    new Vector3(0.53035F, 0.53035F, 0.53035F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FreeChest"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                    "CalfL",
                    new Vector3(-0.17192F, 0F, 0.06818F),
                    new Vector3(11.42771F, 254.1884F, 248.1759F),
                    new Vector3(0.43731F, 0.43731F, 0.43731F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GhostOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMask"),
                    "Head",
                    new Vector3(-0.00882F, 0.02083F, -0.00018F),
                    new Vector3(276.4727F, 265.7863F, 184.187F),
                    new Vector3(0.40407F, 0.40407F, 0.40407F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                    "Head",
                    new Vector3(-0.06176F, -0.01791F, 0.00166F),
                    new Vector3(275.9064F, 193.2016F, 256.7304F),
                    new Vector3(0.5149F, 0.5149F, 0.5149F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHurt"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                    "UpperArmR",
                    new Vector3(-0.17071F, -0.01349F, 0.03077F),
                    new Vector3(21.5527F, 350.6821F, 264.4691F),
                    new Vector3(0.53049F, 0.53049F, 0.53049F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                    "UpperArmR",
                    new Vector3(0.00892F, -0.00643F, 0.02995F),
                    new Vector3(86.36105F, 180F, 128.4917F),
                    new Vector3(0.42372F, 0.42372F, 0.42372F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                    "UpperArmR",
                    new Vector3(0.03504F, -0.00082F, 0.02598F),
                    new Vector3(86.40773F, 25.21872F, 316.2144F),
                    new Vector3(0.38768F, 0.38768F, 0.38768F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HeadHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkullcrown"),
                    "Stomach",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(273.4867F, 135.418F, 314.635F),
                    new Vector3(0.42549F, 0.14232F, 0.06098F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScythe"),
                    "Chest",
                    new Vector3(-0.03308F, -0.09726F, -0.09856F),
                    new Vector3(350.556F, 265.27F, 47.07748F),
                    new Vector3(0.1326F, 0.1326F, 0.1326F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealWhileSafe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySnail"),
                    "Chest",
                    new Vector3(-0.0154F, 0.00386F, -0.08951F),
                    new Vector3(289.0062F, 134.8082F, 310.6563F),
                    new Vector3(0.06232F, 0.06232F, 0.06232F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealingPotion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                    "Chest",
                    new Vector3(0.33746F, 0.07496F, -0.13546F),
                    new Vector3(2.72144F, 26.42111F, 123.2781F),
                    new Vector3(0.03958F, 0.03958F, 0.03958F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Hoof"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHoof"),
                    "CalfR",
                    new Vector3(-0.23198F, -0.03454F, -0.00557F),
                    new Vector3(347.6306F, 92.22505F, 349.4429F),
                    new Vector3(0.06146F, 0.06146F, 0.06146F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IceRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIceRing"),
                    "Weapon1",
                    new Vector3(-0.00004F, -0.29257F, 0.00227F),
                    new Vector3(88.36865F, 180F, 180F),
                    new Vector3(1.05934F, 1.05934F, 1.05934F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Icicle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                    "Base",
                    new Vector3(-0.57335F, -0.35878F, 0.54711F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.14135F, 1.14135F, 1.14135F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IgniteOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasoline"),
                    "ThighR",
                    new Vector3(-0.21432F, 0.02654F, -0.05867F),
                    new Vector3(356.4352F, 101.9237F, 12.05293F),
                    new Vector3(0.3768F, 0.3768F, 0.3768F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ImmuneToDebuff"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                    "ThighL",
                    new Vector3(-0.3871F, -0.0046F, 0.00151F),
                    new Vector3(282.7988F, 36.4323F, 236.6014F),
                    new Vector3(0.44683F, 0.44683F, 0.44683F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseHealing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(-0.08814F, 0.00229F, -0.02002F),
                    new Vector3(10.80937F, 189.1713F, 296.6983F),
                    new Vector3(0.2711F, 0.2711F, 0.2711F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(-0.08126F, -0.02722F, 0.03102F),
                    new Vector3(326.4096F, 353.422F, 107.7479F),
                    new Vector3(0.2711F, 0.2711F, 0.2711F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Incubator"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                    "Chest",
                    new Vector3(0.46654F, -0.04898F, -0.02532F),
                    new Vector3(0F, 0F, 242.7011F),
                    new Vector3(0.0271F, 0.0271F, 0.0271F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Infusion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInfusion"),
                    "Stomach",
                    new Vector3(-0.11257F, -0.09326F, -0.07456F),
                    new Vector3(72.34042F, 81.6787F, 173.741F),
                    new Vector3(0.31418F, 0.31418F, 0.31418F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["JumpBoost"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaxBird"),
                    "Head",
                    new Vector3(0.28422F, -0.08865F, 0.00029F),
                    new Vector3(270.7214F, 105.572F, 344.4293F),
                    new Vector3(0.64393F, 0.64393F, 0.64393F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["KillEliteFrenzy"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(352.4864F, 359.8075F, 91.47167F),
                    new Vector3(0.18973F, 0.30654F, 0.18545F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Knurl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnurl"),
                    "UpperArmL",
                    new Vector3(-0.13667F, 0F, -0.0341F),
                    new Vector3(0F, 90.58415F, 0F),
                    new Vector3(0.06143F, 0.06143F, 0.06143F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LaserTurbine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                    "Chest",
                    new Vector3(-0.07936F, -0.00502F, 0.08767F),
                    new Vector3(1.93567F, 18.03741F, 84.07816F),
                    new Vector3(0.16202F, 0.16202F, 0.16202F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LightningStrikeOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                    "Weapon1",
                    new Vector3(-0.42028F, 5.29001F, -0.00003F),
                    new Vector3(270F, 270F, 0F),
                    new Vector3(5.49203F, 5.49203F, 5.49203F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarDagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                    "Chest",
                    new Vector3(0.10186F, -0.1191F, -0.10232F),
                    new Vector3(338.8077F, 145.1357F, 19.83542F),
                    new Vector3(0.41636F, 0.41636F, 0.41636F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPrimaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdEye"),
                    "Head",
                    new Vector3(-0.05757F, 0.0784F, 0F),
                    new Vector3(0F, 0F, 178.0567F),
                    new Vector3(0.17132F, 0.17132F, 0.17132F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSecondaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdClaw"),
                    "LowerArmR",
                    new Vector3(0.06801F, -0.01247F, -0.0108F),
                    new Vector3(15.80004F, 177.5419F, 44.8153F),
                    new Vector3(0.44315F, 0.44315F, 0.44315F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSun"],
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Chest",
                    new Vector3(-0.04936F, 0.00867F, -0.00285F),
                    new Vector3(52.65432F, 203.9559F, 302.8301F),
                    new Vector3(0.79016F, 0.79016F, 0.79016F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.70481F, 0.50603F, 0.50603F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSpecialReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                    "Base",
                    new Vector3(-0.25416F, 0F, -0.52881F),
                    new Vector3(0F, 29.1494F, 0F),
                    new Vector3(0.27284F, 0.27284F, 0.27284F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarTrinket"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeads"),
                    "Weapon2",
                    new Vector3(0.00108F, -0.01079F, 0.06368F),
                    new Vector3(358.6184F, 283.6354F, 187.8235F),
                    new Vector3(0.47496F, 0.47496F, 0.47496F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarUtilityReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                    "ThighR",
                    new Vector3(-0.37929F, -0.09009F, -0.03725F),
                    new Vector3(27.00071F, 356.6058F, 92.07581F),
                    new Vector3(0.52793F, 0.52793F, 0.52793F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Medkit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMedkit"),
                    "Stomach",
                    new Vector3(-0.09133F, 0.08399F, 0.08063F),
                    new Vector3(9.62198F, 266.6945F, 141.5418F),
                    new Vector3(0.33793F, 0.33793F, 0.33793F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MinorConstructOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
                    "Base",
                    new Vector3(-0.65144F, 0.42902F, 0.55856F),
                    new Vector3(80.96356F, 287.6261F, 17.42066F),
                    new Vector3(0.37053F, 0.37053F, 0.37053F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Missile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                    "Chest",
                    new Vector3(-0.13292F, -0.30816F, 0.26233F),
                    new Vector3(332.872F, 274.6673F, 210.6637F),
                    new Vector3(0.08496F, 0.08496F, 0.08496F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MissileVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                    "Chest",
                    new Vector3(-0.13292F, -0.30816F, 0.26233F),
                    new Vector3(332.872F, 274.6673F, 210.6637F),
                    new Vector3(0.08496F, 0.08496F, 0.08496F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MonstersOnShrineUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                    "Chest",
                    new Vector3(0.10718F, -0.06976F, -0.14651F),
                    new Vector3(318.5956F, 241.9845F, 55.37966F),
                    new Vector3(0.06832F, 0.06832F, 0.06832F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoreMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayICBM"),
                    "Weapon2",
                    new Vector3(0.00001F, -0.00001F, -0.00724F),
                    new Vector3(270.0343F, 0F, 0F),
                    new Vector3(0.05051F, 0.03542F, 0.05051F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoveSpeedOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                    "ThighR",
                    new Vector3(-0.07882F, -0.07285F, -0.03515F),
                    new Vector3(340.9146F, 217.0527F, 349.2076F),
                    new Vector3(0.15008F, 0.15008F, 0.15008F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Mushroom"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroom"),
                    "UpperArmL",
                    new Vector3(-0.21452F, -0.01852F, 0F),
                    new Vector3(359.8263F, 176.3329F, 177.2911F),
                    new Vector3(0.04849F, 0.04849F, 0.04849F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MushroomVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                    "UpperArmL",
                    new Vector3(-0.21452F, -0.01852F, 0F),
                    new Vector3(359.8263F, 176.3329F, 177.2911F),
                    new Vector3(0.04849F, 0.04849F, 0.04849F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NearbyDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDiamond"),
                    "Weapon1",
                    new Vector3(-0.00007F, -1.06143F, 0F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.34213F, 0.34213F, 0.34213F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Head",
                    new Vector3(-0.00049F, 0.00381F, -0.0324F),
                    new Vector3(277.5947F, 332.854F, 117.8132F),
                    new Vector3(0.33344F, 0.33344F, 0.33344F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Head",
                    new Vector3(0.00012F, 0.00236F, 0.0321F),
                    new Vector3(276.4F, 213.8822F, 236.4123F),
                    new Vector3(-0.33344F, 0.33344F, 0.33344F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                    "Chest",
                    new Vector3(-0.08416F, 0.00222F, -0.03143F),
                    new Vector3(3.78738F, 339.8946F, 67.3221F),
                    new Vector3(0.06341F, 0.06341F, 0.06341F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["OutOfCombatArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                    "Chest",
                    new Vector3(0.1417F, 0.17515F, -0.00002F),
                    new Vector3(273.0986F, 49.45996F, 40.49522F),
                    new Vector3(0.16797F, 0.16797F, 0.16797F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ParentEgg"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayParentEgg"),
                    "Stomach",
                    new Vector3(-0.10783F, 0.16496F, -0.00002F),
                    new Vector3(359.8991F, 357.5553F, 93.34335F),
                    new Vector3(0.06368F, 0.06368F, 0.06368F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Pearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPearl"),
                    "Weapon2",
                    new Vector3(0.0015F, 0.00082F, 0.10955F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.03185F, 0.03185F, 0.03185F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PermanentDebuffOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScorpion"),
                    "Chest",
                    new Vector3(-0.06522F, -0.03391F, 0.00002F),
                    new Vector3(316.1097F, 87.57201F, 1.6803F),
                    new Vector3(0.69564F, 0.69564F, 0.69564F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PersonalShield"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Chest",
                    new Vector3(0.017F, 0.13226F, -0.07194F),
                    new Vector3(19.15755F, 267.4608F, 197.797F),
                    new Vector3(0.10545F, 0.10545F, 0.10545F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Phasing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStealthkit"),
                    "ThighL",
                    new Vector3(-0.04001F, -0.09112F, 0.08379F),
                    new Vector3(316.4637F, 333.8938F, 22.97336F),
                    new Vector3(0.18643F, 0.18643F, 0.18643F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Plant"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                    "ThighL",
                    new Vector3(-0.08009F, 0.03541F, 0.09696F),
                    new Vector3(335.8869F, 0F, 0F),
                    new Vector3(0.05146F, 0.05146F, 0.05146F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PrimarySkillShuriken"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShuriken"),
                    "Weapon1",
                    new Vector3(-0.00004F, 5.60065F, -0.00001F),
                    new Vector3(0F, 0F, 42.54298F),
                    new Vector3(3.01001F, 3.01001F, 3.01001F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomDamageZone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                    "Chest",
                    new Vector3(0.01378F, -0.16449F, -0.00026F),
                    new Vector3(276.7775F, 268.7126F, 180F),
                    new Vector3(0.06611F, 0.06611F, 0.06611F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomEquipmentTrigger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                    "Stomach",
                    new Vector3(-0.08825F, -0.03055F, 0.13065F),
                    new Vector3(47.06321F, 3.02906F, 145.5836F),
                    new Vector3(0.12922F, 0.12922F, 0.12922F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomlyLunar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDomino"),
                    "Base",
                    new Vector3(-0.53483F, 0.70248F, -0.46068F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RegeneratingScrap"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                    "Pelvis",
                    new Vector3(0.03842F, -0.09118F, -0.10504F),
                    new Vector3(345.6934F, 298.993F, 172.4313F),
                    new Vector3(0.23735F, 0.23735F, 0.23735F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RepeatHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCorpseflower"),
                    "Head",
                    new Vector3(-0.01523F, -0.04884F, 0.0607F),
                    new Vector3(60.63478F, 241.9955F, 238.6078F),
                    new Vector3(0.18921F, 0.18921F, 0.18921F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SecondarySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "Weapon1",
                    new Vector3(-1.11932F, 0.9232F, 0.03563F),
                    new Vector3(316.7937F, 90F, 0F),
                    new Vector3(0.37788F, 0.37788F, 0.37788F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Seed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySeed"),
                    "UpperArmR",
                    new Vector3(-0.21146F, -0.11173F, 0.00186F),
                    new Vector3(48.4791F, 90.64008F, 179.8186F),
                    new Vector3(0.0466F, 0.0466F, 0.0466F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShieldOnly"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(-0.05696F, -0.0162F, 0.05616F),
                    new Vector3(2.64936F, 358.8113F, 104.1655F),
                    new Vector3(0.20791F, 0.20791F, -0.20791F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(-0.05464F, -0.01103F, -0.05545F),
                    new Vector3(19.52339F, 140.4061F, 241.6312F),
                    new Vector3(-0.20791F, 0.20791F, 0.20791F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShinyPearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                    "Weapon2",
                    new Vector3(0.0015F, 0.00082F, 0.15858F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.03185F, 0.03185F, 0.03185F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                    "Chest",
                    new Vector3(0.05391F, -0.09344F, -0.11145F),
                    new Vector3(342.7508F, 257.3364F, 156.4554F),
                    new Vector3(0.31822F, 0.31822F, 0.31822F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SiphonOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                    "ThighL",
                    new Vector3(-0.07012F, -0.12082F, -0.02354F),
                    new Vector3(86.50252F, 125.313F, 35.87975F),
                    new Vector3(0.04928F, 0.04928F, 0.04928F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBauble"),
                    "ThighR",
                    new Vector3(-0.37085F, -0.12037F, -0.27551F),
                    new Vector3(67.68509F, 330.4566F, 254.6695F),
                    new Vector3(0.29434F, 0.29434F, 0.29434F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                    "ThighR",
                    new Vector3(-0.37085F, -0.12037F, -0.27551F),
                    new Vector3(67.68509F, 330.4566F, 254.6695F),
                    new Vector3(0.29434F, 0.29434F, 0.29434F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBuckler"),
                    "LowerArmR",
                    new Vector3(-0.05956F, 0F, 0.00471F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.12884F, 0.12884F, 0.10943F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySoda"),
                    "ThighL",
                    new Vector3(-0.16451F, 0.01251F, 0.0845F),
                    new Vector3(48.4608F, 76.2423F, 352.2041F),
                    new Vector3(0.20126F, 0.20126F, 0.20126F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintOutOfCombat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWhip"),
                    "Pelvis",
                    new Vector3(-0.02088F, 0.08482F, -0.13579F),
                    new Vector3(313.5333F, 344.2413F, 139.483F),
                    new Vector3(0.24102F, 0.24102F, 0.24102F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintWisp"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                    "UpperArmL",
                    new Vector3(-0.10021F, 0.06967F, -0.00856F),
                    new Vector3(276.0522F, 207.7586F, 241.1779F),
                    new Vector3(0.11252F, 0.11252F, 0.11252F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Squid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                    "ThighL",
                    new Vector3(-0.03824F, 0.08333F, 0.04903F),
                    new Vector3(326.709F, 204.4014F, 330.0656F),
                    new Vector3(0.03508F, 0.03508F, 0.03508F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StickyBomb"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                    "ThighR",
                    new Vector3(-0.11271F, -0.04039F, 0.09211F),
                    new Vector3(359.5684F, 351.5864F, 267.0843F),
                    new Vector3(0.17605F, 0.17605F, 0.17605F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StrengthenBurn"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasTank"),
                    "Stomach",
                    new Vector3(-0.11088F, -0.00745F, -0.13458F),
                    new Vector3(0F, 184.5643F, 316.3503F),
                    new Vector3(0.09983F, 0.09983F, 0.09983F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StunChanceOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                    "CalfL",
                    new Vector3(-0.16393F, 0.07841F, 0.03764F),
                    new Vector3(348.4774F, 96.99784F, 0F),
                    new Vector3(0.54443F, 0.54443F, 0.54443F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Syringe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                    "Pelvis",
                    new Vector3(0.0115F, -0.0868F, -0.05007F),
                    new Vector3(29.2408F, 202.8279F, 172.7485F),
                    new Vector3(0.12552F, 0.12552F, 0.12552F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TPHealingNova"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                    "CalfR",
                    new Vector3(-0.12784F, -0.00002F, -0.04384F),
                    new Vector3(0F, 181.2885F, 0F),
                    new Vector3(0.51094F, 0.51094F, 0.51094F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Talisman"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTalisman"),
                    "Base",
                    new Vector3(-0.00618F, 0.63525F, -0.97044F),
                    new Vector3(273.7931F, -0.00106F, 90.05895F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Thorns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                    "Chest",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TitanGoldDuringTP"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                    "LowerArmL",
                    new Vector3(-0.11036F, 0.00594F, -0.05133F),
                    new Vector3(6.01114F, 211.5156F, 167.1084F),
                    new Vector3(0.16295F, 0.16295F, 0.16295F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tooth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothNecklaceDecal"),
                    "Chest",
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                    "Chest",
                    new Vector3(-0.02876F, 0.09061F, -0.00215F),
                    new Vector3(297.75F, 302.9919F, 146.0738F),
                    new Vector3(1.85182F, 1.85182F, 1.85182F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(-0.02876F, 0.09061F, -0.00215F),
                    new Vector3(297.75F, 302.9919F, 146.0738F),
                    new Vector3(1.85182F, 1.85182F, 1.85182F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(-0.02876F, 0.09061F, -0.00215F),
                    new Vector3(297.75F, 302.9919F, 146.0738F),
                    new Vector3(1.85182F, 1.85182F, 1.85182F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(-0.02876F, 0.09061F, -0.00215F),
                    new Vector3(297.75F, 302.9919F, 146.0738F),
                    new Vector3(1.85182F, 1.85182F, 1.85182F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(-0.02876F, 0.09061F, -0.00215F),
                    new Vector3(297.75F, 302.9919F, 146.0738F),
                    new Vector3(1.85182F, 1.85182F, 1.85182F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCache"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKey"),
                    "ThighL",
                    new Vector3(-0.14021F, -0.06508F, -0.08526F),
                    new Vector3(59.17723F, 33.03851F, 22.66089F),
                    new Vector3(0.74328F, 0.74328F, 0.74328F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCacheVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                    "ThighL",
                    new Vector3(-0.14021F, -0.06508F, -0.08526F),
                    new Vector3(59.17723F, 33.03851F, 22.66089F),
                    new Vector3(0.74328F, 0.74328F, 0.74328F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "Chest",
                    new Vector3(0F, 0F, 0.08919F),
                    new Vector3(350.852F, 8.47526F, 294.7631F),
                    new Vector3(0.438F, 0.438F, 0.438F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "Chest",
                    new Vector3(-0.00007F, -0.0002F, -0.04558F),
                    new Vector3(6.66802F, 328.9416F, 279.8497F),
                    new Vector3(0.438F, 0.438F, 0.438F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VoidMegaCrabItem"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                    "UpperArmL",
                    new Vector3(-0.13371F, -0.00969F, -0.05004F),
                    new Vector3(7.38821F, 158.979F, 98.27061F),
                    new Vector3(0.12946F, 0.12946F, 0.12946F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WarCryOnMultiKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPauldron"),
                    "UpperArmL",
                    new Vector3(0.03958F, 0.00274F, 0.00404F),
                    new Vector3(354.2749F, 88.56773F, 292.0291F),
                    new Vector3(0.54331F, 0.54331F, 0.54331F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WardOnLevel"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarbanner"),
                    "Stomach",
                    new Vector3(-0.05886F, -0.0751F, 0.00435F),
                    new Vector3(7.87043F, 271.5752F, 94.1637F),
                    new Vector3(0.25155F, 0.25155F, 0.25155F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BFG"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBFG"),
                    "Chest",
                    new Vector3(-0.05241F, -0.05763F, -0.0891F),
                    new Vector3(274.0755F, 297.032F, 128.4786F),
                    new Vector3(0.26567F, 0.26567F, 0.26567F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Blackhole"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravCube"),
                    "Base",
                    new Vector3(-0.97842F, 0.06183F, 0.61138F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornGhost"),
                    "Head",
                    new Vector3(-0.12285F, -0.04926F, 0.00027F),
                    new Vector3(278.572F, 95.48124F, 350.7148F),
                    new Vector3(0.55372F, 0.55372F, 0.55372F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                    "Base",
                    new Vector3(-0.14879F, 0F, 0.75265F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunterConsumed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                    "Head",
                    new Vector3(-0.12285F, -0.04926F, 0.00027F),
                    new Vector3(278.572F, 95.48124F, 350.7148F),
                    new Vector3(0.55372F, 0.55372F, 0.55372F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BurnNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPotion"),
                    "Chest",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(288.8406F, 216.4799F, 168.8159F),
                    new Vector3(0.02355F, 0.02355F, 0.02355F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Cleanse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaterPack"),
                    "Chest",
                    new Vector3(0.14395F, -0.11461F, 0.0835F),
                    new Vector3(70.92757F, 39.65055F, 133.8359F),
                    new Vector3(0.03591F, 0.03591F, 0.03591F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CommandMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileRack"),
                    "Chest",
                    new Vector3(0.12244F, -0.1298F, 0.06633F),
                    new Vector3(359.3361F, 88.01109F, 173.3083F),
                    new Vector3(0.29028F, 0.29028F, 0.29028F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CrippleWard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEffigy"),
                    "Chest",
                    new Vector3(0.1873F, -0.08521F, 0.10441F),
                    new Vector3(295.8356F, 220.1704F, 200.4775F),
                    new Vector3(0.29254F, 0.29254F, 0.29254F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                "Head",
                    new Vector3(-0.03174F, 0.15514F, 0.0035F), 
                    new Vector3(85.88844F, -0.00067F, 90.00305F),
                    new Vector3(0.25319F, 0.25319F, 0.25319F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathProjectile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                "Chest",
                    new Vector3(0.16766F, -0.12286F, 0.08525F),
                    new Vector3(69.67999F, 50.26144F, 163.9764F),
                    new Vector3(0.06357F, 0.06357F, 0.06357F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DroneBackup"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRadio"),
                "Chest",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(72.18387F, 111.6263F, 175.5474F),
                    new Vector3(0.40563F, 0.40563F, 0.40563F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteEarthEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
                "Head",
                    new Vector3(-0.03547F, 0.01515F, 0.00062F),
                    new Vector3(66.78201F, 265.3508F, 354.8017F),
                    new Vector3(0.56731F, 0.56731F, 0.56731F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteFireEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                "Head",
                    new Vector3(-0.03184F, 0.00922F, -0.0449F),
                    new Vector3(299.5107F, 65.9709F, 358.9877F),
                    new Vector3(0.08431F, 0.08431F, 0.08431F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                "Head",
                    new Vector3(-0.00778F, -0.00009F, 0.02929F),
                    new Vector3(303.0084F, 129.0667F, 345.5892F),
                    new Vector3(-0.08431F, 0.08431F, 0.08431F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteHauntedEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                "Head",
                    new Vector3(-0.1659F, -0.04074F, 0.00015F),
                    new Vector3(359.1253F, 268.6508F, 175.3329F),
                    new Vector3(0.03771F, 0.03771F, 0.03771F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteIceEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                "Head",
                    new Vector3(-0.1659F, -0.04074F, 0.00015F),
                    new Vector3(0.62398F, 269.5133F, 175.3432F),
                    new Vector3(0.027F, 0.027F, 0.027F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLightningEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                "Head",
                    new Vector3(-0.11848F, 0.03596F, 0.00849F),
                    new Vector3(0.4577F, 267.4763F, 175.3239F),
                    new Vector3(0.17012F, 0.17012F, 0.17012F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                "Head",
                    new Vector3(-0.12754F, -0.04059F, 0.00014F),
                    new Vector3(29.96698F, 268.4289F, 174.6443F),
                    new Vector3(0.11111F, 0.11111F, 0.11111F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLunarEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
                "Weapon2",
                    new Vector3(-0.0018F, -0.00003F, 0.00003F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.10973F, 0.10973F, 0.10973F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElitePoisonEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                "Head",
                    new Vector3(-0.11392F, -0.02106F, 0.00013F),
                    new Vector3(359.908F, 268.7842F, 184.6919F),
                    new Vector3(0.03412F, 0.03412F, 0.03412F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteVoidEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                "Head",
                    new Vector3(-0.06073F, 0.05887F, 0.00009F),
                    new Vector3(6.68977F, 271.4023F, 4.85081F),
                    new Vector3(0.10687F, 0.10687F, 0.10687F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireBallDash"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEgg"),
                "Chest",
                    new Vector3(0.15974F, -0.10132F, 0.07128F),
                    new Vector3(30.52946F, 262.4586F, 299.5043F),
                    new Vector3(0.17738F, 0.17738F, 0.17738F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Fruit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFruit"),
                "Chest",
                    new Vector3(0.28622F, -0.09301F, 0.06754F),
                    new Vector3(14.64435F, 185.3035F, 292.4649F),
                    new Vector3(0.1661F, 0.1661F, 0.1661F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GainArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                "Chest",
                    new Vector3(0.20489F, -0.13018F, 0.07082F),
                    new Vector3(359.7424F, 278.5154F, 208.6419F),
                    new Vector3(0.40787F, 0.40787F, 0.40787F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Gateway"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVase"),
                "Chest",
                    new Vector3(0.1693F, -0.12095F, 0.06688F),
                    new Vector3(26.50218F, 186.884F, 241.7023F),
                    new Vector3(0.151F, 0.151F, 0.151F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldGat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldGat"),
                "Chest",
                    new Vector3(-0.23025F, -0.028F, -0.1179F),
                    new Vector3(354.6325F, 181.3255F, 264.8886F),
                    new Vector3(0.06294F, 0.06294F, 0.06294F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GummyClone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGummyClone"),
                "Chest",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(284.2619F, 308.9692F, 158.3721F),
                    new Vector3(0.16579F, 0.16579F, 0.16579F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IrradiatingLaser"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
                "Chest",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(288.8406F, 216.4799F, 168.8159F),
                    new Vector3(0.02355F, 0.02355F, 0.02355F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Jetpack"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBugWings"),
                "Chest",
                    new Vector3(0.02232F, -0.198F, 0.0004F),
                    new Vector3(341.8364F, 273.0865F, 359.1889F),
                    new Vector3(0.11712F, 0.11712F, 0.11712F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LifestealOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                "Chest",
                    new Vector3(0.13591F, -0.11995F, 0.0319F),
                    new Vector3(273.6542F, 239.7232F, 174.4895F),
                    new Vector3(0.09251F, 0.09251F, 0.09251F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Lightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                "UpperArmR",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(309.9693F, 200.9844F, 205.8545F),
                    new Vector3(0.59543F, 0.59543F, 0.59543F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPortalOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
                "Base",
                    new Vector3(-0.07429F, 0.18084F, 0.69831F),
                    new Vector3(288.8406F, 283.5468F, 168.8159F),
                    new Vector3(0.73365F, 0.73365F, 0.73365F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Meteor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteor"), 
                "Base",
                    new Vector3(-0.55825F, 0.20828F, 0.85794F),
                    new Vector3(288.8406F, 216.4799F, 168.8159F),
                    new Vector3(0.70175F, 0.70175F, 0.70175F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Molotov"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMolotov"),
                "Chest",
                    new Vector3(0.22696F, -0.11999F, 0.03676F),
                    new Vector3(74.16283F, 71.43959F, 147.375F),
                    new Vector3(0.19197F, 0.19197F, 0.19197F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MultiShopCard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                "Chest",
                    new Vector3(0.13594F, -0.11987F, 0.07037F),
                    new Vector3(39.66876F, 77.09492F, 149.3253F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["QuestVolatileBattery"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                "Chest",
                    new Vector3(0.07987F, -0.06247F, -0.00336F),
                    new Vector3(86.91802F, 180F, 180F),
                    new Vector3(0.25982F, 0.25982F, 0.25982F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Recycle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRecycler"),
                "Chest",
                    new Vector3(0.16128F, -0.11206F, 0.05585F),
                    new Vector3(353.6138F, 130.504F, 229.9137F),
                    new Vector3(0.04379F, 0.04379F, 0.04379F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Saw"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
                "Base",
                    new Vector3(-0.13317F, 0.06781F, 0.82615F),
                    new Vector3(7.49069F, 272.4546F, 99.77021F),
                    new Vector3(0.08395F, 0.08395F, 0.08395F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Scanner"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScanner"),
                "Chest",
                    new Vector3(0.15204F, -0.11691F, 0.07519F),
                    new Vector3(53.42557F, 42.03024F, 314.0683F),
                    new Vector3(0.09205F, 0.09205F, 0.09205F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TeamWarCry"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                "Chest",
                    new Vector3(0.16681F, -0.1251F, 0.07008F),
                    new Vector3(67.55209F, 56.33176F, 180.2672F),
                    new Vector3(0.03767F, 0.03767F, 0.03767F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tonic"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTonic"),
                "Chest",
                    new Vector3(0.16517F, -0.11428F, 0.06912F),
                    new Vector3(296.7336F, 204.7305F, 245.3597F),
                    new Vector3(0.15088F, 0.15088F, 0.15088F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VendingMachine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                "Chest",
                    new Vector3(0.12321F, -0.12622F, 0.07193F),
                    new Vector3(62.56076F, 79.48996F, 163.741F),
                    new Vector3(0.1224F, 0.1224F, 0.1224F)
                    )
                ));
        }
    }
}