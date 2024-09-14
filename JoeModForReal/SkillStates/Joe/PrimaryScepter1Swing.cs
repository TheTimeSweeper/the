using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Joe {

    public class PrimaryScepter1Swing : Primary1Swing {
        public static float BeamDamage => TestValueManager.swrodBeam;

        protected override void FireAttack() {

            if (isAuthority && !hasFired) {
                FireSwordBeamAuthority();
            }

            base.FireAttack();
        }

        private void FireSwordBeamAuthority() {

            Ray aimRay = base.GetAimRay();

            //Util.PlaySound("play_joe_loz_swordShoot", gameObject);

            ProjectileManager.instance.FireProjectile(Modules.Projectiles.JoeSwordBeam,
                aimRay.origin,
                Util.QuaternionSafeLookRotation(aimRay.direction),
                base.gameObject,
                PrimaryScepter1Swing.BeamDamage * this.damageStat,
                0f,
                rolledCrit,
                DamageColorIndex.Default,
                null,
                100);
        }

        protected override EntityState GetJumpSwingState() {
            return new PrimaryScepter1JumpSwingFall();
        }
    }
}