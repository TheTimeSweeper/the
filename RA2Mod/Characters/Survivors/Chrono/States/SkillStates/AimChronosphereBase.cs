using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.States
{
    public abstract class AimChronosphereBase : AimThrowableBase
    {
        public static float BaseRadius => ChronoConfig.M3_Chronosphere_Radius.Value;

        public static string EnterSoundString = "Play_ChronosphereHumStart";
        //public static string LoopSoundString = "Play_ChronosphereHumLoop";
        public static string LoopStopSoundString = "Stop_ChronosphereHumLoop";
        public static string ExitSoundString = "Play_ChronosphereHumEnd";
        public float skillsPlusMulti = 1;

        protected bool castSuccessful;

        protected float viewRadius;

        private bool unpressed;
        private bool repressed;

        private bool validTarget;

        public override void OnEnter()
        {
            projectilePrefab = ChronoAssets.chronoBombProjectile;
            endpointVisualizerPrefab = ChronoAssets.endPointivsualizer;
            endpointVisualizerRadiusScale = BaseRadius;
            arcVisualizerPrefab = ChronoAssets.arcvisualizer;
            maxDistance = 120;
            rayRadius = 1f;
            setFuse = false;
            damageCoefficient = 0f;
            baseMinimumDuration = 0.2f;
            projectileBaseSpeed = 30;            

            base.OnEnter();
            PlayEnterSounds();

            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.1f);
            GetModelAnimator().SetBool("isHandOut", true);

            viewRadius = BaseRadius;

            viewRadius *= skillsPlusMulti;

            base.characterBody.hideCrosshair = false;

        }

        protected virtual void PlayEnterSounds()
        {
            Util.PlaySound(EnterSoundString, gameObject);
            //Util.PlaySound(LoopSoundString, gameObject);
        }

        protected virtual void PlayExitSounds()
        {
            Util.PlaySound(LoopStopSoundString, gameObject);
            Util.PlaySound(ExitSoundString, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            if (isAuthority)
            {
                if (inputBank.skill2.down)
                {
                    outer.SetNextStateToMain();
                }

                if (!inputBank.skill1.down)
                {
                    unpressed = true;
                }
                else
                {
                    if (unpressed)
                        repressed = true;
                }
            }

            endpointVisualizerRadiusScale = Mathf.Lerp(endpointVisualizerRadiusScale, viewRadius, 0.5f);
        }
        public override void Update()
        {
            base.Update();

            endpointVisualizerTransform.gameObject.SetActive(validTarget);
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            dest = default(AimThrowableBase.TrajectoryInfo);
            Ray aimRay = base.GetAimRay();
            RaycastHit raycastHit;
            bool foundPoint = false;

            if(Physics.SphereCast(aimRay, rayRadius, out raycastHit, maxDistance, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                foundPoint = true;
            }
            if (!foundPoint && Util.CharacterSpherecast(base.gameObject, aimRay, this.rayRadius, out raycastHit, this.maxDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal) && raycastHit.collider.GetComponent<HurtBox>())
            {
                foundPoint = true;
            }

            if ((raycastHit.point - transform.position).magnitude > maxDistance)
                foundPoint = false;

            if (foundPoint)
            {
                dest.hitPoint = raycastHit.point;
                dest.hitNormal = raycastHit.normal;
                validTarget = true;
            }
            else
            {
                dest.hitPoint = aimRay.GetPoint(this.maxDistance);
                dest.hitNormal = -aimRay.direction;
                validTarget = false;
            }

            Vector3 vector = dest.hitPoint - aimRay.origin;
            dest.speedOverride = this.projectileBaseSpeed;
            dest.finalRay = aimRay;
            dest.travelTime = this.projectileBaseSpeed / vector.magnitude;
        }

        public override bool KeyIsDown()
        {
            return !repressed;
        }

        // Token: 0x06003B19 RID: 15129 RVA: 0x0002B5A9 File Offset: 0x000297A9
        public override void OnExit()
        {
            base.OnExit();

            PlayExitSounds();
        }

        private void RefundStock()
        {
            base.skillLocator.utility.AddOneStock();
        }

        public override void FireProjectile() { }

        public override EntityState PickNextState()
        {
            if (validTarget)
            {
                castSuccessful = true;
                return ActuallyPickNextState(currentTrajectoryInfo.hitPoint);
            }
            else
            {
                return EntityStateCatalog.InstantiateState(ref outer.mainStateType);
            }
        }

        protected abstract EntityState ActuallyPickNextState(Vector3 point);

        // Token: 0x06003B1A RID: 15130 RVA: 0x000150E1 File Offset: 0x000132E1
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}