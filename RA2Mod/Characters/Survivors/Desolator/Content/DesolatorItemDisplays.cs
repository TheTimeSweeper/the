using RA2Mod.General;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorItemDisplays : ItemDisplaysBase {

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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AlienHead"], "DisplayAlienHead",
                                                                       "ThighL",
                                                                       new Vector3(-0.04745F, 0.03449F, -0.12276F),
                                                                       new Vector3(85.25261F, 326.2674F, 295.7773F),
                                                                       new Vector3(0.78469F, 0.78469F, 0.78469F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ArmorPlate"], "DisplayRepulsionArmorPlate",
                "ThighR",
                new Vector3(0.10735F, 0.3885F, -0.14583F),
                new Vector3(89.15119F, 83.65778F, 133.1551F),
                new Vector3(0.27476F, 0.20158F, 0.19593F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorReductionOnHit"], 
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                    "HandL",
                    new Vector3(0.00057F, 0.15232F, 0.3006F),
                    new Vector3(356.5829F, 9.22878F, 97.30781F),
                    new Vector3(0.21889F, 0.21889F, 0.21889F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AttackSpeedOnCrit"], "DisplayWolfPelt",
                "RadCannonItems",
                new Vector3(0.14103F, -0.5174F, -0.1563F),
                new Vector3(1.00267F, 251.9007F, 90.01363F),
                new Vector3(0.35065F, 0.33159F, 0.34689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AutoCastEquipment"], "DisplayFossil",
                                                                       "Pelvis",
                                                                            new Vector3(-0.25927F, 0.08903F, 0.06303F),
                                                                            new Vector3(38.63874F, 32.62672F, 5.83489F),
                                                                            new Vector3(0.538F, 0.538F, 0.538F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Bandolier"], "DisplayBandolier",
                                                                       "Chest",
                                                                       new Vector3(-0.02665F, -0.12398F, 0.07355F),
                                                                       new Vector3(84.96292F, 261.7131F, 266.1029F),
                                                                       new Vector3(-0.59767F, -0.79844F, -1.08218F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BarrierOnKill"], "DisplayBrooch",
                "Chest",
                new Vector3(-0.30562F, 0.24149F, 0.24346F),
                new Vector3(72.7022F, 356.2823F, 35.75062F),
                new Vector3(0.51673F, 0.44291F, 0.44291F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BarrierOnOverHeal"], "DisplayAegis",
                "LowerArmR",
                new Vector3(-0.1128F, 0.20398F, -0.01836F),
                new Vector3(347.7241F, 340.3412F, 268.7817F),
                new Vector3(0.17978F, 0.17978F, 0.17978F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Bear"], "DisplayBear",
                "Chest",
                new Vector3(-0.11495F, 0.32628F, -0.59116F),
                new Vector3(6.77534F, 162.5734F, 357.7492F),
                new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BearVoid"], "DisplayBearVoid",
                "Chest",
                new Vector3(-0.11495F, 0.32628F, -0.59116F),
                new Vector3(6.77534F, 162.5734F, 357.7492F),
                new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BeetleGland"], "DisplayBeetleGland",
                                                                       "ThighR",
                                                                       new Vector3(0.12715F, 0.1495F, -0.10074F),
                                                                       new Vector3(322.6508F, 250.1892F, 155.5382F),
                                                                       new Vector3(0.08223F, 0.08223F, 0.08223F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Behemoth"],
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                                               "MuzzleGauntlet",
                                               new Vector3(-0.00327F, 0.09583F, -0.11694F),
                                               new Vector3(270.0626F, 179.5512F, 0F),
                                               new Vector3(0.07343F, 0.06298F, 0.07343F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftLeg)//gauntlet coil
                ));

            //again, don't have to do this 
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHit"],
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "Gauntlet",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitVoid"],
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                                               "Gauntlet",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BleedOnHitAndExplode"], "DisplayBleedOnHitAndExplode",
                "Pelvis",
                new Vector3(-0.16815F, -0.03765F, -0.1891F),
                new Vector3(27.95107F, 14.43496F, 188.8343F),
                new Vector3(0.05F, 0.05F, 0.05F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BonusGoldPackOnKill"], "DisplayTome",
                "ThighL",
                new Vector3(-0.14834F, 0.26422F, -0.09133F),
                new Vector3(357.876F, 244.903F, 356.5704F),
                new Vector3(0.06581F, 0.06581F, 0.06581F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BossDamageBonus"], "DisplayAPRound",
                "UpperArmR",
                new Vector3(-0.12873F, -0.01893F, -0.0259F),
                new Vector3(81.60097F, 66.40557F, 333.9616F),
                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BounceNearby"], "DisplayHook",
                "Chest",
                new Vector3(-0.24089F, 0.36383F, -0.49469F),
                new Vector3(349.7779F, 3.14952F, 88.90458F),
                new Vector3(-0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ChainLightning"], "DisplayUkulele",
                                                                       "HandL",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ChainLightningVoid"], "DisplayUkuleleVoid",
                                                                       "HandL",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Clover"], "DisplayClover", 
                "Head",
                new Vector3(-0.13423F, 0.21851F, 0.00042F),
                new Vector3(36.86481F, 280.1299F, 357.5783F),
                new Vector3(0.38906F, 0.38906F, 0.38906F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CloverVoid"], "DisplayCloverVoid",
                "Head",
                new Vector3(-0.13423F, 0.21851F, 0.00042F),
                new Vector3(36.86481F, 280.1299F, 357.5783F),
                new Vector3(0.38906F, 0.38906F, 0.38906F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CooldownOnCrit"],
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                                               "Head",
                                                new Vector3(-0.00809F, 0.24008F, -0.03391F),
                                                new Vector3(290.1801F, 179.7206F, 183.4566F),
                                                new Vector3(0.34358F, 0.43777F, 0.31126F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritGlasses"], "DisplayGlasses",
                "Head",
                new Vector3(0.00508F, 0.22511F, 0.14625F),
                new Vector3(351.4943F, 0F, 0F),
                new Vector3(0.25964F, 0.18505F, 0.23623F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritGlassesVoid"], "DisplayGlassesVoid",
                "Head",
                new Vector3(0.00508F, 0.22511F, 0.14625F),
                new Vector3(351.4943F, 0F, 0F),
                new Vector3(0.25964F, 0.18505F, 0.23623F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Crowbar"], "DisplayCrowbar",
                "Chest",
                new Vector3(-0.36774F, 0.38355F, 0.40067F),
                new Vector3(69.89381F, 133.9579F, 4.16005F),
                new Vector3(0.33687F, 0.33687F, 0.33687F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Dagger"], "DisplayDagger",
                "ShoulderL",
                new Vector3(0.11559F, 0.25783F, -0.05096F),
                new Vector3(8.98564F, 311.4426F, 225.7203F),
                new Vector3(-0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["DeathMark"], "DisplayDeathMark",
                                                                       "HandL",
                                                                       new Vector3(0.05266F, 0.08043F, 0.006F),
                                                                       new Vector3(33.41785F, 88.12914F, 182.5425F),
                                                                       new Vector3(0.02281F, 0.02281F, 0.02281F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"], "DisplayWarHorn",
                                                                       "Pelvis",
                                                                       new Vector3(0.27233F, -0.06941F, 0.16087F),
                                                                       new Vector3(16.42447F, 94.45877F, 169.1269F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EquipmentMagazine"], "DisplayBattery",
                                                                       "Pelvis",
                                                                       new Vector3(-0.0533F, -0.14259F, 0.17139F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EquipmentMagazineVoid"], "DisplayFuelCellVoid",
                                                                       "Pelvis",
                                                                       new Vector3(-0.0533F, -0.14259F, 0.17139F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExecuteLowHealthElite"], "DisplayGuillotine",
                                                                       "LowerArmL",
                                                           new Vector3(-0.03041F, 0.2141F, -0.11377F),
                                                           new Vector3(355.1272F, 79.21696F, 103.286F),
                                                           new Vector3(0.15428F, 0.15428F, 0.15252F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExplodeOnDeath"], "DisplayWilloWisp",
                                                                       "Pelvis",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExplodeOnDeathVoid"], "DisplayWillowWispVoid",
                                                                       "Pelvis",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExtraLife"], "DisplayHippo",
                "Chest",
                new Vector3(-0.31555F, 0.3727F, -0.53233F),
                new Vector3(3.90636F, 225.3154F, 8.83919F),
                new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ExtraLifeVoid"], "DisplayHippoVoid",
                "Chest",
                new Vector3(-0.31555F, 0.3727F, -0.53233F),
                new Vector3(3.90636F, 225.3154F, 8.83919F),
                new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FallBoots"],
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "CalfL",
                                               new Vector3(-0.00251F, 0.37538F, -0.00142F),
                                               new Vector3(356.3479F, 168.6573F, 171.8978F),
                                               new Vector3(0.32954F, 0.32954F, 0.32954F)),
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "CalfR",
                                               new Vector3(0.00199F, 0.37549F, 0.01848F),
                                               new Vector3(6.25589F, 22.00069F, 174.1347F),
                                               new Vector3(0.32954F, 0.32954F, 0.32954F)
                )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Feather"],
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                                               "ShoulderL",
                                               new Vector3(0.02972F, 0.22344F, 0.04502F),
                                               new Vector3(0.47761F, 233.9261F, 304.7706F),
                                               new Vector3(-0.04399F, 0.02643F, 0.02588F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FireballsOnHit"], "DisplayFireballsOnHit",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.12971F, 0.39333F, 0.05068F),
                                                                       new Vector3(285.9243F, 174.6441F, 314.6451F),
                                                                       new Vector3(0.06613F, 0.06613F, 0.06613F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FireRing"], "DisplayFireRing",
                "ShoulderCoil",
                new Vector3(-0.00006F, 0.26409F, -0.00232F),
                new Vector3(270.5098F, 180F, 180F),
                new Vector3(0.88761F, 0.88761F, 0.88761F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["IceRing"], "DisplayIceRing",
                "ShoulderCoil",
                new Vector3(-0.00002F, 0.12534F, 0.00173F),
                new Vector3(270.7819F, 0F, 0F),
                new Vector3(0.88761F, 0.88761F, 0.88761F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ElementalRingVoid"], "DisplayVoidRing",
                "ShoulderCoil",
                new Vector3(-0.00002F, 0.12534F, 0.00173F),
                new Vector3(270.7819F, 0F, 0F),
                new Vector3(0.88761F, 0.88761F, 0.88761F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Firework"], "DisplayFirework",
                "CalfR",
                new Vector3(0.1465F, 0.10909F, -0.16216F),
                new Vector3(77.63947F, 102.6782F, 301.4056F),
                new Vector3(0.30346F, 0.30346F, 0.30346F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FlatHealth"], "DisplaySteakCurved",
                "Chest",
                new Vector3(0.04961F, 0.00136F, 0.43891F),
                new Vector3(29.55667F, 34.38698F, 280.2396F),
                new Vector3(0.09613F, 0.09613F, 0.09613F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FocusConvergence"], "DisplayFocusedConvergence",
                                                                       "Root",
                                                                       new Vector3(-1.16428F, 2.27374F, -0.03402F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.11393F, 0.11393F, 0.11393F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GhostOnKill"], "DisplayMask",
                "Head",
                new Vector3(0.00643F, 0.22249F, 0.11843F),
                new Vector3(347.9811F, 0.00722F, 0.51603F),
                new Vector3(0.57599F, 0.57599F, 0.44426F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GoldOnHit"], "DisplayBoneCrown",
                "Head",
                new Vector3(0.00389F, 0.23753F, -0.0317F),
                new Vector3(346.4499F, 0.63607F, 359.7216F),
                new Vector3(0.83876F, 0.83197F, 0.71985F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HeadHunter"], "DisplaySkullCrown",
                "Pelvis",
                new Vector3(0.00641F, 0.01023F, -0.01839F),
                new Vector3(359.9472F, 173.7351F, 179.5188F),
                new Vector3(0.72756F, 0.24537F, 0.26047F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealOnCrit"], "DisplayScythe",
                "Chest",
                new Vector3(-0.28682F, -0.03348F, -0.52752F),
                new Vector3(298.971F, 294.8149F, 101.4755F),
                new Vector3(0.13636F, 0.13636F, 0.13636F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealWhileSafe"], "DisplaySnail",
                "Chest",
                new Vector3(-0.24609F, 0.40578F, 0.06474F),
                new Vector3(5.87493F, 219.3425F, 316.443F),
                new Vector3(0.06654F, 0.06654F, 0.06654F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Hoof"],
                ItemDisplays.CreateDisplayRule("DisplayHoof",
                    "CalfR",
                    new Vector3(-0.01826F, 0.42641F, -0.0456F),
                    new Vector3(79.93874F, 359.8341F, 341.8235F),
                    new Vector3(0.11376F, 0.10848F, 0.10381F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Icicle"], "DisplayFrostRelic",
                                                                       "Root",
                                                                       new Vector3(1.05635F, 1.9722F, 0.42236F),
                                                                       new Vector3(90F, 0.000F, 0.0f),
                                                                       new Vector3(1.38875F, 1.38875F, 1.38875F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["IgniteOnKill"], "DisplayGasoline",
                "CalfL",
                new Vector3(-0.17199F, 0.15226F, 0.07802F),
                new Vector3(81.46445F, 242.3112F, 218.6413F),
                new Vector3(0.49393F, 0.49393F, 0.49393F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseHealing"],
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                "Head",
                new Vector3(0.10743F, 0.21824F, -0.00894F),
                new Vector3(353.9438F, 81.39291F, 0F),
                new Vector3(0.29875F, 0.29875F, 0.29875F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                "Head",
                new Vector3(-0.09241F, 0.23557F, -0.03701F),
                new Vector3(356.1463F, 266.133F, 355.7455F),
                new Vector3(0.3801F, 0.3801F, 0.3801F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Incubator"], "DisplayAncestralIncubator",
                                                                       "Chest",
                                                                       new Vector3(0.19515F, -0.05889F, -0.1113F),
                                                                       new Vector3(9.51898F, 20.83393F, 340.9285F),
                                                                       new Vector3(0.02595F, 0.02595F, 0.02595F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Infusion"], "DisplayInfusion",
                "Pelvis",
                new Vector3(0.20568F, -0.11764F, 0.14463F),
                new Vector3(10.36471F, 37.47284F, 182.8334F),
                new Vector3(0.38486F, 0.38486F, 0.3884F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["JumpBoost"], "DisplayWaxBird",
                "Head",
                new Vector3(0F, -0.18583F, -0.06927F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.78163F, 0.78163F, 0.78163F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["KillEliteFrenzy"], "DisplayBrainstalk",
                "Head",
                new Vector3(0.01376F, 0.17984F, -0.0195F),
                new Vector3(0F, 151.4707F, 0F),
                new Vector3(0.3F, 0.42441F, 0.29789F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Knurl"], "DisplayKnurl",
                "UpperArmL",
                new Vector3(0.07278F, 0.10867F, -0.10823F),
                new Vector3(59.65347F, 150.458F, 145.3407F),
                new Vector3(0.06807F, 0.06807F, 0.06807F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LaserTurbine"], "DisplayLaserTurbine",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.13105F, -0.01649F, -0.09407F),
                                                                       new Vector3(357.5772F, 332.1058F, 93.51953F),
                                                                       new Vector3(0.28718F, 0.28718F, 0.28718F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LightningStrikeOnHit"], "DisplayChargedPerforator",
                                                                       "Gauntlet",
                                                                       new Vector3(0.13462F, 0.31661F, 0.04629F),
                                                                       new Vector3(353.3758F, 49.16793F, 165.8503F),
                                                                       new Vector3(1.05476F, 1.05476F, 1.05476F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarDagger"], "DisplayLunarDagger",
                "Chest",
                new Vector3(0.03957F, 0.06033F, -0.28021F),
                new Vector3(40.80685F, 0.30102F, 86.32623F),
                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarPrimaryReplacement"], "DisplayBirdEye",
                "Head",
                new Vector3(0.00452F, 0.23203F, 0.15428F),
                new Vector3(282.9004F, 180F, 180F),
                new Vector3(0.23053F, 0.23053F, 0.23053F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarSecondaryReplacement"], "DisplayBirdClaw",
                                                                       "UpperArmL",
                                                                       new Vector3(0.02726F, 0.36699F, -0.08066F),
                                                                       new Vector3(346.9768F, 234.68F, 334.6309F),
                                                                       new Vector3(0.56869F, 0.56869F, 0.56869F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarSpecialReplacement"], "DisplayBirdHeart",
                                                                       "Root",
                                                                       new Vector3(-1.18817F, 1.85883F, -0.45721F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.33744F, 0.33744F, 0.33744F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarTrinket"], "DisplayBeads",
                "Chest",
                new Vector3(-0.35139F, 0.10088F, 0.19873F),
                new Vector3(12.47539F, 158.354F, 310.8714F),
                new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarUtilityReplacement"], "DisplayBirdFoot",
                                                                       "CalfR",
                                                                       new Vector3(0.17329F, 0.10984F, -0.04414F),
                                                                       new Vector3(20.57682F, 195.7362F, 24.03913F),
                                                                       new Vector3(0.84595F, 0.84595F, 0.84595F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Medkit"], "DisplayMedkit",
                                                                       "Pelvis",
                                                                       new Vector3(0.23334F, 0.11768F, 0.08936F),
                                                                       new Vector3(64.35487F, 280.9532F, 69.71149F),
                                                                       new Vector3(0.6F, 0.6F, 0.6F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Missile"], "DisplayMissileLauncher",
                "Chest",
                new Vector3(-0.33674F, 0.79419F, -0.36383F),
                new Vector3(345.3658F, 346.8422F, 17.42729F),
                new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MissileVoid"], "DisplayMissileLauncherVoid",
                "Chest",
                new Vector3(-0.33674F, 0.79419F, -0.36383F),
                new Vector3(345.3658F, 346.8422F, 17.42729F),
                new Vector3(0.10969F, 0.10969F, 0.10969F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MonstersOnShrineUse"], "DisplayMonstersOnShrineUse",
                "Chest",
                new Vector3(0.23066F, 0.28471F, 0.36134F),
                new Vector3(36.37239F, 319.6228F, 8.08606F),
                new Vector3(0.07148F, 0.07506F, 0.07148F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Mushroom"], "DisplayMushroom",
                                                                       "ThighL",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MushroomVoid"], "DisplayMushroomVoid",
                                                                       "ThighL",
                                                                       new Vector3(-0.03565F, 0.46698F, -0.08964F),
                                                                       new Vector3(354.2128F, 304.5275F, 81.31252F),
                                                                       new Vector3(0.08819F, 0.08819F, 0.08819F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["NearbyDamageBonus"], "DisplayDiamond",
                                                                       "LowerArmL",
                                                                       new Vector3(0.11959F, 0.21592F, -0.01724F),
                                                                       new Vector3(343.342F, 107.0507F, 356.529F),
                                                                       new Vector3(0.08798F, 0.08798F, 0.08798F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnHeal"],
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "Head",
                    new Vector3(0.09685F, 0.22836F, 0.03628F),
                    new Vector3(1.60112F, 352.6636F, 12.2446F),
                    new Vector3(0.42003F, 0.42003F, 0.42003F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                    "Head",
                    new Vector3(-0.08624F, 0.27474F, 0.0248F),
                    new Vector3(1.60112F, 352.6636F, 350.5993F),
                    new Vector3(-0.42003F, 0.42003F, 0.42003F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["NovaOnLowHealth"], "DisplayJellyGuts",
                "Head",
                new Vector3(0.05414F, 0.12002F, -0.16011F),
                new Vector3(345.7531F, 325.5393F, 340.7766F),
                new Vector3(0.10713F, 0.10713F, 0.10713F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ParentEgg"], "DisplayParentEgg",
                "Chest",
                new Vector3(0.0683F, -0.12335F, 0.44213F),
                new Vector3(24.21236F, 82.34745F, 7.68247F),
                new Vector3(0.09096F, 0.09096F, 0.09096F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Pearl"], "DisplayPearl",
                                                                       "LowerArmL",
                                                                       new Vector3(-0.00001F, 0.19616F, -0.02199F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["PersonalShield"], "DisplayShieldGenerator",
                "Chest",
                new Vector3(0.23047F, 0.13303F, 0.3389F),
                new Vector3(279.9448F, 70.539F, 320.1869F),
                new Vector3(0.1923F, 0.1923F, 0.1923F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Phasing"], "DisplayStealthkit",
                "CalfL",
                new Vector3(-0.0475F, 0.22072F, -0.14554F),
                new Vector3(7.66029F, 301.0364F, 271.7473F),
                new Vector3(0.32966F, 0.35099F, 0.35099F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Plant"], "DisplayInterstellarDeskPlant",
                                                                       "UpperArmL",
                                                                       new Vector3(0.1207F, 0.23304F, 0.02712F),
                                                                       new Vector3(358.9562F, 83.84697F, 0F),
                                                                       new Vector3(0.08447F, 0.08351F, 0.08351F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomDamageZone"], "DisplayRandomDamageZone",
                "Chest",
                new Vector3(0.03633F, 0.2296F, -0.22385F),
                new Vector3(352.6384F, 348.6428F, 359.8451F),
                new Vector3(0.08041F, 0.10462F, 0.10462F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RepeatHeal"], "DisplayCorpseFlower",
                "Chest",
                new Vector3(-0.03781F, 0.40963F, 0.37945F),
                new Vector3(341.261F, 253.3983F, 311.1857F),
                new Vector3(0.19141F, 0.19141F, 0.19141F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SecondarySkillMagazine"], "DisplayDoubleMag",
                "RadCannonItems",
                new Vector3(0.03344F, -0.17337F, 0.3568F),
                new Vector3(282.734F, 177.5681F, 183.5196F),
                new Vector3(0.09739F, 0.0789F, 0.10072F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Seed"], "DisplaySeed",
                "ShoulderL",
                new Vector3(-0.05625F, 0.25764F, 0.10641F),
                new Vector3(310.428F, 58.70779F, 96.00226F),
                new Vector3(0.0537F, 0.0537F, 0.0537F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShieldOnly"],
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ShinyPearl"], "DisplayShinyPearl",
                                                                       "LowerArmL",
                                                                       new Vector3(-0.03611F, 0.26797F, 0.01394F),
                                                                       new Vector3(278.2202F, 291.1136F, 78.58687F),
                                                                       new Vector3(0.10381F, 0.10381F, 0.10381F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockNearby"],
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                    "RadCannonItems",
                    new Vector3(-0.00688F, -0.29012F, -0.1949F),
                    new Vector3(271.7961F, 135.3845F, 226.0192F),
                    new Vector3(0.51375F, 0.46564F, 0.51375F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SiphonOnLowHealth"], "DisplaySiphonOnLowHealth",
                                                                       "ThighL",
                                                                       new Vector3(0.01442F, 0.13594F, 0.20742F),
                                                                       new Vector3(358.4349F, 359.0759F, 190.1793F),
                                                                       new Vector3(0.08824F, 0.08824F, 0.08844F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SlowOnHit"], "DisplayBauble",
                "ThighR",
                new Vector3(0.01611F, 0.77795F, 0.01265F),
                new Vector3(358.1689F, 29.80261F, 166.009F),
                new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SlowOnHitVoid"], "DisplayBaubleVoid",
                "ThighR",
                new Vector3(0.01611F, 0.77795F, 0.01265F),
                new Vector3(358.1689F, 29.80261F, 166.009F),
                new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintArmor"], "DisplayBuckler",
                                                                       "LowerArmL",
                                                                       new Vector3(0.08204F, 0.15889F, -0.02187F),
                                                                       new Vector3(339.7112F, 94.07677F, 338.7357F),
                                                                       new Vector3(0.16504F, 0.16504F, 0.17364F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintBonus"], "DisplaySoda",
                                                                       "Pelvis",
                                                                       new Vector3(-0.20959F, -0.08101F, 0.09991F),
                                                                       new Vector3(74.71255F, 119.2519F, 25.34358F),
                                                                       new Vector3(0.36722F, 0.36722F, 0.36722F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintOutOfCombat"], "DisplayWhip",
                                                                       "Pelvis",
                                                                       new Vector3(0.04666F, 0.05334F, 0.20305F),
                                                                       new Vector3(352.989F, 106.0337F, 188.9492F),
                                                                       new Vector3(0.32902F, 0.32902F, 0.32902F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["SprintWisp"], "DisplayBrokenMask",
                "UpperArmR",
                new Vector3(-0.09306F, 0.18375F, -0.15035F),
                new Vector3(349.2491F, 210.8764F, 254.2809F),
                new Vector3(0.1353F, 0.1353F, 0.1353F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Squid"], "DisplaySquidTurret",
                "ThighR",
                new Vector3(0.0112F, 0.15094F, -0.1663F),
                new Vector3(15.84112F, 56.06977F, 248.9113F),
                new Vector3(0.03817F, 0.03817F, 0.03817F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StickyBomb"], "DisplayStickyBomb",
                "Chest",
                new Vector3(0.32154F, -0.01396F, 0.18067F),
                new Vector3(9.82307F, 70.10319F, 341.8348F),
                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StunChanceOnHit"], "DisplayStunGrenade",
                                                                       "Pelvis",
                                                                       new Vector3(-0.1332F, -0.04592F, 0.16319F),
                                                                       new Vector3(69.5353F, 188.4479F, 216.9409F),
                                                                       new Vector3(0.82778F, 0.82778F, 0.82778F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Syringe"], "DisplaySyringeCluster",
                                                                       "Chest",
                                                                       new Vector3(0.25482F, 0.04429F, -0.05188F),
                                                                       new Vector3(325.976F, 201.993F, 122.6787F),
                                                                       new Vector3(0.15369F, 0.15369F, 0.15369F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TPHealingNova"], "DisplayGlowFlower",
                "Chest",
                new Vector3(-0.14935F, 0.40363F, 0.33175F),
                new Vector3(333.9761F, 318.3988F, 332.8572F),
                new Vector3(0.29081F, 0.29081F, 0.29081F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Talisman"], "DisplayTalisman",
                                                                       "Root",
                                                                       new Vector3(1.35249F, 1.40125F, 0.0762F),
                                                                       new Vector3(0F, 359.9599F, 0F),
                                                                       new Vector3(1F, 1F, 1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Thorns"], "DisplayRazorwireLeft",
                                                                "UpperArmL",
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TitanGoldDuringTP"], "DisplayGoldHeart",
                                                                       "UpperArmL",
                                                                       new Vector3(-0.03646F, 0.22899F, -0.13723F),
                                                                       new Vector3(0.41243F, 193.9074F, 89.14413F),
                                                                       new Vector3(0.20999F, 0.20999F, 0.20999F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tooth"],
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                    "Chest",
                    new Vector3(-0.00256F, 0.34779F, 0.44961F),
                    new Vector3(358.7835F, 348.6807F, 2.7268F),
                    new Vector3(2.59916F, 2.59916F, 2.59916F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "Chest",
                    new Vector3(0.06538F, 0.34287F, 0.43687F),
                    new Vector3(355.8568F, 31.64544F, 357.1176F),
                    new Vector3(1.67906F, 2.3004F, 1.91776F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Chest",
                    new Vector3(0.12409F, 0.34201F, 0.4141F),
                    new Vector3(349.3203F, 27.71486F, 7.13324F),
                    new Vector3(1.48967F, 1.51727F, 1.51727F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                    "Chest",
                    new Vector3(-0.06767F, 0.34851F, 0.4443F),
                    new Vector3(354.2122F, 340.7911F, 0.3483F),
                    new Vector3(1.88356F, 2.14586F, 2.1551F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                    "Chest",
                    new Vector3(-0.12534F, 0.34209F, 0.41505F),
                    new Vector3(355.3388F, 329.3073F, 353.9711F),
                    new Vector3(1.50954F, 1.67906F, 1.67906F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TreasureCache"], "DisplayKey",
                "ThighR",
                new Vector3(0.08121F, 0.24582F, -0.18579F),
                new Vector3(75.82722F, 246.9623F, 268.144F),
                new Vector3(0.97312F, 0.97312F, 0.97312F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TreasureCacheVoid"], "DisplayKeyVoid",
                "ThighR",
                new Vector3(0.08121F, 0.24582F, -0.18579F),
                new Vector3(75.82722F, 246.9623F, 268.144F),
                new Vector3(0.97312F, 0.97312F, 0.97312F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                    "ShoulderCoil",
                    new Vector3(-0.00016F, 0.18959F, 0.00011F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.77599F, 0.77599F, 0.77599F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["WarCryOnMultiKill"], "DisplayPauldron",
                                                                       "UpperArmL",
                                                                       new Vector3(0.07232F, 0.04651F, 0.02796F),
                                                                       new Vector3(75.22794F, 86.33639F, 353.4002F),
                                                                       new Vector3(0.78022F, 0.78022F, 0.78022F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["WardOnLevel"], "DisplayWarbanner",
                "Chest",
                new Vector3(-0.01386F, 0.39404F, -0.16034F),
                new Vector3(270F, 270F, 0F),
                new Vector3(0.3955F, 0.3955F, 0.3955F)));
            #endregion items

            #region quips
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BFG"], "DisplayBFG",
                                                                       "Chest",
                                                                       new Vector3(0.1534F, 0.25475F, -0.16729F),
                                                                       new Vector3(345.7108F, 3.08143F, 339.1621F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Blackhole"], "DisplayGravCube",
                                                                       "Root",
                                                                       new Vector3(-0.66354F, 1.82863F, -0.72814F),
                                                                       new Vector3(358.3824F, 269.3275F, 292.5764F),
                                                                       new Vector3(0.97127F, 0.97127F, 0.97127F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Cleanse"], "DisplayWaterPack",
                "Chest",
                new Vector3(-0.02774F, -0.1669F, -0.48165F),
                new Vector3(3.40681F, 158.5054F, 3.34285F),
                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CommandMissile"], "DisplayMissileRack",
                "Chest",
                new Vector3(-0.3838F, 0.31081F, -0.33751F),
                new Vector3(84.91959F, 278.8435F, 355.9485F),
                new Vector3(0.41591F, 0.41591F, 0.41591F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BurnNearby"], "DisplayPotion",
                "Chest",
                new Vector3(-0.07616F, -0.02671F, -0.53618F),
                new Vector3(345.6282F, 5.15145F, 313.8587F),
                new Vector3(0.04021F, 0.04021F, 0.04021F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CrippleWard"], "DisplayEffigy",
                "Chest",
                new Vector3(-0.09173F, -0.10187F, -0.52282F),
                new Vector3(344.0816F, 355.49F, 353.5142F),
                new Vector3(0.43494F, 0.43494F, 0.43494F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritOnUse"], "DisplayNeuralImplant",
                "Head",
                new Vector3(0.00238F, 0.21157F, 0.31608F),
                new Vector3(0F, 0F, 0F),
                new Vector3(0.2817F, 0.24288F, 0.21792F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["DeathProjectile"], "DisplayDeathProjectile",
                "Chest",
                new Vector3(-0.08221F, -0.03949F, -0.58687F),
                new Vector3(1.51913F, 157.7134F, 352.3248F),
                new Vector3(0.07455F, 0.07455F, 0.07455F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["DroneBackup"], "DisplayRadio",
                "Chest",
                new Vector3(-0.09843F, 0.05468F, -0.55405F),
                new Vector3(13.20926F, 148.944F, 354.7385F),
                new Vector3(0.44868F, 0.44868F, 0.44868F)));
            //E for Affix
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteFireEquipment"],
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                    "Head",
                    new Vector3(0.11248F, 0.23317F, 0.07247F),
                    new Vector3(73.18252F, 13.13047F, 351.5704F),
                    new Vector3(0.10653F, 0.10653F, 0.10653F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                    "Head",
                    new Vector3(-0.09046F, 0.21458F, 0.06099F),
                    new Vector3(76.07268F, 302.3215F, 337.4471F),
                    new Vector3(-0.10653F, 0.10653F, 0.10653F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteHauntedEquipment"], "DisplayEliteStealthCrown",
                "Head",
                new Vector3(0.003F, 0.36331F, -0.01731F),
                new Vector3(283.6363F, 161.8954F, 202.6884F),
                new Vector3(0.05349F, 0.04822F, 0.04822F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteIceEquipment"], "DisplayEliteIceCrown",
                "Head",
                new Vector3(0.00126F, 0.40108F, -0.05516F),
                new Vector3(274.0714F, 176.7575F, 186.9696F),
                new Vector3(0.03F, 0.03F, 0.03F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLightningEquipment"],
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                "Head",
                new Vector3(0.00569F, 0.31329F, 0.1105F),
                new Vector3(286.2097F, 0F, 0F),
                new Vector3(0.32674F, 0.33169F, 0.27995F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                "Head",
                new Vector3(0.00349F, 0.31152F, 0.00864F),
                new Vector3(276.6768F, 180F, 180F),
                new Vector3(-0.23472F, 0.23828F, 0.18719F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteLunarEquipment"], "DisplayEliteLunar,Eye",
                                                                       "MuzzleGauntlet",
                                                                       new Vector3(0F, -0.00002F, 0.25223F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.34723F, 0.32214F, 0.34723F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ElitePoisonEquipment"], "DisplayEliteUrchinCrown",
                "Head",
                new Vector3(0.0075F, 0.30405F, -0.03869F),
                new Vector3(270F, 101.1706F, 0F),
                new Vector3(0.05235F, 0.05235F, 0.05235F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FireBallDash"], "DisplayEgg",
                "Chest",
                new Vector3(-0.10461F, -0.06181F, -0.53043F),
                new Vector3(285.525F, 178.49F, 82.11471F),
                new Vector3(0.27985F, 0.27985F, 0.27985F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Fruit"], "DisplayFruit",
                "Chest",
                new Vector3(-0.41659F, 0.12255F, -0.01998F),
                new Vector3(20.29139F, 141.7408F, 51.23672F),
                new Vector3(0.30035F, 0.30035F, 0.3098F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GainArmor"], "DisplayElephantFigure",
                "Chest",
                new Vector3(-0.07707F, -0.08411F, -0.56364F),
                new Vector3(297.1219F, 187.9791F, 143.9449F),
                new Vector3(0.50749F, 0.50749F, 0.50749F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Gateway"], "DisplayVase",
                "Chest",
                new Vector3(-0.04578F, 0.0095F, -0.57164F),
                new Vector3(323.5828F, 342.6787F, 338.7314F),
                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GoldGat"], "DisplayGoldGat",
                "RadCannonItems",
                new Vector3(-0.0151F, -0.64559F, -0.36629F),
                new Vector3(1.70226F, 95.77624F, 269.2221F),
                new Vector3(0.11175F, 0.11175F, 0.11175F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Jetpack"], "DisplayBugWings",
                                                                       "Chest",
                                                                       new Vector3(-0.00211F, 0.04164F, -0.25163F),
                                                                       new Vector3(353.4624F, 0.485F, 0F),
                                                                       new Vector3(0.21034F, 0.21034F, 0.21034F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LifestealOnHit"], "DisplayLifestealOnHit",
                "Chest",
                new Vector3(0.03665F, 0.00586F, -0.57189F),
                new Vector3(350.0634F, 307.6892F, 293.9554F),
                new Vector3(0.10813F, 0.10813F, 0.10813F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Lightning"],
                ItemDisplays.CreateDisplayRule("DisplayLightningArmRight",
                                               "UpperArmR",
                                               new Vector3(0, 0, 0),
                                               new Vector3(0, 0, 0),
                                               new Vector3(1, 1, 1)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Meteor"], "DisplayMeteor",
                                                                       "Root",
                                                                       new Vector3(-0.77876F, 1.59534F, -0.4727F),
                                                                       new Vector3(90F, 0F, 0F),
                                                                       new Vector3(0.76542F, 0.76645F, 0.76645F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["QuestVolatileBattery"], "DisplayBatteryArray",
                                                                       "Chest",
                                                                       new Vector3(-0.00726F, 0.17612F, -0.29028F),
                                                                       new Vector3(10.45362F, 0F, 0F),
                                                                       new Vector3(0.26392F, 0.26392F, 0.26392F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Recycle"], "DisplayRecycler",
                "Chest",
                new Vector3(-0.05967F, -0.04341F, -0.54385F),
                new Vector3(60.3261F, 52.27971F, 339.3513F),
                new Vector3(0.05003F, 0.05003F, 0.05003F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Saw"], "DisplaySawmerangFollower",
                                                                       "Root",
                                                                       new Vector3(-1.31868F, 1.08747F, 0.11552F),
                                                                       new Vector3(271.0232F, 185.0242F, 175.0119F),
                                                                       new Vector3(0.18144F, 0.18144F, 0.18144F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Scanner"], "DisplayScanner",
                "Gauntlet",
                new Vector3(-0.0171F, -0.73381F, -0.18067F),
                new Vector3(19.94147F, 180.6102F, 182.7584F),
                new Vector3(0.16488F, 0.16488F, 0.16488F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["TeamWarCry"], "DisplayTeamWarCry",
                "Chest",
                new Vector3(-0.06307F, -0.05372F, -0.56312F),
                new Vector3(10.25613F, 158.3576F, 1.93827F),
                new Vector3(0.05692F, 0.05692F, 0.05692F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Tonic"], "DisplayTonic",
                "Chest",
                new Vector3(-0.06437F, -0.01045F, -0.51252F),
                new Vector3(6.00044F, 147.3401F, 324.9803F),
                new Vector3(0.21F, 0.21F, 0.21F)));

            #endregion quips

            #region dlc1

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["PrimarySkillShuriken"], "DisplayShuriken",
                "RadCannonRotation",
                new Vector3(0.00019F, 0.40126F, 0.00027F),
                new Vector3(90F, 45.90783F, 0F),
                new Vector3(1.10179F, 1.10179F, 1.10179F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["OutOfCombatArmor"], "DisplayOddlyShapedOpal",
                "Chest",
                new Vector3(-0.28973F, 0.03979F, 0.27493F),
                new Vector3(14.99223F, 320.1134F, 359.455F),
                new Vector3(0.26172F, 0.26172F, 0.26396F)));
            //head
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSun"],
                ItemDisplays.CreateDisplayRule("DisplaySunHead",
                    "Head",
                    new Vector3(-0.00924F, 0.14101F, -0.00556F),
                    new Vector3(343.5095F, 0.32651F, 344.8476F),
                    new Vector3(1.02498F, 1.02498F, 1.02498F)),
                ItemDisplays.CreateDisplayRule("DisplaySunHeadNeck",
                                               "Head",
                                               new Vector3(0.01731F, -0.00814F, -0.01618F),
                                               new Vector3(358.2472F, 289.4888F, 7.87784F),
                                               new Vector3(1.76587F, 0.94832F, -1.97986F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));
            //size
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MinorConstructOnKill"], "DisplayDefenseNucleus",
                                                                       "Root",
                                                                       new Vector3(0.87222F, 1.62075F, -0.18692F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.42591F, 0.42591F, 0.42591F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"], "DisplayLunarShoulderNature",
                "UpperArmR",
                new Vector3(-0.11625F, 0.2148F, 0.03754F),
                new Vector3(66.38692F, 273.7457F, 321.5652F),
                new Vector3(1F, 1F, 0.71356F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"], "DisplayLunarShoulderStone",
                "UpperArmL",
                new Vector3(0.0558F, 0.08469F, 0.11575F),
                new Vector3(14.1122F, 311.7242F, 199.4901F),
                new Vector3(0.70327F, 0.70327F, 0.75598F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"], "DisplayCoffee",
                "Pelvis",
                new Vector3(0.25532F, -0.09082F, 0.01386F),
                new Vector3(359.6145F, 167.6089F, 178.8729F),
                new Vector3(0.22904F, 0.22904F, 0.22904F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GoldOnHurt"], "DisplayRollOfPennies",
                "CalfL",
                new Vector3(-0.15018F, 0.08705F, -0.10092F),
                new Vector3(349.0828F, 241.7495F, 154.4265F),
                new Vector3(0.70517F, 0.70517F, 0.70517F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FragileDamageBonus"], "DisplayDelicateWatch",
                "MuzzleGauntlet",
                new Vector3(-0.10033F, 0.00133F, -0.24572F),
                new Vector3(359.1902F, 3.50813F, 86.42814F),
                new Vector3(0.52513F, 0.77737F, 0.63279F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["ImmuneToDebuff"], "DisplayRainCoatBelt",
                "Pelvis",
                new Vector3(0.00895F, -0.00357F, -0.04703F),
                new Vector3(3.51853F, 356.9552F, 179.6973F),
                new Vector3(1.21097F, 1.21097F, 1.21097F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomEquipmentTrigger"], "DisplayBottledChaos",
                "Chest",
                new Vector3(0.01483F, 0.08663F, -0.47726F),
                new Vector3(2.11297F, 300.013F, 355.1463F),
                new Vector3(0.21247F, 0.21247F, 0.21247F)));
            
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["StrengthenBurn"], "DisplayGasTank",
                "ThighL",
                new Vector3(-0.17203F, 0.28377F, 0.03584F),
                new Vector3(336.0911F, 232.8531F, 149.2666F),
                new Vector3(0.1444F, 0.1444F, 0.1444F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["VoidMegaCrabItem"], "DisplayMegaCrabItem",
                "Chest",
                new Vector3(-0.0341F, -0.08938F, 0.37019F),
                new Vector3(19.34091F, 343.9275F, 11.60458F),
                new Vector3(0.15797F, 0.15797F, 0.15797F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RegeneratingScrap"], "DisplayRegeneratingScrap",
                "Chest",
                new Vector3(-0.39116F, 0.41053F, -0.4183F),
                new Vector3(1.24481F, 88.81411F, 342.7563F),
                new Vector3(0.23956F, 0.23956F, 0.23956F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["PermanentDebuffOnHit"], "DisplayScorpion",
                                                                       "Chest",
                                                                       new Vector3(0.00002F, 0.34411F, -0.28039F),
                                                                       new Vector3(69.77431F, 0F, 0F),
                                                                       new Vector3(0.6956F, 0.6956F, 0.6956F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["CritDamage"], "DisplayLaserSight",
                                                                       "MuzzleGauntlet",
                                                                       new Vector3(0.02581F, 0.06311F, -0.07445F),
                                                                       new Vector3(77.06108F, 90F, 0F),
                                                                       new Vector3(0.11604F, 0.11604F, 0.11604F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["FreeChest"], "DisplayShippingRequestForm",
                "Chest",
                new Vector3(-0.21202F, 0.14076F, 0.34367F),
                new Vector3(87.29941F, 246.0616F, 287.2457F),
                new Vector3(0.32389F, 0.32476F, 0.32389F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MoveSpeedOnKill"], "DisplayGrappleHook",
                                                                       "HandL",
                                                                       new Vector3(-0.02752F, 0.10228F, -0.03501F),
                                                                       new Vector3(272.8237F, 180F, 180F),
                                                                       new Vector3(0.21889F, 0.21889F, 0.21889F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["RandomlyLunar"], "DisplayDomino",
                                                                       "Root",
                                                                       new Vector3(-0.81447F, 1.5122F, -0.5534F),
                                                                       new Vector3(283.1516F, 0F, 0F),
                                                                       new Vector3(1.4558F, 1.4558F, 1.4558F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["HealingPotion"], "DisplayHealingPotion",
                "ThighR",
                new Vector3(0.17872F, 0.17555F, 0.04157F),
                new Vector3(346.0888F, 101.4104F, 236.5228F),
                new Vector3(0.05555F, 0.05555F, 0.05555F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MoreMissile"], "DisplayICBM",
                "MuzzleGauntlet",
                new Vector3(0.00005F, -0.00195F, 0.11847F),
                new Vector3(89.08871F, 180F, 180F),
                new Vector3(0.12255F, 0.12255F, 0.12255F)));

            //quips
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunter"],
                ItemDisplays.CreateDisplayRule("DisplayTricornGhost",
                    "Head",
                    new Vector3(0.00418F, 0.34703F, -0.05367F),
                    new Vector3(19.33619F, 0.13045F, 359.9412F),
                    new Vector3(0.88509F, 0.88509F, 0.88509F)),
                ItemDisplays.CreateDisplayRule("DisplayBlunderbuss",
                                               "Chest",
                                               new Vector3(1.03663F, 0.08362F, -0.12434F),
                                               new Vector3(84.07404F, 180F, 180F),
                                               new Vector3(1F, 1F, 1F))));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["BossHunterConsumed"], "DisplayTricornUsed",
                "Head",
                new Vector3(0.00418F, 0.34703F, -0.05367F),
                new Vector3(19.33619F, 0.13045F, 359.9412F),
                new Vector3(0.88509F, 0.88509F, 0.88509F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteVoidEquipment"], "DisplayAffixVoid",
                "Head",
                new Vector3(0.00447F, 0.20537F, 0.06957F),
                new Vector3(33.11541F, 0F, 0F),
                new Vector3(0.21654F, 0.21654F, 0.21654F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["GummyClone"], "DisplayGummyClone",
                "Chest",
                new Vector3(-0.09979F, 0.02486F, -0.55092F),
                new Vector3(3.72412F, 314.6473F, 25.76961F),
                new Vector3(0.21042F, 0.21042F, 0.21042F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["IrradiatingLaser"], "DisplayIrradiatingLaser",
                                                                       "Chest",
                                                                       new Vector3(-0.27719F, 0.23919F, 0.02543F),
                                                                       new Vector3(352.0599F, 356.6538F, 35.57267F),
                                                                       new Vector3(0.1573F, 0.1573F, 0.1573F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["LunarPortalOnUse"], "DisplayLunarPortalOnUse",
                                                                        "Chest",
                                                                       new Vector3(-0.94044F, 0.25836F, -0.24732F),
                                                                       new Vector3(1F, 1F, 1F),
                                                                       new Vector3(0.85193F, 0.85193F, 0.85193F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["Molotov"], "DisplayMolotov",
                "Chest",
                new Vector3(-0.11779F, -0.10792F, -0.53645F),
                new Vector3(10.72197F, 156.8406F, 15.58382F),
                new Vector3(0.23692F, 0.23692F, 0.23692F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["MultiShopCard"], "DisplayExecutiveCard",
                "Chest",
                new Vector3(-0.23652F, 0.05291F, -0.6227F),
                new Vector3(5.60329F, 192.8223F, 90.94372F),
                new Vector3(0.80199F, 0.90211F, 0.84485F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["VendingMachine"], "DisplayVendingMachine",
                "Chest",
                new Vector3(-0.10834F, -0.02167F, -0.51368F),
                new Vector3(357.9618F, 120.804F, 342.1309F),
                new Vector3(0.17147F, 0.17147F, 0.17147F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ItemDisplays.KeyAssets["EliteEarthEquipment"], "DisplayEliteMendingAntlers",
                "Head",
                new Vector3(0.0006F, 0.25036F, -0.02003F),
                new Vector3(347.7208F, 355.6827F, 359.5909F),
                new Vector3(0.82375F, 0.82375F, 0.82375F)));

            #endregion

            #region compat

            try {
                if (GeneralCompat.TinkersSatchelInstalled) {
                    SetTinkersSatchelDisplayRules(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Log.Debug("error adding displays for Tinker's Satchel \n" + e);
            }


            try {
                if (GeneralCompat.AetheriumInstalled) {
                    SetAetheriumDisplayRules(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Log.Debug("error adding displays for Aetherium \n" + e);
            }

            try {
                if (GeneralCompat.ScepterInstalled) {
                    FixScepterDisplayRule(itemDisplayRules);
                }
            }
            catch (System.Exception e) {
                Log.Debug("error adding displays for Scepter \n" + e);
            }

            #endregion
        }

        #region tinker
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetTinkersSatchelDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.Moustache.instance.itemDef,
                                                                       ThinkInvisible.TinkersSatchel.Moustache.instance.idrPrefab,
                                                                       "Head",
                                                                       new Vector3(-0.00753F, 0.07013F, 0.17869F),
                                                                       new Vector3(33.49914F, 264.096F, 0F),
                                                                       new Vector3(0.27773F, 0.19794F, 0.25268F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.itemDef,
                                                                       ThinkInvisible.TinkersSatchel.EnterCombatDamage.instance.idrPrefab,
                                                                       "Head",
                                                                       new Vector3(-0.0066F, 0.0789F, 0.18609F),
                                                                       new Vector3(351.4943F, 0F, 0F),
                                                                       new Vector3(0.25964F, 0.18505F, 0.23623F)));
        }
        #endregion tinker

        #region aeth
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetAetheriumDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.AccursedPotion.instance.ItemDef,
                                                                       Aetherium.Items.AccursedPotion.ItemBodyModelPrefab,
                                                                       "Pelvis",
                                                                        new Vector3(-0.23086F, -0.06147F, -0.14462F),
                                                                        new Vector3(7.2165F, 2.14944F, 187.7972F),
                                                                        new Vector3(0.06197F, 0.06197F, 0.06197F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.AlienMagnet.instance.ItemDef,
                                                                       Aetherium.Items.AlienMagnet.ItemBodyModelPrefab,
                                                                       "Root",
                                                                       new Vector3(-0.85034F, 1.83867F, -0.0771F),
                                                                       new Vector3(351.4943F, 0F, 0F),
                                                                       new Vector3(0.10931F, 0.10931F, 0.10931F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.BlasterSword.instance.ItemDef,
                                                                       Aetherium.Items.BlasterSword.ItemBodyModelPrefab,
                                                                       "MuzzleGauntlet",
                                                                       new Vector3(0F, 0.02108F, 0.26126F),
                                                                       new Vector3(270F, 0F, 0F),
                                                                       new Vector3(0.05965F, 0.05965F, 0.05965F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.BloodthirstyShield.instance.ItemDef,
                                                                       Aetherium.Items.BloodthirstyShield.ItemBodyModelPrefab,
                                                                       "LowerArmL",
                                                                        new Vector3(0.13143F, 0.23771F, -0.03587F),
                                                                        new Vector3(350.8013F, 89.93404F, 351.3853F),
                                                                        new Vector3(0.18505F, 0.18505F, 0.18505F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.EngineersToolbelt.instance.ItemDef,
                                                                       Aetherium.Items.EngineersToolbelt.ItemBodyModelPrefab,
                                                                       "Pelvis",
                                                                       new Vector3(-0.00021F, -0.06567F, -0.00099F),
                                                                       new Vector3(6.90201F, 0F, 0F),
                                                                       new Vector3(0.25964F, 0.2202F, 0.22626F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.FeatheredPlume.instance.ItemDef,
                                                                       Aetherium.Items.FeatheredPlume.ItemBodyModelPrefab,
                                                                       "Head",
                                                                       new Vector3(-0.00001F, 0.30476F, -0.04557F),
                                                                       new Vector3(0.81232F, 46.88409F, 35.18819F),
                                                                       new Vector3(0.25964F, 0.18505F, 0.23623F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.Voidheart.instance.ItemDef,
                                                                       Aetherium.Items.Voidheart.ItemBodyModelPrefab,
                                                                       "Chest",
                                                                       new Vector3(-0.09509F, 0.21416F, 0.43664F),
                                                                       new Vector3(19.57261F, 324.6978F, 346.6553F),
                                                                       new Vector3(0.09114F, 0.09114F, 0.09114F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.InspiringDrone.instance.ItemDef,
                                                                       Aetherium.Items.InspiringDrone.ItemBodyModelPrefab,
                                                                       "Root",
                                                                       new Vector3(-1.2399F, 2.07553F, 1.77888F),
                                                                       new Vector3(8.32874F, 152.667F, 1.73238F),
                                                                       new Vector3(0.16571F, 0.16571F, 0.16571F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.NailBomb.instance.ItemDef,
                                                                       Aetherium.Items.NailBomb.ItemBodyModelPrefab,
                                                                       "Pelvis",
                                                                       new Vector3(0.07335F, -0.05404F, 0.1652F),
                                                                       new Vector3(76.28304F, 0F, 350.3321F),
                                                                       new Vector3(0.09878F, 0.09878F, 0.09878F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.SharkTeeth.instance.ItemDef,
                                                                       Aetherium.Items.SharkTeeth.ItemBodyModelPrefab,
                                                                       "CalfR",
                                                                       new Vector3(0.00621F, 0.17859F, 0.0036F),
                                                                       new Vector3(291.9038F, 69.92281F, 115.0305F),
                                                                       new Vector3(0.16309F, 0.16515F, 0.12384F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.ShieldingCore.instance.ItemDef,
                                                                       Aetherium.Items.ShieldingCore.ItemBodyModelPrefab,
                                                                       "Chest",
                                                                        new Vector3(0.16269F, 0.25508F, -0.13215F),
                                                                        new Vector3(45.64022F, 254.5071F, 91.32857F),
                                                                        new Vector3(0.32088F, 0.32088F, 0.32088F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.UnstableDesign.instance.ItemDef,
                                                                       Aetherium.Items.UnstableDesign.ItemBodyModelPrefab,
                                                                       "Chest",
                                                                       new Vector3(0.00771F, -0.03459F, -0.19881F),
                                                                       new Vector3(359.6312F, 44.63949F, 0.45308F),
                                                                       new Vector3(0.88639F, 0.88639F, 0.88639F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.WeightedAnklet.instance.ItemDef,
                                                                       Aetherium.Items.WeightedAnklet.ItemBodyModelPrefab,
                                                                       "CalfL",
                                                                       new Vector3(0.0045F, 0.21226F, -0.00913F),
                                                                       new Vector3(3.47227F, 0F, 7.27702F),
                                                                       new Vector3(0.34528F, 0.26704F, 0.31415F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(Aetherium.Items.WitchesRing.instance.ItemDef,
                ItemDisplays.CreateDisplayRule(Aetherium.Items.WitchesRing.ItemBodyModelPrefab,
                    "ShoulderCoil",
                    new Vector3(0.01633F, 0.14144F, 0.00517F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.27176F, 0.29525F, 0.27176F)),
                ItemDisplays.CreateDisplayRule(Aetherium.Items.WitchesRing.CircleBodyModelPrefab,
                    "ShoulderCoil",
                    new Vector3(0.0062F, 0.15695F, -0.00231F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.36182F, 0.36182F, 0.0209F))
                ));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(Aetherium.Items.ZenithAccelerator.instance.ItemDef,
                                                                       Aetherium.Items.ZenithAccelerator.ItemBodyModelPrefab,
                                                                       "Chest",
                                                                        new Vector3(-0.1696F, 0.50055F, -0.39726F),
                                                                        new Vector3(355.2099F, 0F, 0F),
                                                                        new Vector3(0.1688F, 0.1688F, 0.1688F)));
        }
        #endregion aeth

        #region scepter
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void FixScepterDisplayRule(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(AncientScepter.AncientScepterItem.instance.ItemDef,
                                                                       AncientScepter.AncientScepterItem.displayPrefab,
                                                                       "Hammer",
                                                                       new Vector3(-0.05514F, 0.3274F, 0.00725F),
                                                                       new Vector3(359.7871F, 184.096F, 357.1938F),
                                                                       new Vector3(0.32553F, 0.23201F, 0.29617F)));
        }

        #endregion scepter
    }
}
