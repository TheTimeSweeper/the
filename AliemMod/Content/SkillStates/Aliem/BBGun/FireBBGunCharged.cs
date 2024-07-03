using EntityStates;
using UnityEngine;
using AliemMod.Modules;
using AliemMod.Content;

namespace ModdedEntityStates.Aliem
{
    public class FireBBGunCharged : RayGunFire
    {
        public override float BaseDuration => 0.5f;

        public override float BaseDamageCoefficient => _chargedDamageCoefficient;
        public override string soundString => _chargedShootSound;

        public override GameObject projectile => Projectiles.BBGunProjectilePrefabBig;

        private float _chargedDamageCoefficient;
        private string _chargedShootSound = "Play_AliemBBCharged";


        public FireBBGunCharged()
        {
            _chargedDamageCoefficient = AliemConfig.M1_BBGunCharged_Damage_Max.Value;
        }

        public FireBBGunCharged(float dam_)
        {
            _chargedDamageCoefficient = dam_;
        }

        public override void PlayAnimation(float duration)
        {
            PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration * 3);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
