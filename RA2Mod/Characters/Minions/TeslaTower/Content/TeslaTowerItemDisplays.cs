using RA2Mod;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Minions.TeslaTower
{
    public class TeslaTowerItemDisplays : ItemDisplaysBase
    {

        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {

            /*for custom copy format in keb's helper
            {childName},
                                                                       {localPos}, 
                                                                       {localAngles},
                                                                       {localScale})
                                                                                         // for some reason idph can only paste one ) at the end
            */

            /*for items with multiple displays (with CreateDisplayRuleGroupWithRules):
            {childName},
                                               {localPos}, 
                                               {localAngles},
                                               {localScale})
            */

            #region items

            #region dlc0
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AlienHead"], "DisplayAlienHead",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.12195F, 1.83474F, 0.02626F),
                                                                       new Vector3(286.0501F, 175.2631F, 195.4978F),
                                                                       new Vector3(0.84128F, 0.84128F, 0.84128F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ArmorPlate"], "DisplayRepulsionArmorPlate",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.16755F, 0.76987F, 0.13001F),
                                                                       new Vector3(281.5381F, 194.4372F, 211.2772F),
                                                                       new Vector3(0.28372F, 0.28372F, 0.23368F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorReductionOnHit"],
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                                               "Base Pillar Items 2",
                                               new Vector3(0.14994F, 1.38776F, 0.06917F),
                                               new Vector3(278.3626F, 272.4615F, 112.437F),
                                               new Vector3(0.12638F, 0.14647F, 0.11705F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AttackSpeedOnCrit"], "DisplayWolfPelt",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.11493F, 2.13755F, -0.10622F),
                                                                       new Vector3(23.00055F, -0.00001F, -0.00001F),
                                                                       new Vector3(0.35065F, 0.33159F, 0.34689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AutoCastEquipment"], "DisplayFossil",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.22101F, 1.01383F, -0.00188F),
                                                                       new Vector3(0.37674F, 4.57783F, 355.3054F),
                                                                       new Vector3(0.538F, 0.538F, 0.538F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Bandolier"], "DisplayBandolier",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.0372F, 1.66879F, -0.01848F),
                                                                       new Vector3(270F, 353.8612F, 0F),
                                                                       new Vector3(0.88198F, 1.2875F, 0.83608F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BarrierOnKill"], "DisplayBrooch",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.15479F, 1.16892F, 0.05377F),
                                                                       new Vector3(78.58831F, 340.5514F, 340.9078F),
                                                                       new Vector3(0.51673F, 0.44291F, 0.44291F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BarrierOnOverHeal"], "DisplayAegis",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.21549F, 0.61307F, 0.15459F),
                                                                       new Vector3(281.0792F, 167.9977F, 160.3025F),
                                                                       new Vector3(0.17978F, 0.17978F, 0.17978F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Bear"], "DisplayBear",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.06049F, 0.4541F, 0.16859F),
                                                                       new Vector3(0.23775F, 0F, 0F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BearVoid"], "DisplayBearVoid",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.06049F, 0.4541F, 0.16859F),
                                                                       new Vector3(0.23775F, 0F, 0F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BeetleGland"], "DisplayBeetleGland",
            //                                                           "Base Pillar Items 2",
            //                                                           new Vector3(0.12715F, 0.1495F, -0.10074F),
            //                                                           new Vector3(322.6508F, 250.1892F, 155.5382F),
            //                                                           new Vector3(0.08223F, 0.08223F, 0.08223F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Behemoth"],
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                                               "Tower Circle Items 1",
                                                                       new Vector3(-0.01856F, 0.02557F, -0.81141F),
                                                                       new Vector3(355.2818F, 186.3016F, 90.00741F),
                                                                       new Vector3(0.0707F, 0.06064F, 0.0707F))));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHit"],
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "Tower Circle Items 1",
                                                                       new Vector3(0.53535F, 0.10435F, 0.40261F),
                                                                       new Vector3(80.39637F, 76.14368F, 202.7981F),
                                                                       new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitVoid"],
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                                               "Tower Circle Items 1",
                                                                       new Vector3(0.53535F, 0.10435F, 0.40261F),
                                                                       new Vector3(80.39637F, 76.14368F, 202.7981F),
                                                                       new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BleedOnHitAndExplode"], "DisplayBleedOnHitAndExplode",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.11185F, 2.1479F, -0.02351F),
                                                                       new Vector3(317.8096F, 17.28981F, 181.4668F),
                                                                       new Vector3(0.06488F, 0.06488F, 0.06488F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BonusGoldPackOnKill"], "DisplayTome",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(-0.09424F, 0.22502F, 0.23012F),
                                                                       new Vector3(352.5968F, 359.689F, 350.0272F),
                                                                       new Vector3(0.06581F, 0.06581F, 0.06581F)));


            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritGlasses"], "DisplayGlasses",
                                                                       "Head",
                                                                       new Vector3(-0.00734F, -0.54642F, 0.02669F),
                                                                       new Vector3(85.51096F, 180F, 180F),
                                                                       new Vector3(0.79081F, 0.49978F, 0.63801F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritGlassesVoid"], "DisplayGlassesVoid",
                                                                       "Head",
                                                                       new Vector3(-0.00734F, -0.54642F, 0.02669F),
                                                                       new Vector3(85.51096F, 180F, 180F),
                                                                       new Vector3(0.79081F, 0.49978F, 0.63801F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Knurl"], "DisplayKnurl",
                                                                       "Center Orb Items",
                                                                       new Vector3(0F, 0.37373F, 0F),
                                                                       new Vector3(270F, 0.00001F, 0F),
                                                                       new Vector3(0.19261F, 0.19261F, 0.19261F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarSpecialReplacement"], "DisplayBirdHeart",
                                                                       "Center Orb Items",
                                                                       new Vector3(0.60769F, 0.69338F, -0.56221F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FireRing"], "DisplayFireRing",
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.93601F, 0F),
                                                                       new Vector3(90F, 311.4591F, 0F),
                                                                       new Vector3(1.79638F, 1.79638F, 1.79638F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["IceRing"], "DisplayIceRing", //did
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.36372F, 0F),
                                                                       new Vector3(90F, 317.7292F, 0F),
                                                                       new Vector3(1.85154F, 1.82009F, 1.82009F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Icicle"], "DisplayFrostRelic", //did
                                                                       "Center Orb Items",
                                                                       new Vector3(0.50918F, 0.61061F, 0.68821F),
                                                                       new Vector3(-0.00027F, -0.00004F, 270.5284F),
                                                                       new Vector3(2F, 2F, 2F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SecondarySkillMagazine"], "DisplayDoubleMag",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.21797F, 2.04043F, -0.51536F),
                                                                       new Vector3(284.4332F, 24.39617F, 173.9319F),
                                                                       new Vector3(0.09266F, 0.07507F, 0.09583F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Thorns"],
                ItemDisplays.CreateDisplayRule("DisplayRazorwireLeft",
                                               "Tower Pole Items",
                                               new Vector3(0.02491F, 2.43398F, 0F),
                                               new Vector3(281.4276F, 270F, 90F),
                                               new Vector3(0.91738F, 0.91738F, 0.99915F)),
                ItemDisplays.CreateDisplayRule("DisplayRazorwireLeft",
                                               "Tower Pole Items",
                                               new Vector3(0.02491F, 3.20885F, 0F),
                                               new Vector3(281.4276F, 270F, 90F),
                                               new Vector3(0.91738F, 0.91738F, 0.99915F)),
                ItemDisplays.CreateDisplayRule("DisplayRazorwireLeft",
                                               "Tower Pole Items",
                                               new Vector3(0.02491F, 4.00314F, 0F),
                                               new Vector3(281.4276F, 270F, 90F),
                                               new Vector3(0.91738F, 0.91738F, 0.99915F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FocusConvergence"], "DisplayFocusedConvergence",
                                                                       "Center Orb Items",
                                                                       new Vector3(-0.55533F, 1.02293F, 0.76675F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.16F, 0.16F, 0.16F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Talisman"], "DisplayTalisman",
                                                                       "Center Orb Items",
                                                                       new Vector3(-0.75421F, 0.73147F, -0.65204F),
                                                                       new Vector3(0.90015F, 232.5897F, 356.8947F),
                                                                       new Vector3(1F, 1F, 1F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockNearby"],
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                                               "Head",
                                                                       new Vector3(0F, 0F, 0.57969F),
                                                                       new Vector3(90F, 0F, 0F),
                                                                       new Vector3(0.85851F, 0.77812F, 0.85851F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BossDamageBonus"], "DisplayAPRound",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.22192F, 1.21805F, -0.01648F),
                                                                       new Vector3(275.2023F, 237.2791F, 28.30476F),
                                                                       new Vector3(0.63484F, 0.63484F, 0.63484F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BounceNearby"], "DisplayHook",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.42614F, 1.97968F, 0.0163F),
                                                                       new Vector3(36.40829F, 273.5378F, 2.10163F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ChainLightning"], "DisplayUkulele",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.63999F, 0.01054F, -0.08867F),
                                                                       new Vector3(353.4633F, 258.7738F, 1.20806F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ChainLightningVoid"], "DisplayUkuleleVoid",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.63999F, 0.01054F, -0.08867F),
                                                                       new Vector3(353.4633F, 258.7738F, 1.20806F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Clover"], "DisplayClover",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.45241F, 0.29418F),
                                                                       new Vector3(334.5203F, 272.9179F, 336.9202F),
                                                                       new Vector3(0.65004F, 0.65004F, 0.65004F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CloverVoid"], "DisplayCloverVoid",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.45241F, 0.29418F),
                                                                       new Vector3(334.5203F, 272.9179F, 336.9202F),
                                                                       new Vector3(0.65004F, 0.65004F, 0.65004F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CooldownOnCrit"],
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                                               "Tower Circle Items 3",
                                                                       new Vector3(-0.00001F, 0.04427F, -0.52765F),
                                                                       new Vector3(273.6421F, 195.3425F, 347.8005F),
                                                                       new Vector3(0.26026F, 0.3316F, 0.2916F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Crowbar"], "DisplayCrowbar",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.38127F, 1.94494F, 0.62707F),
                                                                       new Vector3(65.92451F, 206.7916F, 350.2542F),
                                                                       new Vector3(0.35514F, 0.35514F, 0.35514F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Dagger"], "DisplayDagger",
                                                                       "Tower Circle Items 3",
                                                                       new Vector3(0.06265F, 0.08072F, 0.23106F),
                                                                       new Vector3(47.08094F, 246.5786F, 1.93198F),
                                                                       new Vector3(1.01515F, 1.01515F, -1.01515F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["DeathMark"], "DisplayDeathMark",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0F, 2.24824F, -0.13345F),
                                                                       new Vector3(280.6515F, 183.2082F, 174.8475F),
                                                                       new Vector3(0.05481F, 0.05481F, 0.05481F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"], "DisplayWarHorn",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.22347F, 1.059F, 0.16087F),
                                                                       new Vector3(2.13955F, 265.6363F, 339.9073F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EquipmentMagazine"], "DisplayBattery",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0533F, 1.70262F, 0.07699F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EquipmentMagazineVoid"], "DisplayFuelCellVoid",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0533F, 1.70262F, 0.07699F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExecuteLowHealthElite"], "DisplayGuillotine",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.03041F, 0.02747F, -0.61301F),
                                                                       new Vector3(280.7616F, 349.6049F, 195.709F),
                                                                       new Vector3(0.15428F, 0.15428F, 0.15252F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExplodeOnDeath"], "DisplayWilloWisp",
                                                                       "Tower Circle Items 3",
                                                                       new Vector3(0.56232F, -0.01616F, -0.10021F),
                                                                       new Vector3(355.1682F, 91.66299F, 356.5107F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExtraLife"], "DisplayHippo",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0F, 0.84776F, 0.09658F),
                                                                       new Vector3(347.9707F, 3.47827F, 11.20283F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExtraLifeVoid"], "DisplayHippoVoid",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0F, 0.84776F, 0.09658F),
                                                                       new Vector3(347.9707F, 3.47827F, 11.20283F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            //itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FallBoots,
            //    ItemDisplays.CreateDisplayRule("DisplayGravBoots",
            //                                   "Tower Pole Items",
            //                                   new Vector3(-0.00251F, 0.37538F, -0.00142F),
            //                                   new Vector3(356.3479F, 168.6573F, 171.8978F),
            //                                   new Vector3(0.32954F, 0.32954F, 0.32954F)),
            //    ItemDisplays.CreateDisplayRule("DisplayGravBoots",
            //                                   "Tower Pole Items",
            //                                   new Vector3(0.00199F, 0.37549F, 0.01848F),
            //                                   new Vector3(6.25589F, 22.00069F, 174.1347F),
            //                                   new Vector3(0.32954F, 0.32954F, 0.32954F)
            //    )));
            ////itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Feather,
            ////    ItemDisplays.CreateDisplayRule("DisplayFeather",
            ////                                   "Base Pillar Items 3",
            ////                                   new Vector3(0.02972F, 0.22344F, 0.04502F),
            ////                                   new Vector3(0.47761F, 233.9261F, 304.7706F),
            ////                                   new Vector3(-0.04399F, 0.02643F, 0.02588F))
            //    ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FireballsOnHit"], "DisplayFireballsOnHit",
                "Tower Circle Items 1",
                new Vector3(0.72202F, -0.09951F, -0.12706F),
                new Vector3(70.96928F, 96.40028F, 357.7675F),
                new Vector3(0.08588F, 0.08588F, 0.08588F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Firework"], "DisplayFirework",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0.27019F, 0.36856F, 0.05241F),
                                                                       new Vector3(299.2795F, 347.5153F, 329.2786F),
                                                                       new Vector3(0.24082F, 0.24082F, 0.24082F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FlatHealth"], "DisplaySteakCurved",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.09571F, 2.09672F, 0.00897F),
                                                                       new Vector3(357.0212F, 351.2194F, 180.0431F),
                                                                       new Vector3(0.12057F, 0.12057F, 0.12057F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GhostOnKill"], "DisplayMask",
                                                                       "Head",
                                                                       new Vector3(0.47888F, -0.02005F, 0.03394F),
                                                                       new Vector3(0F, 95.8286F, 90.00001F),
                                                                       new Vector3(1.48088F, 1.48088F, 1.1422F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GoldOnHit"], "DisplayBoneCrown",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0891F, 2.23077F, -0.32324F),
                                                                       new Vector3(351.5005F, 354.2409F, 21.81469F),
                                                                       new Vector3(0.83876F, 0.83197F, 0.71985F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HeadHunter"], "DisplaySkullCrown",
                "Center Cylinder Items",
                new Vector3(0.00093F, 0.95486F, -0.00924F),
                new Vector3(0F, 44.69136F, 0F),
                new Vector3(1.43419F, 0.00288F, 0.53448F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealOnCrit"], "DisplayScythe",
                "Tower Circle Items 2",
                new Vector3(0.57507F, 0.05548F, -0.2405F),
                new Vector3(318.6227F, 198.8829F, 86.57573F),
                new Vector3(0.19725F, 0.19012F, 0.19725F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealWhileSafe"], "DisplaySnail",
                "Base Pillar Items 4",
                new Vector3(0.14666F, 2.2179F, -0.25092F),
                new Vector3(345.152F, 186.6709F, 28.23785F),
                new Vector3(0.06654F, 0.06654F, 0.06654F)));

            //itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Hoof,
            //    ItemDisplays.CreateDisplayRule("DisplayHoof",
            //                                   "Tower Pole Items",
            //                                   new Vector3(-0.02149F, 0.35335F, -0.04254F),
            //                                   new Vector3(79.93871F, 359.8341F, 341.8235F),
            //                                   new Vector3(0.11376F, 0.10848F, 0.09155F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["IgniteOnKill"], "DisplayGasoline",
                "Base Pillar Items 2",
                new Vector3(0.08906F, 0.80477F, 0.1426F),
                new Vector3(282.4633F, 219.2414F, 56.33241F),
                new Vector3(0.49393F, 0.49393F, 0.49393F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseHealing"],
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                    "Head",
                    new Vector3(0.22819F, 0.20532F, 0.4216F),
                    new Vector3(7.27722F, 56.76645F, 31.95804F),
                    new Vector3(0.54949F, 0.54949F, 0.54949F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                    "Head",
                    new Vector3(-0.20702F, 0.20532F, 0.42159F),
                    new Vector3(353.0391F, 291.7709F, 296.1823F),
                    new Vector3(0.55232F, 0.55232F, 0.55232F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Incubator"], "DisplayAncestralIncubator",
                "Base Pillar Items 1",
                new Vector3(0.17208F, 1.26778F, -0.11132F),
                new Vector3(16.46702F, 16.01868F, 319.7823F),
                new Vector3(0.03839F, 0.03839F, 0.03839F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Infusion"], "DisplayInfusion",
                "Base Pillar Items 3",
                new Vector3(0.12594F, 0.77161F, 0.17482F),
                new Vector3(351.2162F, 3.85623F, 358.6136F),
                new Vector3(0.55983F, 0.55983F, 0.56498F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["JumpBoost"], "DisplayWaxBird",
            //                                                           "Head",
            //                                                           new Vector3(0F, -0.26665F, -0.06925F),
            //                                                           new Vector3(0F, 0F, 0F),
            //                                                           new Vector3(0.78163F, 0.78163F, 0.78163F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["KillEliteFrenzy"], "DisplayBrainstalk",
                "Head",
                new Vector3(0.07072F, 0F, 0.12917F),
                new Vector3(298.8424F, 81.91605F, 99.21015F),
                new Vector3(0.8319F, 1.17689F, 0.82605F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LaserTurbine"], "DisplayLaserTurbine",
                "Base Pillar Items 1",
                new Vector3(0.16131F, 1.73025F, -0.00889F),
                new Vector3(273.8917F, 279.1016F, 304.6547F),
                new Vector3(0.28718F, 0.28718F, 0.28718F)));
            //uh
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LightningStrikeOnHit"], "DisplayChargedPerforator",
                "Tower Circle Items 1",
                new Vector3(0.56274F, -0.03396F, -0.40049F),
                new Vector3(349.7067F, 130.554F, 0F),
                new Vector3(1.37899F, 1.37899F, 1.37899F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarDagger"], "DisplayLunarDagger",
                "Base Pillar Items 2",
                new Vector3(-0.17068F, 1.81185F, -0.01557F),
                new Vector3(32.61504F, 203.7614F, 95.6869F),
                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarPrimaryReplacement"], "DisplayBirdEye",
                "Head",
                new Vector3(-0.58134F, 0.05828F, 0.13176F),
                new Vector3(274.2981F, 102.4017F, 178.3634F),
                new Vector3(0.51693F, 0.51693F, 0.51693F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarSecondaryReplacement"], "DisplayBirdClaw",
                "Tower Circle Items 3",
                new Vector3(0.26557F, 0.06149F, -0.44262F),
                new Vector3(19.86829F, 72.36673F, 193.5589F),
                new Vector3(0.76927F, 0.76927F, 0.76927F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarTrinket"], "DisplayBeads",
                "Base Pillar Items 3",
                new Vector3(0.05881F, 1.27475F, 0.09195F),
                new Vector3(12.47539F, 158.354F, 310.8714F),
                new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarUtilityReplacement"], "DisplayBirdFoot",
                "Tower Circle Items 3",
                new Vector3(-0.68067F, 0.06271F, -0.04414F),
                new Vector3(20.5769F, 16.05076F, 24.0392F),
                new Vector3(1.1023F, 1.1023F, 1.1023F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Medkit"], "DisplayMedkit",
                "Base Pillar Items 4",
                new Vector3(-0.2363F, 0.60527F, 0.00262F),
                new Vector3(277.7881F, 173.5872F, 101.4044F),
                new Vector3(0.6F, 0.6F, 0.6F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Missile"], "DisplayMissileLauncher",
                "Tower Circle Items 1",
                new Vector3(-1.03845F, -0.01693F, 0F),
                new Vector3(278.1269F, 94.51868F, 357.4038F),
                new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MissileVoid"], "DisplayMissileLauncherVoid",
                "Tower Circle Items 1",
                new Vector3(-1.03845F, -0.01693F, 0F),
                new Vector3(278.1269F, 94.51868F, 357.4038F),
                new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MonstersOnShrineUse"], "DisplayMonstersOnShrineUse",
                "Base Pillar Items 4",
                new Vector3(-0.17546F, 1.48525F, 0.04709F),
                new Vector3(342.505F, 57.45448F, 359.2402F),
                new Vector3(0.07658F, 0.07658F, 0.07658F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Mushroom"],
                ItemDisplays.CreateDisplayRule("DisplayMushroom",
                    "Base Pillar Items 2",
                    new Vector3(0.51541F, 0.55842F, -0.52174F),
                    new Vector3(53.83402F, 47.78959F, 352.6951F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroom",
                    "Base Pillar Items 2",
                    new Vector3(0.51541F, 0.55842F, -1.51359F),
                    new Vector3(53.8341F, 130.7545F, 352.6952F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroom",
                    "Base Pillar Items 2",
                    new Vector3(-0.51028F, 0.55842F, -1.51359F),
                    new Vector3(53.83389F, 218.8743F, 352.6954F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroom",
                    "Base Pillar Items 2",
                    new Vector3(-0.51027F, 0.55842F, -0.51797F),
                    new Vector3(54.14387F, 310.3226F, 357.8461F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MushroomVoid"],
                ItemDisplays.CreateDisplayRule("DisplayMushroomVoid",
                    "Base Pillar Items 2",
                    new Vector3(0.51541F, 0.55842F, -0.52174F),
                    new Vector3(53.83402F, 47.78959F, 352.6951F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroomVoid",
                    "Base Pillar Items 2",
                    new Vector3(0.51541F, 0.55842F, -1.51359F),
                    new Vector3(53.8341F, 130.7545F, 352.6952F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroomVoid",
                    "Base Pillar Items 2",
                    new Vector3(-0.51028F, 0.55842F, -1.51359F),
                    new Vector3(53.83389F, 218.8743F, 352.6954F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F)),
                ItemDisplays.CreateDisplayRule("DisplayMushroomVoid",
                    "Base Pillar Items 2",
                    new Vector3(-0.51027F, 0.55842F, -0.51797F),
                    new Vector3(54.14387F, 310.3226F, 357.8461F),
                    new Vector3(0.08819F, 0.08819F, 0.08819F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["NearbyDamageBonus"], "DisplayDiamond",
                "Tower Circle Items 2",
                new Vector3(0F, -0.00019F, 0.01223F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.45618F, 0.45618F, 0.45618F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnHeal"],
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "Head",
                    new Vector3(0.26368F, -0.29918F, 0.24861F),
                    new Vector3(46.98225F, 160.6363F, 198.4732F),
                    new Vector3(1.08689F, 1.08689F, 1.08689F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "Head",
                    new Vector3(-0.26258F, -0.30283F, 0.24862F),
                    new Vector3(40.64878F, 210.166F, 171.4716F),
                    new Vector3(-1.08689F, 1.08689F, 1.08689F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["NovaOnLowHealth"], "DisplayJellyGuts",
                "Center Orb Items",
                new Vector3(0.06597F, 0.15596F, -0.27338F),
                new Vector3(301.804F, 127.671F, 188.8221F),
                new Vector3(0.21249F, 0.19301F, 0.1773F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ParentEgg"], "DisplayParentEgg",
            //                                                           "Center Cylinder Items",
            //                                                           new Vector3(0.0289F, -0.08568F, 0.41864F),
            //                                                           new Vector3(24.21236F, 82.34744F, 7.68247F),
            //                                                           new Vector3(0.09096F, 0.09096F, 0.09096F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Pearl"], "DisplayPearl",
                "Tower Pole Items",
                new Vector3(0F, 4.67966F, 0F),
                new Vector3(270F, 0F, 0F),
                new Vector3(0.25445F, 0.25445F, 0.25445F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ShinyPearl"], "DisplayShinyPearl",
                "Tower Pole Items",
                new Vector3(0F, 4.46878F, 0F),
                new Vector3(270F, 0F, 0F),
                new Vector3(0.27549F, 0.27549F, 0.27549F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["PersonalShield"], "DisplayShieldGenerator",
                "Base Pillar Items 4",
                new Vector3(0.06651F, 1.26096F, 0.11535F),
                new Vector3(279.6884F, 180F, 180F),
                new Vector3(0.17577F, 0.17577F, 0.17577F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Phasing"], "DisplayStealthkit",
                "Base Pillar Items 4",
                new Vector3(0.26837F, 1.15796F, -0.09757F),
                new Vector3(274.2249F, 231.3939F, 218.47F),
                new Vector3(0.30717F, 0.32704F, 0.32704F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Plant"], "DisplayInterstellarDeskPlant",
                "Base Pillar Items 2",
                new Vector3(0.02901F, 1.8732F, -0.02318F),
                new Vector3(338.1215F, 355.6318F, 1.124F),
                new Vector3(0.08447F, 0.08351F, 0.08351F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomDamageZone"], "DisplayRandomDamageZone",
                "Head",
                new Vector3(-0.00428F, 0.64035F, 0.00119F),
                new Vector3(74.00678F, 351.5429F, 353.7615F),
                new Vector3(0.12349F, 0.16068F, 0.16068F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RepeatHeal"], "DisplayCorpseFlower",
                "Base Pillar Items 4",
                new Vector3(-0.09343F, 0.53108F, 0.20008F),
                new Vector3(79.20382F, 0F, 0F),
                new Vector3(0.26073F, 0.26073F, 0.26073F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Seed"], "DisplaySeed",
                "Base Pillar Items 1",
                new Vector3(0.10127F, 1.26195F, 0.15204F),
                new Vector3(53.35876F, 0F, 0F),
                new Vector3(0.04528F, 0.04528F, 0.04528F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShieldOnly"],
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                    "Head",
                    new Vector3(0.22151F, -0.43766F, 0.29863F),
                    new Vector3(2.31435F, 88.85326F, 170.1495F),
                    new Vector3(0.34731F, 0.34731F, -0.34731F)),
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                    "Head",
                    new Vector3(-0.22036F, -0.44357F, 0.30641F),
                    new Vector3(1.73096F, 270.086F, 9.12577F),
                    new Vector3(0.34731F, -0.34731F, -0.34731F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SiphonOnLowHealth"], "DisplaySiphonOnLowHealth",
                "Base Pillar Items 1",
                new Vector3(-0.12888F, 1.4428F, -0.00006F),
                new Vector3(353.8603F, 169.8634F, 346.4065F),
                new Vector3(0.07441F, 0.07441F, 0.07441F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SlowOnHit"], "DisplayBauble",
                "Base Pillar Items 2",
                new Vector3(-0.42016F, 0.65198F, -0.00886F),
                new Vector3(354.2812F, 145.2101F, 351.8673F),
                new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintArmor"], "DisplayBuckler",
                "Base Pillar Items 1",
                new Vector3(-0.24142F, 0.78789F, -0.08124F),
                new Vector3(358.0662F, 277.6346F, 160.2807F),
                new Vector3(0.16504F, 0.16504F, 0.17364F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintBonus"], "DisplaySoda",
                "Center Cylinder Items",
                new Vector3(-0.26749F, 1.971F, 0.33715F),
                new Vector3(284.3611F, 233.97F, 323.4904F),
                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintOutOfCombat"], "DisplayWhip",
                "Center Cylinder Items",
                new Vector3(0.31023F, 1.89248F, -0.28582F),
                new Vector3(0.18984F, 42.7935F, 7.26023F),
                new Vector3(0.3416F, 0.3416F, 0.3416F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintWisp"], "DisplayBrokenMask",
                "Base Pillar Items 4",
                new Vector3(-0.20491F, 1.69711F, -0.25564F),
                new Vector3(6.07575F, 278.4805F, 12.87031F),
                new Vector3(0.15714F, 0.15714F, 0.15714F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Squid"], "DisplaySquidTurret",
                "Base Pillar Items 2",
                new Vector3(-0.25939F, 0.24929F, -0.12578F),
                new Vector3(312.6068F, 181.5555F, 278.2833F),
                new Vector3(0.06125F, 0.06125F, 0.06125F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StickyBomb"], "DisplayStickyBomb",
                "Tower Circle Items 2",
                new Vector3(0F, 0.02171F, 0.64254F),
                new Vector3(15.6987F, 354.6728F, 25.87285F),
                new Vector3(0.29399F, 0.29399F, 0.29399F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StunChanceOnHit"], "DisplayStunGrenade",
                "Base Pillar Items 3",
                new Vector3(0.26394F, 0.41456F, -0.20193F),
                new Vector3(279.5173F, 19.44482F, 70.03345F),
                new Vector3(0.7F, 0.7F, 0.7F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Syringe"], "DisplaySyringeCluster",
                "Center Orb Items",
                new Vector3(-0.23194F, 0.1928F, 0.24862F),
                new Vector3(325.976F, 201.993F, 305.0298F),
                new Vector3(0.15369F, 0.15369F, 0.15369F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TPHealingNova"], "DisplayGlowFlower",
                "Base Pillar Items 1",
                new Vector3(0.23182F, 0.33212F, 0.01518F),
                new Vector3(352.2631F, 76.92137F, 355.6303F),
                new Vector3(0.32806F, 0.32806F, 0.32806F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TitanGoldDuringTP"], "DisplayGoldHeart",
                "Base Pillar Items 2",
                new Vector3(-0.26652F, 0.78933F, -0.17898F),
                new Vector3(339.0721F, 271.1716F, 39.60462F),
                new Vector3(0.285F, 0.285F, 0.285F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tooth"],
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                    "Center Cylinder Items",
                    new Vector3(0.25992F, 1.98451F, 0.32514F),
                    new Vector3(357.9241F, 50.43452F, 1.12892F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "Center Cylinder Items",
                    new Vector3(0.25992F, 1.98451F, 0.32514F),
                    new Vector3(357.9241F, 50.43452F, 1.12892F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Center Cylinder Items",
                    new Vector3(0.25992F, 1.98451F, 0.32514F),
                    new Vector3(357.9241F, 50.43452F, 1.12892F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "Center Cylinder Items",
                    new Vector3(0.25992F, 1.98451F, 0.32514F),
                    new Vector3(357.9241F, 50.43452F, 1.12892F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Center Cylinder Items",
                    new Vector3(0.25992F, 1.98451F, 0.32514F),
                    new Vector3(357.9241F, 50.43452F, 1.12892F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TreasureCache"], "DisplayKey",
                "Base Pillar Items 1",
                new Vector3(0.18247F, 1.94495F, -0.18615F),
                new Vector3(32.94204F, 351.2929F, 271.0139F),
                new Vector3(0.95146F, 0.95146F, 0.95146F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TreasureCacheVoid"], "DisplayKeyVoid",
                "Base Pillar Items 1",
                new Vector3(0.18247F, 1.94495F, -0.18615F),
                new Vector3(32.94204F, 351.2929F, 271.0139F),
                new Vector3(0.95146F, 0.95146F, 0.95146F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "Base Pillar Items 3",
                                               new Vector3(-0.07077F, 0.11496F, -0.08471F),
                                               new Vector3(85.10727F, 115.2514F, 217.2669F),
                                               new Vector3(1.15746F, 1.06979F, 1.15746F)),
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "Base Pillar Items 3",
                                               new Vector3(0.07879F, 0.1459F, -0.07698F),
                                               new Vector3(87.39181F, 239.0657F, 322.6216F),
                                               new Vector3(1.15746F, -1.06979F, 1.15746F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["WarCryOnMultiKill"], "DisplayPauldron",
                "Base Pillar Items 3",
                new Vector3(-0.16216F, 2.08765F, -0.06308F),
                new Vector3(281.0883F, 236.7797F, 257.3943F),
                new Vector3(0.59759F, 0.59759F, 0.59759F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["WardOnLevel"], "DisplayWarbanner",
                "Base Pillar Items 2",
                new Vector3(0.21625F, 1.60956F, -0.25575F),
                new Vector3(356.9693F, 0F, 2.04273F),
                new Vector3(0.5F, 0.5F, 0.5F)));

            //man there are a lot of these
            #endregion dlc0

            #region dlc1            

            //size
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"], "DisplayCoffee",
                "Base Pillar Items 4",
                new Vector3(0.26202F, 0.39386F, 0F),
                new Vector3(1.72852F, 194.4009F, 352.2361F),
                new Vector3(0.22904F, 0.22904F, 0.22904F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritDamage"], "DisplayLaserSight",
                "Tower Circle Items 2",
                new Vector3(-0.38904F, -0.00012F, 0.46421F),
                new Vector3(271.5071F, 257.4642F, 240.6037F),
                new Vector3(0.14349F, 0.14349F, 0.14349F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FragileDamageBonus"], "DisplayDelicateWatch",
                "Tower Pole Items",
                new Vector3(0F, 4.58982F, 0.01893F),
                new Vector3(89.7651F, 299.088F, 212.606F),
                new Vector3(2.16166F, 2.49614F, 2.03189F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FreeChest"], "DisplayShippingRequestForm",
                "Base Pillar Items 3",
                new Vector3(0.19999F, 1.61878F, -0.13984F),
                new Vector3(83.14848F, 0.27344F, 278.1145F),
                new Vector3(0.32389F, 0.5398F, 0.32389F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GoldOnHurt"], "DisplayRollOfPennies",
                "Base Pillar Items 1",
                new Vector3(0.24098F, 0.87626F, -0.05947F),
                new Vector3(1.14704F, 260.3747F, 13.05269F),
                new Vector3(0.70517F, 0.70517F, 0.70517F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"], "DisplayLunarShoulderNature",
                "Base Pillar Items 2",
                new Vector3(0.14997F, 2.25437F, -0.33591F),
                new Vector3(359.0807F, 178.8851F, 32.22269F),
                new Vector3(0.76408F, 0.76408F, 0.54522F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"], "DisplayLunarShoulderStone",
                "Base Pillar Items 2",
                new Vector3(-0.16049F, 2.31049F, -0.36117F),
                new Vector3(351.0297F, 0F, 0F),
                new Vector3(0.44941F, 0.44941F, 0.48309F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealingPotion"], "DisplayHealingPotion",
                "Base Pillar Items 2",
                new Vector3(0.2665F, 0.83508F, -0.14448F),
                new Vector3(356.3516F, 67.53532F, 22.20713F),
                new Vector3(0.05555F, 0.05555F, 0.05555F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ImmuneToDebuff"], "DisplayRainCoatBelt",
                "Base Pillar Items 1",
                new Vector3(-0.0718F, 1.91369F, -0.12838F),
                new Vector3(1.07281F, 77.79327F, 348.2528F),
                new Vector3(0.64681F, 0.64681F, 0.64681F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSun"],
                ItemDisplays.CreateDisplayRule("DisplaySunHead",
                    "Head",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(87.99718F, 179.9993F, 179.9992F),
                    new Vector3(3.46806F, 3.46805F, 3.46805F)),
                ItemDisplays.CreateDisplayRule("DisplaySunHeadNeck",
                    "Tower Circle Items 3",
                    new Vector3(0.09147F, 0.07838F, -0.04999F),
                    new Vector3(0.00038F, 113.8363F, 6.50182F),
                    new Vector3(2.69913F, 1.738F, 2.75935F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MinorConstructOnKill"], "DisplayDefenseNucleus",
                "Center Orb Items",
                new Vector3(0.87223F, 0.78232F, 0.2389F),
                new Vector3(-0.00001F, 20.31543F, 0.00003F),
                new Vector3(0.42591F, 0.42591F, 0.42591F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MoreMissile"], "DisplayICBM",
                "Tower Circle Items 2",
                new Vector3(0.54369F, -0.00014F, 0.37995F),
                new Vector3(297.4216F, 235.1873F, 86.46127F),
                new Vector3(0.16442F, 0.16442F, 0.16442F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MoveSpeedOnKill"], "DisplayGrappleHook",
                "Base Pillar Items 3",
                new Vector3(-0.11554F, 1.4008F, -0.03503F),
                new Vector3(272.8235F, 82.76733F, 179.998F),
                new Vector3(0.21889F, 0.21889F, 0.21889F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["OutOfCombatArmor"], "DisplayOddlyShapedOpal",
                "Base Pillar Items 2",
                new Vector3(-0.17964F, 0.619F, 0.17791F),
                new Vector3(353.2889F, 343.9033F, 356.5015F),
                new Vector3(0.35097F, 0.35097F, 0.35397F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["PermanentDebuffOnHit"], "DisplayScorpion",
                "Head",
                new Vector3(0F, 0.45619F, 0.27918F),
                new Vector3(26.70639F, 180F, 179.9999F),
                new Vector3(1.80308F, 1.80308F, 1.80308F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PrimarySkillShuriken"],
                ItemDisplays.CreateDisplayRule("DisplayShuriken",
                    "Tower Circle Items 1",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 9.12265F, 0F),
                    new Vector3(4.32477F, 4.32477F, 2.71916F)),
                ItemDisplays.CreateDisplayRule("DisplayShuriken",
                    "Tower Circle Items 2",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 9.12265F, 0F),
                    new Vector3(3.77506F, 3.77506F, 2.37353F)),
                ItemDisplays.CreateDisplayRule("DisplayShuriken",
                    "Tower Circle Items 3",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(90F, 9.12265F, 0F),
                    new Vector3(3.31178F, 3.31178F, 2.08225F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomEquipmentTrigger"], "DisplayBottledChaos",
                "Base Pillar Items 4",
                new Vector3(-0.22797F, 1.14629F, -0.22715F),
                new Vector3(0.00015F, 96.66051F, 10.29673F),
                new Vector3(0.21247F, 0.21247F, 0.21247F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomlyLunar"], "DisplayDomino",
                "Center Orb Items",
                new Vector3(-0.02238F, 0.72454F, -0.79059F),
                new Vector3(283.1516F, 0F, 0F),
                new Vector3(1.4558F, 1.4558F, 1.4558F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RegeneratingScrap"], "DisplayRegeneratingScrap",
                "Base Pillar Items 3",
                new Vector3(0.18009F, 2.2927F, -0.33996F),
                new Vector3(1.40897F, 253.9884F, 359.4492F),
                new Vector3(0.23956F, 0.23956F, 0.23956F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StrengthenBurn"], "DisplayGasTank",
                "Base Pillar Items 2",
                new Vector3(-0.21246F, 1.90778F, -0.25407F),
                new Vector3(330.1871F, 215.485F, 340.8301F),
                new Vector3(0.1444F, 0.1444F, 0.1444F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["VoidMegaCrabItem"], "DisplayMegaCrabItem",
                "Base Pillar Items 3",
                new Vector3(0.26469F, 0.73444F, -0.02599F),
                new Vector3(358.8192F, 84.68645F, 347.3702F),
                new Vector3(0.15797F, 0.15797F, 0.15797F)));

            #endregion dlc1

            #endregion items

            #region quips
            //don't need any besides aspects
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteFireEquipment"],
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                                                       new Vector3(0.20151F, -0.11489F, 0.45878F),
                                                                       new Vector3(350.427F, 182.9044F, 163.0342F),
                                                                       new Vector3(0.17951F, 0.17951F, 0.17951F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                                                       new Vector3(-0.20151F, -0.11489F, 0.4324F),
                                                                       new Vector3(350.3648F, 177.3109F, 195.6752F),
                                                                       new Vector3(-0.17951F, 0.17951F, 0.17951F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteHauntedEquipment"], "DisplayEliteStealthCrown",
                                                                 "Head",
                                                                       new Vector3(-0.00813F, 0.01963F, 0.67331F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.13404F, 0.13404F, 0.13404F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteIceEquipment"], "DisplayEliteIceCrown",
                                                                "Head",
                                                                       new Vector3(0F, 0F, 0.68394F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.07108F, 0.07108F, 0.07108F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLightningEquipment"],
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                                                       new Vector3(0F, -0.26289F, 0.52805F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.67703F, 0.68728F, 0.58008F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                                                       new Vector3(0F, 0.11166F, 0.5604F),
                                                                       new Vector3(339.066F, 0F, 0F),
                                                                       new Vector3(-0.45029F, 0.45713F, 0.35911F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteLunarEquipment"], "DisplayEliteLunar,Eye",
                                                                       "Head",
                                                                       new Vector3(0F, 0F, 0.8236F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.81075F, 0.81075F, 0.81075F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ElitePoisonEquipment"], "DisplayEliteUrchinCrown",
                                                                "Head",
                                                                       new Vector3(0F, 0F, 0.49957F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.14435F, 0.14435F, 0.14435F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteEarthEquipment"], "DisplayEliteMendingAntlers",
                                                                       "Head",
                                                                       new Vector3(0F, -0.15922F, 0.4271F),
                                                                       new Vector3(82.54549F, 180F, 180F),
                                                                       new Vector3(1.6972F, 1.6972F, 1.6972F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteVoidEquipment"], "DisplayAffixVoid",
                                                                       "Head",
                                                                       new Vector3(-0.00512F, -0.11767F, 0.46191F),
                                                                       new Vector3(89.78602F, 180F, 180F),
                                                                       new Vector3(0.64773F, 0.55528F, 0.58107F)));

            //only need elites
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack"], "DisplayBugWings",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.208f, 0.208f, 0),
            //                                                    new Vector3(0, 270, 0),
            //                                                    new Vector3(0.25f, 0.25f, 0.25f)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat"], "DisplayGoldGat",
            //                                                    "Base Pillar Items 3",
            //                                                    new Vector3(0.09687F, 0.32573F, 0.22415F),
            //                                                    new Vector3(279.4109F, 187.7572F, 345.6136F),
            //                                                    new Vector3(0.11175F, 0.11175F, 0.11175F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG"], "DisplayBFG",
            //                                                           "Center Cylinder Items",
            //                                                           new Vector3(0.07101F, 0.41512F, -0.19564F),
            //                                                           new Vector3(348.6131F, 1.95604F, 339.4123F),
            //                                                           new Vector3(0.4F, 0.4F, 0.4F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby"], "DisplayPotion",
            //                                                           "Center Cylinder Items",
            //                                                           new Vector3(-0.21288F, -0.09195F, -0.09708F),
            //                                                           new Vector3(345.6282F, 5.15145F, 313.8587F),
            //                                                           new Vector3(0.04021F, 0.04021F, 0.04021F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse"], "DisplayWaterPack", //brokey
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.32123F, 0.17569F, -0.01137F),
            //                                                    new Vector3(357.2928F, 90.24006F, 0.69147F),
            //                                                    new Vector3(0.1F, 0.1F, 0.1F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile"], "DisplayMissileRack",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.26506F, 0.45562F, 0.00002F),
            //                                                    new Vector3(90F, 90F, 0F),
            //                                                    new Vector3(0.5F, 0.5F, 0.5F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard"], "DisplayEffigy",
            //                                                           "Head",
            //                                                           new Vector3(-0.00062F, 0.16551F, -0.13266F),
            //                                                           new Vector3(34.73222F, 167.4949F, 348.1801F),
            //                                                           new Vector3(0.354F, 0.354F, 0.354F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery"], "DisplayBatteryArray",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.33257F, 0.3451F, -0.01117F),
            //                                                    new Vector3(315F, 90F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit"], "DisplayFruit",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.13026F, 0.28527F, -0.16847F),
            //                                                    new Vector3(356.3801F, 347.5225F, 216.8458F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse"], "DisplayNeuralImplant",
            //                                                    "Head",
            //                                                    new Vector3(-0.20606F, 0.06706F, -0.00143F),
            //                                                    new Vector3(0F, 90F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup"], "DisplayRadio",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(-0.24166F, 0.34219F, -0.08679F),
            //                                                    new Vector3(348.3908F, 272.9134F, 58.67701F),
            //                                                    new Vector3(0.4F, 0.4F, 0.4F)));
            //itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
            //    ItemDisplays.CreateDisplayRule("DisplayLightningArmCustom",
            //                                   "LightningArm1",
            //                                   new Vector3(0, 0, 0),
            //                                   new Vector3(0, 0, 0),
            //                                   new Vector3(0.8752531f, 0.8752531f, 0.8752531f))));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor"], "DisplayElephantFigure",
            //                                                    "Tower Pole Items",
            //                                                    new Vector3(-0.17336F, 0.08326F, -0.00223F),
            //                                                    new Vector3(90F, 268.5319F, 0F),
            //                                                    new Vector3(0.3F, 0.3F, 0.3F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle"], "DisplayRecycler",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.31706F, 0.37802F, 0.00421F),
            //                                                    new Vector3(0F, 0F, 348.9059F),
            //                                                    new Vector3(0.06F, 0.06F, 0.06F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash"], "DisplayEgg",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.08967F, 0.06729F, 0.29295F),
            //                                                    new Vector3(90F, 0F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic"], "DisplayTonic",
            //                                                    "Base Pillar Items 2",
            //                                                    new Vector3(-0.00001F, 0.2792F, 0.18109F),
            //                                                    new Vector3(359.9935F, 359.8932F, 180.1613F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway"], "DisplayVase",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.11973F, 0.54832F, 0.26252F),
            //                                                    new Vector3(359.2803F, 349.704F, 3.12542F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner"], "DisplayScanner",
            //                                                    "Head",
            //                                                    new Vector3(0.37265F, 0.86982F, 0.02202F),
            //                                                    new Vector3(293.9302F, 144.0041F, 339.115F),
            //                                                    new Vector3(0.25F, 0.25F, 0.25F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile"], "DisplayDeathProjectile",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(-0.23665F, 0.35165F, 0.12038F),
            //                                                    new Vector3(335.7568F, 279.3308F, 358.6632F),
            //                                                    new Vector3(0.04F, 0.04F, 0.04F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit"], "DisplayLifestealOnHit",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.49716F, -0.16366F, -0.1139F),
            //                                                    new Vector3(324.7334F, 298.6617F, 280.0614F),
            //                                                    new Vector3(0.1347F, 0.1347F, 0.1347F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry"], "DisplayTeamWarCry",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(-0.24951F, -0.12858F, -0.00915F),
            //                                                    new Vector3(9.0684F, 272.2467F, 359.5266F),
            //                                                    new Vector3(0.1F, 0.1F, 0.1F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Saw"], "DisplaySawmerangFollower",
            //                                                    "Root",
            //                                                    new Vector3(0.62211F, 0.43106F, 1.15734F),
            //                                                    new Vector3(358.3824F, 269.3275F, 292.5764F),
            //                                                    new Vector3(0.225F, 0.225F, 0.225F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Meteor"], "DisplayMeteor",
            //                                                    "Root",
            //                                                    new Vector3(1.04502F, 0.51043F, 0.67377F),
            //                                                    new Vector3(90F, 0F, 0F),
            //                                                    new Vector3(1.2F, 1.2F, 1.2F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Blackhole"], "DisplayGravCube",
            //                                                    "Root",
            //                                                    new Vector3(0.62211F, 0.43106F, 1.15734F),
            //                                                    new Vector3(358.3824F, 269.3275F, 292.5764F),
            //                                                    new Vector3(0.225F, 0.225F, 0.225F)));
            #endregion quips

            #region compat
            try
            {
                if (General.GeneralCompat.TinkersSatchelInstalled)
                {
                    SetTinkersSatchelDisplayRules(itemDisplayRules);
                }
            }
            catch (Exception e)
            {
                Log.Warning("error adding displays for Tinker's Satchel \n" + e);
            }

            //try {
            //    if (Compat.TinkersSatchelInstalled) {
            //        SetAetheriumDisplayRules(itemDisplayRules);
            //    }
            //}
            //catch (System.Exception e) {
            //    Helpers.LogWarning("error adding displays for Aetherium \n" + e);
            //}

            #endregion
        }
        #region tinker
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetTinkersSatchelDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.Moustache.instance.itemDef,
                                                                       ThinkInvisible.TinkersSatchel.Moustache.instance.idrPrefab,
                                                                       "Head",
                                                                       new Vector3(0.00258F, -0.57439F, -0.13698F),
                                                                       new Vector3(336.7115F, 242.5089F, 232.2233F),
                                                                       new Vector3(0.49463F, 0.49463F, 0.49463F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.itemDef,
                                                                       ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.idrPrefab,
                                                                       "Head",
                                                                        new Vector3(0.00258F, -0.54389F, -0.22055F),
                                                                        new Vector3(76.97079F, 170.091F, 169.2874F),
                                                                        new Vector3(0.64135F, 0.64135F, 0.64135F)));
        }
        #endregion tinker

        #region aeth
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetAetheriumDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {
        }
        #endregion aeth
    }
}
