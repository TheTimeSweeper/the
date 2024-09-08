using EntityStates;
using R2API;
using RoR2;

namespace RA2Mod.Survivors.Desolator.States
{
    public class RadBeam : GenericBulletBaseState {

        public static float BaseDuration = 1.0f;
        public static float DamageCoefficient = 1.0f;

        public static int RadPrimaryStacks = 2;
        public static float RadDamageMultiplier = 0.7f;

        public float skillsPlusDurationMultiplier = 1;

        public virtual string muzzleString => "MuzzleGauntlet";

        public override void OnEnter() {

            EntityStates.Toolbot.FireSpear goodstate = new EntityStates.Toolbot.FireSpear();

            baseDuration = BaseDuration * skillsPlusDurationMultiplier;
            bulletCount = 1;
            maxDistance = goodstate.maxDistance;
            bulletRadius = goodstate.bulletRadius;
            useSmartCollision = true;
            damageCoefficient = DamageCoefficient;
            procCoefficient = 1f;
            force = 100f;
            minSpread = 0;
            maxSpread = 0;
            spreadPitchScale = 1f;
            spreadYawScale = 1f;
            spreadBloomValue = 2000f;//uh
            recoilAmplitudeY = 1;
            recoilAmplitudeX = 1;
            muzzleName = muzzleString;
            fireSoundString = "Play_Desolator_Beam_Short";
            muzzleFlashPrefab = goodstate.muzzleFlashPrefab;
            tracerEffectPrefab = null; // DesolatorAssets.DesolatorTracerSnipe;
            hitEffectPrefab = DesolatorAssets.IrradiatedImpactEffect;
            base.OnEnter();

            PlayShootAnimation();
        }

        public virtual void PlayShootAnimation()
        {
            PlayAnimation("Desolator, Override", "DesolatorShoot");
        }
        
        public override void ModifyBullet(BulletAttack bulletAttack) {
            //bulletAttack.damageType = DamageType.BlightOnHit;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
            //DamageAPI.AddModdedDamageType(bulletAttack, Modules.DamageTypes.DesolatorArmorShred);
            DamageAPI.AddModdedDamageType(bulletAttack, DesolatorDamageTypes.DesolatorDotPrimary);
        }
    }
}
