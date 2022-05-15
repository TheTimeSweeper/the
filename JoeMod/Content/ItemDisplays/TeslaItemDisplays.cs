using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Characters {
    public class TeslaItemDisplays : ItemDisplaysBase {

        public override bool printUnused => true;

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
            #region Examples
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.CritGlasses, "DisplayGlasses",
                                                                       "Head",
                                                                       new Vector3(0.00001F, 0.11364F, 0.15493F),
                                                                       new Vector3(351.4943F, 0F, 0F),
                                                                       new Vector3(0.25964F, 0.18505F, 0.23623F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CritGlassesVoid, "DisplayGlassesVoid",
                                                                       "Head",
                                                                       new Vector3(0.00001F, 0.11364F, 0.15493F),
                                                                       new Vector3(351.4943F, 0F, 0F),
                                                                       new Vector3(0.25964F, 0.18505F, 0.23623F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SecondarySkillMagazine, "DisplayDoubleMag",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.20847F, 0.32423F, 0.14056F),
                                                                       new Vector3(284.718F, 135.7026F, 146.5896F),
                                                                       new Vector3(0.09266F, 0.07507F, 0.09583F)));
            //press number keys to show/hide weapons (and their displays)
            //you don't have to do this. just do the default shotGauntlet I'll do the rest
            //if you do want to i'll kiss ya
            //just don't skimp out on putting things on weapons because doing them all would be too much
            #endregion

            #region items
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AlienHead, "DisplayAlienHead",
                                                                       "ThighL",
                                                                       new Vector3(-0.04745F, 0.03449F, -0.12276F),
                                                                       new Vector3(85.25261F, 326.2674F, 295.7773F),
                                                                       new Vector3(0.78469F, 0.78469F, 0.78469F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate",
                                                                       "ThighR",
                                                                       new Vector3(0.00112F, 0.41399F, -0.10019F),
                                                                       new Vector3(79.06744F, 78.83567F, 120.6044F),
                                                                       new Vector3(0.26921F, 0.25349F, 0.20332F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit, 
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                                               "Hammer",
                                               new Vector3(-0.00422F, 0.31156F, 0.00071F),
                                               new Vector3(270F, 97.21723F, 0F),
                                               new Vector3(0.14997F, 0.14997F, 0.14997F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftArm)));//hammer
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.11492F, 0.16431F, -0.0027F),
                                                                       new Vector3(351.9471F, 7.09396F, 74.07991F),
                                                                       new Vector3(0.35065F, 0.33159F, 0.34689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil",
                                                                       "Pelvis",
                                                                            new Vector3(-0.25927F, 0.08903F, 0.06303F),
                                                                            new Vector3(38.63874F, 32.62672F, 5.83489F),
                                                                            new Vector3(0.538F, 0.538F, 0.538F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier",
                                                                       "Chest",
                                                                       new Vector3(-0.02665F, -0.12398F, 0.07355F),
                                                                       new Vector3(84.96292F, 261.7131F, 266.1029F),
                                                                       new Vector3(-0.59767F, -0.79844F, -1.08218F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch",
                                                                       "Chest",
                                                                       new Vector3(-0.15758F, 0.12047F, 0.32227F),
                                                                       new Vector3(81.57899F, 39.53221F, 69.93452F),
                                                                       new Vector3(0.51673F, 0.44291F, 0.44291F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.13373F, 0.06411F, 0.14015F),
                                                                       new Vector3(350.1368F, 34.37434F, 261.9982F),
                                                                       new Vector3(0.17978F, 0.17978F, 0.17978F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bear, "DisplayBear",
                                                                       "Chest",
                                                                       new Vector3(0.10369F, 0.32846f, -0.25795f),
                                                                       new Vector3(341.301F, 129.5673F, 350.8157F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.BearVoid, "DisplayBearVoid",
                                                                       "Chest",
                                                                       new Vector3(0.10369F, 0.32846f, -0.25795f),
                                                                       new Vector3(341.301F, 129.5673F, 350.8157F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland",
                                                                       "ThighR",
                                                                       new Vector3(0.12715F, 0.1495F, -0.10074F),
                                                                       new Vector3(322.6508F, 250.1892F, 155.5382F),
                                                                       new Vector3(0.08223F, 0.08223F, 0.08223F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                                               "MuzzleGauntlet",
                                               new Vector3(-0.00328F, 0.06594F, -0.11691F),
                                               new Vector3(270.0626F, 179.5512F, 0F),
                                               new Vector3(0.0707F, 0.06064F, 0.0707F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftLeg)//gauntlet coil
                ));

            //again, don't have to do this 
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "Gauntlet",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                                               "Gauntlet",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode",
                                                                       "Pelvis",
                                                                       new Vector3(-0.16195F, -0.04051F, -0.15709F),
                                                                       new Vector3(27.95107F, 14.43496F, 188.8343F),
                                                                       new Vector3(0.05F, 0.05F, 0.05F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome",
                                                                       "ThighL",
                                                                       new Vector3(-0.08873F, 0.26178F, -0.06341F),
                                                                       new Vector3(357.876F, 244.903F, 356.5704F),
                                                                       new Vector3(0.06581F, 0.06581F, 0.06581F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.02864F, -0.10999F, -0.17898F),
                                                                       new Vector3(81.6009F, 66.40582F, 20.96977F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook",
                                                                       "Chest",
                                                                       new Vector3(-0.00091F, 0.04853F, -0.31956F),
                                                                       new Vector3(332.7138F, 3.51125F, 359.9452F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele",
                                                                       "HandL",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ChainLightningVoid, "DisplayUkuleleVoid",
                                                                       "HandL",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                                                                       "Chest",
                                                                       new Vector3(0.12598F, 0.48287F, 0.09201F),
                                                                       new Vector3(323.631F, 197.1626F, 30.83308F),
                                                                       new Vector3(0.38572F, 0.38572F, 0.38572F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.16892F, 0.01383F),
                                                                       new Vector3(319.4796F, 277.4703F, 335.4419F),
                                                                       new Vector3(0.38906F, 0.38906F, 0.38906F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.CloverVoid, "DisplayCloverVoid",
                                                                       "Head",
                                                                       new Vector3(0.13634F, 0.16892F, 0.01383F),
                                                                       new Vector3(319.4796F, 277.4703F, 335.4419F),
                                                                       new Vector3(0.4618838f, 0.4618838f, 0.4618838f)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                                               "Head",
                                               new Vector3(-0.00806F, 0.14284F, -0.00987F),
                                               new Vector3(290.1801F, 179.7206F, 183.4566F),
                                               new Vector3(0.34358F, 0.43777F, 0.31126F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)
                ));
            //CritGlasses: see example above
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar",
                                                                       "Head",
                                                                       new Vector3(-0.48973F, 0.06765F, 0.09769F),
                                                                       new Vector3(79.73706F, 132.4057F, 36.30824F),
                                                                       new Vector3(0.33687F, 0.33687F, 0.33687F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger",
                                                                       "ShoulderR",
                                                                       new Vector3(0.01315F, 0.08225F, 0.00107F),
                                                                       new Vector3(0.94903F, 19.78056F, 178.113F),
                                                                       new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark",
                                                                       "HandL",
                                                                       new Vector3(0.05266F, 0.08043F, 0.006F),
                                                                       new Vector3(33.41785F, 88.12914F, 182.5425F),
                                                                       new Vector3(0.02281F, 0.02281F, 0.02281F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn",
                                                                       "Pelvis",
                                                                       new Vector3(0.27233F, -0.06941F, 0.16087F),
                                                                       new Vector3(16.42447F, 94.45877F, 169.1269F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery",
                                                                       "Pelvis",
                                                                       new Vector3(-0.0533F, -0.14259F, 0.17139F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.EquipmentMagazineVoid, "DisplayFuelCellVoid",
                                                                       "Pelvis",
                                                                       new Vector3(-0.0533F, -0.14259F, 0.17139F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                                                                       "LowerArmL",
                                                           new Vector3(-0.03041F, 0.2141F, -0.11377F),
                                                           new Vector3(355.1272F, 79.21696F, 103.286F),
                                                           new Vector3(0.15428F, 0.15428F, 0.15252F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                                                                       "Pelvis",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExplodeOnDeathVoid, "DisplayWillowWispVoid",
                                                                       "Pelvis",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
                                                                       "Chest",
                                                                       new Vector3(-0.07243F, 0.36143F, -0.2244F),
                                                                       new Vector3(320.8498F, 194.0099F, 9.15483F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ExtraLifeVoid, "DisplayHippoVoid",
                                                                       "Chest",
                                                                       new Vector3(-0.07243F, 0.36143F, -0.2244F),
                                                                       new Vector3(320.8498F, 194.0099F, 9.15483F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
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
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                                               "ShoulderL",
                                               new Vector3(0.02972F, 0.22344F, 0.04502F),
                                               new Vector3(0.47761F, 233.9261F, 304.7706F),
                                               new Vector3(-0.04399F, 0.02643F, 0.02588F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.12971F, 0.39333F, 0.05068F),
                                                                       new Vector3(285.9243F, 174.6441F, 314.6451F),
                                                                       new Vector3(0.06613F, 0.06613F, 0.06613F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.08381F, 0.2699F, -0.013F),
                                                                       new Vector3(81.32565F, 242.2187F, 242.4897F),
                                                                       new Vector3(0.8466F, 0.8466F, 0.8466F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing",
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.04069F, -0.04961F, -0.03573F),
                                                                       new Vector3(81.32563F, 242.2187F, 242.4897F),
                                                                       new Vector3(0.8466F, 0.8466F, 0.8466F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ElementalRingVoid, "DisplayVoidRing",
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.06307F, 0.11617F, -0.02393F),
                                                                       new Vector3(81.32556F, 242.2189F, 147.0079F),
                                                                       new Vector3(0.8466F, 0.8466F, 0.8466F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                                                                       "CalfR",
                                                                       new Vector3(-0.15331F, 0.08333F, 0.00614F),
                                                                       new Vector3(82.45944F, 286.4524F, 127.7181F),
                                                                       new Vector3(0.30346F, 0.30346F, 0.30346F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                                                                       "Chest",
                                                                       new Vector3(0.04689F, 0.01979F, 0.37003F),
                                                                       new Vector3(45.04784F, 40.97579F, 189.787F),
                                                                       new Vector3(0.12057F, 0.12057F, 0.12057F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                                                                       "Root",
                                                                       new Vector3(-1.16428F, 2.27374F, -0.03402F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.11393F, 0.11393F, 0.11393F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask",
                                                                       "Head",
                                                                       new Vector3(0.00195F, 0.12905F, 0.14157F),
                                                                       new Vector3(347.9811F, 0.00722F, 0.51603F),
                                                                       new Vector3(0.57599F, 0.57599F, 0.44426F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown",
                                                                       "Head",
                                                                       new Vector3(-0.00059F, 0.14425F, -0.00858F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.83876F, 0.83197F, 0.71985F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown",
                                                                       "Pelvis",
                                                                       new Vector3(0.00093F, 0.01007F, -0.00924F),
                                                                       new Vector3(359.9472F, 173.7351F, 179.5188F),
                                                                       new Vector3(0.65271F, 0.21035F, 0.21426F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                                                                       "Chest",
                                                                       new Vector3(-0.17266F, 0.15629F, -0.2405F),
                                                                       new Vector3(287.4156F, 289.5892F, 358.1514F),
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
                                                                       "ShoulderL",
                                                                       new Vector3(0.04002F, 0.36383F, -0.0365F),
                                                                       new Vector3(333.1747F, 352.5555F, 18.78204F),
                                                                       new Vector3(0.06807F, 0.06807F, 0.06807F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                                                                       "UpperArmR",
                                                                       new Vector3(-0.13105F, -0.01649F, -0.09407F),
                                                                       new Vector3(357.5772F, 332.1058F, 93.51953F),
                                                                       new Vector3(0.28718F, 0.28718F, 0.28718F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",
                                                                       "Gauntlet",
                                                                       new Vector3(0.13462F, 0.31661F, 0.04629F),
                                                                       new Vector3(353.3758F, 49.16793F, 165.8503F),
                                                                       new Vector3(1.05476F, 1.05476F, 1.05476F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                                                                       "Chest",
                                                                       new Vector3(0.10422F, 0.0628F, -0.25083F),
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
                                                                       "Hammer",
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
                                                                       new Vector3(0.00987F, 0.21324F, -0.35959F),
                                                                       new Vector3(352.7343F, 356.8195F, 358.8024F),
                                                                       new Vector3(0.08041F, 0.10462F, 0.10462F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower",
                                                                       "Chest",
                                                                       new Vector3(-0.22553F, 0.32547F, -0.10069F),
                                                                       new Vector3(356.7155F, 152.0292F, 314.6104F),
                                                                       new Vector3(0.19141F, 0.19141F, 0.19141F)));

            //SecondarySkillMagazine: see example above
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Seed, "DisplaySeed", //don
                                                                       "ShoulderL",
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
                                               "Gauntlet",
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
                                                                       "Pelvis",
                                                                       new Vector3(0.07811F, 0.22854F, 0.06211F),
                                                                       new Vector3(340.5661F, 1.37354F, 26.74462F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade",
                                                                       "Pelvis",
                                                                       new Vector3(-0.14987F, -0.04276F, 0.19991F),
                                                                       new Vector3(69.53522F, 188.4478F, 279.2932F),
                                                                       new Vector3(0.7F, 0.7F, 0.7F)));
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
                                               "ShoulderL",
                                               new Vector3(-0.00443F, 0.08458F, -0.12574F),
                                               new Vector3(328.7329F, 134.3506F, 231.5772F),
                                               new Vector3(0.93891F, 0.93891F, 0.93891F)),
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "ShoulderR",
                                               new Vector3(0.00299F, 0.07534F, -0.14124F),
                                               new Vector3(61.60355F, 340.5154F, 185.3896F),
                                               new Vector3(0.93891F, 0.93891F, 0.93891F))
                ));
            //todo: replace Shoulder?
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
                                                                       "MuzzleGauntlet",
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
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.23731F, -0.23173F, -0.10405F),
                                                                       new Vector3(326.8785F, 70.94123F, 220.1015F),
                                                                       new Vector3(0.11175F, 0.11175F, 0.11175F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings",
                                                                       "Chest",
                                                                       new Vector3(-0.00164F, 0.048F, -0.19613F),
                                                                       new Vector3(334.8581F, 0.485F, 0F),
                                                                       new Vector3(0.21034F, 0.21034F, 0.21034F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
                                                                       "Chest",
                                                                       new Vector3(-0.21675F, 0.0426F, -0.26157F),
                                                                       new Vector3(344.2445F, 23.32607F, 284.143F),
                                                                       new Vector3(0.10813F, 0.10813F, 0.10813F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule("DisplayLightningArmCustom",
                                               "LightningArm1",
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
                                                                       "ShoulderR",
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
                                                                       new Vector3(0.9966F, 1.99435F, -0.48328F),
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
                                                                       "MuzzleGauntlet",
                                                                       new Vector3(0.00003F, 0.07762F, -0.11352F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.61928F, 0.63961F, 0.63279F)));

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
                                                                       "MuzzleGauntlet",
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
                                                                       new Vector3(-0.9928F, 1.51223F, -0.91639F),
                                                                       new Vector3(283.1516F, 0F, 0F),
                                                                       new Vector3(1F, 1F, 1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.HealingPotion, "DisplayHealingPotion",
                                                                       "ThighR",
                                                                       new Vector3(0.11909F, 0.16047F, 0.05359F),
                                                                       new Vector3(346.0888F, 101.4104F, 236.5228F),
                                                                       new Vector3(0.05555F, 0.05555F, 0.05555F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.MoreMissile, "DisplayICBM",
                                                                       "MuzzleGauntlet",
                                                                       new Vector3(-0.00278F, 0.06494F, 0.06925F),
                                                                       new Vector3(89.08871F, 180F, 180F),
                                                                       new Vector3(0.12255F, 0.12255F, 0.12255F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.EliteVoidEquipment, "DisplayAffixVoid",
                                                                       "Head",
                                                                       new Vector3(0F, 0.11227F, 0.09264F),
                                                                       new Vector3(33.11541F, 0F, 0F),
                                                                       new Vector3(0.21654F, 0.21654F, 0.21654F)));

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
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.Molotov, "DisplayMolotov",
            //                                                           "Chest",
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1)));
            //                                                           //wat
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule("AffixYellow", "DisplayEliteMendingAntlers",
            //                                                           "Chest",
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.VendingMachine, "DisplayVendingMachine",
                                                                       "Chest",
                                                                       new Vector3(1, 1, 1),
                                                                       new Vector3(1, 1, 1),
                                                                       new Vector3(1, 1, 1)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.LunarPortalOnUse, "DisplayLunarPortalOnUse",
            //                                                           "Chest",
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.GummyClone, "DisplayGummyClone",
                                                                       "Chest",
                                                                       new Vector3(-0.19473F, 0.18492F, -0.1854F),
                                                                       new Vector3(5.06793F, 52.68151F, 9.01866F),
                                                                       new Vector3(0.18973F, 0.18973F, 0.18973F)));

            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule("IrradiatingLaser", "DisplayIrradiatingLaser",
            //                                                           "Chest",
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1)));
            //itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Equipment.MultiShopCard, "DisplayExecutiveCard",
            //                                                           "Chest",
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1),
            //                                                           new Vector3(1, 1, 1)));

            #endregion
        }
    }
}
