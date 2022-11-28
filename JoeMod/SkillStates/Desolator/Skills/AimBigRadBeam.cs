using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Desolator {

    public class AimBigRadBeam : AimThrowableBase {

        public static float BlastDamageCoefficient = 4.0f;
        public static float PoolDamageCoefficient = 0.2f;
        public static float DotZoneLifetime = 4;
        public static float BaseAttackRadius = 10;

        private bool _crit;

        public override void OnEnter() {

            //excuse old enforcer code again
            EntityStates.Toolbot.AimStunDrone goodState = new EntityStates.Toolbot.AimStunDrone();
            arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            projectilePrefab = Modules.Assets.DesolatorCrocoLeapProjectile;// EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab;
            endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = BaseAttackRadius;
            maxDistance = 40;
            rayRadius = 1.6f;
            setFuse = false;
            damageCoefficient = PoolDamageCoefficient;
            baseMinimumDuration = 0.2f;

            _crit = RollCrit();
            //this.projectileBaseSpeed = 80;
            base.OnEnter();

            //todo bod
            //PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.1f);
            //GetModelAnimator().SetBool("isHandOut", true);
        }

        // Token: 0x060006AD RID: 1709 RVA: 0x0001CDF3 File Offset: 0x0001AFF3
        public override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            base.ModifyProjectile(ref fireProjectileInfo);
            fireProjectileInfo.position = currentTrajectoryInfo.hitPoint;
            fireProjectileInfo.rotation = Quaternion.identity;
            fireProjectileInfo.speedOverride = 0f;
            //fireProjectileInfo.damageTypeOverride = DamageType.BlightOnHit;
            fireProjectileInfo.crit = _crit;
        }

        public override void OnProjectileFiredLocal() {
            Util.PlaySound("Play_Desolator_Beam_Deep2", gameObject);

            //PlayAnimation("Gesture, Additive", "Shock", "Shock.playbackRate", 0.3f);
            PlayAnimation("Desolator, Override", "DesolatorShoot");
        }

        public override void FireProjectile() {
            base.FireProjectile();

            //show tracer beam
            EffectData effectData = new EffectData {
                origin = currentTrajectoryInfo.hitPoint,
                start = FindModelChild("MuzzleGauntlet").position
            };
            effectData.SetChildLocatorTransformReference(gameObject, GetModelChildLocator().FindChildIndex("MuzzleGauntlet"));
            EffectManager.SpawnEffect(Modules.Assets.DesolatorTracerRebar, effectData, true);

            if (base.isAuthority) {

                BlastAttack blast = new BlastAttack {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = currentTrajectoryInfo.hitPoint,
                    radius = BaseAttackRadius,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = damageStat * BlastDamageCoefficient,
                    crit = _crit,
                    damageType = DamageType.Generic,
                    //damageColorIndex = DamageColorIndex.Default,

                    procCoefficient = 1,
                    //procChainMask = 
                    //losType = BlastAttack.LoSType.NearestHit,

                    baseForce = -5, //enfucker void grenade here we go
                                    //bonusForce = ;

                    //impactEffect = EffectIndex.uh;
                };
                R2API.DamageAPI.AddModdedDamageType(blast, Modules.DamageTypes.DesolatorDot);
                blast.Fire();

                EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoLeapExplosion"), new EffectData {
                    origin = currentTrajectoryInfo.hitPoint,
                    scale = BaseAttackRadius
                }, true);
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }

        public override EntityState PickNextState() {
            WindDownState windDownState = new WindDownState();
            windDownState.windDownTime = 0.8f;
            return windDownState;
        }
    }
}
