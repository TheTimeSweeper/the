using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {

    public class FireChargedZapPunch : ZapPunchWithDeflect {

        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerRailgunCryo");
        public static float MaxChargeDamageCoefficient = 10;
        public static float MaxBeamDamageCoefficient = 20;
        public static float MaxDistance = 50;

        public static float RecoilAmplitude = 1;


        public float chargeMultiplier = 0.5f;

        private bool commandedTowers;
        private HurtBox commandTarget;

        public override void OnEnter() {

            TeslaTowerControllerController controller = GetComponent<TeslaTowerControllerController>();

            if (controller && controller.coilReady && base.isAuthority) {
                //client will find the commandtarget and serialize it
                commandTarget = characterBody.hurtBoxGroup.hurtBoxes[2];//guantlet hurtbox
            }

            //server will deserialize a commandTarget from the client
            if (commandTarget) {

                if (NetworkServer.active) {
                    controller.commandTowersGauntlet(commandTarget);
                }

                commandedTowers = true;

                if(!characterMotor.isGrounded)
                    SmallHop(characterMotor, hitHopVelocity);
            }

            SetDurations();

            base.OnEnter();

            if (commandedTowers) {
                attack.damageColorIndex = DamageColorIndex.WeakPoint;
            }
        }

        protected override float GetDamageCoefficient() {
            return MaxChargeDamageCoefficient * chargeMultiplier;
        }

        private void SetDurations() {
            if (commandedTowers) {
                BasePunchDuration = 1; 
                animationDuration = 0.4f;
                baseAttackStartTime = 0.29f;
                baseAttackEndTime = 0.58f;
            } else {
                BasePunchDuration = 1;
                animationDuration = 0.9f;
                baseAttackStartTime = 0.214f;
                baseAttackEndTime = 0.58f;
            }
        }

        protected override void PlayAttackAnimation() {

            if (commandedTowers) {

                PlayCrossfade("Gesture, Override", "ShockPunchBeef", "Punch.playbackRate", animationDuration, 0.1f);

            } else {

                PlayCrossfade("Gesture, Override", "ShockPunchBeefLite", "Punch.playbackRate", animationDuration, 0.1f);
            }
        }

        protected override void FireConeProjectile() {
            if (!commandedTowers) {
                base.FireConeProjectile();
            }
        }

        protected override void OnFireAttackEnter() {
            base.OnFireAttackEnter();

            if (!commandedTowers)
                return;

            Ray aimRay = base.GetAimRay();

            base.AddRecoil(-3f * RecoilAmplitude, -5f * RecoilAmplitude, -0.5f * RecoilAmplitude, 0.5f * RecoilAmplitude);

            if (base.isAuthority) {
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = base.gameObject;
                bulletAttack.weapon = base.gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                //bulletAttack.minSpread = this.minSpread;
                //bulletAttack.maxSpread = this.maxSpread;
                bulletAttack.bulletCount = 1u;
                bulletAttack.damage = chargeMultiplier * MaxBeamDamageCoefficient * this.damageStat;
                bulletAttack.force = 100f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                bulletAttack.muzzleName = "MuzzleGauntlet";
                bulletAttack.hitEffectPrefab = this.hitEffectPrefab;
                bulletAttack.isCrit = base.RollCrit();
                bulletAttack.HitEffectNormal = false;
                bulletAttack.radius = 3f;
                bulletAttack.damageType = DamageType.Shock5s;
                bulletAttack.damageColorIndex = DamageColorIndex.WeakPoint;
                bulletAttack.smartCollision = true;
                bulletAttack.maxDistance = MaxDistance;
                bulletAttack.stopperMask = 0;// LayerIndex.world.collisionMask;
                bulletAttack.Fire();
            }
        }
    }
}