using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireCharged : SwordFire
    {
        public override GameObject projectile => Projectiles.SwordProjectilePrefabBig;
        public override string soundString => "Play_AliemEnergySwordCharged";
        public override float BaseDamageCoefficient => _swordDamageCoefficient;

        private float _swordDamageCoefficient;
        private float _swordSpeedCoefficient;
        private float _projectileSpeed;

        public SwordFireCharged()
        {
            _swordDamageCoefficient = AliemConfig.M1_SwordCharged_Damage_Max.Value;
            _swordSpeedCoefficient = AliemConfig.M1_SwordCharged_Speed_Max.Value;
        }

        public SwordFireCharged(float dam, float sped)
        {
            _swordDamageCoefficient = dam;
            _swordSpeedCoefficient = sped;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            EntityStateMachine machine = RoR2.EntityStateMachine.FindByCustomName(gameObject, "Body");
            machine.SetNextState(new SwordFireChargedDash(_swordSpeedCoefficient));
        }

        public override void FireProjectile()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                aimRay = this.ModifyProjectileAimRay(aimRay);
                //aimRay.direction = Util.ApplySpread(aimRay.direction, this.minSpread, this.maxSpread, 1f, 1f, 0f, this.projectilePitchBonus);
                ProjectileManager.instance.FireProjectile(
                    this.projectilePrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(aimRay.direction),
                    base.gameObject, this.damageStat * this.damageCoefficient,
                    this.force, Util.CheckRoll(this.critStat, base.characterBody.master),
                    DamageColorIndex.Default,
                    null,
                    _swordSpeedCoefficient);
            }
        }
    }
}