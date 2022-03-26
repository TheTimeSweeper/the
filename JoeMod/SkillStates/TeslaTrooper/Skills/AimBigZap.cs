using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper
{
    public class AimBigZap : AimThrowableBase //AimMortar
    {
        public static string EnterSoundString = EntityStates.Treebot.Weapon.AimMortar.enterSoundString;
        public static string ExitSoundString = EntityStates.Treebot.Weapon.AimMortar.exitSoundString;
        public float skillsPlusMulti;

        private TeslaCoilControllerController coilController;
        private TeslaTrackerComponent tracker;

        private Sprite originalSprite;
        private bool showingEmpowered;

        public override void OnEnter() {
            coilController = GetComponent<TeslaCoilControllerController>();
            tracker = GetComponent<TeslaTrackerComponent>();

            EntityStates.Toolbot.AimStunDrone goodState = new EntityStates.Toolbot.AimStunDrone();
            maxDistance = 30;
            rayRadius = 1.6f;
            //this.arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            projectilePrefab = goodState.projectilePrefab;// EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab;
            endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = BigZap.BaseAttackRadius;
            setFuse = false;
            damageCoefficient = 0f;
            baseMinimumDuration = 0.2f;
            //this.projectileBaseSpeed = 80;

            base.OnEnter();
            Util.PlaySound(EnterSoundString, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            //scrapping this cooldown setup
            if (coilController && coilController.GetNearestTower()) {

                EnterEmpowered();
            } else {
                if (showingEmpowered)
                    ExitEmpowered();
            }

            if (coilController && coilController.coilReady && tracker?.GetTrackingTarget() != null) {

                endpointVisualizerRadiusScale = Tower.TowerBigZap.BaseAttackRadius;

                maxDistance = TeslaTrackerComponent.maxTrackingDistance;

            } else {

                maxDistance = 30;
                endpointVisualizerRadiusScale = BigZap.BaseAttackRadius;
            }
            endpointVisualizerRadiusScale *= skillsPlusMulti;

            //todo incombat
            //PlayAnimation("Gesture, Override", "HandOut");
        }

        // Token: 0x06003B19 RID: 15129 RVA: 0x0002B5A9 File Offset: 0x000297A9
        public override void OnExit()
        {
            base.OnExit();

            outer.SetNextState(new BigZap() { aimPoint = currentTrajectoryInfo.hitPoint });
            if (!outer.destroying)
            {
                Util.PlaySound(ExitSoundString, gameObject);
            }

            if (showingEmpowered)
                ExitEmpowered();
        }

        public override void FireProjectile() { }

        // Token: 0x06003B1A RID: 15130 RVA: 0x000150E1 File Offset: 0x000132E1
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void EnterEmpowered() {
            showingEmpowered = true;

            //scrapping this cooldown system. it was novel though
            //originalSpecial = skillLocator.special;
            //skillLocator.special = coilController.GetNearestTower().GetComponent<SkillLocator>().secondary;
            //if (originalSprite == null)
            //    originalSprite = skillLocator.secondary.skillDef.icon;
            //skillLocator.secondary.skillDef.icon = skillLocator.special.icon;

            tracker?.setIndicatorEmpowered(true);
        }

        private void ExitEmpowered() {
            showingEmpowered = false;

            //skillLocator.special = skillLocator.FindSkill("Special");
            //skillLocator.secondary.skillDef.icon = originalSprite;

            tracker?.setIndicatorEmpowered(false);
        }
    }
}