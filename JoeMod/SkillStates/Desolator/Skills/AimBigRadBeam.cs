using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Desolator {


    public class AimBigRadBeam : AimThrowableBase {

        public static float DamageCoefficient = 1f;
        public static float BaseAttackRadius = 10;

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
            damageCoefficient = DamageCoefficient;
            baseMinimumDuration = 0.2f;
            //this.projectileBaseSpeed = 80;
            base.OnEnter();

            //todo bod
            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.1f);
            GetModelAnimator().SetBool("isHandOut", true);
        }

        // Token: 0x060006AD RID: 1709 RVA: 0x0001CDF3 File Offset: 0x0001AFF3
        public override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            base.ModifyProjectile(ref fireProjectileInfo);
            fireProjectileInfo.position = currentTrajectoryInfo.hitPoint;
            fireProjectileInfo.rotation = Quaternion.identity;
            fireProjectileInfo.speedOverride = 0f;
            fireProjectileInfo.damageTypeOverride = DamageType.BlightOnHit;
        }

        public override void FireProjectile() {
            base.FireProjectile();
            
            Util.PlaySound("Play_Desolator_Beam_Deep2", gameObject);

            EffectData effectData = new EffectData {
                origin = currentTrajectoryInfo.hitPoint,
                start = FindModelChild("MuzzleGauntlet").position
            };
            effectData.SetChildLocatorTransformReference(gameObject, GetModelChildLocator().FindChildIndex("MuzzleGauntlet"));
            EffectManager.SpawnEffect(Modules.Assets.DesolatorTracerRebar, effectData, false);
        }
    }
}
