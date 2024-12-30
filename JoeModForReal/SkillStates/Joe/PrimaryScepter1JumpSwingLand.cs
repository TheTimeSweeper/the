using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class PrimaryScepter1JumpSwingLand : Primary1JumpSwingLand {

        public static float beems => (int)TestValueManager.beems;
        
        protected override void FireAttack() {
            if (base.isAuthority && !hasFired) {
                float angle = 360.00f / beems;

                Vector3 aimDir = GetAimRay().direction;

                for (int i = 0; i < beems; i++) {
                    Vector3 dir = aimDir;
                    dir.y = 0;
                    dir = Quaternion.Euler(0, angle * i, 0) * dir;

                    FireSwordBeamAuthority(dir);
                }
            }

            base.FireAttack();
        }
        protected override void PlayAttackAnimation() {
            //todo spin attack animation
            base.PlayAnimation("Arms, Override", "jumpSwingLand", "jumpSwing.playbackRate", this.duration);
        }

        //dry
        private void FireSwordBeamAuthority(Vector3 direction) {

            Ray aimRay = base.GetAimRay();

            ProjectileManager.instance.FireProjectile(Modules.Projectiles.JoeSwordBeam,
                FindModelChild("JumpSwingMuzzle").position + direction,
                Util.QuaternionSafeLookRotation(direction),
                base.gameObject,
                PrimaryScepter1Swing.BeamDamage * this.damageStat,
                0f,
                rolledCrit,
                DamageColorIndex.Default,
                null,
                100,
                DamageTypeCombo.GenericPrimary);
        }
    }
}