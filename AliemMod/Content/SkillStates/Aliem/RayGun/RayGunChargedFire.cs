using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem {

    public class RayGunChargedFire : RayGunFire {
        
		new public static float BaseDuration = 0.5f;
        
        public static float BaseDamage = 6;
		public float ChargedCoefficient = BaseDamage;
        
        private string _shootSound = "Play_RayGunBigClassic";

        public override GameObject projectile => Modules.Projectiles.RayGunProjectilePrefabBig;


        public RayGunChargedFire() {
        }

        public RayGunChargedFire(float dam_, string shootSound_) {
            ChargedCoefficient = dam_;
            _shootSound = shootSound_;
        }

        protected override void ModifyState() {

			base.baseDuration = BaseDuration;
			base.damageCoefficient = ChargedCoefficient;
            base.attackSoundString = _shootSound;
		}

        public override void PlayAnimation(float duration) {
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
