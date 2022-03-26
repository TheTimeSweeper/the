using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Characters {
    public class TeslaItemDisplays : ItemDisplaysBase {

        public override bool printUnused => false;

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
                                                                       new Vector3(0.0067F, 0.34007F, -0.09922F),
                                                                       new Vector3(85.61907F, 80.1209F, 118.01F),
                                                                       new Vector3(0.34677F, 0.32652F, 0.26189F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit, 
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                                               "Hammer",
                                               new Vector3(-0.00422F, 0.31156F, 0.00071F),
                                               new Vector3(270F, 97.21723F, 0F),
                                               new Vector3(0.14997F, 0.14997F, 0.14997F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftArm)));
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
                                               new Vector3(-0.00463F, -0.01497F, 0.12559F),
                                               new Vector3(270F, 185.3228F, 0F),
                                               new Vector3(0.07772F, 0.07121F, 0.07435F))
                ));

            //again, don't have to do this 
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
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
                                                                       new Vector3(-0.10572F, 0.06399F, -0.13554F),
                                                                       new Vector3(2.74418F, 317.9566F, 277.9646F),
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                                                                       "LowerArmL",
                                                                       new Vector3(-0.02832F, 0.20602F, -0.12107F),
                                                                       new Vector3(355.1272F, 79.21696F, 103.286F),
                                                                       new Vector3(0.19042F, 0.19042F, 0.18825F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                                                                       "Pelvis",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
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
                                               new Vector3(54.60126F, 295.0085F, 319.2301F),
                                               new Vector3(-0.04399F, 0.02643F, 0.02588F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                                                                       "Gauntlet",
                                                                       new Vector3(-0.12971F, 0.39333F, 0.05068F),
                                                                       new Vector3(285.9243F, 174.6441F, 314.6451F),
                                                                       new Vector3(0.06613F, 0.06613F, 0.06613F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",//don
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.08381F, 0.2699F, -0.013F),
                                                                       new Vector3(81.32565F, 242.2187F, 242.4897F),
                                                                       new Vector3(0.8466F, 0.8466F, 0.8466F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                                                                "Pelvis",
                                                                new Vector3(0.18761F, 0.03323F, 0.29922F),
                                                                new Vector3(25.02743F, 64.14127F, 78.41231F),
                                                                new Vector3(0.3F, 0.3F, 0.3F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                                                                "Head",
                                                                new Vector3(0.20784F, 0.6441F, -0.42684F),
                                                                new Vector3(18.96805F, 137.7607F, 184.7427F),
                                                                new Vector3(0.23622F, 0.23622F, 0.23622F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                                                                "Root",
                                                                new Vector3(1.5252F, 0.29608F, -0.90705F),
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(0.16F, 0.16F, 0.16f)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask",
                                                                "Head",
                                                                new Vector3(-0.09385F, 0.08206F, -0.00262F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(0.6F, 0.6F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown",
                                                                "Head",
                                                                new Vector3(-0.01105F, 0.16442F, 0F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(1F, 1F, 1F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown",
                                                                "Head",
                                                                new Vector3(0.00841F, 0.15581F, -0.01431F),
                                                                new Vector3(0F, 272.3771F, 0F),
                                                                new Vector3(0.215F, 0.25F, 0.25F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                                                                "Chest",
                                                                new Vector3(-0.27846F, 0.20935F, -0.07484F),
                                                                new Vector3(315.5308F, 156.2412F, 116.7797F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail",//don
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
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing",//don
                                                                       "ShoulderCoil",
                                                                       new Vector3(0.04069F, -0.04961F, -0.03573F),
                                                                       new Vector3(81.32563F, 242.2187F, 242.4897F),
                                                                       new Vector3(0.8466F, 0.8466F, 0.8466F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Icicle, "DisplayFrostRelic",
                                                                "Root",
                                                                new Vector3(1.03325F, 0.72699F, -1.50698F),
                                                                new Vector3(0F, 0F, 352.2637F),
                                                                new Vector3(2F, 2F, 2F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline",
                                                                "CalfL",
                                                                new Vector3(0.0827F, 0.20543F, 0.13498F),
                                                                new Vector3(84.03997F, 219.4316F, 123.9467F),
                                                                new Vector3(0.6F, 0.6F, 0.6F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(-0.04267F, 0.2029F, 0.07954F),
                                               new Vector3(0F, 336.5692F, 0F),
                                               new Vector3(0.3801F, 0.3801F, 0.3801F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(-0.02354F, 0.19663F, -0.09852F),
                                               new Vector3(356.4267F, 200.3571F, 357.6026F),
                                               new Vector3(0.3801F, 0.3801F, 0.3801F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(JunkContent.Items.Incubator, "DisplayAncestralIncubator",
                                                                "Chest",
                                                                new Vector3(0.0175F, 0.27074F, -0.2402F),
                                                                new Vector3(9.51898F, 20.83393F, 340.9285F),
                                                                new Vector3(0.05F, 0.05F, 0.05F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion",
                                                                "UpperArmR",
                                                                new Vector3(-0.00565F, 0.25692F, 0.09434F),
                                                                new Vector3(353.1847F, 167.6059F, 191.5657F),
                                                                new Vector3(0.22F, 0.22F, 0.22F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.JumpBoost, "DisplayWaxBird",
                                                                "Head",
                                                                new Vector3(0.08598F, -0.40624F, 0F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(1F, 1F, 1F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.KillEliteFrenzy, "DisplayBrainstalk",
                                                                "Head",
                                                                new Vector3(0.03902F, 0.09536F, -0.01466F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.3F, 0.42441F, 0.29789F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Knurl, "DisplayKnurl",
                                                                "ShoulderR",
                                                                new Vector3(-0.18628F, 0.07248F, 0.01291F),
                                                                new Vector3(14.34703F, 17.29507F, 9.36443F),
                                                                new Vector3(0.06807F, 0.06807F, 0.06807F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                                                                "Chest",
                                                                new Vector3(0.02338F, 0.45523F, 0.26226F),
                                                                new Vector3(353.3965F, 177.3169F, 359.8706F),
                                                                new Vector3(0.3917F, 0.3917F, 0.3917F)));
            //uh
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",
                                                                       "Gauntlet",
                                                                       new Vector3(0.13462F, 0.31661F, 0.04629F),
                                                                       new Vector3(353.3758F, 49.16793F, 165.8503F),
                                                                       new Vector3(1.05476F, 1.05476F, 1.05476F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                                                                "Chest",
                                                                new Vector3(0.44847F, 0.46014F, -0.17672F),
                                                                new Vector3(36.96157F, 337.9217F, 59.63353F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarPrimaryReplacement, "DisplayBirdEye",
                                                                "Head",
                                                                new Vector3(-0.17937F, 0.19138F, 0.00173F),
                                                                new Vector3(288.4116F, 90F, 180F),
                                                                new Vector3(0.23F, 0.23F, 0.23F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSecondaryReplacement, "DisplayBirdClaw",
                                                                "UpperArmR",
                                                                new Vector3(-0.09267F, 0.30336F, 0.07865F),
                                                                new Vector3(11.86698F, 23.21703F, 336.8174F),
                                                                new Vector3(0.73743F, 0.73743F, 0.73743F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSpecialReplacement, "DisplayBirdHeart",
                                                                "Root",
                                                                new Vector3(1.08295F, 0.10245F, 0.70849F),
                                                                new Vector3(0F, 356.794F, 0F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads",
                                                                "Chest",
                                                                new Vector3(-0.27122F, -0.01226F, 0.1432F),
                                                                new Vector3(356.3077F, 96.14975F, 300.8724F),
                                                                new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot",
                                                                "Head",
                                                                new Vector3(-0.06777F, -0.67912F, 0.05479F),
                                                                new Vector3(11.31161F, 131.1614F, 347.5229F),
                                                                new Vector3(1F, 1F, 1F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Medkit, "DisplayMedkit",
                                                                "Pelvis",
                                                                new Vector3(0.17854F, 0.1163F, 0.18107F),
                                                                new Vector3(66.94987F, 172.1296F, 280.228F),
                                                                new Vector3(0.6F, 0.6F, 0.6F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Missile, "DisplayMissileLauncher",
                                                                "Chest",
                                                                new Vector3(-0.03963F, 0.93601F, 0.30045F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(0.15F, 0.15F, 0.15F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.MonstersOnShrineUse, "DisplayMonstersOnShrineUse",
                                                                "CalfR",
                                                                new Vector3(0.13579F, 0.08768F, 0.03325F),
                                                                new Vector3(313.4106F, 1.43869F, 179.9522F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Mushroom, "DisplayMushroom",
                                                                "ShoulderL",
                                                                new Vector3(-0.04362F, 0.10807F, 0.12538F),
                                                                new Vector3(339.9099F, 271.5678F, 263.9858F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NearbyDamageBonus, "DisplayDiamond",
                                                                "Head",
                                                                new Vector3(0.18143F, 0.06812F, 0.2154F),
                                                                new Vector3(0F, 302.551F, 0F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnHeal,
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                               new Vector3(-0.09828F, 0.02123F, 0.09876F),
                                               new Vector3(0F, 258.8F, 0F),
                                               new Vector3(0.42003F, 0.42003F, 0.42003F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                               new Vector3(-0.09828F, 0.02123F, -0.09876F),
                                               new Vector3(0F, 282.2F, 0F),
                                               new Vector3(-0.42003F, 0.42003F, 0.42003F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.NovaOnLowHealth, "DisplayJellyGuts",
                                                                "Chest",
                                                                new Vector3(0.19091F, 0.56656F, 0.02692F),
                                                                new Vector3(47.12267F, 291.8234F, 174.7421F),
                                                                new Vector3(0.15203F, 0.15203F, 0.15203F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ParentEgg, "DisplayParentEgg",
                                                                "Head",
                                                                new Vector3(0.14496F, -1.03094F, -0.17467F),
                                                                new Vector3(346.8829F, 38.09669F, 355.9596F),
                                                                new Vector3(0.17754F, 0.22934F, 0.22934F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Pearl, "DisplayPearl",
                                                                "Head",
                                                                new Vector3(0F, 0.28757F, -0.0031F),
                                                                new Vector3(270.6174F, 180F, 180F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.PersonalShield, "DisplayShieldGenerator",
                                                                "Chest",
                                                                new Vector3(-0.21979F, 0.20802F, 0.19508F),
                                                                new Vector3(77.34012F, 284.6326F, 184.1077F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Phasing, "DisplayStealthkit",
                                                                "ThighL",
                                                                new Vector3(-0.01651F, 0.30469F, 0.09591F),
                                                                new Vector3(77.74274F, 288.25F, 110.3669F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Plant, "DisplayInterstellarDeskPlant",
                                                                "UpperArmL",
                                                                new Vector3(0.1111F, 0.23598F, 0.01194F),
                                                                new Vector3(347.855F, 83.84697F, 0F),
                                                                new Vector3(0.12989F, 0.12841F, 0.12841F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RandomDamageZone, "DisplayRandomDamageZone",
                                                                "Chest",
                                                                new Vector3(0.3403F, 0.21702F, 0.00818F),
                                                                new Vector3(352.5229F, 270.9943F, 0.51174F),
                                                                new Vector3(0.12349F, 0.16068F, 0.16068F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower",
                                                                "Chest",
                                                                new Vector3(-0.12734F, 0.37462F, -0.19835F),
                                                                new Vector3(356.7155F, 152.0292F, 314.6104F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));

            //SecondarySkillMagazine: see example above
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Seed, "DisplaySeed", //don
                                                                       "ShoulderL",
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
                                                                "Head",
                                                                new Vector3(-0.00001F, 0.41146F, 0F),
                                                                new Vector3(270F, 5F, 0F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                                               "Gauntlet",
                                               new Vector3(0.01605F, 0.19069F, 0.07331F),
                                               new Vector3(76.7001F, 0F, 0F),
                                               new Vector3(0.51375F, 0.46564F, 0.51375F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftLeg)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SiphonOnLowHealth, "DisplaySiphonOnLowHealth",
                                                                "Pelvis",
                                                                new Vector3(-0.2131F, 0.15274F, 0.2284F),
                                                                new Vector3(341.8052F, 309.3467F, 179.0716F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SlowOnHit, "DisplayBauble",
                                                                "Pelvis",
                                                                new Vector3(-0.39172F, 0.59777F, 0.30968F),
                                                                new Vector3(0F, 37.11069F, 165.892F),
                                                                new Vector3(0.43102F, 0.43102F, 0.43102F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintArmor, "DisplayBuckler",
                                                                "HandR",
                                                                new Vector3(0.02175F, 0.11961F, 0.01957F),
                                                                new Vector3(340.9992F, 352.6358F, 2.40951F),
                                                                new Vector3(0.24846F, 0.24846F, 0.26141F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintBonus, "DisplaySoda",
                                                                "Pelvis",
                                                                new Vector3(0.16407F, 0.08059F, -0.26964F),
                                                                new Vector3(284.3611F, 127.6331F, 323.4904F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintOutOfCombat, "DisplayWhip",
                                                                "Pelvis",
                                                                new Vector3(0F, 0.03903F, 0.41504F),
                                                                new Vector3(0F, 90F, 200F),
                                                                new Vector3(0.2175F, 0.2175F, 0.2175F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.SprintWisp, "DisplayBrokenMask",
                                                                "UpperArmR",
                                                                new Vector3(-0.02311F, 0.20304F, 0.09964F),
                                                                new Vector3(335.4409F, 2.64948F, 180F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Squid, "DisplaySquidTurret",
                                                                "ThighR",
                                                                new Vector3(-0.15008F, 0.21693F, -0.02095F),
                                                                new Vector3(6.47033F, 168.8895F, 289.4581F),
                                                                new Vector3(0.06125F, 0.06125F, 0.06125F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StickyBomb, "DisplayStickyBomb",
                                                                "Pelvis",
                                                                new Vector3(0.2025f, 0.202f, -0.208f),
                                                                new Vector3(345, 15, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade",
                                                                "Pelvis",
                                                                new Vector3(-0.05505F, 0.06835F, 0.41775F),
                                                                new Vector3(69.53522F, 188.4478F, 279.2932F),
                                                                new Vector3(0.7F, 0.7F, 0.7F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Syringe, "DisplaySyringeCluster",
                                                                "Chest",
                                                                new Vector3(0.14042F, 0.15594F, 0.27929F),
                                                                new Vector3(306.6051F, 62.59568F, 144.8416F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Talisman, "DisplayTalisman",
                                                                "Root",
                                                                new Vector3(0.95614F, -0.65443F, -0.36344F),
                                                                new Vector3(285.4026F, 243.6804F, 26.18949F),
                                                                new Vector3(1F, 1F, 1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower",
                                                                "Chest",
                                                                new Vector3(-0.21578F, 0.33212F, 0.01521F),
                                                                new Vector3(338.9304F, 272.9285F, 358.9245F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Thorns, "DisplayRazorwireLeft",
                                                                "UpperArmL",
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TitanGoldDuringTP, "DisplayGoldHeart",
                                                                "Chest",
                                                                new Vector3(-0.26652F, 0.29248F, -0.17898F),
                                                                new Vector3(318.8546F, 247.2778F, 52.25115F),
                                                                new Vector3(0.285F, 0.285F, 0.285F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
                ItemDisplays.CreateDisplayRule("DisplayToothNecklaceDecal",
                                               "Chest",
                                               new Vector3(-0.27247F, 0.71571F, 0F),
                                               new Vector3(306.6009F, 90.42825F, -0.00005F),
                                               new Vector3(0.66603F, 0.74592F, 0.99444F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                                               "Chest",
                                               new Vector3(-0.27216F, 0.23967F, -0.00159F),
                                               new Vector3(343.8618F, 272.5413F, 0F),
                                               new Vector3(3.12165F, 3.12165F, 3.12165F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                               new Vector3(-0.25596F, 0.27523F, 0.05371F),
                                               new Vector3(326.0107F, 249.5056F, 45.71984F),
                                               new Vector3(1.67906F, 2.3004F, 1.91776F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                               new Vector3(-0.22902F, 0.31349F, 0.09914F),
                                               new Vector3(330.2874F, 267.0437F, 43.70941F),
                                               new Vector3(1.48967F, 1.51727F, 1.51727F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                               new Vector3(-0.26062F, 0.27563F, -0.04991F),
                                               new Vector3(335.7749F, 271.632F, 320.4687F),
                                               new Vector3(1.67906F, 1.67906F, 1.67906F)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                               new Vector3(-0.23263F, 0.30768F, -0.09565F),
                                               new Vector3(330.3425F, 273.7813F, 316.531F),
                                               new Vector3(1.50954F, 1.67906F, 1.67906F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TreasureCache, "DisplayKey",
                                                                "Pelvis",
                                                                new Vector3(0.1747F, 0.1516F, -0.21049F),
                                                                new Vector3(0F, 25F, 270F),
                                                                new Vector3(0.23F, 0.23F, 0.23F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "ShoulderL",
                                               new Vector3(-0.07077F, 0.11496F, -0.08471F),
                                               new Vector3(85.10727F, 115.2514F, 217.2669F),
                                               new Vector3(1.15746F, 1.06979F, 1.15746F)),
                ItemDisplays.CreateDisplayRule("DisplayAfterburnerShoulderRing",
                                               "ShoulderR",
                                               new Vector3(0.07879F, 0.1459F, -0.07698F),
                                               new Vector3(87.39181F, 239.0657F, 322.6216F),
                                               new Vector3(1.15746F, -1.06979F, 1.15746F))
                ));
            //todo: replace Shoulder?
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WarCryOnMultiKill, "DisplayPauldron",
                                                                "ShoulderL",
                                                                new Vector3(-0.02566F, 0.26861F, 0.01654F),
                                                                new Vector3(1.32502F, 0.16224F, 358.6667F),
                                                                new Vector3(0.96987F, 0.96987F, 0.96987F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.WardOnLevel, "DisplayWarbanner",
                                                                "Head",
                                                                new Vector3(0.32394F, 0.06169F, 0.02166F),
                                                                new Vector3(288.6897F, 151.0441F, 245.5378F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            #endregion items

            #region quips
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(-0.16395F, 0.23242F, -0.00456F),
                                               new Vector3(282.7277F, 263.7881F, 4.58943F),
                                               new Vector3(0.32674F, 0.33169F, 0.27995F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(-0.04419F, 0.25413F, -0.00132F),
                                               new Vector3(271.5886F, 127.7425F, 140.5118F),
                                               new Vector3(-0.23472F, 0.23828F, 0.18719F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixHaunted, "DisplayEliteStealthCrown",
                                                                 "Head",
                                                                 new Vector3(-0.00036F, 0.29331F, 0.00002F),
                                                                 new Vector3(277.0447F, 92.71517F, 179.9999F),
                                                                 new Vector3(0.06F, 0.06F, 0.06F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixLunar, "DisplayEliteLunar,Eye",
                                                                "Head",
                                                                new Vector3(0.45354F, 0.41148F, -0.13191F),
                                                                new Vector3(12.59876F, 127.1027F, 82.99396F),
                                                                new Vector3(0.7389F, 0.7389F, 0.7389F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixPoison, "DisplayEliteUrchinCrown",
                                                                "Head",
                                                                new Vector3(0F, 0.14782F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.06F, 0.06F, 0.06F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(-0.08578F, 0.19256F, 0.0959F),
                                               new Vector3(80.22388F, 292.1413F, 13.54407F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(-0.07901F, 0.20542F, -0.1071F),
                                               new Vector3(77.77889F, 252.6081F, 10.54621F),
                                               new Vector3(-0.12953F, 0.12351F, 0.10424F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.AffixWhite, "DisplayEliteIceCrown",
                                                                "Head",
                                                                new Vector3(0F, 0.212F, 0F),
                                                                new Vector3(270F, 270F, 0F),
                                                                new Vector3(0.03F, 0.03F, 0.03F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings",
                                                                "Chest",
                                                                new Vector3(0.208f, 0.208f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            //todo: you know what to do
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat",
                                                                "ShoulderR",
                                                                new Vector3(0.09687F, 0.32573F, 0.22415F),
                                                                new Vector3(279.4109F, 187.7572F, 345.6136F),
                                                                new Vector3(0.11175F, 0.11175F, 0.11175F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG",
                                                                "Chest",
                                                                new Vector3(0.07101F, 0.41512F, -0.19564F),
                                                                new Vector3(348.1825F, 276.8288F, 25.02212F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray",
                                                                "Chest",
                                                                new Vector3(0.33257F, 0.3451F, -0.01117F),
                                                                new Vector3(315F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack",
                                                                "Chest",
                                                                new Vector3(0.26506F, 0.45562F, 0.00002F),
                                                                new Vector3(90F, 90F, 0F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit",
                                                                "Pelvis",
                                                                new Vector3(0.13026F, 0.28527F, -0.16847F),
                                                                new Vector3(356.3801F, 347.5225F, 216.8458F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant",
                                                                "Head",
                                                                new Vector3(-0.20606F, 0.06706F, -0.00143F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio",
                                                                "Chest",
                                                                new Vector3(-0.24166F, 0.34219F, -0.08679F),
                                                                new Vector3(348.3908F, 272.9134F, 58.67701F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule("DisplayLightningArmCustom",
                                               "LightningArm1",
                                               new Vector3(0, 0, 0),
                                               new Vector3(0, 0, 0),
                                               new Vector3(0.8752531f, 0.8752531f, 0.8752531f)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion",
                                                                "Pelvis",
                                                                new Vector3(0.07092F, 0.3068F, 0.50169F),
                                                                new Vector3(36.01561F, 13.14477F, 138.4107F),
                                                                new Vector3(0.03F, 0.03F, 0.03F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy",
                                                                "Head",
                                                                new Vector3(0.08686F, 0.30249F, -0.21051F),
                                                                new Vector3(10.46124F, 170.2762F, 1.58898F),
                                                                new Vector3(0.22F, 0.22F, 0.22F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure",
                                                                "CalfL",
                                                                new Vector3(-0.17336F, 0.08326F, -0.00223F),
                                                                new Vector3(90F, 268.5319F, 0F),
                                                                new Vector3(0.3F, 0.3F, 0.3F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler",
                                                                "Chest",
                                                                new Vector3(0.31706F, 0.37802F, 0.00421F),
                                                                new Vector3(0F, 0F, 348.9059F),
                                                                new Vector3(0.06F, 0.06F, 0.06F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg",
                                                                "Pelvis",
                                                                new Vector3(0.08967F, 0.06729F, 0.29295F),
                                                                new Vector3(90F, 0F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack",
                                                                "Chest",
                                                                new Vector3(0.32123F, 0.17569F, -0.01137F),
                                                                new Vector3(357.2928F, 90.24006F, 0.69147F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic",
                                                                "ThighR",
                                                                new Vector3(-0.00001F, 0.2792F, 0.18109F),
                                                                new Vector3(359.9935F, 359.8932F, 180.1613F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase",
                                                                "Chest",
                                                                new Vector3(0.11973F, 0.54832F, 0.26252F),
                                                                new Vector3(359.2803F, 349.704F, 3.12542F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner",
                                                                "Head",
                                                                new Vector3(0.37265F, 0.86982F, 0.02202F),
                                                                new Vector3(293.9302F, 144.0041F, 339.115F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile",
                                                                "Chest",
                                                                new Vector3(-0.23665F, 0.35165F, 0.12038F),
                                                                new Vector3(335.7568F, 279.3308F, 358.6632F),
                                                                new Vector3(0.04F, 0.04F, 0.04F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
                                                                "Chest",
                                                                new Vector3(0.49716F, -0.16366F, -0.1139F),
                                                                new Vector3(324.7334F, 298.6617F, 280.0614F),
                                                                new Vector3(0.1347F, 0.1347F, 0.1347F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry",
                                                                "Chest",
                                                                new Vector3(-0.24951F, -0.12858F, -0.00915F),
                                                                new Vector3(9.0684F, 272.2467F, 359.5266F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Saw, "DisplaySawmerangFollower",
                                                                "Root",
                                                                new Vector3(0.62211F, 0.43106F, 1.15734F),
                                                                new Vector3(358.3824F, 269.3275F, 292.5764F),
                                                                new Vector3(0.225F, 0.225F, 0.225F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Meteor, "DisplayMeteor",
                                                                "Root",
                                                                new Vector3(1.04502F, 0.51043F, 0.67377F),
                                                                new Vector3(90F, 0F, 0F),
                                                                new Vector3(1.2F, 1.2F, 1.2F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Blackhole, "DisplayGravCube",
                                                                "Root",
                                                                new Vector3(0.62211F, 0.43106F, 1.15734F),
                                                                new Vector3(358.3824F, 269.3275F, 292.5764F),
                                                                new Vector3(0.225F, 0.225F, 0.225F)));
            #endregion quips

        }
    }
}
