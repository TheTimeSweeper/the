using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public abstract class AimChronosphereBase : AimThrowableBase
    {
        public static float BaseRadius = ChronoConfig.M3Radius.Value;

        public static string EnterSoundString = "Play_ChronosphereHumStart";
        public static string LoopSoundString = "Play_ChronosphereHumLoop";
        public static string LoopStopSoundString = "Stop_ChronosphereHumLoop";
        public static string ExitSoundString = "Play_ChronosphereHumEnd";
        public float skillsPlusMulti = 1;

        protected bool castSuccessful;

        protected float viewRadius;

        private bool unpressed;
        private bool repressed;

        public override void OnEnter()
        {
            projectilePrefab = ChronoAssets.chronoBombProjectile;
            endpointVisualizerPrefab = ChronoAssets.endPointivsualizer;
            endpointVisualizerRadiusScale = BaseRadius;
            arcVisualizerPrefab = ChronoAssets.arcvisualizer;
            maxDistance = 120;
            rayRadius = 0.4f;
            setFuse = false;
            damageCoefficient = 0f;
            baseMinimumDuration = 0.2f;
            projectileBaseSpeed = 120;            

            base.OnEnter();
            PlayEnterSounds();

            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.1f);
            GetModelAnimator().SetBool("isHandOut", true);

            viewRadius = BaseRadius;

            viewRadius *= skillsPlusMulti;

        }

        protected virtual void PlayEnterSounds()
        {
            Util.PlaySound(EnterSoundString, gameObject);
            Util.PlaySound(LoopSoundString, gameObject);
        }

        protected virtual void PlayExitSounds()
        {
            Util.PlaySound(LoopStopSoundString, gameObject);
            Util.PlaySound(EnterSoundString, gameObject);
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

        public override bool KeyIsDown()
        {
            return !repressed;
        }

        // Token: 0x06003B19 RID: 15129 RVA: 0x0002B5A9 File Offset: 0x000297A9
        public override void OnExit()
        {
            base.OnExit();

            if (!castSuccessful)
                RefundStock();

            PlayExitSounds();
        }

        private void RefundStock()
        {
            base.skillLocator.utility.AddOneStock();
        }

        //todo rework this to a simple projectile
        //instead of using a fake one in OnEnter and then not using it actually
        public override void FireProjectile() { }

        public override EntityState PickNextState()
        {
            castSuccessful = true;
            return ActuallyPickNextState(currentTrajectoryInfo.hitPoint);
        }

        protected abstract EntityState ActuallyPickNextState(Vector3 point);

        // Token: 0x06003B1A RID: 15130 RVA: 0x000150E1 File Offset: 0x000132E1
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}