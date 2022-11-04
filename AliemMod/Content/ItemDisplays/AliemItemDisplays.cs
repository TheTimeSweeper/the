using Modules.Characters;
using Modules;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace AliemMod.Content {

    class AliemItemDisplays : ItemDisplaysBase {

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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AlienHead, "DisplayAlienHead",
                "ThighL",
                new Vector3(-0.04745F, 0.03449F, -0.12276F),
                new Vector3(85.25274F, 326.2674F, 295.7773F),
                new Vector3(2.23134F, 2.23134F, 2.23134F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate",
                "ThighR",
                new Vector3(-0.01832F, 0.41394F, -0.11176F),
                new Vector3(84.67023F, 259.4688F, 290.3134F),
                new Vector3(0.49491F, 0.46601F, 0.37378F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
                ItemDisplays.CreateDisplayRule("DisplayWarHammer",
                    "HandR",
                    new Vector3(0.01538F, 0.49285F, -0.3681F),
                    new Vector3(1.86189F, 164.0802F, 269.2046F),
                    new Vector3(0.39546F, 0.39546F, 0.39546F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt",
                "Head",
                new Vector3(0.00143F, 0.69474F, -0.76762F),
                new Vector3(273.2969F, 180F, 180F),
                new Vector3(0.89178F, 0.84331F, 0.88223F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil",
                "ThighL",
                new Vector3(-0.17117F, -0.01162F, -0.0499F),
                new Vector3(29.65431F, 346.3208F, 350.4796F),
                new Vector3(1.12029F, 1.12029F, 1.12029F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier",
                "Chest",
                new Vector3(-0.00265F, 0.37996F, -0.11701F),
                new Vector3(44.03649F, 79.60893F, 82.34425F),
                new Vector3(-1.6705F, -1.63246F, -3.02471F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch",
                "Chest2",
                new Vector3(-0.35135F, 0.02688F, 0.25276F),
                new Vector3(71.07784F, 0.16461F, 26.71287F),
                new Vector3(1.5944F, 1.36663F, 1.36663F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis",
                "LowerArmR",
                new Vector3(0.10709F, 0.39036F, 0.05387F),
                new Vector3(2.55888F, 145.6002F, 263.719F),
                new Vector3(0.39975F, 0.39975F, 0.39975F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bear, "DisplayBear",
                "Chest",
                new Vector3(0.12443F, -0.00746F, 0.28586F),
                new Vector3(30.80385F, 13.63447F, 23.88048F),
                new Vector3(0.44118F, 0.45414F, 0.45414F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.BearVoid, "DisplayBearVoid",
                "Chest",
                new Vector3(0.12443F, -0.00746F, 0.28586F),
                new Vector3(30.80385F, 13.63447F, 23.88048F),
                new Vector3(0.44118F, 0.45414F, 0.45414F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland",
                "ThighR",
                new Vector3(0.16862F, 0.11588F, -0.0858F),
                new Vector3(322.6508F, 250.1892F, 127.0396F),
                new Vector3(0.10181F, 0.10181F, 0.10181F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                    "BlasterMuzzle",
                    new Vector3(-0.00193F, 0.66647F, -0.75955F),
                    new Vector3(270.0626F, 179.5512F, 0F),
                    new Vector3(0.19786F, 0.1697F, 0.19786F))));
            
            //again, don't have to do this 
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                    "BlasterMuzzle",
                    new Vector3(0.14398F, -0.06159F, 0.00002F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.74606F, 0.74606F, 0.88324F))));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                    "BlasterMuzzle",
                    new Vector3(0.14398F, -0.06159F, 0.00002F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.74606F, 0.74606F, 0.88324F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode",
                "Head",
                new Vector3(-0.25304F, -0.39041F, 0.18082F),
                new Vector3(83.24033F, 150.6364F, 293.0996F),
                new Vector3(0.15689F, 0.15689F, 0.15689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome",
                "ThighL",
                new Vector3(-0.13515F, 0.36489F, 0.01095F),
                new Vector3(358.9265F, 261.2257F, 356.1119F),
                new Vector3(0.15212F, 0.15212F, 0.15212F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound",
                "UpperArmR",
                new Vector3(0.12556F, 0.31315F, 0.16259F),
                new Vector3(4.34577F, 134.6293F, 88.73745F),
                new Vector3(1.43431F, 1.43431F, 1.43431F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook",
                "Chest2",
                new Vector3(-0.23922F, 0.35038F, -0.39306F),
                new Vector3(310.0912F, 1.33264F, 2.42477F),
                new Vector3(1.24345F, 1.24345F, 1.24345F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele",
                "Chest2",
                new Vector3(0.42542F, 0.31625F, -0.5334F),
                new Vector3(353.3568F, 171.7765F, 52.03571F),
                new Vector3(1.09587F, 1.04021F, 1.05676F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ChainLightningVoid, "DisplayUkuleleVoid",
                "Chest2",
                new Vector3(0.42542F, 0.31625F, -0.5334F),
                new Vector3(353.3568F, 171.7765F, 52.03571F),
                new Vector3(1.09587F, 1.04021F, 1.05676F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                "AntennaR",
                new Vector3(0.13024F, 0.38854F, -0.03534F),
                new Vector3(319.4796F, 277.4703F, 335.4419F),
                new Vector3(1.16115F, 1.16115F, 1.16115F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CloverVoid, "DisplayCloverVoid",
                "AntennaR",
                new Vector3(0.13024F, 0.38854F, -0.03534F),
                new Vector3(319.4796F, 277.4703F, 335.4419F),
                new Vector3(1.16115F, 1.16115F, 1.16115F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.CritGlasses, "DisplayGlasses",
                "Head",
                new Vector3(0.00001F, 1.01925F, -0.37287F),
                new Vector3(278.9824F, 180F, 180F),
                new Vector3(1.85349F, 0.74353F, 1.03004F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritGlassesVoid, "DisplayGlassesVoid",
                "Head",
                new Vector3(0.00001F, 1.01925F, -0.37287F),
                new Vector3(278.9824F, 180F, 180F),
                new Vector3(1.85349F, 0.74353F, 1.03004F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar",
                "Chest2",
                new Vector3(-0.28113F, 0.3326F, -0.44273F),
                new Vector3(356.3831F, 163.2484F, 327.3824F),
                new Vector3(0.58132F, 0.58132F, 0.58132F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger",
                "UpperArmR",
                new Vector3(0.00396F, -0.16866F, -0.00005F),
                new Vector3(1.74541F, 117.3748F, 181.1896F),
                new Vector3(1.49505F, 1.49505F, 1.49505F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark",
                "HandL",
                new Vector3(-0.11776F, 0.39411F, 0.03615F),
                new Vector3(66.60409F, 299.9192F, 190.3377F),
                new Vector3(0.08932F, 0.08932F, 0.08932F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn",
                "Chest",
                new Vector3(0.6555F, -0.40535F, -0.4617F),
                new Vector3(0.15996F, 55.56916F, 320.0925F),
                new Vector3(0.8246F, 0.8246F, 0.8246F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery",
                "Pelvis",
                new Vector3(-0.02123F, 0.05971F, 0.29265F),
                new Vector3(0F, 90F, 0F),
                new Vector3(0.41475F, 0.41475F, 0.41475F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.EquipmentMagazineVoid, "DisplayFuelCellVoid",
                "Pelvis",
                new Vector3(-0.02123F, 0.05971F, 0.29265F),
                new Vector3(0F, 90F, 0F),
                new Vector3(0.41475F, 0.41475F, 0.41475F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                "LowerArmL",
                new Vector3(-0.05132F, 0.35923F, 0.1256F),
                new Vector3(5.70256F, 216.4105F, 95.05837F),
                new Vector3(0.20912F, 0.20912F, 0.20673F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                "ThighR",
                new Vector3(0.05579F, 0.13332F, -0.27792F),
                new Vector3(343.7091F, 34.81034F, 198.2103F),
                new Vector3(0.09775F, 0.09775F, 0.09775F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExplodeOnDeathVoid, "DisplayWillowWispVoid",
                "ThighR",
                new Vector3(0.05579F, 0.13332F, -0.27792F),
                new Vector3(343.7091F, 34.81034F, 198.2103F),
                new Vector3(0.09775F, 0.09775F, 0.09775F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
                "Chest",
                new Vector3(0.35834F, 0.22007F, 0.32236F),
                new Vector3(29.18295F, 39.3069F, 21.76472F),
                new Vector3(0.45512F, 0.46849F, 0.46849F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExtraLifeVoid, "DisplayHippoVoid",
                "Chest",
                new Vector3(0.35834F, 0.22007F, 0.32236F),
                new Vector3(29.18295F, 39.3069F, 21.76472F),
                new Vector3(0.45512F, 0.46849F, 0.46849F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                    "CalfL",
                    new Vector3(-0.03998F, 0.36831F, 0.03805F),
                    new Vector3(356.3479F, 168.6573F, 171.8978F),
                    new Vector3(0.47644F, 0.47644F, 0.47644F)),
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                    "CalfR",
                    new Vector3(0.03822F, 0.36983F, 0.0297F),
                    new Vector3(6.25589F, 22.00069F, 174.1347F),
                    new Vector3(0.45976F, 0.45976F, 0.45976F)
                )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                    "UpperArmL",
                    new Vector3(-0.0067F, -0.14641F, -0.1187F),
                    new Vector3(319.5564F, 121.8395F, 284.8557F),
                    new Vector3(-0.09738F, 0.05851F, 0.05729F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",
                "AntennaR",
                new Vector3(0.0152F, 0.08839F, 0.00426F),
                new Vector3(81.32568F, 242.2187F, 242.4897F),
                new Vector3(0.73432F, 0.73432F, 0.73432F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing",
                "AntennaL",
                new Vector3(0.0152F, 0.08839F, 0.00426F),
                new Vector3(81.32568F, 242.2187F, 242.4897F),
                new Vector3(0.73432F, 0.73432F, 0.73432F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ElementalRingVoid, "DisplayVoidRing",
                "AntennaR",
                new Vector3(0.0152F, 0.08839F, 0.00426F),
                new Vector3(81.32568F, 242.2187F, 242.4897F),
                new Vector3(0.73432F, 0.73432F, 0.73432F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                "Head",
                new Vector3(0.41501F, 1.28924F, 0.17724F),
                new Vector3(343.1581F, 0F, 343.3397F),
                new Vector3(0.15618F, 0.15618F, 0.15618F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                "CalfR",
                new Vector3(-0.12263F, 0.07645F, -0.07368F),
                new Vector3(82.45949F, 286.4524F, 127.7181F),
                new Vector3(0.65023F, 0.65023F, 0.65023F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                "JawLower",
                new Vector3(0.88387F, 0.99279F, -0.03392F),
                new Vector3(335.4378F, 136.135F, 64.55423F),
                new Vector3(0.28F, 0.28F, 0.28F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                "Root",
                new Vector3(-3.04144F, 0.9062F, 1.93789F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.09264F, 0.09264F, 0.09264F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask",
                "Head",
                new Vector3(-0.00718F, 1.05774F, -0.33445F),
                new Vector3(286.4083F, 181.8289F, 178.213F),
                new Vector3(2.23667F, 2.23667F, 1.72514F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown",
                "Head",
                new Vector3(-0.0006F, 0.2534F, -0.61398F),
                new Vector3(287.1593F, 180F, 180F),
                new Vector3(2.86702F, 2.8478F, 2.46402F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown",
                "Pelvis",
                new Vector3(0.00093F, 0.01007F, -0.00924F),
                new Vector3(359.9472F, 173.7351F, 179.5188F),
                new Vector3(1.43198F, 0.46149F, 0.47007F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                "Chest2",
                new Vector3(-0.36174F, 0.17879F, -0.49493F),
                new Vector3(326.4503F, 261.141F, 91.65668F),
                new Vector3(0.37566F, 0.37566F, 0.37566F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail",
                "Head",
                new Vector3(-0.67156F, 0.08516F, -0.33247F),
                new Vector3(73.85525F, 87.90349F, 203.6308F),
                new Vector3(0.15696F, 0.15696F, 0.15696F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
                ItemDisplays.CreateDisplayRule("DisplayHoof",
                    "CalfR",
                    new Vector3(-0.01251F, 0.51227F, -0.08279F),
                    new Vector3(73.96525F, 8.95291F, 351.8484F),
                    new Vector3(0.19166F, 0.18277F, 0.12107F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Icicle, "DisplayFrostRelic",
                "Root",
                new Vector3(-2.30989F, 0.08953F, 0.58426F),
                new Vector3(4.9156F, 180F, 180F),
                new Vector3(0.89001F, 0.89001F, 0.89001F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline",
                "CalfL",
                new Vector3(-0.17991F, 0.28211F, 0.04411F),
                new Vector3(88.27116F, 81.28543F, 105.8364F),
                new Vector3(1.15196F, 1.15196F, 1.15196F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                    "Head",
                    new Vector3(0.54116F, 0.28647F, -0.51496F),
                    new Vector3(359.2423F, 103.6492F, 256.0834F),
                    new Vector3(0.77832F, 0.77832F, 0.77832F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                    "Head",
                    new Vector3(-0.54116F, 0.28647F, -0.51496F),
                    new Vector3(10.07285F, 262.0654F, 101.3235F),
                    new Vector3(0.77832F, 0.77832F, 0.77832F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion",
                "Chest2",
                new Vector3(0.307F, 0.04063F, 0.2938F),
                new Vector3(352.3945F, 31.51675F, 3.32787F),
                new Vector3(0.8266F, 0.8266F, 0.83421F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.JumpBoost, "DisplayWaxBird",
                "Head",
                new Vector3(-0.00001F, 0.4117F, 0.56803F),
                new Vector3(272.1789F, 0F, 0F),
                new Vector3(2.23468F, 2.23468F, 2.23468F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.KillEliteFrenzy, "DisplayBrainstalk",
                "Head",
                new Vector3(-0.00001F, 0.36619F, -0.39888F),
                new Vector3(286.1715F, 180F, 176.6998F),
                new Vector3(1.0169F, 1.43861F, 1.00975F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Knurl, "DisplayKnurl",
                "Chest2",
                new Vector3(-0.51941F, 0.27F, 0.04192F),
                new Vector3(279.7953F, 270.4613F, 274.147F),
                new Vector3(0.16543F, 0.16543F, 0.16543F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                "UpperArmR",
                new Vector3(0.10781F, -0.13115F, 0.15006F),
                new Vector3(1.83191F, 142.9561F, 86.14008F),
                new Vector3(0.34094F, 0.34094F, 0.34094F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",
                "Head",
                new Vector3(-0.39332F, 1.25581F, 0.03287F),
                new Vector3(302.0018F, 239.8243F, 124.4361F),
                new Vector3(2.5731F, 2.5731F, 2.5731F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                "Chest2",
                new Vector3(0.02923F, 0.3919F, -0.51312F),
                new Vector3(21.8722F, 273.5556F, 85.98581F),
                new Vector3(1.17642F, 1.17642F, 1.17642F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarPrimaryReplacement,
                ItemDisplays.CreateDisplayRule("DisplayBirdEye",
                    "Head",
                    new Vector3(0.61005F, 0.82627F, -0.34575F),
                    new Vector3(302.5341F, 236.3812F, 222.9178F),
                    new Vector3(1.08988F, 1.08988F, 1.08988F)),
                ItemDisplays.CreateDisplayRule("DisplayBirdEye",
                    "Head",
                    new Vector3(-0.61005F, 0.82627F, -0.34575F),
                    new Vector3(53.71616F, 287.5238F, 209.6046F),
                    new Vector3(1.08988F, 1.08988F, 1.08988F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSecondaryReplacement, "DisplayBirdClaw",
                "UpperArmL",
                new Vector3(-0.00433F, 0.66629F, 0.11186F),
                new Vector3(4.7949F, 97.77225F, 315.3593F),
                new Vector3(0.98533F, 0.98533F, 0.98533F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSpecialReplacement, "DisplayBirdHeart",
                "Root",
                new Vector3(2.62284F, -0.81566F, 1.69272F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.29213F, 0.29213F, 0.29213F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads",
                "HandR",
                new Vector3(0.06943F, 0.44428F, 0.01516F),
                new Vector3(12.4754F, 158.354F, 110.715F),
                new Vector3(2.04835F, 2.04835F, 2.04835F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot",
                "CalfR",
                new Vector3(0.20381F, 0.09716F, -0.04779F),
                new Vector3(20.57682F, 195.7362F, 24.03913F),
                new Vector3(1.14453F, 1.14453F, 1.14453F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Medkit, "DisplayMedkit",
                "Pelvis",
                new Vector3(0.51873F, -0.03912F, -0.15506F),
                new Vector3(64.35487F, 280.9532F, 343.4109F),
                new Vector3(0.91891F, 0.91891F, 0.91891F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Missile, "DisplayMissileLauncher",
                "Chest2",
                new Vector3(-0.99626F, 0.87369F, -0.85697F),
                new Vector3(333.4188F, 342.6633F, 51.17248F),
                new Vector3(0.25674F, 0.25674F, 0.25674F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MissileVoid, "DisplayMissileLauncherVoid",
                "Chest2",
                new Vector3(-0.99626F, 0.87369F, -0.85697F),
                new Vector3(333.4188F, 342.6633F, 51.17248F),
                new Vector3(0.25674F, 0.25674F, 0.25674F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.MonstersOnShrineUse, "DisplayMonstersOnShrineUse",
                "BlasterMuzzle",
                new Vector3(-0.34888F, -0.00001F, -0.85001F),
                new Vector3(323.0377F, 180F, 180F),
                new Vector3(0.18194F, 0.18194F, 0.18194F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Mushroom, "DisplayMushroom",
                "ThighL",
                new Vector3(-0.03708F, 0.55397F, -0.10728F),
                new Vector3(354.2128F, 304.5275F, 81.31253F),
                new Vector3(0.16292F, 0.16292F, 0.16292F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MushroomVoid, "DisplayMushroomVoid",
                "ThighL",
                new Vector3(-0.03708F, 0.55397F, -0.10728F),
                new Vector3(354.2128F, 304.5275F, 81.31253F),
                new Vector3(0.16292F, 0.16292F, 0.16292F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NearbyDamageBonus, "DisplayDiamond",
                "BlasterMuzzle",
                new Vector3(0F, 0F, 0F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.27F, 0.27F, 0.27F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnHeal,
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "JawLower",
                    new Vector3(-0.50982F, 0.78817F, 0.02107F),
                    new Vector3(281.7473F, 359.0303F, 326.7813F),
                    new Vector3(-1.21657F, 1.21657F, 1.21657F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "JawLower",
                    new Vector3(0.50975F, 0.78219F, 0.00585F),
                    new Vector3(61.46427F, 192.0264F, 162.9206F),
                    new Vector3(1.21657F, -1.21657F, -1.21657F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NovaOnLowHealth, "DisplayJellyGuts",
                "Head",
                new Vector3(0.12161F, -0.46106F, -0.30676F),
                new Vector3(335.7832F, 322.5751F, 348.6393F),
                new Vector3(0.2279F, 0.2279F, 0.2279F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ParentEgg, "DisplayParentEgg",
                "Chest",
                new Vector3(-0.20077F, -0.05798F, 0.33572F),
                new Vector3(24.21236F, 82.34745F, 7.68247F),
                new Vector3(0.14702F, 0.15563F, 0.14702F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Pearl, "DisplayPearl",
                "BlasterMuzzle",
                new Vector3(0F, 0F, 0F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.23837F, 0.23837F, 0.23837F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.PersonalShield, "DisplayShieldGenerator",
                "Chest2",
                new Vector3(0.18718F, -0.0303F, 0.36232F),
                new Vector3(283.4827F, 192.1683F, 184.5579F),
                new Vector3(0.29487F, 0.29487F, 0.29487F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Phasing, "DisplayStealthkit",
                "ThighL",
                new Vector3(0.10671F, 0.27818F, -0.22565F),
                new Vector3(7.21859F, 264.7771F, 260.5748F),
                new Vector3(0.67724F, 0.72107F, 0.72107F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Plant, "DisplayInterstellarDeskPlant",
                "Chest2",
                new Vector3(-0.17058F, 0.4592F, -0.46827F),
                new Vector3(336.6842F, 180.0002F, 224.2677F),
                new Vector3(0.15501F, 0.15325F, 0.15325F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RandomDamageZone, "DisplayRandomDamageZone",
                "Chest2",
                new Vector3(0.01955F, 0.22582F, -0.55984F),
                new Vector3(352.7343F, 356.8195F, 358.8024F),
                new Vector3(0.16608F, 0.21608F, 0.21608F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower",
                "Head",
                new Vector3(-0.44449F, -0.21154F, -0.29882F),
                new Vector3(347.1371F, 165.4696F, 219.927F),
                new Vector3(0.67991F, 0.67991F, 0.67991F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SecondarySkillMagazine, "DisplayDoubleMag",
                "BlasterMuzzle",
                new Vector3(0.03986F, -0.57908F, -0.71921F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.16153F, 0.13086F, 0.16705F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Seed, "DisplaySeed",
                "UpperArmL",
                new Vector3(-0.17144F, 0.59496F, 0.2469F),
                new Vector3(304.1826F, 4.82484F, 117.7949F),
                new Vector3(0.0866F, 0.0866F, 0.0866F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShieldOnly,
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                    "AntennaL",
                    new Vector3(-0.04259F, 0.26466F, -0.02614F),
                    new Vector3(350.3095F, 290.9932F, 203.2978F),
                    new Vector3(-0.92525F, -0.92525F, -0.92525F)),
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                    "AntennaR",
                    new Vector3(0.05234F, 0.34563F, -0.01423F),
                    new Vector3(357.7084F, 73.81708F, 163.456F),
                    new Vector3(0.92525F, -0.92525F, -0.92525F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ShinyPearl, "DisplayShinyPearl",
                "BlasterMuzzle",
                new Vector3(0.00001F, -0.00001F, 0.17818F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.23047F, 0.23047F, 0.23047F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                    "Chest2",
                    new Vector3(0.23686F, 0.50777F, -0.43372F),
                    new Vector3(294.005F, -0.00002F, 346.5252F),
                    new Vector3(0.51375F, 0.46564F, 0.51375F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SiphonOnLowHealth, "DisplaySiphonOnLowHealth",
                "UpperArmR",
                new Vector3(0.09677F, 0.41783F, 0.12635F),
                new Vector3(2.94545F, 227.4575F, 91.35806F),
                new Vector3(0.15407F, 0.15407F, 0.15441F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SlowOnHit, "DisplayBauble",
                "Head",
                new Vector3(0.86313F, -0.12346F, 1.73249F),
                new Vector3(299.4404F, 250.9929F, 92.84981F),
                new Vector3(1.28294F, 1.28294F, 1.28294F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.SlowOnHitVoid, "DisplayBaubleVoid",
                "Head",
                new Vector3(0.86313F, -0.12346F, 1.73249F),
                new Vector3(299.4404F, 250.9929F, 92.84981F),
                new Vector3(1.28294F, 1.28294F, 1.28294F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintArmor, "DisplayBuckler",
                "LowerArmL",
                new Vector3(-0.09264F, 0.26383F, 0.00683F),
                new Vector3(0.50452F, 267.8212F, 192.3497F),
                new Vector3(0.27256F, 0.27256F, 0.28676F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintBonus, "DisplaySoda",
                "Pelvis",
                new Vector3(-0.43441F, -0.05291F, -0.23556F),
                new Vector3(83.48098F, 178.4103F, 83.75294F),
                new Vector3(0.7442F, 0.7442F, 0.7442F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintOutOfCombat, "DisplayWhip",
                "Pelvis",
                new Vector3(0.21202F, 0.13121F, -0.3269F),
                new Vector3(0.25901F, 66.96402F, 181.4458F),
                new Vector3(0.50205F, 0.50205F, 0.50205F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintWisp, "DisplayBrokenMask",
                "LowerArmR",
                new Vector3(0F, 0.00001F, 0.16208F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.27709F, 0.27709F, 0.28064F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Squid, "DisplaySquidTurret",
                "Chest",
                new Vector3(0.29965F, 0.20324F, -0.35667F),
                new Vector3(18.8973F, 73.10674F, 261.9941F),
                new Vector3(0.15216F, 0.15216F, 0.15216F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StickyBomb, "DisplayStickyBomb",
                "ThighR",
                new Vector3(0.14866F, 0.1103F, 0.07951F),
                new Vector3(0.72324F, 69.3668F, 230.8888F),
                new Vector3(0.49384F, 0.49384F, 0.49384F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade",
                "Pelvis",
                new Vector3(-0.46512F, -0.0306F, 0.15349F),
                new Vector3(77.78546F, 250.1462F, 276.7083F),
                new Vector3(0.9552F, 0.9552F, 0.9552F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Syringe, "DisplaySyringeCluster",
                "Head",
                new Vector3(0.65557F, -0.10574F, -0.1022F),
                new Vector3(22.02188F, 204.3293F, 123.7495F),
                new Vector3(0.3826F, 0.3826F, 0.3826F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower",
                "Head",
                new Vector3(0.41012F, 0.63111F, -0.69291F),
                new Vector3(345.3089F, 147.8771F, 21.81723F),
                new Vector3(1.08733F, 1.08733F, 1.08733F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Talisman, "DisplayTalisman",
                "Root",
                new Vector3(2.71987F, 0.19196F, 2.26755F),
                new Vector3(82.82658F, 179.9599F, 180F),
                new Vector3(0.67098F, 0.67098F, 0.67098F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Thorns, "DisplayRazorwireLeft",
                "UpperArmL",
                new Vector3(0F, 0F, 0F),
                new Vector3(270F, 0F, 0F),
                new Vector3(1.30964F, 1.30964F, 1.30964F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TitanGoldDuringTP, "DisplayGoldHeart",
                "UpperArmL",
                new Vector3(0.09354F, 0.08975F, -0.2382F),
                new Vector3(342.3814F, 145.1818F, 25.25277F),
                new Vector3(0.49324F, 0.49324F, 0.49324F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                    "JawLower",
                    new Vector3(0.00137F, 1.37451F, -0.14882F),
                    new Vector3(301.3659F, 181.2764F, 0.55263F),
                    new Vector3(9.47466F, 9.47466F, 9.47466F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "JawLower",
                    new Vector3(0.24857F, 1.31074F, -0.14255F),
                    new Vector3(292.6077F, 126.7667F, 43.91162F),
                    new Vector3(6.71988F, 9.20659F, 7.67519F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Head",
                    new Vector3(0.12476F, 1.3115F, -0.12263F),
                    new Vector3(275.0388F, 180F, 180F),
                    new Vector3(6.7138F, 7.47924F, 7.47924F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "Head",
                    new Vector3(-0.15169F, 1.31855F, -0.11786F),
                    new Vector3(279.1406F, 216.2326F, 144.1158F),
                    new Vector3(6.13003F, 7.86482F, 7.77687F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Head",
                    new Vector3(-0.27194F, 1.31476F, -0.0457F),
                    new Vector3(288.7133F, 194.577F, 344.5414F),
                    new Vector3(-6.67229F, 7.48147F, 7.48147F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TreasureCache, "DisplayKey",
                "ThighR",
                new Vector3(0.13288F, 0.34394F, 0.13914F),
                new Vector3(10.74143F, 142.8751F, 264.398F),
                new Vector3(2.43618F, 2.43618F, 2.43618F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.TreasureCacheVoid, "DisplayKeyVoid",
                "ThighR",
                new Vector3(0.13288F, 0.34394F, 0.13914F),
                new Vector3(10.74143F, 142.8751F, 264.398F),
                new Vector3(2.43618F, 2.43618F, 2.43618F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                    "UpperArmL",
                    new Vector3(0.25216F, -0.08412F, 0.01908F),
                    new Vector3(338.0003F, 169.2254F, 71.08247F),
                    new Vector3(1.42234F, 1.42234F, 1.42234F)),
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                    "UpperArmR",
                    new Vector3(-0.03636F, -0.18198F, -0.11838F),
                    new Vector3(15.65939F, 288.8891F, 90.57043F),
                    new Vector3(1.42234F, 1.42234F, 1.42234F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WarCryOnMultiKill, "DisplayPauldron",
                "UpperArmL",
                new Vector3(0.69756F, -0.97347F, 0.40355F),
                new Vector3(9.42379F, 191.1616F, 155.7245F),
                new Vector3(1.59732F, 1.59732F, 1.59732F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WardOnLevel, "DisplayWarbanner",
                "Pelvis",
                new Vector3(-0.01386F, -0.02773F, 0.28608F),
                new Vector3(72.86674F, 0F, 270F),
                new Vector3(0.76969F, 0.76969F, 0.76969F)));
            #endregion items


            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                    "Head",
                    new Vector3(-0.01223F, 1.14896F, -0.46899F),
                    new Vector3(16.94042F, 180F, 180F),
                    new Vector3(0.44335F, 0.56488F, 0.40164F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(JunkContent.Items.Incubator, "DisplayAncestralIncubator",
                "Chest",
                new Vector3(0F, -0.01086F, 0.2805F),
                new Vector3(86.61887F, 0F, 0F),
                new Vector3(0.05675F, 0.05675F, 0.05675F)));

            #region quips
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG",
                "Head",
                new Vector3(0F, -0.24077F, -0.44684F),
                new Vector3(282.1764F, 180F, 180F),
                new Vector3(1.05274F, 1.05274F, 1.05274F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Blackhole, "DisplayGravCube",
                "Root",
                new Vector3(-2.1388F, 2.07383F, 3.72212F),
                new Vector3(358.3824F, 269.3275F, 292.5764F),
                new Vector3(0.44798F, 0.44798F, 0.44798F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion",
                "ThighL",
                new Vector3(-0.18402F, 0.11479F, 0.1647F),
                new Vector3(346.0888F, 101.4104F, 236.5228F),
                new Vector3(0.14594F, 0.14594F, 0.14594F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack",
                "Chest",
                new Vector3(-0.38586F, 0.09949F, -0.23497F),
                new Vector3(358.7266F, 204.1129F, 343.2047F),
                new Vector3(0.10558F, 0.10558F, 0.10558F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack",
                "Chest",
                new Vector3(-0.29763F, 0.13521F, -0.2938F),
                new Vector3(58.10476F, 148.1461F, 295.6303F),
                new Vector3(0.51881F, 0.51881F, 0.51881F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy",
                "Chest",
                new Vector3(-0.3453F, 0.02325F, -0.27103F),
                new Vector3(344.8184F, 48.49264F, 345.2058F),
                new Vector3(0.76744F, 0.76744F, 0.76744F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant",
                "Head",
                new Vector3(0F, 1.3562F, -0.4028F),
                new Vector3(288.8796F, 180F, 180F),
                new Vector3(0.79284F, 0.67582F, 0.60637F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile",
                "Chest",
                new Vector3(-0.37094F, 0.13258F, -0.40528F),
                new Vector3(3.34161F, 210.3051F, 0.3832F),
                new Vector3(0.16985F, 0.16985F, 0.16985F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio",
                "Chest",
                new Vector3(-0.31941F, 0.35709F, -0.38112F),
                new Vector3(15.50678F, 219.127F, 5.22551F),
                new Vector3(1.05848F, 1.05848F, 1.05848F)));
            //E for Affix
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                    "Head",
                    new Vector3(0.50193F, 0.52867F, -0.60352F),
                    new Vector3(287.7487F, 0F, 0F),
                    new Vector3(0.28811F, 0.28811F, 0.28811F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                    "Head",
                    new Vector3(-0.50193F, 0.52867F, -0.60352F),
                    new Vector3(287.7487F, 0F, 0F),
                    new Vector3(-0.28811F, 0.28811F, 0.28811F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixHaunted, "DisplayEliteStealthCrown",
                "Head",
                new Vector3(0F, 0.2182F, -1.24623F),
                new Vector3(0F, 0F, 180F),
                new Vector3(0.24274F, 0.21882F, 0.21882F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixWhite, "DisplayEliteIceCrown",
                "Head",
                new Vector3(-0.0003F, 0.34138F, -1.17297F),
                new Vector3(4.71233F, 180.3457F, 180F),
                new Vector3(0.10787F, 0.10787F, 0.10787F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                    "Head",
                    new Vector3(0F, 0.67225F, -0.88794F),
                    new Vector3(353.3259F, 180F, 180F),
                    new Vector3(1.06916F, 1.08535F, 0.91605F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                    "Head",
                    new Vector3(0F, 0.21259F, -0.67587F),
                    new Vector3(16.14397F, 180F, 180F),
                    new Vector3(0.84023F, 0.84023F, 0.72154F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixLunar, "DisplayEliteLunar,Eye",
                "BlasterMuzzle",
                new Vector3(-0.00001F, 0F, 0.51214F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.97023F, 0.90012F, 0.97023F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixPoison, "DisplayEliteUrchinCrown",
                "Head",
                new Vector3(0F, 0.42653F, -0.69298F),
                new Vector3(0F, 176.1556F, 0F),
                new Vector3(0.20102F, 0.20102F, 0.20102F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg",
                "Chest",
                new Vector3(-0.36926F, 0.22407F, -0.37353F),
                new Vector3(273.1251F, 238.5774F, 227.3353F),
                new Vector3(0.5476F, 0.5476F, 0.5476F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit", 
                "Chest",
                new Vector3(-0.38219F, -0.20111F, -0.06255F),
                new Vector3(8.23708F, 117.072F, 26.72394F),
                new Vector3(0.54046F, 0.54046F, 0.55747F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure",
                "Chest",
                new Vector3(-0.37884F, 0.02529F, -0.3432F),
                new Vector3(277.7346F, 120.7361F, 247.5209F),
                new Vector3(1.40785F, 1.40785F, 1.40785F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase",
                "Chest",
                new Vector3(-0.38316F, 0.37524F, -0.52765F),
                new Vector3(329.6677F, 98.36839F, 322.3167F),
                new Vector3(0.38566F, 0.38566F, 0.38566F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat",
                "Head",
                new Vector3(-0.03559F, -0.53565F, -1.17573F),
                new Vector3(0.29125F, 95.83733F, 250.4841F),
                new Vector3(0.33432F, 0.33432F, 0.33432F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings",
                "Chest2",
                new Vector3(-0.00211F, 0.04164F, -0.25163F),
                new Vector3(353.4624F, 0.485F, 0F),
                new Vector3(0.31474F, 0.31474F, 0.31474F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
                "Head",
                new Vector3(-0.94177F, 0.2803F, -0.55421F),
                new Vector3(349.0632F, 72.66389F, 210.2734F),
                new Vector3(0.23928F, 0.23928F, 0.23928F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule("DisplayLightningArmRight",
                    "UpperArmR",
                    new Vector3(-0.46922F, 0.00002F, -0.00001F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(2.43136F, 2.43136F, 2.43136F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Meteor, "DisplayMeteor",
                "Root",
                new Vector3(1.99839F, 1.59533F, 1.94409F),
                new Vector3(90F, 0F, 0F),
                new Vector3(0.76542F, 0.76645F, 0.76645F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray",
                "Chest2",
                new Vector3(-0.00726F, 0.19846F, -0.4114F),
                new Vector3(10.45362F, 0F, 0F),
                new Vector3(0.58441F, 0.58441F, 0.58441F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler",
                "Chest",
                new Vector3(-0.35262F, 0.25821F, -0.26688F),
                new Vector3(71.62891F, 122.654F, 329.68F),
                new Vector3(0.13933F, 0.13933F, 0.13933F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Saw, "DisplaySawmerangFollower",
                "Root",
                new Vector3(-1.8725F, 1.08661F, 0.11587F),
                new Vector3(353.1408F, 0.02619F, 0.0896F),
                new Vector3(0.31156F, 0.31156F, 0.31156F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner",
                "UpperArmR",
                new Vector3(-0.01928F, 0.22198F, 0.16164F),
                new Vector3(0.47662F, 333.8952F, 74.53624F),
                new Vector3(0.27798F, 0.27798F, 0.27798F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry",
                "Chest",
                new Vector3(-0.35818F, 0.13941F, -0.31636F),
                new Vector3(10.76094F, 229.7132F, 12.76454F),
                new Vector3(0.13029F, 0.13029F, 0.13029F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic",
                "Chest",
                new Vector3(-0.38928F, 0.27093F, -0.34903F),
                new Vector3(0F, 191.2099F, 343.7879F),
                new Vector3(0.48425F, 0.48425F, 0.48425F)));

            #endregion quips

            #region dlc1

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.AttackSpeedAndMoveSpeed, "DisplayCoffee",
                "Pelvis",
                new Vector3(0.41462F, -0.01737F, 0.19108F),
                new Vector3(0.08375F, 151.7734F, 180.1561F),
                new Vector3(0.49654F, 0.49654F, 0.49654F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritDamage, "DisplayLaserSight",
                "BlasterMuzzle",
                new Vector3(0.27703F, -0.03916F, -0.85357F),
                new Vector3(90F, 90F, 0F),
                new Vector3(0.25821F, 0.25821F, 0.25821F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.FragileDamageBonus, "DisplayDelicateWatch",
                "HandL",
                new Vector3(-0.02902F, -0.04717F, -0.01335F),
                new Vector3(275.3239F, 0.00046F, 104.6101F),
                new Vector3(1.14952F, 1.82183F, 1.17459F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.FreeChest, "DisplayShippingRequestForm",
                "Chest",
                new Vector3(-0.50356F, 0.12129F, 0.11323F),
                new Vector3(40.35332F, 204.6597F, 261.2637F),
                new Vector3(0.44837F, 0.44837F, 0.44837F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.GoldOnHurt, "DisplayRollOfPennies",
                "CalfL",
                new Vector3(0.00539F, 0.10213F, -0.13568F),
                new Vector3(353.7165F, 194.5831F, 181.0946F),
                new Vector3(0.74986F, 0.74986F, 0.74986F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HalfAttackSpeedHalfCooldowns, "DisplayLunarShoulderNature",
                "UpperArmR",
                new Vector3(0.168F, -0.07291F, -0.15282F),
                new Vector3(335.568F, 353.7823F, 248.5501F),
                new Vector3(2.09428F, 2.09428F, 1.4944F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HalfSpeedDoubleHealth, "DisplayLunarShoulderStone",
                "UpperArmL",
                new Vector3(-0.05518F, 0.03162F, -0.19076F),
                new Vector3(334.6733F, 123.6424F, 219.4931F),
                new Vector3(1.31927F, 1.31927F, 1.41815F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HealingPotion, "DisplayHealingPotion",
                "ThighL",
                new Vector3(-0.18402F, 0.11479F, 0.1647F),
                new Vector3(346.0888F, 101.4104F, 236.5228F),
                new Vector3(0.08254F, 0.08254F, 0.08254F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ImmuneToDebuff, "DisplayRainCoatBelt",
                "Pelvis",
                new Vector3(0F, 0F, 0F),
                new Vector3(4.47548F, 180F, 180F),
                new Vector3(1.76629F, 1.76629F, 1.76629F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.LunarSun,
                ItemDisplays.CreateDisplayRule("DisplaySunHead",
                    "Head",
                    new Vector3(-0.00925F, 0.42354F, 0.09981F),
                    new Vector3(289.9818F, 182.754F, 176.6481F),
                    new Vector3(4.96869F, 4.96869F, 4.96869F)),
                ItemDisplays.CreateDisplayRule("DisplaySunHeadNeck",
                    "Chest2",
                    new Vector3(-0.08873F, 0.14329F, 0.03215F),
                    new Vector3(26.44894F, 359.4203F, 7.24537F),
                    new Vector3(6.43598F, 4.834F, 6.43598F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MinorConstructOnKill, "DisplayDefenseNucleus", "Root",
                new Vector3(2.05899F, -1.50395F, 1.61798F),
                new Vector3(87.02218F, 0F, 0F),
                new Vector3(0.09557F, 0.09557F, 0.09557F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MoreMissile, "DisplayICBM",
                "BlasterMuzzle",
                new Vector3(-0.00002F, -0.0045F, 0.28002F),
                new Vector3(89.08871F, 180F, 180F),
                new Vector3(0.27016F, 0.27016F, 0.27016F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MoveSpeedOnKill, "DisplayGrappleHook",
                "HandL",
                new Vector3(0.0773F, 0.49137F, -0.13871F),
                new Vector3(272.8235F, 180F, 198.4255F),
                new Vector3(0.50945F, 0.50945F, 0.50945F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.OutOfCombatArmor, "DisplayOddlyShapedOpal",
                "Chest2",
                new Vector3(-0.5084F, -0.1132F, 0.19136F),
                new Vector3(354.3865F, 324.0328F, 15.43204F),
                new Vector3(0.72944F, 0.72944F, 0.77849F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.PermanentDebuffOnHit, "DisplayScorpion",
                "Head",
                new Vector3(0F, -0.27455F, -0.47836F),
                new Vector3(321.817F, 0F, 0F),
                new Vector3(2.54155F, 2.54155F, 2.54155F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.PrimarySkillShuriken, "DisplayShuriken",
                "Pelvis",
                new Vector3(-0.46512F, -0.0306F, 0.15349F),
                new Vector3(77.78548F, 250.1462F, 276.7083F),
                new Vector3(1.90855F, 1.90855F, 1.90855F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RandomEquipmentTrigger, "DisplayBottledChaos",
                "Chest2",
                new Vector3(-0.33949F, -0.01705F, -0.44352F),
                new Vector3(344.3086F, 357.0746F, 10.69999F),
                new Vector3(0.38097F, 0.38097F, 0.38097F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RandomlyLunar, "DisplayDomino",
                "Root",
                new Vector3(-3.0151F, -1.1793F, 0.85897F),
                new Vector3(283.1516F, 0F, 0F),
                new Vector3(0.33647F, 0.33647F, 0.33647F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RegeneratingScrap, "DisplayRegeneratingScrap",
                "Head",
                new Vector3(-0.65753F, -0.08958F, 0.10294F),
                new Vector3(314.6952F, 131.0824F, 240.7385F),
                new Vector3(0.52508F, 0.52508F, 0.52508F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.StrengthenBurn, "DisplayGasTank",
                "Pelvis",
                new Vector3(-0.08011F, -0.02243F, -0.29884F),
                new Vector3(339.9525F, 170.2501F, 213.7814F),
                new Vector3(0.24152F, 0.24152F, 0.24152F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.VoidMegaCrabItem, "DisplayMegaCrabItem",
                "Chest",
                new Vector3(-0.1421F, 0.10773F, 0.34723F),
                new Vector3(323.592F, 286.2206F, 330.7858F),
                new Vector3(0.27109F, 0.27109F, 0.27109F)));

            //quips
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunter,
                ItemDisplays.CreateDisplayRule("DisplayTricornGhost",
                    "Head",
                    new Vector3(0.00157F, 0.28983F, -0.98398F),
                    new Vector3(307.0779F, 0.69018F, 359.4664F),
                    new Vector3(3.58098F, 3.58098F, 3.58098F)),
                ItemDisplays.CreateDisplayRule("DisplayBlunderbuss",
                    "Chest",
                    new Vector3(2.4375F, -0.08533F, 0.38143F),
                    new Vector3(84.07401F, 180F, 180F),
                    new Vector3(0.88941F, 0.88941F, 0.88941F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.BossHunterConsumed, "DisplayTricornUsed",
                "Head",
                new Vector3(0.00157F, 0.28983F, -0.98398F),
                new Vector3(307.0779F, 0.69018F, 359.4664F),
                new Vector3(3.58098F, 3.58098F, 3.58098F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Elites.Earth.eliteEquipmentDef, "DisplayEliteMendingAntlers",
                "Head",
                new Vector3(-0.00861F, 0.2106F, -0.53213F),
                new Vector3(271.3503F, 310.9296F, 48.51595F),
                new Vector3(2.54511F, 2.17896F, 2.17896F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.EliteVoidEquipment, "DisplayAffixVoid",
                "Head",
                new Vector3(-0.01556F, 1.14903F, -0.21271F),
                new Vector3(331.9779F, 359.1009F, 0.69465F),
                new Vector3(0.59628F, 0.59628F, 0.59628F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.GummyClone, "DisplayGummyClone",
                "Chest",
                new Vector3(-0.354F, 0.31998F, -0.27801F),
                new Vector3(9.39416F, 32.93084F, 4.31414F),
                new Vector3(0.43106F, 0.43106F, 0.43106F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule("IrradiatingLaser", "DisplayIrradiatingLaser",
                "Chest2",
                new Vector3(0.32473F, 0.19736F, -0.31676F),
                new Vector3(347.662F, 0.0222F, 301.8855F),
                new Vector3(0.36085F, 0.36085F, 0.36085F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.LunarPortalOnUse, "DisplayLunarPortalOnUse",
                "Chest",
                new Vector3(2.43372F, 0.5016F, -0.77127F),
                new Vector3(1F, 1F, 1F),
                new Vector3(0.45052F, 0.45052F, 0.45052F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.Molotov, "DisplayMolotov",
                "Chest",
                new Vector3(-0.32732F, 0.1376F, -0.37134F),
                new Vector3(349.4521F, 329.4246F, 341.1057F),
                new Vector3(0.47037F, 0.47037F, 0.47037F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.MultiShopCard, "DisplayExecutiveCard",
                "Chest",
                new Vector3(-0.34477F, 0.23037F, -0.31532F),
                new Vector3(348.0215F, 57.09729F, 284.7846F),
                new Vector3(2.01102F, 2.05534F, 2.11851F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.VendingMachine, "DisplayVendingMachine",
                "Chest",
                new Vector3(-0.34035F, 0.32171F, -0.25755F),
                new Vector3(17.56048F, 229.9249F, 358.521F),
                new Vector3(0.34086F, 0.34086F, 0.34086F)));

            #endregion

            #region compat

            try {
                if (Compat.TinkersSatchelInstalled) {
                    SetTinkersSatchelDisplayRules(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Helpers.LogWarning("error adding displays for Tinker's Satchel \n" + e);
            }


            try {
                if (Compat.AetheriumInstalled) {
                    SetAetheriumDisplayRules(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Helpers.LogWarning("error adding displays for Aetherium \n" + e);
            }
            
            try {
                if (Compat.ScepterInstalled) {
                    FixScepterDisplayRule(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Helpers.LogWarning("error adding displays for Scepter \n" + e);
            }

            #endregion
        }

        #region tinker
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetTinkersSatchelDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.Moustache.instance.itemDef, ThinkInvisible.TinkersSatchel.Moustache.instance.idrPrefab,
                "Head",
                new Vector3(-0.01752F, 1.24475F, -0.33803F),
                new Vector3(12.28016F, 304.6343F, 79.31148F),
                new Vector3(0.91095F, 0.64924F, 0.82879F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.itemDef, ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.idrPrefab, 
                "Head",
                new Vector3(-0.01582F, 1.32491F, -0.1451F),
                new Vector3(280.0381F, 185.8797F, 173.3946F),
                new Vector3(1.25328F, 0.99609F, 1.14028F)));
        }
        #endregion tinker

        #region aeth
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetAetheriumDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.AlienMagnet.instance.ItemDef, Aetherium.Items.AlienMagnet.ItemBodyModelPrefab,
                "Root",
                new Vector3(1.47972F, -0.26855F, 2.64377F),
                new Vector3(279.4077F, 180F, 180F),
                new Vector3(0.23718F, 0.23718F, 0.23718F)));
        }
        #endregion aeth

        #region scepter
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void FixScepterDisplayRule(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(AncientScepter.AncientScepterItem.instance.ItemDef, AncientScepter.AncientScepterItem.displayPrefab,
                "Pelvis",
                new Vector3(-0.50937F, -0.0893F, 0.0132F),
                new Vector3(86.38914F, 55.0462F, 214.3129F),
                new Vector3(0.6447F, 0.45949F, 0.58656F)));
        }

        #endregion scepter
    }
}

