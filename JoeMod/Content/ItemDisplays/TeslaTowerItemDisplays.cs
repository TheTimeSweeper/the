using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Modules.Characters {
    public class TeslaTowerItemDisplays : ItemDisplaysBase {

        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

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

            #region done
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AlienHead, "DisplayAlienHead",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.12195F, 1.83474F, 0.02626F),
                                                                       new Vector3(286.0501F, 175.2631F, 195.4978F),
                                                                       new Vector3(0.84128F, 0.84128F, 0.84128F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.16755F, 0.76987F, 0.13001F),
                                                                       new Vector3(281.5381F, 194.4372F, 211.2772F),
                                                                       new Vector3(0.28372F, 0.28372F, 0.23368F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                                               "Base Pillar Items 2",
                                               new Vector3(0.14994F, 1.38776F, 0.06917F),
                                               new Vector3(278.3626F, 272.4615F, 112.437F),
                                               new Vector3(0.12638F, 0.14647F, 0.11705F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.11493F, 2.13755F, -0.10622F),
                                                                       new Vector3(23.00055F, -0.00001F, -0.00001F),
                                                                       new Vector3(0.35065F, 0.33159F, 0.34689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.22101F, 1.01383F, -0.00188F),
                                                                       new Vector3(0.37674F, 4.57783F, 355.3054F),
                                                                       new Vector3(0.538F, 0.538F, 0.538F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.0372F, 1.66879F, -0.01848F),
                                                                       new Vector3(270F, 353.8612F, 0F),
                                                                       new Vector3(0.88198F, 1.2875F, 0.83608F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.15479F, 1.16892F, 0.05377F),
                                                                       new Vector3(78.58831F, 340.5514F, 340.9078F),
                                                                       new Vector3(0.51673F, 0.44291F, 0.44291F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.21549F, 0.61307F, 0.15459F),
                                                                       new Vector3(281.0792F, 167.9977F, 160.3025F),
                                                                       new Vector3(0.17978F, 0.17978F, 0.17978F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bear, "DisplayBear",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.06049F, 0.4541F, 0.16859F),
                                                                       new Vector3(0.23775F, 0F, 0F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.BearVoid, "DisplayBearVoid",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.06049F, 0.4541F, 0.16859F),
                                                                       new Vector3(0.23775F, 0F, 0F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland",
            //                                                           "Base Pillar Items 2",
            //                                                           new Vector3(0.12715F, 0.1495F, -0.10074F),
            //                                                           new Vector3(322.6508F, 250.1892F, 155.5382F),
            //                                                           new Vector3(0.08223F, 0.08223F, 0.08223F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                                               "Tower Circle Items 1",
                                                                       new Vector3(-0.01856F, 0.02557F, -0.81141F),
                                                                       new Vector3(355.2818F, 186.3016F, 90.00741F),
                                                                       new Vector3(0.0707F, 0.06064F, 0.0707F))));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "Tower Circle Items 1",
                                                                       new Vector3(0.53535F, 0.10435F, 0.40261F),
                                                                       new Vector3(80.39637F, 76.14368F, 202.7981F),
                                                                       new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                                               "Tower Circle Items 1",
                                                                       new Vector3(0.53535F, 0.10435F, 0.40261F),
                                                                       new Vector3(80.39637F, 76.14368F, 202.7981F),
                                                                       new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.11185F, 2.1479F, -0.02351F),
                                                                       new Vector3(317.8096F, 17.28981F, 181.4668F),
                                                                       new Vector3(0.06488F, 0.06488F, 0.06488F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(-0.09424F, 0.22502F, 0.23012F),
                                                                       new Vector3(352.5968F, 359.689F, 350.0272F),
                                                                       new Vector3(0.06581F, 0.06581F, 0.06581F)));


            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.CritGlasses, "DisplayGlasses",
                                                                       "Head",
                                                                       new Vector3(-0.00734F, -0.54642F, 0.02669F),
                                                                       new Vector3(85.51096F, 180F, 180F),
                                                                       new Vector3(0.79081F, 0.49978F, 0.63801F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritGlassesVoid, "DisplayGlassesVoid",
                                                                       "Head",
                                                                       new Vector3(-0.00734F, -0.54642F, 0.02669F),
                                                                       new Vector3(85.51096F, 180F, 180F),
                                                                       new Vector3(0.79081F, 0.49978F, 0.63801F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Knurl, "DisplayKnurl",
                                                                       "Center Orb Items",
                                                                       new Vector3(0F, 0.37373F, 0F),
                                                                       new Vector3(270F, 0.00001F, 0F),
                                                                       new Vector3(0.19261F, 0.19261F, 0.19261F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSpecialReplacement, "DisplayBirdHeart",
                                                                       "Center Orb Items",
                                                                       new Vector3(0.60769F, 0.69338F, -0.56221F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.93601F, 0F),
                                                                       new Vector3(90F, 311.4591F, 0F),
                                                                       new Vector3(1.79638F, 1.79638F, 1.79638F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing", //did
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.36372F, 0F),
                                                                       new Vector3(90F, 317.7292F, 0F),
                                                                       new Vector3(1.85154F, 1.82009F, 1.82009F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Icicle, "DisplayFrostRelic", //did
                                                                       "Center Orb Items",
                                                                       new Vector3(0.50918F, 0.61061F, 0.68821F),
                                                                       new Vector3(-0.00027F, -0.00004F, 270.5284F),
                                                                       new Vector3(2F, 2F, 2F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SecondarySkillMagazine, "DisplayDoubleMag",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.21797F, 2.04043F, -0.51536F),
                                                                       new Vector3(284.4332F, 24.39617F, 173.9319F),
                                                                       new Vector3(0.09266F, 0.07507F, 0.09583F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Thorns,
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

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                                                                       "Center Orb Items",
                                                                       new Vector3(-0.55533F, 1.02293F, 0.76675F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.16F, 0.16F, 0.16F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Talisman, "DisplayTalisman",
                                                                       "Center Orb Items",
                                                                       new Vector3(-0.75421F, 0.73147F, -0.65204F),
                                                                       new Vector3(0.90015F, 232.5897F, 356.8947F),
                                                                       new Vector3(1F, 1F, 1F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                                               "Head",
                                                                       new Vector3(0F, 0F, 0.57969F),
                                                                       new Vector3(90F, 0F, 0F),
                                                                       new Vector3(0.85851F, 0.77812F, 0.85851F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.22192F, 1.21805F, -0.01648F),
                                                                       new Vector3(275.2023F, 237.2791F, 28.30476F),
                                                                       new Vector3(0.63484F, 0.63484F, 0.63484F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.42614F, 1.97968F, 0.0163F),
                                                                       new Vector3(36.40829F, 273.5378F, 2.10163F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.63999F, 0.01054F, -0.08867F),
                                                                       new Vector3(353.4633F, 258.7738F, 1.20806F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ChainLightningVoid, "DisplayUkuleleVoid",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.63999F, 0.01054F, -0.08867F),
                                                                       new Vector3(353.4633F, 258.7738F, 1.20806F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.45241F, 0.29418F),
                                                                       new Vector3(334.5203F, 272.9179F, 336.9202F),
                                                                       new Vector3(0.65004F, 0.65004F, 0.65004F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CloverVoid, "DisplayCloverVoid",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.45241F, 0.29418F),
                                                                       new Vector3(334.5203F, 272.9179F, 336.9202F),
                                                                       new Vector3(0.65004F, 0.65004F, 0.65004F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                                               "Tower Circle Items 3",
                                                                       new Vector3(-0.00001F, 0.04427F, -0.52765F),
                                                                       new Vector3(273.6421F, 195.3425F, 347.8005F),
                                                                       new Vector3(0.26026F, 0.3316F, 0.2916F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.38127F, 1.94494F, 0.62707F),
                                                                       new Vector3(65.92451F, 206.7916F, 350.2542F),
                                                                       new Vector3(0.35514F, 0.35514F, 0.35514F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger",
                                                                       "Tower Circle Items 3",
                                                                       new Vector3(0.06265F, 0.08072F, 0.23106F),
                                                                       new Vector3(47.08094F, 246.5786F, 1.93198F),
                                                                       new Vector3(1.01515F, 1.01515F, -1.01515F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0F, 2.24824F, -0.13345F),
                                                                       new Vector3(280.6515F, 183.2082F, 174.8475F),
                                                                       new Vector3(0.05481F, 0.05481F, 0.05481F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.22347F, 1.059F, 0.16087F),
                                                                       new Vector3(2.13955F, 265.6363F, 339.9073F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0533F, 1.70262F, 0.07699F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.EquipmentMagazineVoid, "DisplayFuelCellVoid",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0533F, 1.70262F, 0.07699F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                                                                       "Tower Circle Items 2",
                                                                       new Vector3(-0.03041F, 0.02747F, -0.61301F),
                                                                       new Vector3(280.7616F, 349.6049F, 195.709F),
                                                                       new Vector3(0.15428F, 0.15428F, 0.15252F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                                                                       "Tower Circle Items 3",
                                                                       new Vector3(0.56232F, -0.01616F, -0.10021F),
                                                                       new Vector3(355.1682F, 91.66299F, 356.5107F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0F, 0.84776F, 0.09658F),
                                                                       new Vector3(347.9707F, 3.47827F, 11.20283F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExtraLifeVoid, "DisplayHippoVoid",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0F, 0.84776F, 0.09658F),
                                                                       new Vector3(347.9707F, 3.47827F, 11.20283F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            //itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
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
            ////itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
            ////    ItemDisplays.CreateDisplayRule("DisplayFeather",
            ////                                   "Base Pillar Items 3",
            ////                                   new Vector3(0.02972F, 0.22344F, 0.04502F),
            ////                                   new Vector3(0.47761F, 233.9261F, 304.7706F),
            ////                                   new Vector3(-0.04399F, 0.02643F, 0.02588F))
            //    ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                "Tower Circle Items 1",
                new Vector3(0.72202F, -0.09951F, -0.12706F),
                new Vector3(70.96928F, 96.40028F, 357.7675F),
                new Vector3(0.08588F, 0.08588F, 0.08588F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0.27019F, 0.36856F, 0.05241F),
                                                                       new Vector3(299.2795F, 347.5153F, 329.2786F),
                                                                       new Vector3(0.24082F, 0.24082F, 0.24082F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.09571F, 2.09672F, 0.00897F),
                                                                       new Vector3(357.0212F, 351.2194F, 180.0431F),
                                                                       new Vector3(0.12057F, 0.12057F, 0.12057F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask",
                                                                       "Head",
                                                                       new Vector3(0.47888F, -0.02005F, 0.03394F),
                                                                       new Vector3(0F, 95.8286F, 90.00001F),
                                                                       new Vector3(1.48088F, 1.48088F, 1.1422F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.0891F, 2.23077F, -0.32324F),
                                                                       new Vector3(351.5005F, 354.2409F, 21.81469F),
                                                                       new Vector3(0.83876F, 0.83197F, 0.71985F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown",
                "Center Cylinder Items",
                new Vector3(0.00093F, 0.95486F, -0.00924F),
                new Vector3(0F, 44.69136F, 0F),
                new Vector3(1.43419F, 0.00288F, 0.53448F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                "Tower Circle Items 2",
                new Vector3(0.57507F, 0.05548F, -0.2405F),
                new Vector3(318.6227F, 198.8829F, 86.57573F),
                new Vector3(0.19725F, 0.19012F, 0.19725F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail",
                "Base Pillar Items 4",
                new Vector3(0.14666F, 2.2179F, -0.25092F),
                new Vector3(345.152F, 186.6709F, 28.23785F),
                new Vector3(0.06654F, 0.06654F, 0.06654F)));

            //itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
            //    ItemDisplays.CreateDisplayRule("DisplayHoof",
            //                                   "Tower Pole Items",
            //                                   new Vector3(-0.02149F, 0.35335F, -0.04254F),
            //                                   new Vector3(79.93871F, 359.8341F, 341.8235F),
            //                                   new Vector3(0.11376F, 0.10848F, 0.09155F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline",
                "Base Pillar Items 2",
                new Vector3(0.08906F, 0.80477F, 0.1426F),
                new Vector3(282.4633F, 219.2414F, 56.33241F),
                new Vector3(0.49393F, 0.49393F, 0.49393F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(JunkContent.Items.Incubator, "DisplayAncestralIncubator",
                "Base Pillar Items 1",
                new Vector3(0.17208F, 1.26778F, -0.11132F),
                new Vector3(16.46702F, 16.01868F, 319.7823F),
                new Vector3(0.03839F, 0.03839F, 0.03839F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion",
                "Base Pillar Items 3",
                new Vector3(0.12594F, 0.77161F, 0.17482F),
                new Vector3(351.2162F, 3.85623F, 358.6136F),
                new Vector3(0.55983F, 0.55983F, 0.56498F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.JumpBoost, "DisplayWaxBird",
            //                                                           "Head",
            //                                                           new Vector3(0F, -0.26665F, -0.06925F),
            //                                                           new Vector3(0F, 0F, 0F),
            //                                                           new Vector3(0.78163F, 0.78163F, 0.78163F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.KillEliteFrenzy, "DisplayBrainstalk",
                "Head",
                new Vector3(0.07072F, 0F, 0.12917F),
                new Vector3(298.8424F, 81.91605F, 99.21015F),
                new Vector3(0.8319F, 1.17689F, 0.82605F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                "Base Pillar Items 1",
                new Vector3(0.16131F, 1.73025F, -0.00889F),
                new Vector3(273.8917F, 279.1016F, 304.6547F),
                new Vector3(0.28718F, 0.28718F, 0.28718F)));
            //uh
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",
                "Tower Circle Items 1",
                new Vector3(0.56274F, -0.03396F, -0.40049F),
                new Vector3(349.7067F, 130.554F, 0F),
                new Vector3(1.37899F, 1.37899F, 1.37899F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                "Base Pillar Items 2",
                new Vector3(-0.17068F, 1.81185F, -0.01557F),
                new Vector3(32.61504F, 203.7614F, 95.6869F),
                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarPrimaryReplacement, "DisplayBirdEye",
                "Head",
                new Vector3(-0.56078F, -0.07984F, 0.12818F),
                new Vector3(278.5961F, 279.9584F, 0.82052F),
                new Vector3(0.51693F, 0.51693F, 0.51693F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSecondaryReplacement, "DisplayBirdClaw",
                "Tower Circle Items 3",
                new Vector3(0.26557F, 0.06149F, -0.44262F),
                new Vector3(19.86829F, 72.36673F, 193.5589F),
                new Vector3(0.76927F, 0.76927F, 0.76927F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads",
                "Base Pillar Items 3",
                new Vector3(0.05881F, 1.27475F, 0.09195F),
                new Vector3(12.47539F, 158.354F, 310.8714F),
                new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot", 
                "Tower Circle Items 3",
                new Vector3(-0.68067F, 0.06271F, -0.04414F),
                new Vector3(20.5769F, 16.05076F, 24.0392F),
                new Vector3(1.1023F, 1.1023F, 1.1023F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Medkit, "DisplayMedkit",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.23334F, 0.11768F, 0.08936F),
                                                                       new Vector3(64.35487F, 280.9532F, 69.71149F),
                                                                       new Vector3(0.6F, 0.6F, 0.6F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Missile, "DisplayMissileLauncher",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.49146F, 0.49759F, -0.24416F),
                                                                       new Vector3(337.2415F, 348.6425F, 38.64057F),
                                                                       new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MissileVoid, "DisplayMissileLauncherVoid",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.49146F, 0.49759F, -0.24416F),
                                                                       new Vector3(337.2415F, 348.6425F, 38.64057F),
                                                                       new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.MonstersOnShrineUse, "DisplayMonstersOnShrineUse",
                                                                       "Tower Pole Items",
                                                                       new Vector3(-0.01711F, 0.28384F, 0.02934F),
                                                                       new Vector3(73.75125F, 52.4324F, 48.00412F),
                                                                       new Vector3(0.12101F, 0.12101F, 0.12101F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Mushroom, "DisplayMushroom",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MushroomVoid, "DisplayMushroomVoid",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NearbyDamageBonus, "DisplayDiamond",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.12542F, 0.21772F, -0.01903F),
                                                                       new Vector3(0.76041F, 2.61635F, 16.98922F),
                                                                       new Vector3(0.0996F, 0.0996F, 0.0996F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnHeal,
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                                new Vector3(0.09265F, 0.1409F, 0.05795F),
                                                new Vector3(1.60112F, 352.6636F, 12.2446F),
                                                new Vector3(0.42003F, 0.42003F, 0.42003F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                               new Vector3(-0.09265F, 0.1409F, 0.05795F),
                                               new Vector3(1.60112F, 352.6636F, 350.5993F),
                                               new Vector3(-0.42003F, 0.42003F, 0.42003F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NovaOnLowHealth, "DisplayJellyGuts",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.01615F, 0.37581F, -0.11174F),
                                                                       new Vector3(356.3561F, 333.6493F, 343.5101F),
                                                                       new Vector3(0.10713F, 0.10713F, 0.10713F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ParentEgg, "DisplayParentEgg",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.0289F, -0.08568F, 0.41864F),
                                                                       new Vector3(24.21236F, 82.34744F, 7.68247F),
                                                                       new Vector3(0.09096F, 0.09096F, 0.09096F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Pearl, "DisplayPearl",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.00001F, 0.19616F, -0.02199F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.PersonalShield, "DisplayShieldGenerator",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.17905F, 0.12874F, 0.32696F),
                                                                       new Vector3(279.9448F, 70.53902F, 320.1869F),
                                                                       new Vector3(0.1923F, 0.1923F, 0.1923F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Phasing, "DisplayStealthkit",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0.1005F, 0.27421F, -0.15216F),
                                                                       new Vector3(4.18787F, 230.1978F, 263.3475F),
                                                                       new Vector3(0.32966F, 0.35099F, 0.35099F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Plant, "DisplayInterstellarDeskPlant",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.1207F, 0.23304F, 0.02712F),
                                                                       new Vector3(358.9562F, 83.84697F, 0F),
                                                                       new Vector3(0.08447F, 0.08351F, 0.08351F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RandomDamageZone, "DisplayRandomDamageZone",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.3403F, 0.21702F, 0.00818F),
                                                                new Vector3(352.5229F, 270.9943F, 0.51174F),
                                                                new Vector3(0.12349F, 0.16068F, 0.16068F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.12734F, 0.37462F, -0.19835F),
                                                                new Vector3(356.7155F, 152.0292F, 314.6104F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));

            //SecondarySkillMagazine: see example above
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Seed, "DisplaySeed",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.00351F, 0.27648F, 0.10802F),
                                                                       new Vector3(330.0132F, 62.84435F, 84.55939F),
                                                                       new Vector3(0.0537F, 0.0537F, 0.0537F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShieldOnly,
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(-0.11649F, 0.23106F, 0.10907F),
                                               new Vector3(348.7565F, 171.4471F, 14.55577F),
                                               new Vector3(0.19801F, 0.19801F, 0.19801F)),
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(-0.11515F, 0.2296F, -0.10686F),
                                               new Vector3(348.5939F, 13.64931F, 161.6449F),
                                               new Vector3(0.19801F, -0.19801F, 0.19801F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ShinyPearl, "DisplayShinyPearl",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.03611F, 0.26797F, 0.01394F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SiphonOnLowHealth, "DisplaySiphonOnLowHealth",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.2131F, 0.15274F, 0.2284F),
                                                                new Vector3(341.8052F, 309.3467F, 179.0716F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SlowOnHit, "DisplayBauble",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.39172F, 0.59777F, 0.30968F),
                                                                new Vector3(0F, 37.11069F, 165.892F),
                                                                new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintArmor, "DisplayBuckler",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.08204F, 0.15889F, -0.02187F),
                                                                       new Vector3(339.7112F, 94.07677F, 338.7357F),
                                                                       new Vector3(0.16504F, 0.16504F, 0.17364F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintBonus, "DisplaySoda",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.16407F, 0.08059F, -0.26964F),
                                                                new Vector3(284.3611F, 127.6331F, 323.4904F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintOutOfCombat, "DisplayWhip",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0F, 0.01446F, 0.19113F),
                                                                       new Vector3(0F, 90F, 200F),
                                                                       new Vector3(0.2175F, 0.2175F, 0.2175F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintWisp, "DisplayBrokenMask",
                                                                "Base Pillar Items 3",
                                                                new Vector3(-0.02311F, 0.20304F, 0.09964F),
                                                                new Vector3(335.4409F, 2.64948F, 180F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Squid, "DisplaySquidTurret",
                                                                "Base Pillar Items 2",
                                                                new Vector3(-0.15008F, 0.21693F, -0.02095F),
                                                                new Vector3(6.47033F, 168.8895F, 289.4581F),
                                                                new Vector3(0.06125F, 0.06125F, 0.06125F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StickyBomb, "DisplayStickyBomb",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.30611F, 0.27243F, -0.27997F),
                                                                       new Vector3(12.0223F, 157.6354F, 350.9629F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.14987F, -0.04276F, 0.19991F),
                                                                       new Vector3(69.53522F, 188.4478F, 279.2932F),
                                                                       new Vector3(0.7F, 0.7F, 0.7F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Syringe, "DisplaySyringeCluster",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.25482F, 0.04429F, -0.05188F),
                                                                       new Vector3(325.976F, 201.993F, 122.6787F),
                                                                       new Vector3(0.15369F, 0.15369F, 0.15369F)));

            //man there are a lot of these
            #endregion done

            #region not done            
            /*

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.21578F, 0.33212F, 0.01521F),
                                                                new Vector3(338.9304F, 272.9285F, 358.9245F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TitanGoldDuringTP, "DisplayGoldHeart",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.26652F, 0.29248F, -0.17898F),
                                                                new Vector3(318.8546F, 247.2778F, 52.25115F),
                                                                new Vector3(0.285F, 0.285F, 0.285F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                                               "Center Cylinder Items",
                                                new Vector3(0.00096F, 0.29871F, 0.38082F),
                                                new Vector3(358.7835F, 348.6807F, 2.7268F),
                                                new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Center Cylinder Items",
                                                new Vector3(0.08436F, 0.28255F, 0.34988F),
                                                new Vector3(355.8568F, 31.64544F, 357.1176F),
                                                new Vector3(1.67906F, 2.3004F, 1.91776F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Center Cylinder Items",
                                                new Vector3(0.14556F, 0.28629F, 0.31617F),
                                                new Vector3(349.3203F, 27.71486F, 7.13324F),
                                                new Vector3(1.48967F, 1.51727F, 1.51727F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Center Cylinder Items",
                                                new Vector3(-0.08285F, 0.28376F, 0.35114F),
                                                new Vector3(354.2122F, 340.7911F, 0.3483F),
                                                new Vector3(1.88356F, 2.14586F, 2.1551F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Center Cylinder Items",
                                                new Vector3(-0.14622F, 0.28251F, 0.31784F),
                                                new Vector3(355.3388F, 329.3073F, 353.9711F),
                                                new Vector3(1.50954F, 1.67906F, 1.67906F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TreasureCache, "DisplayKey",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.31675F, 0.14937F, -0.19804F),
                                                                       new Vector3(33.4258F, 15.80067F, 227.8426F),
                                                                       new Vector3(0.95146F, 0.95146F, 0.95146F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.TreasureCacheVoid, "DisplayKeyVoid",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.31675F, 0.14937F, -0.19804F),
                                                                       new Vector3(33.4258F, 15.80067F, 227.8426F),
                                                                       new Vector3(0.95146F, 0.95146F, 0.95146F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
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

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WarCryOnMultiKill, "DisplayPauldron",
                                                                "Base Pillar Items 3",
                                                                new Vector3(-0.02566F, 0.26861F, 0.01654F),
                                                                new Vector3(1.32502F, 0.16224F, 358.6667F),
                                                                new Vector3(0.96987F, 0.96987F, 0.96987F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WardOnLevel, "DisplayWarbanner",
                                                                        "Head",
                                                                        new Vector3(0.32394F, 0.06169F, 0.02166F),
                                                                        new Vector3(288.6897F, 151.0441F, 245.5378F),
                                                                        new Vector3(0.5F, 0.5F, 0.5F)));
            */
            #endregion not done

            #endregion items

            #region quips
            //don't need any besides aspects
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixHaunted, "DisplayEliteStealthCrown",
                                                                 "Head",
                                                                       new Vector3(-0.00813F, 0.01963F, 0.67331F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.13404F, 0.13404F, 0.13404F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixWhite, "DisplayEliteIceCrown",
                                                                "Head",
                                                                       new Vector3(0F, 0F, 0.68394F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.07108F, 0.07108F, 0.07108F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixLunar, "DisplayEliteLunar,Eye",
                                                                       "Head",
                                                                       new Vector3(0F, 0F, 0.8236F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.81075F, 0.81075F, 0.81075F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixPoison, "DisplayEliteUrchinCrown",
                                                                "Head",
                                                                       new Vector3(0F, 0F, 0.49957F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.14435F, 0.14435F, 0.14435F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Elites.Earth.eliteEquipmentDef, "DisplayEliteMendingAntlers",
                                                                       "Head",
                                                                       new Vector3(0F, -0.15922F, 0.4271F),
                                                                       new Vector3(82.54549F, 180F, 180F),
                                                                       new Vector3(1.6972F, 1.6972F, 1.6972F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.EliteVoidEquipment, "DisplayAffixVoid",
                                                                       "Head",
                                                                       new Vector3(-0.00512F, -0.11767F, 0.46191F),
                                                                       new Vector3(89.78602F, 180F, 180F),
                                                                       new Vector3(0.64773F, 0.55528F, 0.58107F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.208f, 0.208f, 0),
            //                                                    new Vector3(0, 270, 0),
            //                                                    new Vector3(0.25f, 0.25f, 0.25f)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat",
            //                                                    "Base Pillar Items 3",
            //                                                    new Vector3(0.09687F, 0.32573F, 0.22415F),
            //                                                    new Vector3(279.4109F, 187.7572F, 345.6136F),
            //                                                    new Vector3(0.11175F, 0.11175F, 0.11175F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG",
            //                                                           "Center Cylinder Items",
            //                                                           new Vector3(0.07101F, 0.41512F, -0.19564F),
            //                                                           new Vector3(348.6131F, 1.95604F, 339.4123F),
            //                                                           new Vector3(0.4F, 0.4F, 0.4F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion",
            //                                                           "Center Cylinder Items",
            //                                                           new Vector3(-0.21288F, -0.09195F, -0.09708F),
            //                                                           new Vector3(345.6282F, 5.15145F, 313.8587F),
            //                                                           new Vector3(0.04021F, 0.04021F, 0.04021F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack", //brokey
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.32123F, 0.17569F, -0.01137F),
            //                                                    new Vector3(357.2928F, 90.24006F, 0.69147F),
            //                                                    new Vector3(0.1F, 0.1F, 0.1F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.26506F, 0.45562F, 0.00002F),
            //                                                    new Vector3(90F, 90F, 0F),
            //                                                    new Vector3(0.5F, 0.5F, 0.5F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy",
            //                                                           "Head",
            //                                                           new Vector3(-0.00062F, 0.16551F, -0.13266F),
            //                                                           new Vector3(34.73222F, 167.4949F, 348.1801F),
            //                                                           new Vector3(0.354F, 0.354F, 0.354F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.33257F, 0.3451F, -0.01117F),
            //                                                    new Vector3(315F, 90F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.13026F, 0.28527F, -0.16847F),
            //                                                    new Vector3(356.3801F, 347.5225F, 216.8458F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant",
            //                                                    "Head",
            //                                                    new Vector3(-0.20606F, 0.06706F, -0.00143F),
            //                                                    new Vector3(0F, 90F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio",
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
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure",
            //                                                    "Tower Pole Items",
            //                                                    new Vector3(-0.17336F, 0.08326F, -0.00223F),
            //                                                    new Vector3(90F, 268.5319F, 0F),
            //                                                    new Vector3(0.3F, 0.3F, 0.3F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.31706F, 0.37802F, 0.00421F),
            //                                                    new Vector3(0F, 0F, 348.9059F),
            //                                                    new Vector3(0.06F, 0.06F, 0.06F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.08967F, 0.06729F, 0.29295F),
            //                                                    new Vector3(90F, 0F, 0F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic",
            //                                                    "Base Pillar Items 2",
            //                                                    new Vector3(-0.00001F, 0.2792F, 0.18109F),
            //                                                    new Vector3(359.9935F, 359.8932F, 180.1613F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.11973F, 0.54832F, 0.26252F),
            //                                                    new Vector3(359.2803F, 349.704F, 3.12542F),
            //                                                    new Vector3(0.21F, 0.21F, 0.21F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner",
            //                                                    "Head",
            //                                                    new Vector3(0.37265F, 0.86982F, 0.02202F),
            //                                                    new Vector3(293.9302F, 144.0041F, 339.115F),
            //                                                    new Vector3(0.25F, 0.25F, 0.25F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(-0.23665F, 0.35165F, 0.12038F),
            //                                                    new Vector3(335.7568F, 279.3308F, 358.6632F),
            //                                                    new Vector3(0.04F, 0.04F, 0.04F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(0.49716F, -0.16366F, -0.1139F),
            //                                                    new Vector3(324.7334F, 298.6617F, 280.0614F),
            //                                                    new Vector3(0.1347F, 0.1347F, 0.1347F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry",
            //                                                    "Center Cylinder Items",
            //                                                    new Vector3(-0.24951F, -0.12858F, -0.00915F),
            //                                                    new Vector3(9.0684F, 272.2467F, 359.5266F),
            //                                                    new Vector3(0.1F, 0.1F, 0.1F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Saw, "DisplaySawmerangFollower",
            //                                                    "Root",
            //                                                    new Vector3(0.62211F, 0.43106F, 1.15734F),
            //                                                    new Vector3(358.3824F, 269.3275F, 292.5764F),
            //                                                    new Vector3(0.225F, 0.225F, 0.225F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Meteor, "DisplayMeteor",
            //                                                    "Root",
            //                                                    new Vector3(1.04502F, 0.51043F, 0.67377F),
            //                                                    new Vector3(90F, 0F, 0F),
            //                                                    new Vector3(1.2F, 1.2F, 1.2F)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Blackhole, "DisplayGravCube",
            //                                                    "Root",
            //                                                    new Vector3(0.62211F, 0.43106F, 1.15734F),
            //                                                    new Vector3(358.3824F, 269.3275F, 292.5764F),
            //                                                    new Vector3(0.225F, 0.225F, 0.225F)));
            #endregion quips

            #region compat
            try {
                if (Compat.TinkersSatchelInstalled) {
                    SetTinkersSatchelDisplayRules(itemDisplayRules);
                }
            } catch (System.Exception e) {
                Helpers.LogWarning("error adding displays for Tinker's Satchel \n" + e);
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
        private void SetTinkersSatchelDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

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
        private void SetAetheriumDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {
        }
        #endregion aeth
    }
}
