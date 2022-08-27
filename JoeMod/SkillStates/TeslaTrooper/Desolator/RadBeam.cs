using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {

    public class AimBigRadBeam : AimBigZap {

    }

    public class RadBeam : GenericBulletBaseState {
        public override void OnEnter() {

            EntityStates.Toolbot.FireSpear goodstate = new EntityStates.Toolbot.FireSpear();

            baseDuration = 0.6f;
            bulletCount = 1;
            maxDistance = goodstate.maxDistance;
            bulletRadius = goodstate.bulletRadius;
            useSmartCollision = goodstate.useSmartCollision;
            damageCoefficient = 2f;
            procCoefficient = 1f;
            force = 100f;
            minSpread = 0;
            maxSpread = 0;
            spreadPitchScale = 1f;
            spreadYawScale = 1f;
            spreadBloomValue = 0.5f;
            recoilAmplitudeY = 1;
            recoilAmplitudeX = 1;
            muzzleName = "MuzzleGauntlet";
            fireSoundString = "desolatorFire";
            //muzzleFlashPrefab = goodstate.muzzleFlashPrefab;
            tracerEffectPrefab = Modules.Assets.DesolatorTracer;
            //hitEffectPrefab;
            base.OnEnter();
        }
        public override void ModifyBullet(BulletAttack bulletAttack) {
            bulletAttack.damageType = DamageType.BlightOnHit;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
        }
    }
}
