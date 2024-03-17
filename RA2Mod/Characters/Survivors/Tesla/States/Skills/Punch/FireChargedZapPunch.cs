using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Tesla.States
{

    public class FireChargedZapPunch : ZapPunch
    {

        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerRailgunCryo");
        public static float MaxChargeDamageCoefficient = 8;
        public static float MaxBeamDamageCoefficient = 12;
        public static float MaxDistance = 50;

        public static float minPushForce = 930;
        public static float maxPushForce = 2690;

        public static float RecoilAmplitude = 1;

        public float chargeMultiplier = 0.5f;

        private int towerMultiplier = 1;
        private bool commandedTowers;
        private HurtBox commandTarget;

        public override void OnEnter()
        {

            TeslaTowerControllerController controller = GetComponent<TeslaTowerControllerController>();

            if (controller && controller.coilReady && isAuthority)
            {
                //client will find the commandtarget and serialize it
                commandTarget = characterBody.hurtBoxGroup.hurtBoxes[2];//guantlet hurtbox
            }

            //server will deserialize a commandTarget from the client
            if (commandTarget)
            {

                if (NetworkServer.active)
                {
                    controller.commandTowersGauntlet(commandTarget);
                }

                towerMultiplier = controller.NearbyTowers();

                commandedTowers = true;

                if (!characterMotor.isGrounded)
                    SmallHop(characterMotor, hitHopVelocity);
            }

            SetDurations();

            base.OnEnter();

            if (commandedTowers)
            {
                attack.damageColorIndex = DamageColorIndex.WeakPoint;
            }

            attack.pushAwayForce = Mathf.Lerp(minPushForce, maxPushForce, chargeMultiplier);
        }

        protected override float GetDamageCoefficient()
        {
            return MaxChargeDamageCoefficient * chargeMultiplier;
        }

        private void SetDurations()
        {
            if (commandedTowers)
            {
                BasePunchDuration = 1;
                animationDuration = 1.0f;
                baseAttackStartTime = 0.29f;
                baseAttackEndTime = 0.58f;
            }
            else
            {
                BasePunchDuration = 1;
                animationDuration = 0.4f;
                baseAttackStartTime = 0.214f;
                baseAttackEndTime = 0.58f;
            }
        }

        protected override void PlayAttackAnimation()
        {

            if (commandedTowers)
            {

                PlayCrossfade("Gesture, Override", "ShockPunchBeef", "Punch.playbackRate", animationDuration, 0.1f);

            }
            else
            {

                PlayCrossfade("Gesture, Override", "ShockPunchBeefLite", "Punch.playbackRate", animationDuration, 0.1f);
            }
        }

        protected override void FireConeProjectile()
        {
            if (!commandedTowers)
            {
                base.FireConeProjectile();
            }
        }

        protected override void FireAttackEnter()
        {
            base.FireAttackEnter();

            if (!commandedTowers)
                return;

            AddRecoil(-3f * RecoilAmplitude, -5f * RecoilAmplitude, -0.5f * RecoilAmplitude, 0.5f * RecoilAmplitude);

            Util.PlaySound("Play_tower_btesat2a_tesla_tower_attack", gameObject);

            Ray aimRay = GetAimRay();

            if (isAuthority)
            {
                BulletAttack bulletAttack = new BulletAttack();
                bulletAttack.owner = gameObject;
                bulletAttack.weapon = gameObject;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.aimVector = aimRay.direction;
                //bulletAttack.minSpread = this.minSpread;
                //bulletAttack.maxSpread = this.maxSpread;
                bulletAttack.bulletCount = 1u;
                bulletAttack.damage = chargeMultiplier * MaxBeamDamageCoefficient * towerMultiplier * damageStat;
                bulletAttack.force = 100f;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                bulletAttack.muzzleName = "MuzzleGauntlet";
                bulletAttack.hitEffectPrefab = hitEffectPrefab;
                bulletAttack.isCrit = attack.isCrit;
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

        // Token: 0x0600419A RID: 16794 RVA: 0x0002F86B File Offset: 0x0002DA6B
        public override void OnSerialize(NetworkWriter writer)
        {

            writer.Write(HurtBoxReference.FromHurtBox(commandTarget));
        }

        // Token: 0x0600419B RID: 16795 RVA: 0x0010A8CC File Offset: 0x00108ACC
        public override void OnDeserialize(NetworkReader reader)
        {

            commandTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}