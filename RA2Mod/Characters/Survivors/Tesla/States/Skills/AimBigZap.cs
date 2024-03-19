using EntityStates;
using RA2Mod.Minions.TeslaTower.States;
using RoR2;
using System;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{
    public class AimBigZap : AimThrowableBase //AimMortar
    {
        public static string EnterSoundString = EntityStates.Treebot.Weapon.AimMortar.enterSoundString;
        public static string ExitSoundString = EntityStates.Treebot.Weapon.AimMortar.exitSoundString;
        public float skillsPlusMulti = 1;

        private TeslaTowerControllerController coilController;
        private TeslaTrackerComponentZap tracker;

        private bool showingEmpowered;
        protected bool castSuccessful;

        private float viewRadius;

        public override void OnEnter()
        {
            coilController = GetComponent<TeslaTowerControllerController>();
            tracker = GetComponent<TeslaTrackerComponentZap>();

            //excuse old enforcer code
            EntityStates.Toolbot.AimStunDrone goodState = new EntityStates.Toolbot.AimStunDrone();
            //arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            projectilePrefab = goodState.projectilePrefab;// EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab;
            endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = BigZap.BaseAttackRadius;
            maxDistance = 30;
            rayRadius = 1.6f;
            setFuse = false;
            damageCoefficient = 0f;
            baseMinimumDuration = 0.2f;
            //this.projectileBaseSpeed = 80;

            base.OnEnter();
            Util.PlaySound(EnterSoundString, gameObject);

            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.1f);
            GetModelAnimator().SetBool("isHandOut", true);
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            base.UpdateTrajectoryInfo(out dest);

            HurtBox target = tracker?.GetTrackingTarget();
            if (target != null && showingEmpowered)
            {
                dest.hitPoint = target.transform.position;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            //scrapping the cooldown setup
            if (coilController && coilController.GetNearestTower())
            {

                ShowEmpowered();
            }
            else
            {
                if (showingEmpowered)
                    RemoveEmpowered();
            }

            if (coilController && coilController.GetNearestTower() && tracker?.GetTrackingTarget() != null)
            {
                viewRadius = TowerBigZap.BaseAttackRadius;

                maxDistance = TeslaTrackerComponent.maxTrackingDistance;

            }
            else
            {

                viewRadius = BigZap.BaseAttackRadius;

                maxDistance = 30;
            }

            viewRadius *= skillsPlusMulti;

            endpointVisualizerRadiusScale = Mathf.Lerp(endpointVisualizerRadiusScale, viewRadius, 0.5f);
        }

        public override bool KeyIsDown()
        {
            return base.KeyIsDown() && !characterBody.isSprinting;
        }

        // Token: 0x06003B19 RID: 15129 RVA: 0x0002B5A9 File Offset: 0x000297A9
        public override void OnExit()
        {
            base.OnExit();

            if (!castSuccessful)
                RefundStock();

            if (showingEmpowered)
                RemoveEmpowered();
        }

        private void RefundStock()
        {
            activatorSkillSlot.AddOneStock();
        }

        //todo rework this to a simple projectile
        //instead of using a fake one in OnEnter and then not using it actually
        public override void FireProjectile() { }

        public override EntityState PickNextState()
        {
            castSuccessful = true;
            return new BigZap() { aimPoint = currentTrajectoryInfo.hitPoint };
        }

        // Token: 0x06003B1A RID: 15130 RVA: 0x000150E1 File Offset: 0x000132E1
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void ShowEmpowered()
        {
            showingEmpowered = true;

            //scrapping this cooldown system. it was novel though
            //originalSpecial = skillLocator.special;
            //skillLocator.special = coilController.GetNearestTower().GetComponent<SkillLocator>().secondary;
            //if (originalSprite == null)
            //    originalSprite = skillLocator.secondary.skillDef.icon;
            //skillLocator.secondary.skillDef.icon = skillLocator.special.icon;

            tracker?.SetIndicatorEmpowered(true);
        }

        private void RemoveEmpowered()
        {
            showingEmpowered = false;

            //skillLocator.special = skillLocator.FindSkill("Special");
            //skillLocator.secondary.skillDef.icon = originalSprite;

            tracker?.SetIndicatorEmpowered(false);
        }
    }
}