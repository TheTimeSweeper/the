using EntityStates;
using RoR2;
using RoR2.Projectile;

namespace ModdedEntityStates.Desolator {

    public class RadBeam : GenericBulletBaseState {

        public static float DamageCoefficient = 1.2f;

        public override void OnEnter() {

            EntityStates.Toolbot.FireSpear goodstate = new EntityStates.Toolbot.FireSpear();

            baseDuration = 1f;
            bulletCount = 1;
            maxDistance = goodstate.maxDistance;
            bulletRadius = goodstate.bulletRadius;
            useSmartCollision = goodstate.useSmartCollision;
            damageCoefficient = DamageCoefficient;
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
            fireSoundString = "Play_Desolator_Beam_Short";
            muzzleFlashPrefab = goodstate.muzzleFlashPrefab;
            tracerEffectPrefab = Modules.Assets.DesolatorTracerSnipe;
            //hitEffectPrefab;
            base.OnEnter();

            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.05f);
            GetModelAnimator().SetBool("isHandOut", true);
            PlayAnimation("Gesture, Additive", "Shock");
        }
        public override void ModifyBullet(BulletAttack bulletAttack) {
            bulletAttack.damageType = DamageType.BlightOnHit;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
        }
    }
}
