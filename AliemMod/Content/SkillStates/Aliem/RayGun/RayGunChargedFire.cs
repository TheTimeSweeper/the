using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class RayGunChargedFire : RayGunFire {

        public override float BaseDuration => 0.5f;

        public override float BaseDamageCoefficient => _chargedDamageCoefficient;
        public override string soundString => _chargedShootSound;

        public override GameObject projectile => Projectiles.RayGunProjectilePrefabBig;

		private float _chargedDamageCoefficient;
        private string _chargedShootSound = "Play_RayGunBigClassic";


        public RayGunChargedFire() {
            _chargedDamageCoefficient = AliemConfig.M1_RayGunCharged_Damage_Max.Value;
        }

        public RayGunChargedFire(float dam_, string shootSound_) {
            _chargedDamageCoefficient = dam_;
            _chargedShootSound = shootSound_;
        }

        public override void PlayAnimation(float duration) {
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration*3);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}
