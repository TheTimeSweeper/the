using RoR2;
using System.Collections.Generic;
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
                                                                       "Base Pillar Items 1",
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
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(1.94468F, -0.13171F, 0.00434F),
                                                                       new Vector3(355.423F, 106.0462F, 279.2548F),
                                                                       new Vector3(0.78469F, 0.78469F, 0.78469F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.69307F, -1.18061F, 1.47428F),
                                                                       new Vector3(349.4186F, 347.1582F, 67.40533F),
                                                                       new Vector3(0.28372F, 0.28372F, 0.23368F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
                ItemDisplays.CreateDisplayRule("DisplayWarhammer",
                                               "Base Pillar Items 2",
                                               new Vector3(1.20629F, 0.5013F, 2.00427F),
                                               new Vector3(357.5856F, 356.2618F, 296.6083F),
                                               new Vector3(0.14328F, 0.16606F, 0.13271F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.11492F, 0.16431F, -0.0027F),
                                                                       new Vector3(351.9471F, 7.09396F, 74.07991F),
                                                                       new Vector3(0.35065F, 0.33159F, 0.34689F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil",
                                                                       "Center Cylinder Items",
                                                                            new Vector3(-0.25927F, 0.08903F, 0.06303F),
                                                                            new Vector3(38.63874F, 32.62672F, 5.83489F),
                                                                            new Vector3(0.538F, 0.538F, 0.538F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.02665F, -0.12398F, 0.07355F),
                                                                       new Vector3(84.96292F, 261.7131F, 266.1029F),
                                                                       new Vector3(-0.59767F, -0.79844F, -1.08218F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.15758F, 0.12047F, 0.32227F),
                                                                       new Vector3(81.57899F, 39.53221F, 69.93452F),
                                                                       new Vector3(0.51673F, 0.44291F, 0.44291F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.13373F, 0.06411F, 0.14015F),
                                                                       new Vector3(350.1368F, 34.37434F, 261.9982F),
                                                                       new Vector3(0.17978F, 0.17978F, 0.17978F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Bear, "DisplayBear",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.10369F, 0.32846f, -0.25795f),
                                                                       new Vector3(341.301F, 129.5673F, 350.8157F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.BearVoid, "DisplayBearVoid",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.10369F, 0.32846f, -0.25795f),
                                                                       new Vector3(341.301F, 129.5673F, 350.8157F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(0.12715F, 0.1495F, -0.10074F),
                                                                       new Vector3(322.6508F, 250.1892F, 155.5382F),
                                                                       new Vector3(0.08223F, 0.08223F, 0.08223F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule("DisplayBehemoth",
                                               "Base Pillar Items 1",
                                               new Vector3(-0.00328F, 0.06594F, -0.11691F),
                                               new Vector3(270.0626F, 179.5512F, 0F),
                                               new Vector3(0.0707F, 0.06064F, 0.0707F))));

            //again, don't have to do this 
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "Base Pillar Items 1",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule("DisplayTriTipVoid",
                                               "Base Pillar Items 1",
                                               new Vector3(0.13601F, 0.1954F, 0.11005F),
                                               new Vector3(280.9362F, 196.7177F, 215.8377F),
                                               new Vector3(0.46347F, 0.46347F, 0.54869F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.16195F, -0.04051F, -0.15709F),
                                                                       new Vector3(27.95107F, 14.43496F, 188.8343F),
                                                                       new Vector3(0.05F, 0.05F, 0.05F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome",
                                                                       "Base Pillar Items 2",
                                                                       new Vector3(-0.08873F, 0.26178F, -0.06341F),
                                                                       new Vector3(357.876F, 244.903F, 356.5704F),
                                                                       new Vector3(0.06581F, 0.06581F, 0.06581F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(-0.02864F, -0.10999F, -0.17898F),
                                                                       new Vector3(81.6009F, 66.40582F, 20.96977F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.00091F, 0.04853F, -0.31956F),
                                                                       new Vector3(332.7138F, 3.51125F, 359.9452F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(DLC1Content.Items.ChainLightningVoid, "DisplayUkuleleVoid",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(-0.09701F, 0.08195F, -0.29808F),
                                                                       new Vector3(30.95269F, 91.83294F, 82.90043F),
                                                                       new Vector3(0.6574F, 0.62401F, 0.63394F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Clover, "DisplayClover",
                                                                       "Center Cylinder Items",
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
                                                                       new Vector3(0.38906F, 0.38906F, 0.38906F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule("DisplaySkull",
                                               "Head",
                                               new Vector3(-0.00806F, 0.14284F, -0.00987F),
                                               new Vector3(290.1801F, 179.7206F, 183.4566F),
                                               new Vector3(0.34358F, 0.43777F, 0.31126F))));
            //CritGlasses: see example above
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar",
                                                                       "Head",
                                                                       new Vector3(-0.48973F, 0.06765F, 0.09769F),
                                                                       new Vector3(79.73706F, 132.4057F, 36.30824F),
                                                                       new Vector3(0.33687F, 0.33687F, 0.33687F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.01315F, 0.08225F, 0.00107F),
                                                                       new Vector3(0.94903F, 19.78056F, 178.113F),
                                                                       new Vector3(0.75F, 0.75F, 0.75F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark",
                                                                       "Base Pillar Items 4",
                                                                       new Vector3(0.05266F, 0.08043F, 0.006F),
                                                                       new Vector3(33.41785F, 88.12914F, 182.5425F),
                                                                       new Vector3(0.02281F, 0.02281F, 0.02281F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.27233F, -0.06941F, 0.16087F),
                                                                       new Vector3(16.42447F, 94.45877F, 169.1269F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.0533F, -0.14259F, 0.17139F),
                                                                       new Vector3(0F, 90F, 0F),
                                                                       new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine",
                                                                       "Base Pillar Items 1",
                                                           new Vector3(-0.03041F, 0.2141F, -0.11377F),
                                                           new Vector3(355.1272F, 79.21696F, 103.286F),
                                                           new Vector3(0.15428F, 0.15428F, 0.15252F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.22248F, -0.01608F, -0.10021F),
                                                                       new Vector3(0.76501F, 29.54675F, 174.773F),
                                                                       new Vector3(0.0642F, 0.0642F, 0.0642F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.07243F, 0.36143F, -0.2244F),
                                                                       new Vector3(320.8498F, 194.0099F, 9.15483F),
                                                                       new Vector3(0.2131F, 0.21936F, 0.21936F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "Tower Pole Items",
                                               new Vector3(-0.00251F, 0.37538F, -0.00142F),
                                               new Vector3(356.3479F, 168.6573F, 171.8978F),
                                               new Vector3(0.32954F, 0.32954F, 0.32954F)),
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "Tower Pole Items",
                                               new Vector3(0.00199F, 0.37549F, 0.01848F),
                                               new Vector3(6.25589F, 22.00069F, 174.1347F),
                                               new Vector3(0.32954F, 0.32954F, 0.32954F)
                )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                                               "Base Pillar Items 3",
                                               new Vector3(0.02972F, 0.22344F, 0.04502F),
                                               new Vector3(0.47761F, 233.9261F, 304.7706F),
                                               new Vector3(-0.04399F, 0.02643F, 0.02588F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(-0.12971F, 0.39333F, 0.05068F),
                                                                       new Vector3(285.9243F, 174.6441F, 314.6451F),
                                                                       new Vector3(0.06613F, 0.06613F, 0.06613F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing",
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.93601F, 0F),
                                                                       new Vector3(90F, 311.4591F, 0F),
                                                                       new Vector3(1.79638F, 1.79638F, 1.79638F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Firework, "DisplayFirework",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.00546F, 0.33392F, -0.28351F),
                                                                       new Vector3(277.4717F, 197.2654F, 126.2112F),
                                                                       new Vector3(0.24082F, 0.24082F, 0.24082F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.04689F, 0.01979F, 0.37003F),
                                                                       new Vector3(45.04784F, 40.97579F, 189.787F),
                                                                       new Vector3(0.12057F, 0.12057F, 0.12057F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.FocusConvergence, "DisplayFocusedConvergence",
                                                                "Root",
                                                                new Vector3(1.5252F, 0.29608F, -0.90705F),
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(0.16F, 0.16F, 0.16f)));

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
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.00093F, 0.01007F, -0.00924F),
                                                                       new Vector3(359.9472F, 173.7351F, 179.5188F),
                                                                       new Vector3(0.65271F, 0.21035F, 0.21426F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.17266F, 0.15629F, -0.2405F),
                                                                       new Vector3(287.4156F, 289.5891F, 46.36879F),
                                                                       new Vector3(0.13636F, 0.13636F, 0.13636F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail",
                                                                       "Head",
                                                                       new Vector3(-0.16092F, 0.0265F, -0.08058F),
                                                                       new Vector3(46.43885F, 226.517F, 348.8213F),
                                                                       new Vector3(0.06654F, 0.06654F, 0.06654F)));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
                ItemDisplays.CreateDisplayRule("DisplayHoof",
                                               "Tower Pole Items",
                                               new Vector3(-0.02149F, 0.35335F, -0.04254F),
                                               new Vector3(79.93871F, 359.8341F, 341.8235F),
                                               new Vector3(0.11376F, 0.10848F, 0.09155F))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing",
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 3.36372F, 0F),
                                                                       new Vector3(90F, 317.7292F, 0F),
                                                                       new Vector3(1.85154F, 1.82009F, 1.82009F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Icicle, "DisplayFrostRelic",
                                                                "Root",
                                                                new Vector3(1.03325F, 0.72699F, -1.50698F),
                                                                new Vector3(0F, 0F, 352.2637F),
                                                                new Vector3(2F, 2F, 2F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline",
                                                                       "Tower Pole Items",
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
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.19515F, -0.05889F, -0.1113F),
                                                                       new Vector3(9.51898F, 20.83393F, 340.9285F),
                                                                       new Vector3(0.02595F, 0.02595F, 0.02595F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion",
                                                                       "Center Cylinder Items",
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
                                                                       "Tower Pole Items",
                                                                       new Vector3(0F, 5.89393F, -0.0186F),
                                                                       new Vector3(271.1347F, 180F, 180F),
                                                                       new Vector3(0.1223F, 0.1223F, 0.1223F)));

            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.00637F, 0.47674F, -0.08688F),
                                                                       new Vector3(349.9394F, 249.6592F, 330.1028F),
                                                                       new Vector3(0.28718F, 0.28718F, 0.28718F)));
            //uh
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LightningStrikeOnHit, "DisplayChargedPerforator",
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0.13462F, 0.31661F, 0.04629F),
                                                                       new Vector3(353.3758F, 49.16793F, 165.8503F),
                                                                       new Vector3(1.05476F, 1.05476F, 1.05476F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.10422F, 0.0628F, -0.25083F),
                                                                       new Vector3(37.25228F, 260.2719F, 79.45548F),
                                                                       new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarPrimaryReplacement, "DisplayBirdEye",
                                                                       "Head",
                                                                       new Vector3(0.00176F, 0.17017F, 0.15579F),
                                                                       new Vector3(282.9004F, 180F, 180F),
                                                                       new Vector3(0.23053F, 0.23053F, 0.23053F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSecondaryReplacement, "DisplayBirdClaw",
                                                                       "Base Pillar Items 3",
                                                                       new Vector3(0.02726F, 0.36699F, -0.08066F),
                                                                       new Vector3(346.9768F, 234.68F, 334.6309F),
                                                                       new Vector3(0.56869F, 0.56869F, 0.56869F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarSpecialReplacement, "DisplayBirdHeart",
                                                                "Root",
                                                                new Vector3(1.08295F, 0.10245F, 0.70849F),
                                                                new Vector3(0F, 356.794F, 0F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.29107F, 0.15572F, 0.16556F),
                                                                       new Vector3(12.47539F, 158.354F, 310.8714F),
                                                                       new Vector3(0.8F, 0.8F, 0.8F)));
            //hello
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot",
                                                                       "Tower Pole Items",
                                                                       new Vector3(0.17329F, 0.10984F, -0.04414F),
                                                                       new Vector3(20.57682F, 195.7362F, 24.03913F),
                                                                       new Vector3(0.84595F, 0.84595F, 0.84595F)));

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
            //hello
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
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule("DisplayTeslaCoil",
                                               "Head",
                                               new Vector3(0.01605F, 0.19069F, 0.07331F),
                                               new Vector3(76.7001F, 0F, 0F),
                                               new Vector3(0.51375F, 0.46564F, 0.51375F))));
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
            
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.Talisman, "DisplayTalisman",
                                                                "Root",
                                                                new Vector3(0.95614F, -0.65443F, -0.36344F),
                                                                new Vector3(285.4026F, 243.6804F, 26.18949F),
                                                                new Vector3(1F, 1F, 1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.21578F, 0.33212F, 0.01521F),
                                                                new Vector3(338.9304F, 272.9285F, 358.9245F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
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
            //todo: replace Shoulder?
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
                                                                       "Base Pillar Items 1",
                                                                       new Vector3(0F, -0.00002F, 0.25223F),
                                                                       new Vector3(0F, 0F, 0F),
                                                                       new Vector3(0.34723F, 0.32214F, 0.34723F)));
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
                                                                "Center Cylinder Items",
                                                                new Vector3(0.208f, 0.208f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat",
                                                                "Base Pillar Items 3",
                                                                new Vector3(0.09687F, 0.32573F, 0.22415F),
                                                                new Vector3(279.4109F, 187.7572F, 345.6136F),
                                                                new Vector3(0.11175F, 0.11175F, 0.11175F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(0.07101F, 0.41512F, -0.19564F),
                                                                       new Vector3(348.6131F, 1.95604F, 339.4123F),
                                                                       new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion",
                                                                       "Center Cylinder Items",
                                                                       new Vector3(-0.21288F, -0.09195F, -0.09708F),
                                                                       new Vector3(345.6282F, 5.15145F, 313.8587F),
                                                                       new Vector3(0.04021F, 0.04021F, 0.04021F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack", //brokey
                                                                "Center Cylinder Items",
                                                                new Vector3(0.32123F, 0.17569F, -0.01137F),
                                                                new Vector3(357.2928F, 90.24006F, 0.69147F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.26506F, 0.45562F, 0.00002F),
                                                                new Vector3(90F, 90F, 0F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy",
                                                                       "Head",
                                                                       new Vector3(-0.00062F, 0.16551F, -0.13266F),
                                                                       new Vector3(34.73222F, 167.4949F, 348.1801F),
                                                                       new Vector3(0.354F, 0.354F, 0.354F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.33257F, 0.3451F, -0.01117F),
                                                                new Vector3(315F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.13026F, 0.28527F, -0.16847F),
                                                                new Vector3(356.3801F, 347.5225F, 216.8458F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant",
                                                                "Head",
                                                                new Vector3(-0.20606F, 0.06706F, -0.00143F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.24166F, 0.34219F, -0.08679F),
                                                                new Vector3(348.3908F, 272.9134F, 58.67701F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule("DisplayLightningArmCustom",
                                               "LightningArm1",
                                               new Vector3(0, 0, 0),
                                               new Vector3(0, 0, 0),
                                               new Vector3(0.8752531f, 0.8752531f, 0.8752531f))));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure",
                                                                "Tower Pole Items",
                                                                new Vector3(-0.17336F, 0.08326F, -0.00223F),
                                                                new Vector3(90F, 268.5319F, 0F),
                                                                new Vector3(0.3F, 0.3F, 0.3F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.31706F, 0.37802F, 0.00421F),
                                                                new Vector3(0F, 0F, 348.9059F),
                                                                new Vector3(0.06F, 0.06F, 0.06F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.08967F, 0.06729F, 0.29295F),
                                                                new Vector3(90F, 0F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic",
                                                                "Base Pillar Items 2",
                                                                new Vector3(-0.00001F, 0.2792F, 0.18109F),
                                                                new Vector3(359.9935F, 359.8932F, 180.1613F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.11973F, 0.54832F, 0.26252F),
                                                                new Vector3(359.2803F, 349.704F, 3.12542F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner",
                                                                "Head",
                                                                new Vector3(0.37265F, 0.86982F, 0.02202F),
                                                                new Vector3(293.9302F, 144.0041F, 339.115F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile",
                                                                "Center Cylinder Items",
                                                                new Vector3(-0.23665F, 0.35165F, 0.12038F),
                                                                new Vector3(335.7568F, 279.3308F, 358.6632F),
                                                                new Vector3(0.04F, 0.04F, 0.04F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit",
                                                                "Center Cylinder Items",
                                                                new Vector3(0.49716F, -0.16366F, -0.1139F),
                                                                new Vector3(324.7334F, 298.6617F, 280.0614F),
                                                                new Vector3(0.1347F, 0.1347F, 0.1347F)));
            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry",
                                                                "Center Cylinder Items",
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
