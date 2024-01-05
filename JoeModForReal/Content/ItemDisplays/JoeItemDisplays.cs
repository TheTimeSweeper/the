using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Modules.Characters {
    public class JoeItemDisplays : ItemDisplaysBase {

        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            #region items
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AlienHead, "DisplayAlienHead",
                "ThighL",
                new Vector3(0.07129F, -0.06721F, 0.04653F),
                new Vector3(342.2635F, 104.16F, 319.1863F),
                new Vector3(0.78469F, 0.78469F, 0.78469F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate",
                "ThighR",
                new Vector3(-0.18141F, -0.05841F, 0.0019F),
                new Vector3(3.63752F, 93.15177F, 42.42118F),
                new Vector3(0.26921F, 0.25349F, 0.4066F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                    "HandL",
                    new Vector3(-0.05077F, -0.01475F, 1.13832F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.3367F, 0.24531F, 0.32235F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt",
                "Chest",
                new Vector3(-0.03784F, -0.11602F, 0F),
                new Vector3(308.3156F, 270F, 180F),
                new Vector3(0.23104F, 0.23104F, 0.23104F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil",
                "ThighL",
                new Vector3(0.03599F, 0.0946F, 0.02596F),
                new Vector3(38.63874F, 32.62673F, 293.4217F),
                new Vector3(0.4265F, 0.4265F, 0.4265F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier",
                "Chest",
                new Vector3(0.12947F, 0.02054F, -0.01471F),
                new Vector3(357.3241F, 144.7069F, 171.8118F),
                new Vector3(0.52884F, 0.52884F, 0.52884F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch",
                "Chest",
                new Vector3(0.04894F, 0.16181F, 0F),
                new Vector3(347.2412F, 90F, 0F),
                new Vector3(0.40468F, 0.34686F, 0.34686F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis",
                "LowerArmR",
                new Vector3(-0.08304F, -0.00474F, 0.05099F),
                new Vector3(297.3256F, 250.8487F, 117.9985F),
                new Vector3(0.13756F, 0.13756F, 0.13756F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bear, "DisplayBear",
                "Chest",
                new Vector3(-0.01323F, -0.10177F, 0.09117F),
                new Vector3(74.75165F, 325.7472F, 60.62724F),
                new Vector3(0.13914F, 0.14323F, 0.14323F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.BearVoid, "DisplayBearVoid",
                "Chest",
                new Vector3(-0.01323F, -0.10177F, 0.09117F),
                new Vector3(74.75165F, 325.7472F, 60.62724F),
                new Vector3(0.13914F, 0.14323F, 0.14323F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland",
                "ThighR",
                new Vector3(0.0614F, -0.08837F, -0.05545F),
                new Vector3(57.0552F, 148.4321F, 57.54914F),
                new Vector3(0.03761F, 0.03537F, 0.03537F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                    "Chest",
                    new Vector3(-0.1302F, 0.04636F, -0.12023F),
                    new Vector3(1.71177F, 266.218F, 359.4298F),
                    new Vector3(0.04103F, 0.03519F, 0.04103F))));


            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                    "HandR",
                    new Vector3(-0.04928F, -0.07821F, -0.00039F),
                    new Vector3(283.591F, 282.978F, 130.8807F),
                    new Vector3(0.27269F, 0.27269F, 0.32283F))));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                    "HandR",
                    new Vector3(-0.04928F, -0.07821F, -0.00039F),
                    new Vector3(283.591F, 282.978F, 130.8807F),
                    new Vector3(0.27269F, 0.27269F, 0.32283F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode",
                "Pelvis",
                new Vector3(-0.11846F, -0.01793F, -0.11984F),
                new Vector3(353.7355F, 9.40715F, 187.8441F),
                new Vector3(0.03818F, 0.03818F, 0.03818F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome",
                "ThighL",
                new Vector3(-0.12143F, -0.00443F, 0.07735F),
                new Vector3(3.99239F, 358.449F, 84.11081F),
                new Vector3(0.04449F, 0.04449F, 0.04449F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound",
                "Head",
                new Vector3(-0.065F, -0.09722F, -0.10281F),
                new Vector3(31.801F, 223.7817F, 176.4737F),
                new Vector3(0.34709F, 0.34709F, 0.34709F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook",
                "Chest",
                new Vector3(0.01859F, -0.14927F, -0.00973F),
                new Vector3(317.5175F, 183.6716F, 174.5724F),
                new Vector3(0.27248F, 0.27248F, 0.27248F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele",
                "Chest",
                new Vector3(-0.02813F, -0.10213F, -0.09024F),
                new Vector3(69.64777F, 258.9214F, 202.0094F),
                new Vector3(0.39786F, 0.37766F, 0.37731F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ChainLightningVoid, "DisplayUkuleleVoid",
                "Chest",
                new Vector3(-0.02813F, -0.10213F, -0.09024F),
                new Vector3(69.64777F, 258.9214F, 202.0094F),
                new Vector3(0.39786F, 0.37766F, 0.37731F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                "Head",
                new Vector3(-0.08897F, -0.00349F, 0.05372F),
                new Vector3(74.02F, 330.7106F, 5.67681F),
                new Vector3(0.34784F, 0.34784F, 0.34784F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CloverVoid, "DisplayCloverVoid",
                "Head",
                new Vector3(-0.08897F, -0.00349F, 0.05372F),
                new Vector3(74.02F, 330.7106F, 5.67681F),
                new Vector3(0.34784F, 0.34784F, 0.34784F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                    "HandR",
                    new Vector3(-0.09938F, -0.0127F, -0.01876F),
                    new Vector3(283.5852F, 326.645F, 303.5377F),
                    new Vector3(0.18033F, 0.22977F, 0.16337F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.CritGlasses, "DisplayGlasses",
                "Head",
                new Vector3(-0.00891F, 0.07045F, -0.00001F),
                new Vector3(270F, 90F, 0F),
                new Vector3(0.16588F, 0.15519F, 0.18847F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritGlassesVoid, "DisplayGlassesVoid",
                "Head",
                new Vector3(-0.00891F, 0.07045F, -0.00001F),
                new Vector3(270F, 90F, 0F),
                new Vector3(0.16588F, 0.15519F, 0.18847F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar",
                "Chest",
                new Vector3(0.17614F, -0.09144F, -0.09915F),
                new Vector3(62.55324F, 139.5489F, 265.7291F),
                new Vector3(0.20109F, 0.20109F, 0.20109F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger",
                "UpperArmR",
                new Vector3(0.03972F, -0.0615F, -0.10619F),
                new Vector3(2.04296F, 68.38008F, 196.3552F),
                new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark",
                "HandL",
                new Vector3(-0.05198F, -0.0104F, -0.2613F),
                new Vector3(350.9094F, 180F, 180F),
                new Vector3(0.02658F, 0.02658F, 0.02658F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn",
                "Pelvis",
                new Vector3(-0.10077F, -0.10391F, -0.10878F),
                new Vector3(12.75722F, 187.0361F, 257.3648F),
                new Vector3(0.26108F, 0.26108F, 0.23699F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery",
                "Pelvis",
                new Vector3(-0.17705F, -0.10629F, 0.02738F),
                new Vector3(0F, 179.7544F, 0F),
                new Vector3(0.14272F, 0.14272F, 0.14272F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.EquipmentMagazineVoid, "DisplayFuelCellVoid",
                "Pelvis",
                new Vector3(-0.17705F, -0.10629F, 0.02738F),
                new Vector3(0F, 179.7544F, 0F),
                new Vector3(0.14272F, 0.14272F, 0.14272F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                "LowerArmR",
                new Vector3(-0.07793F, -0.04122F, 0.016F),
                new Vector3(320.1651F, 0F, 0F),
                new Vector3(0.10182F, 0.10182F, 0.10066F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                "Pelvis",
                new Vector3(-0.07138F, 0.07983F, -0.10021F),
                new Vector3(313.9774F, 216.1529F, 226.8736F),
                new Vector3(0.04035F, 0.04035F, 0.04035F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExplodeOnDeathVoid, "DisplayWillowWispVoid",
                "Pelvis",
                new Vector3(-0.07138F, 0.07983F, -0.10021F),
                new Vector3(313.9774F, 216.1529F, 226.8736F),
                new Vector3(0.04035F, 0.04035F, 0.04035F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
                "Head",
                new Vector3(0.0397F, -0.0887F, 0.01381F),
                new Vector3(72.35673F, 222.8316F, 319.8407F),
                new Vector3(0.14082F, 0.14495F, 0.14495F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExtraLifeVoid, "DisplayHippoVoid",
                "Head",
                new Vector3(0.0397F, -0.0887F, 0.01381F),
                new Vector3(72.35673F, 222.8316F, 319.8407F),
                new Vector3(0.14082F, 0.14495F, 0.14495F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                    "CalfL",
                    new Vector3(-0.3018F, -0.00544F, 0.00214F),
                    new Vector3(70.40911F, 162.5544F, 83.66862F),
                    new Vector3(0.13661F, 0.13661F, 0.13661F)),
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                    "CalfR",
                    new Vector3(-0.3018F, -0.00544F, 0.00214F),
                    new Vector3(65.08602F, 7.21902F, 279.7443F),
                    new Vector3(0.13661F, 0.13661F, 0.13661F)
                )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                    "UpperArmL",
                    new Vector3(0.05532F, -0.0176F, 0.01458F),
                    new Vector3(297.8199F, 319.4897F, 4.14784F),
                    new Vector3(-0.02731F, 0.01641F, 0.01607F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",
                "LowerArmR",
                new Vector3(0F, 0F, 0F),
                new Vector3(2.48772F, 269.286F, 266.0015F),
                new Vector3(0.52248F, 0.53076F, 0.52248F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing", //f
                "LowerArmR",
                new Vector3(0F, 0F, 0F),
                new Vector3(2.48772F, 269.286F, 266.0015F),
                new Vector3(0.52248F, 0.53076F, 0.52248F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ElementalRingVoid, "DisplayVoidRing",//f
                "LowerArmR",
                new Vector3(0F, 0F, 0F),
                new Vector3(2.48772F, 269.286F, 266.0015F),
                new Vector3(0.52248F, 0.53076F, 0.52248F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                "HandR",
                new Vector3(-0.16744F, -0.05533F, -0.04449F),
                new Vector3(29.62401F, 259.7357F, 348.1937F),
                new Vector3(0.04189F, 0.04189F, 0.04189F)));
            //stope
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                "CalfR",
                new Vector3(-0.13178F, -0.00693F, -0.07418F),
                new Vector3(82.45949F, 286.4524F, 127.7181F),
                new Vector3(0.25016F, 0.25016F, 0.25016F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                "Chest",
                new Vector3(0.02025F, 0.10566F, 0.11138F),
                new Vector3(309.9925F, 18.50275F, 234.0282F),
                new Vector3(0.07068F, 0.07068F, 0.07068F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                "Root",
                new Vector3(-1.16428F, 3.55328F, -0.03402F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.09477F, 0.09477F, 0.09477F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask",
                "Head",
                new Vector3(-0.01228F, 0.03016F, 0F),
                new Vector3(270F, 90F, 0F),
                new Vector3(0.3756F, 0.43457F, 0.33518F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown",
                "Head",
                new Vector3(-0.06586F, -0.01967F, 0F),
                new Vector3(270F, 90F, 0F),
                new Vector3(0.47749F, 0.54618F, 0.47257F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown",
                "Base",
                new Vector3(-0.10319F, 0F, 0F),
                new Vector3(270F, 90F, 0F),
                new Vector3(0.43883F, 0.14142F, 0.13797F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                "LowerArmR",
                new Vector3(-0.05065F, -0.01216F, 0.02778F),
                new Vector3(11.39977F, 272.3672F, 124.6733F),
                new Vector3(0.13636F, 0.13636F, 0.13636F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail",
                                                                       "Head",
                                                                       new Vector3(-0.16092F, 0.0265F, -0.08058F),
                                                                       new Vector3(46.43885F, 226.517F, 348.8213F),
                                                                       new Vector3(0.06654F, 0.06654F, 0.06654F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
                ItemDisplays.CreateDisplayRule("DisplayHoof",
                                               "CalfR",
                                               new Vector3(-0.02149F, 0.35335F, -0.04254F),
                                               new Vector3(79.93871F, 359.8341F, 341.8235F),
                                               new Vector3(0.11376F, 0.10848F, 0.09155F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Icicle, "DisplayFrostRelic",
                                                                       "Root",
                                                                       new Vector3(1.05635F, 1.9722F, 0.42236F),
                                                                       new Vector3(90F, 0.000F, 0.0f),
                                                                       new Vector3(1.38875F, 1.38875F, 1.38875F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline",
                                                                       "CalfL",
                                                                       new Vector3(-0.07308F, 0.13817F, 0.09159F),
                                                                       new Vector3(81.46447F, 242.3112F, 218.6413F),
                                                                       new Vector3(0.49393F, 0.49393F, 0.49393F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(0.10476F, 0.16255F, 0.00485F),
                                               new Vector3(353.9438F, 81.39291F, 0F),
                                               new Vector3(0.29875F, 0.29875F, 0.29875F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(-0.09592F, 0.16205F, -0.01877F),
                                               new Vector3(356.1463F, 266.133F, 355.7455F),
                                               new Vector3(0.3801F, 0.3801F, 0.3801F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(JunkContent.Items.Incubator, "DisplayAncestralIncubator",
                                                                       "Chest",
                                                                       new Vector3(0.19515F, -0.05889F, -0.1113F),
                                                                       new Vector3(9.51898F, 20.83393F, 340.9285F),
                                                                       new Vector3(0.02595F, 0.02595F, 0.02595F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion",
                                                                       "Pelvis",
                                                                       new Vector3(0.17865F, -0.10954F, 0.10937F),
                                                                       new Vector3(10.36471F, 37.47284F, 182.8334F),
                                                                       new Vector3(0.38486F, 0.38486F, 0.3884F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.JumpBoost, "DisplayWaxBird",
                                                                       "Head",
                                                                       new Vector3(0F, -0.26665F, -0.06925F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.78163F, 0.78163F, 0.78163F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.KillEliteFrenzy, "DisplayBrainstalk",
                                                                       "Head",
                                                                       new Vector3(0.01376F, 0.06547F, -0.01949F),
                                                                       new Vector3(0F, 151.4707F, 0F),
                                                                       new Vector3(0.3F, 0.42441F, 0.29789F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Knurl, "DisplayKnurl",
                                                                       "UpperArmL",
                                                                       new Vector3(0.04002F, 0.36383F, -0.0365F),
                                                                       new Vector3(333.1747F, 352.5555F, 18.78204F),
                                                                       new Vector3(0.06807F, 0.06807F, 0.06807F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.13105F, -0.01649F, -0.09407F),
                                                                       new Vector3(357.5772F, 332.1058F, 93.51953F),
                                                                       new Vector3(0.28718F, 0.28718F, 0.28718F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",//f
                "HandR",
                new Vector3(-0.16744F, -0.05533F, -0.04449F),
                new Vector3(29.62401F, 259.7357F, 348.1937F),
                new Vector3(0.04189F, 0.04189F, 0.04189F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                                                                       "Chest",
                                                                       new Vector3(0.03957F, 0.06033F, -0.28021F),
                                                                       new Vector3(37.25228F, 260.2719F, 79.45548F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarPrimaryReplacement, "DisplayBirdEye",
                                                                       "Head",
                                                                       new Vector3(0.00176F, 0.17017F, 0.15579F),
                                                                       new Vector3(282.9004F, 180F, 180F),
                                                                       new Vector3(0.23053F, 0.23053F, 0.23053F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSecondaryReplacement, "DisplayBirdClaw",
                                                                       "UpperArmL",
                                                                       new Vector3(0.02726F, 0.36699F, -0.08066F),
                                                                       new Vector3(346.9768F, 234.68F, 334.6309F),
                                                                       new Vector3(0.56869F, 0.56869F, 0.56869F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSpecialReplacement, "DisplayBirdHeart",
                                                                       "Root",
                                                                       new Vector3(-1.18817F, 1.85883F, -0.45721F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.33744F, 0.33744F, 0.33744F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads",
                                                                       "Chest",
                                                                       new Vector3(-0.29107F, 0.15572F, 0.16556F),
                                                                       new Vector3(12.47539F, 158.354F, 310.8714F),
                                                                       new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot",
                                                                       "CalfR",
                                                                       new Vector3(0.17329F, 0.10984F, -0.04414F),
                                                                       new Vector3(20.57682F, 195.7362F, 24.03913F),
                                                                       new Vector3(0.84595F, 0.84595F, 0.84595F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Medkit, "DisplayMedkit",
                                                                       "Pelvis",
                                                                       new Vector3(0.23334F, 0.11768F, 0.08936F),
                                                                       new Vector3(64.35487F, 280.9532F, 69.71149F),
                                                                       new Vector3(0.6F, 0.6F, 0.6F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Missile, "DisplayMissileLauncher",
                                                                       "Chest",
                                                                       new Vector3(-0.30528F, 0.60239F, -0.12313F),
                                                                       new Vector3(345.3658F, 346.8422F, 17.42729F),
                                                                       new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MissileVoid, "DisplayMissileLauncherVoid",
                                                                       "Chest",
                                                                       new Vector3(-0.30528F, 0.60239F, -0.12313F),
                                                                       new Vector3(345.3658F, 346.8422F, 17.42729F),
                                                                       new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.MonstersOnShrineUse, "DisplayMonstersOnShrineUse",
                                                                       "HandL",
                                                                       new Vector3(-0.01711F, 0.28384F, 0.02934F),
                                                                       new Vector3(73.75125F, 52.4324F, 48.00412F),
                                                                       new Vector3(0.12101F, 0.12101F, 0.12101F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Mushroom, "DisplayMushroom",
                                                                       "ThighL",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MushroomVoid, "DisplayMushroomVoid",
                                                                       "ThighL",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NearbyDamageBonus, "DisplayDiamond",
                                                                       "LowerArmL",
                                                                       new Vector3(0.11959F, 0.21592F, -0.01724F),
                                                                       new Vector3(343.342F, 107.0507F, 356.529F),
                                                                       new Vector3(0.08798F, 0.08798F, 0.08798F)));
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
                                                                       "Head",
                                                                       new Vector3(0.05007F, 0.02095F, -0.18202F),
                                                                       new Vector3(345.7531F, 325.5393F, 340.7766F),
                                                                       new Vector3(0.10713F, 0.10713F, 0.10713F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ParentEgg, "DisplayParentEgg",
                                                                       "Chest",
                                                                       new Vector3(0.0289F, -0.08568F, 0.41864F),
                                                                       new Vector3(24.21236F, 82.34744F, 7.68247F),
                                                                       new Vector3(0.09096F, 0.09096F, 0.09096F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Pearl, "DisplayPearl",
                                                                       "LowerArmL",
                                                                       new Vector3(-0.00001F, 0.19616F, -0.02199F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.PersonalShield, "DisplayShieldGenerator",
                                                                       "Chest",
                                                                       new Vector3(0.17905F, 0.12874F, 0.32696F),
                                                                       new Vector3(279.9448F, 70.53902F, 320.1869F),
                                                                       new Vector3(0.1923F, 0.1923F, 0.1923F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Phasing, "DisplayStealthkit",
                                                                       "ThighL",
                                                                       new Vector3(0.1005F, 0.27421F, -0.15216F),
                                                                       new Vector3(4.18787F, 230.1978F, 263.3475F),
                                                                       new Vector3(0.32966F, 0.35099F, 0.35099F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Plant, "DisplayInterstellarDeskPlant",
                                                                       "UpperArmL",
                                                                       new Vector3(0.1207F, 0.23304F, 0.02712F),
                                                                       new Vector3(358.9562F, 83.84697F, 0F),
                                                                       new Vector3(0.08447F, 0.08351F, 0.08351F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RandomDamageZone, "DisplayRandomDamageZone",
                                                                       "Chest",
                                                                       new Vector3(0.0071F, 0.21452F, -0.35F),
                                                                       new Vector3(352.7343F, 356.8195F, 358.8024F),
                                                                       new Vector3(0.08041F, 0.10462F, 0.10462F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower",
                                                                       "Chest",
                                                                       new Vector3(-0.22553F, 0.32547F, -0.10069F),
                                                                       new Vector3(356.7155F, 152.0292F, 314.6104F),
                                                                       new Vector3(0.19141F, 0.19141F, 0.19141F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SecondarySkillMagazine, "DisplayDoubleMag",
                                                                       "HandR",
                                                                       new Vector3(-0.20847F, 0.32423F, 0.14056F),
                                                                       new Vector3(284.718F, 135.7026F, 146.5896F),
                                                                       new Vector3(0.09266F, 0.07507F, 0.09583F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Seed, "DisplaySeed", //don
                                                                       "UpperArmL",
                                                                       new Vector3(-0.00351F, 0.27648F, 0.10802F),
                                                                       new Vector3(330.0132F, 62.84435F, 84.55939F),
                                                                       new Vector3(0.0537F, 0.0537F, 0.0537F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShieldOnly,
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(-0.0869F, 0.1876F, 0.07631F),
                                               new Vector3(345.8107F, 93.77663F, 165.6585F),
                                               new Vector3(0.19801F, -0.19801F, 0.19801F)),
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(0.0869F, 0.1876F, 0.07631F),
                                               new Vector3(4.33289F, 82.829F, 175.8128F),
                                               new Vector3(0.19801F, -0.19801F, -0.19801F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ShinyPearl, "DisplayShinyPearl",
                                                                       "LowerArmL",
                                                                       new Vector3(-0.03611F, 0.26797F, 0.01394F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                                               "HandR",
                                               new Vector3(0.01605F, 0.19069F, 0.07331F),
                                               new Vector3(76.7001F, 0F, 0F),
                                               new Vector3(0.51375F, 0.46564F, 0.51375F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftLeg)//gauntlet coil
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SiphonOnLowHealth, "DisplaySiphonOnLowHealth",
                                                                       "ThighL",
                                                                       new Vector3(0.01442F, 0.13594F, 0.20742F),
                                                                       new Vector3(358.4349F, 359.0759F, 190.1793F),
                                                                       new Vector3(0.08824F, 0.08824F, 0.08844F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SlowOnHit, "DisplayBauble",
                                                                       "ThighR",
                                                                       new Vector3(-0.21464F, 0.59199F, 0.17571F),
                                                                       new Vector3(358.1689F, 29.80261F, 166.009F),
                                                                       new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.SlowOnHitVoid, "DisplayBaubleVoid",
                                                                       "ThighR",
                                                                       new Vector3(-0.21464F, 0.59199F, 0.17571F),
                                                                       new Vector3(358.1689F, 29.80261F, 166.009F),
                                                                       new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintArmor, "DisplayBuckler",
                                                                       "LowerArmL",
                                                                       new Vector3(0.08204F, 0.15889F, -0.02187F),
                                                                       new Vector3(339.7112F, 94.07677F, 338.7357F),
                                                                       new Vector3(0.16504F, 0.16504F, 0.17364F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintBonus, "DisplaySoda",
                                                                       "Pelvis",
                                                                       new Vector3(-0.20959F, -0.08101F, 0.09991F),
                                                                       new Vector3(74.71255F, 119.2519F, 25.34358F),
                                                                       new Vector3(0.36722F, 0.36722F, 0.36722F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintOutOfCombat, "DisplayWhip",
                                                                       "Pelvis",
                                                                       new Vector3(0.04666F, 0.05334F, 0.20305F),
                                                                       new Vector3(352.989F, 106.0337F, 188.9492F),
                                                                       new Vector3(0.32902F, 0.32902F, 0.32902F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintWisp, "DisplayBrokenMask",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.01908F, 0.08165F, -0.22117F),
                                                                       new Vector3(358.7429F, 202.6943F, 191.4706F),
                                                                       new Vector3(0.1353F, 0.1353F, 0.1353F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Squid, "DisplaySquidTurret",
                                                                "ThighR",
                                                                new Vector3(-0.15008F, 0.21693F, -0.02095F),
                                                                new Vector3(6.47033F, 168.8895F, 289.4581F),
                                                                new Vector3(0.06125F, 0.06125F, 0.06125F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StickyBomb, "DisplayStickyBomb",
                                                                       "ThighR",
                                                                       new Vector3(-0.03883F, 0.03832F, 0.17497F),
                                                                       new Vector3(343.7692F, 38.4146F, 52.43128F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade",
                                                                       "Pelvis",
                                                                       new Vector3(-0.1332F, -0.04592F, 0.16319F),
                                                                       new Vector3(69.5353F, 188.4479F, 216.9409F),
                                                                       new Vector3(0.82778F, 0.82778F, 0.82778F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Syringe, "DisplaySyringeCluster",
                                                                       "Chest",
                                                                       new Vector3(0.25482F, 0.04429F, -0.05188F),
                                                                       new Vector3(325.976F, 201.993F, 122.6787F),
                                                                       new Vector3(0.15369F, 0.15369F, 0.15369F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Talisman, "DisplayTalisman",
                                                                       "Root",
                                                                       new Vector3(1.35249F, 1.40125F, 0.0762F),
                                                                       new Vector3(0F, 359.9599F, 0F),
                                                                       new Vector3(1F, 1F, 1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower",
                                                                       "Chest",
                                                                       new Vector3(-0.23393F, 0.27665F, 0.07866F),
                                                                       new Vector3(339.4101F, 283.0849F, 334.3318F),
                                                                       new Vector3(0.29081F, 0.29081F, 0.29081F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Thorns, "DisplayRazorwireLeft",
                                                                "UpperArmL",
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TitanGoldDuringTP, "DisplayGoldHeart",
                                                                       "UpperArmL",
                                                                       new Vector3(-0.03646F, 0.22899F, -0.13723F),
                                                                       new Vector3(0.41243F, 193.9074F, 89.14413F),
                                                                       new Vector3(0.20999F, 0.20999F, 0.20999F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                                               "Chest",
                                                new Vector3(0.00096F, 0.29871F, 0.38082F),
                                                new Vector3(358.7835F, 348.6807F, 2.7268F),
                                                new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                                new Vector3(0.08436F, 0.28255F, 0.34988F),
                                                new Vector3(355.8568F, 31.64544F, 357.1176F),
                                                new Vector3(1.67906F, 2.3004F, 1.91776F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                                new Vector3(0.14556F, 0.28629F, 0.31617F),
                                                new Vector3(349.3203F, 27.71486F, 7.13324F),
                                                new Vector3(1.48967F, 1.51727F, 1.51727F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                                new Vector3(-0.08285F, 0.28376F, 0.35114F),
                                                new Vector3(354.2122F, 340.7911F, 0.3483F),
                                                new Vector3(1.88356F, 2.14586F, 2.1551F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                                new Vector3(-0.14622F, 0.28251F, 0.31784F),
                                                new Vector3(355.3388F, 329.3073F, 353.9711F),
                                                new Vector3(1.50954F, 1.67906F, 1.67906F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TreasureCache, "DisplayKey",
                                                                       "ThighR",
                                                                       new Vector3(0.04425F, 0.26278F, -0.12569F),
                                                                       new Vector3(287.302F, 178.3501F, 301.3927F),
                                                                       new Vector3(0.97312F, 0.97312F, 0.97312F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.TreasureCacheVoid, "DisplayKeyVoid",
                                                                       "ThighR",
                                                                       new Vector3(0.04425F, 0.26278F, -0.12569F),
                                                                       new Vector3(287.302F, 178.3501F, 301.3927F),
                                                                       new Vector3(0.97312F, 0.97312F, 0.97312F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "UpperArmL",
                                               new Vector3(-0.00443F, 0.08458F, -0.12574F),
                                               new Vector3(328.7329F, 134.3506F, 231.5772F),
                                               new Vector3(0.93891F, 0.93891F, 0.93891F)),
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "UpperArmR",
                                               new Vector3(0.00299F, 0.07534F, -0.14124F),
                                               new Vector3(61.60355F, 340.5154F, 185.3896F),
                                               new Vector3(0.93891F, 0.93891F, 0.93891F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WarCryOnMultiKill, "DisplayPauldron",
                                                                       "UpperArmL",
                                                                       new Vector3(0.07232F, 0.04651F, 0.02796F),
                                                                       new Vector3(75.22794F, 86.33639F, 353.4002F),
                                                                       new Vector3(0.78022F, 0.78022F, 0.78022F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WardOnLevel, "DisplayWarbanner",
                                                                       "Chest",
                                                                       new Vector3(-0.01386F, 0.29193F, -0.16033F),
                                                                       new Vector3(270F, 270F, 0F),
                                                                       new Vector3(0.3955F, 0.3955F, 0.3955F)));
            #endregion items

            #region quips
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG",
                                                                       "Chest",
                                                                       new Vector3(0.1534F, 0.25475F, -0.16729F),
                                                                       new Vector3(345.7108F, 3.08143F, 339.1621F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Blackhole, "DisplayGravCube",
                                                                       "Root",
                                                                       new Vector3(-0.66354F, 1.82863F, -0.72814F),
                                                                       new Vector3(358.3824F, 269.3275F, 292.5764F),
                                                                       new Vector3(0.97127F, 0.97127F, 0.97127F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack",
                                                                       "Chest",
                                                                       new Vector3(-0.18473F, -0.11703F, -0.117F),
                                                                       new Vector3(359.319F, 222.2601F, 342.1406F),
                                                                       new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack",
                                                                       "Chest",
                                                                       new Vector3(0.21745F, 0.26488F, -0.06576F),
                                                                       new Vector3(48.81585F, 170.4146F, 19.82022F),
                                                                       new Vector3(0.41591F, 0.41591F, 0.41591F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion",
                                                                       "Chest",
                                                                       new Vector3(-0.21288F, -0.09195F, -0.09708F),
                                                                       new Vector3(345.6282F, 5.15145F, 313.8587F),
                                                                       new Vector3(0.04021F, 0.04021F, 0.04021F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy",
                                                                       "Chest",
                                                                       new Vector3(-0.19337F, -0.06776F, -0.11429F),
                                                                       new Vector3(344.8184F, 48.49264F, 345.2058F),
                                                                       new Vector3(0.43494F, 0.43494F, 0.43494F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant",
                                                                       "Head",
                                                                       new Vector3(0F, 0.16189F, 0.32838F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.2817F, 0.24288F, 0.21792F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile",
                                                                       "Chest",
                                                                       new Vector3(-0.23125F, 0.01432F, -0.19918F),
                                                                       new Vector3(3.34161F, 210.3051F, 0.3832F),
                                                                       new Vector3(0.07455F, 0.07455F, 0.07455F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio",
                                                                       "Chest",
                                                                       new Vector3(-0.21277F, 0.10379F, -0.20679F),
                                                                       new Vector3(15.50678F, 219.127F, 5.22551F),
                                                                       new Vector3(0.44868F, 0.44868F, 0.44868F)));
            //E for Affix
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(0.10817F, 0.14348F, 0.09467F),
                                               new Vector3(73.18252F, 13.13047F, 351.5704F),
                                               new Vector3(0.10653F, 0.10653F, 0.10653F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(-0.09398F, 0.14109F, 0.07918F),
                                               new Vector3(76.07266F, 302.3215F, 337.4471F),
                                               new Vector3(-0.10653F, 0.10653F, 0.10653F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixHaunted, "DisplayEliteStealthCrown",
                                                                       "Head",
                                                                       new Vector3(-0.00036F, 0.29331F, 0.00002F),
                                                                       new Vector3(283.6363F, 161.8954F, 202.6884F),
                                                                       new Vector3(0.05349F, 0.04822F, 0.04822F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixWhite, "DisplayEliteIceCrown",
                                                                       "Head",
                                                                       new Vector3(-0.00681F, 0.23285F, -0.01351F),
                                                                       new Vector3(274.0714F, 176.7575F, 186.9696F),
                                                                       new Vector3(0.03F, 0.03F, 0.03F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(-0.00001F, 0.19459F, 0.1399F),
                                               new Vector3(286.2097F, 0F, 0F),
                                               new Vector3(0.32674F, 0.33169F, 0.27995F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(0F, 0.23889F, 0.02665F),
                                               new Vector3(276.6768F, 180F, 180F),
                                               new Vector3(-0.23472F, 0.23828F, 0.18719F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixLunar, "DisplayEliteLunar,Eye",
                                                                       "HandR",
                                                                       new Vector3(0F, -0.00002F, 0.25223F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.34723F, 0.32214F, 0.34723F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixPoison, "DisplayEliteUrchinCrown",
                                                                       "Head",
                                                                       new Vector3(0F, 0.14782F, 0F),
                                                                       new Vector3(270F, 101.1706F, 0F),
                                                                       new Vector3(0.06F, 0.06F, 0.06F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg",
                                                                       "Chest",
                                                                       new Vector3(-0.21262F, 0.07809F, -0.17749F),
                                                                       new Vector3(273.1251F, 238.5773F, 227.3354F),
                                                                       new Vector3(0.27985F, 0.27985F, 0.27985F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit",
                                                                       "Chest",
                                                                       new Vector3(0.09287F, -0.17984F, -0.07355F),
                                                                       new Vector3(349.4059F, 153.6656F, 25.934F),
                                                                       new Vector3(0.30035F, 0.30035F, 0.3098F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure",
                                                                       "Chest",
                                                                       new Vector3(-0.21856F, 0.07924F, -0.1753F),
                                                                       new Vector3(288.4596F, 269.5617F, 178.5659F),
                                                                       new Vector3(0.50749F, 0.50749F, 0.50749F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase",
                                                                       "Chest",
                                                                       new Vector3(-0.2235F, 0.14735F, -0.21644F),
                                                                       new Vector3(329.6677F, 98.36839F, 322.3167F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat",
                                                                       "UpperArmL",
                                                                       new Vector3(0.23731F, -0.23173F, -0.10405F),
                                                                       new Vector3(326.8785F, 70.94123F, 220.1015F),
                                                                       new Vector3(0.11175F, 0.11175F, 0.11175F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings",
                                                                       "Chest",
                                                                       new Vector3(-0.00211F, 0.04164F, -0.25163F),
                                                                       new Vector3(353.4624F, 0.485F, 0F),
                                                                       new Vector3(0.21034F, 0.21034F, 0.21034F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
                                                                       "Chest",
                                                                       new Vector3(-0.21675F, 0.0426F, -0.26157F),
                                                                       new Vector3(344.2445F, 23.32607F, 284.143F),
                                                                       new Vector3(0.10813F, 0.10813F, 0.10813F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule("DisplayLightningArmRight",
                                               "UpperArmR",
                                               new Vector3(0, 0, 0),
                                               new Vector3(0, 0, 0),
                                               new Vector3(0.8752531f, 0.8752531f, 0.8752531f)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)//shoulder coil
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Meteor, "DisplayMeteor",
                                                                       "Root",
                                                                       new Vector3(-0.77876F, 1.59534F, -0.4727F),
                                                                       new Vector3(90F, 0F, 0F),
                                                                       new Vector3(0.76542F, 0.76645F, 0.76645F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray",
                                                                       "Chest",
                                                                       new Vector3(-0.00726F, 0.17612F, -0.29028F),
                                                                       new Vector3(10.45362F, 0F, 0F),
                                                                       new Vector3(0.26392F, 0.26392F, 0.26392F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler",
                                                                       "Chest",
                                                                       new Vector3(-0.22727F, 0.10963F, -0.2025F),
                                                                       new Vector3(71.62891F, 122.654F, 329.68F),
                                                                       new Vector3(0.05003F, 0.05003F, 0.05003F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Saw, "DisplaySawmerangFollower",
                                                                       "Root",
                                                                       new Vector3(-1.31868F, 1.08747F, 0.11552F),
                                                                       new Vector3(271.0232F, 185.0242F, 175.0119F),
                                                                       new Vector3(0.18144F, 0.18144F, 0.18144F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.11256F, 0.12822F, 0.01075F),
                                                                       new Vector3(0.47662F, 333.8952F, 74.53623F),
                                                                       new Vector3(0.16488F, 0.16488F, 0.16488F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry",
                                                                       "Chest",
                                                                       new Vector3(-0.21798F, 0.06038F, -0.16303F),
                                                                       new Vector3(10.76094F, 229.7132F, 12.76454F),
                                                                       new Vector3(0.05692F, 0.05692F, 0.05692F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic",
                                                                       "Chest",
                                                                       new Vector3(-0.21303F, 0.09523F, -0.15713F),
                                                                       new Vector3(0F, 191.2099F, 343.7879F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));

            #endregion quips

            #region dlc1
            try {
                Dlc1Dislpays(itemDisplayRules);
            } catch {
                Helpers.LogWarning("no sotv lol");
            }
            #endregion dlc1
            #region compat

            //try {
            //    if (Compat.TinkersSatchelInstalled) {
            //        SetTinkersSatchelDisplayRules(itemDisplayRules);
            //    }
            //}
            //catch (System.Exception e) {
            //    Helpers.LogWarning("error adding displays for Tinker's Satchel \n" + e);
            //}


            //try {
            //    if (Compat.AetheriumInstalled) {
            //        SetAetheriumDisplayRules(itemDisplayRules);
            //    }
            //}
            //catch (System.Exception e) {
            //    Helpers.LogWarning("error adding displays for Aetherium \n" + e);
            //}

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

        private static void Dlc1Dislpays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.PrimarySkillShuriken, "DisplayShuriken",
                                                                       "LowerArmR",
                                                                       new Vector3(-0.03456F, 0.46907F, 0.0085F),
                                                                       new Vector3(349.3939F, 285.0648F, 44.30583F),
                                                                       new Vector3(0.50943F, 0.50943F, 0.50943F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.OutOfCombatArmor, "DisplayOddlyShapedOpal",
                                                                       "Chest",
                                                                       new Vector3(-0.25193F, 0.07822F, 0.27566F),
                                                                       new Vector3(2.93651F, 320.2274F, 359.4729F),
                                                                       new Vector3(0.26172F, 0.26172F, 0.26396F)));
            //head
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.LunarSun,
                ItemDisplays.CreateDisplayRule("DisplaySunHead",
                                               "Head",
                                               new Vector3(-0.00924F, 0.12266F, -0.00555F),
                                               new Vector3(0F, 337.0038F, 0F),
                                               new Vector3(1.02498F, 0.91771F, 1.02498F)),
                ItemDisplays.CreateDisplayRule("DisplaySunHeadNeck",
                                               "Head",
                                               new Vector3(0.01731F, -0.00814F, -0.01618F),
                                               new Vector3(358.2472F, 289.4888F, 7.87784F),
                                               new Vector3(1.76587F, 0.94832F, -1.97986F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));
            //size
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MinorConstructOnKill, "DisplayDefenseNucleus",
                                                                       "Root",
                                                                       new Vector3(0.87222F, 1.62075F, -0.18692F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.42591F, 0.42591F, 0.42591F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HalfAttackSpeedHalfCooldowns, "DisplayLunarShoulderNature",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.03032F, -0.01502F, 0.15394F),
                                                                       new Vector3(4.0623F, 223.2026F, 248.9687F),
                                                                       new Vector3(1F, 1F, 0.71356F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HalfSpeedDoubleHealth, "DisplayLunarShoulderStone",
                                                                       "UpperArmL",
                                                                       new Vector3(0.05933F, 0.03356F, 0.01257F),
                                                                       new Vector3(353.7406F, 0.61763F, 203.1131F),
                                                                       new Vector3(0.70327F, 0.70327F, 0.75598F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.AttackSpeedAndMoveSpeed, "DisplayCoffee",
                                                                       "Pelvis",
                                                                       new Vector3(0.23219F, -0.09083F, 0F),
                                                                       new Vector3(0.17195F, 76.1399F, 179.9576F),
                                                                       new Vector3(0.22904F, 0.22904F, 0.22904F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.GoldOnHurt, "DisplayRollOfPennies",
                                                                       "CalfL",
                                                                       new Vector3(-0.07301F, 0.07015F, -0.05946F),
                                                                       new Vector3(349.0828F, 241.7495F, 260.9872F),
                                                                       new Vector3(0.70517F, 0.70517F, 0.70517F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.FragileDamageBonus, "DisplayDelicateWatch",
                                                                       "HandR",
                                                                       new Vector3(0.00003F, 0.07762F, -0.11352F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.61928F, 0.77737F, 0.63279F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ImmuneToDebuff, "DisplayRainCoatBelt",
                                                                       "ThighR",
                                                                       new Vector3(-0.01679F, 0.36864F, -0.00077F),
                                                                       new Vector3(1.99664F, 57.48078F, 182.9135F),
                                                                       new Vector3(0.6767F, 0.6767F, 0.6767F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RandomEquipmentTrigger, "DisplayBottledChaos",
                                                                       "Chest",
                                                                       new Vector3(-0.11559F, 0.05489F, -0.22715F),
                                                                       new Vector3(0F, 0F, 10.29679F),
                                                                       new Vector3(0.21247F, 0.21247F, 0.21247F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.StrengthenBurn, "DisplayGasTank",
                                                                       "ThighL",
                                                                       new Vector3(-0.08278F, 0.23788F, 0.093F),
                                                                       new Vector3(0F, 323.1183F, 141.3342F),
                                                                       new Vector3(0.1444F, 0.1444F, 0.1444F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.VoidMegaCrabItem, "DisplayMegaCrabItem",
                                                                       "Chest",
                                                                       new Vector3(-0.05803F, 0.01915F, 0.37486F),
                                                                       new Vector3(17.38989F, 271.4439F, 345.5916F),
                                                                       new Vector3(0.15797F, 0.15797F, 0.15797F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RegeneratingScrap, "DisplayRegeneratingScrap",
                                                                       "Chest",
                                                                       new Vector3(0.19607F, 0.25288F, -0.19083F),
                                                                       new Vector3(0F, 312.8683F, 0F),
                                                                       new Vector3(0.23956F, 0.23956F, 0.23956F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.PermanentDebuffOnHit, "DisplayScorpion",
                                                                       "Chest",
                                                                       new Vector3(0.00002F, 0.34411F, -0.28039F),
                                                                       new Vector3(69.77431F, 0F, 0F),
                                                                       new Vector3(0.6956F, 0.6956F, 0.6956F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritDamage, "DisplayLaserSight",
                                                                       "HandR",
                                                                       new Vector3(0.02581F, 0.06311F, -0.07445F),
                                                                       new Vector3(77.06108F, 90F, 0F),
                                                                       new Vector3(0.11604F, 0.11604F, 0.11604F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.FreeChest, "DisplayShippingRequestForm",
                                                                       "Chest",
                                                                       new Vector3(-0.13004F, -0.05604F, 0.30875F),
                                                                       new Vector3(81.86385F, 157.1635F, 180F),
                                                                       new Vector3(0.32389F, 0.32476F, 0.32389F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MoveSpeedOnKill, "DisplayGrappleHook",
                                                                       "HandL",
                                                                       new Vector3(-0.02752F, 0.10228F, -0.03501F),
                                                                       new Vector3(272.8237F, 180F, 180F),
                                                                       new Vector3(0.21889F, 0.21889F, 0.21889F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.RandomlyLunar, "DisplayDomino",
                                                                       "Root",
                                                                       new Vector3(-0.81447F, 1.5122F, -0.5534F),
                                                                       new Vector3(283.1516F, 0F, 0F),
                                                                       new Vector3(1.4558F, 1.4558F, 1.4558F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HealingPotion, "DisplayHealingPotion",
                                                                       "ThighR",
                                                                       new Vector3(0.11909F, 0.16047F, 0.05359F),
                                                                       new Vector3(346.0888F, 101.4104F, 236.5228F),
                                                                       new Vector3(0.05555F, 0.05555F, 0.05555F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MoreMissile, "DisplayICBM",
                                                                       "HandR",
                                                                       new Vector3(-0.00278F, 0.06494F, 0.06925F),
                                                                       new Vector3(89.08871F, 180F, 180F),
                                                                       new Vector3(0.12255F, 0.12255F, 0.12255F)));

            //quips
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunter,
                ItemDisplays.CreateDisplayRule("DisplayTricornGhost",
                                               "Head",
                                               new Vector3(-0.00001F, 0.25974F, -0.03203F),
                                               new Vector3(22.75723F, 0F, 0F),
                                               new Vector3(0.88509F, 0.88509F, 0.88509F)),
                ItemDisplays.CreateDisplayRule("DisplayBlunderbuss",
                                               "Chest",
                                               new Vector3(1.03663F, 0.08362F, -0.12434F),
                                               new Vector3(84.07404F, 180F, 180F),
                                               new Vector3(1F, 1F, 1F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.BossHunterConsumed, "DisplayTricornUsed",
                                                                       "Head",
                                                                       new Vector3(-0.00001F, 0.25974F, -0.03203F),
                                                                       new Vector3(22.75723F, 0F, 0F),
                                                                       new Vector3(0.88509F, 0.88509F, 0.88509F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.EliteVoidEquipment, "DisplayAffixVoid",
                                                                       "Head",
                                                                       new Vector3(0F, 0.11227F, 0.09264F),
                                                                       new Vector3(33.11541F, 0F, 0F),
                                                                       new Vector3(0.21654F, 0.21654F, 0.21654F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.GummyClone, "DisplayGummyClone",
                                                                       "Chest",
                                                                       new Vector3(-0.19897F, 0.18497F, -0.18384F),
                                                                       new Vector3(5.06793F, 52.68151F, 9.01866F),
                                                                       new Vector3(0.21042F, 0.21042F, 0.21042F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule("IrradiatingLaser", "DisplayIrradiatingLaser",
                                                                       "Chest",
                                                                       new Vector3(-0.27719F, 0.23919F, 0.02543F),
                                                                       new Vector3(352.0599F, 356.6538F, 35.57267F),
                                                                       new Vector3(0.1573F, 0.1573F, 0.1573F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.LunarPortalOnUse, "DisplayLunarPortalOnUse",
                                                                        "Chest",
                                                                       new Vector3(-0.94044F, 0.25836F, -0.24732F),
                                                                       new Vector3(1F, 1F, 1F),
                                                                       new Vector3(0.85193F, 0.85193F, 0.85193F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.Molotov, "DisplayMolotov",
                                                                       "Chest",
                                                                       new Vector3(-0.19325F, 0.04825F, -0.17724F),
                                                                       new Vector3(5.14973F, 2.45011F, 13.37755F),
                                                                       new Vector3(0.23692F, 0.23692F, 0.23692F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.MultiShopCard, "DisplayExecutiveCard",
                                                                       "Chest",
                                                                       new Vector3(-0.2229F, 0.12508F, -0.17122F),
                                                                       new Vector3(342.1718F, 88.47185F, 276.5112F),
                                                                       new Vector3(0.77511F, 0.79219F, 0.81654F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.VendingMachine, "DisplayVendingMachine",
                                                                       "Chest",
                                                                       new Vector3(-0.2022F, 0.15268F, -0.14575F),
                                                                       new Vector3(17.56048F, 229.9249F, 358.521F),
                                                                       new Vector3(0.17147F, 0.17147F, 0.17147F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Elites.Earth.eliteEquipmentDef, "DisplayEliteMendingAntlers",
                                                                       "Head",
                                                                       new Vector3(-0.00526F, 0.1283F, 0.01021F),
                                                                       new Vector3(347.7208F, 355.6827F, 359.5909F),
                                                                       new Vector3(0.82375F, 0.82375F, 0.82375F)));
        }

        #region tinker
        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        //private void SetTinkersSatchelDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.Moustache.instance.itemDef,
        //                                                               ThinkInvisible.TinkersSatchel.Moustache.instance.idrPrefab,
        //                                                               "Head",
        //                                                               new Vector3(-0.00753F, 0.07013F, 0.17869F),
        //                                                               new Vector3(33.49914F, 264.096F, 0F),
        //                                                               new Vector3(0.27773F, 0.19794F, 0.25268F)));
        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.itemDef,
        //                                                               ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.idrPrefab,
        //                                                               "Head",
        //                                                               new Vector3(-0.0066F, 0.0789F, 0.18609F),
        //                                                               new Vector3(351.4943F, 0F, 0F),
        //                                                               new Vector3(0.25964F, 0.18505F, 0.23623F)));
        //}
        #endregion tinker

        #region aeth
        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        //private void SetAetheriumDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.AccursedPotion.instance.ItemDef,
        //                                                               Aetherium.Items.AccursedPotion.ItemBodyModelPrefab,
        //                                                               "Pelvis",
        //                                                               new Vector3(-0.13276F, 0.01346F, -0.18569F),
        //                                                               new Vector3(7.2165F, 2.14944F, 187.7972F),
        //                                                               new Vector3(0.06197F, 0.06197F, 0.06197F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.AlienMagnet.instance.ItemDef,
        //                                                               Aetherium.Items.AlienMagnet.ItemBodyModelPrefab,
        //                                                               "Root",
        //                                                               new Vector3(-0.85034F, 1.83867F, -0.0771F),
        //                                                               new Vector3(351.4943F, 0F, 0F),
        //                                                               new Vector3(0.10931F, 0.10931F, 0.10931F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.BlasterSword.instance.ItemDef,
        //                                                               Aetherium.Items.BlasterSword.ItemBodyModelPrefab,
        //                                                               "HandR",
        //                                                               new Vector3(0F, 0.02108F, 0.26126F),
        //                                                               new Vector3(270F, 0F, 0F),
        //                                                               new Vector3(0.05965F, 0.05965F, 0.05965F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.BloodthirstyShield.instance.ItemDef,
        //                                                               Aetherium.Items.BloodthirstyShield.ItemBodyModelPrefab,
        //                                                               "LowerArmL",
        //                                                               new Vector3(0.16053F, 0.06418F, -0.01218F),
        //                                                               new Vector3(344.4676F, 90.9282F, 351.1721F),
        //                                                               new Vector3(0.18505F, 0.18505F, 0.18505F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.EngineersToolbelt.instance.ItemDef,
        //                                                               Aetherium.Items.EngineersToolbelt.ItemBodyModelPrefab,
        //                                                               "Pelvis",
        //                                                               new Vector3(-0.00021F, -0.06567F, -0.00099F),
        //                                                               new Vector3(6.90201F, 0F, 0F),
        //                                                               new Vector3(0.25964F, 0.2202F, 0.22626F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.FeatheredPlume.instance.ItemDef,
        //                                                               Aetherium.Items.FeatheredPlume.ItemBodyModelPrefab,
        //                                                               "Head",
        //                                                               new Vector3(-0.00001F, 0.30476F, -0.04557F),
        //                                                               new Vector3(0.81232F, 46.88409F, 35.18819F),
        //                                                               new Vector3(0.25964F, 0.18505F, 0.23623F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.Voidheart.instance.ItemDef,
        //                                                               Aetherium.Items.Voidheart.ItemBodyModelPrefab,
        //                                                               "Chest",
        //                                                               new Vector3(-0.09432F, 0.19857F, 0.33693F),
        //                                                               new Vector3(19.57261F, 324.6978F, 346.6553F),
        //                                                               new Vector3(0.09114F, 0.09114F, 0.09114F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.InspiringDrone.instance.ItemDef,
        //                                                               Aetherium.Items.InspiringDrone.ItemBodyModelPrefab,
        //                                                               "Root",
        //                                                               new Vector3(0.52308F, 2.07561F, 1.77891F),
        //                                                               new Vector3(8.3287F, 191.7939F, 1.73238F),
        //                                                               new Vector3(0.16571F, 0.16571F, 0.16571F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.NailBomb.instance.ItemDef,
        //                                                               Aetherium.Items.NailBomb.ItemBodyModelPrefab,
        //                                                               "Pelvis",
        //                                                               new Vector3(0.07335F, -0.05404F, 0.1652F),
        //                                                               new Vector3(76.28304F, 0F, 350.3321F),
        //                                                               new Vector3(0.09878F, 0.09878F, 0.09878F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.SharkTeeth.instance.ItemDef,
        //                                                               Aetherium.Items.SharkTeeth.ItemBodyModelPrefab,
        //                                                               "CalfR",
        //                                                               new Vector3(0.00621F, 0.17859F, 0.0036F),
        //                                                               new Vector3(291.9038F, 69.92281F, 115.0305F),
        //                                                               new Vector3(0.16309F, 0.16515F, 0.12384F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.ShieldingCore.instance.ItemDef,
        //                                                               Aetherium.Items.ShieldingCore.ItemBodyModelPrefab,
        //                                                               "Chest",
        //                                                               new Vector3(0.18271F, 0.25394F, -0.19998F),
        //                                                               new Vector3(44.5879F, 237.1052F, 60.74303F),
        //                                                               new Vector3(0.27672F, 0.27672F, 0.27672F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.UnstableDesign.instance.ItemDef,
        //                                                               Aetherium.Items.UnstableDesign.ItemBodyModelPrefab,
        //                                                               "Chest",
        //                                                               new Vector3(0.00771F, -0.03459F, -0.19881F),
        //                                                               new Vector3(359.6312F, 44.63949F, 0.45308F),
        //                                                               new Vector3(0.88639F, 0.88639F, 0.88639F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.WeightedAnklet.instance.ItemDef,
        //                                                               Aetherium.Items.WeightedAnklet.ItemBodyModelPrefab,
        //                                                               "CalfL",
        //                                                               new Vector3(0.0045F, 0.21226F, -0.00913F),
        //                                                               new Vector3(3.47227F, 0F, 7.27702F),
        //                                                               new Vector3(0.34528F, 0.26704F, 0.31415F)));

        //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(Aetherium.Items.WitchesRing.instance.ItemDef,
        //        ItemDisplays.CreateDisplayRule(Aetherium.Items.WitchesRing.ItemBodyModelPrefab,
        //                                       "UpperArmL",
        //                                                               new Vector3(0.06622F, 0.14004F, -0.01822F),
        //                                                               new Vector3(4.55092F, 359.2983F, 354.1099F),
        //                                                               new Vector3(0.236F, 0.2564F, 0.236F)),
        //        ItemDisplays.CreateDisplayRule(Aetherium.Items.WitchesRing.CircleBodyModelPrefab,
        //                                       "UpperArmL",
        //                                                               new Vector3(0.06689F, 0.14458F, -0.02185F),
        //                                                               new Vector3(81.32572F, 242.2187F, 242.4897F),
        //                                                               new Vector3(0.36182F, 0.36182F, 0.0209F))
        //        ));

        //    itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.ZenithAccelerator.instance.ItemDef,
        //                                                               Aetherium.Items.ZenithAccelerator.ItemBodyModelPrefab,
        //                                                               "Chest",
        //                                                               new Vector3(-0.00658F, 0.24893F, -0.34863F),
        //                                                               new Vector3(306.0561F, 0F, 0F),
        //                                                               new Vector3(0.1688F, 0.1688F, 0.1688F)));
        //}
        #endregion aeth

        #region scepter
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void FixScepterDisplayRule(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(AncientScepter.AncientScepterItem.instance.ItemDef,
                ItemDisplays.CreateDisplayRule(AncientScepter.AncientScepterItem.displayPrefab,
                    "Pelvis",
                    new Vector3(-0.11068F, -0.01037F, 0.14267F),
                    new Vector3(357.6273F, 97.34364F, 191.6927F),
                    new Vector3(0.21136F, 0.15064F, 0.1923F)),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("displaymastersword"),
                    "HandL",
                    new Vector3(-0.04495F, -0.01521F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.65423F, 1.63914F, 1.65423F))));
        }

        #endregion scepter
    }
}
