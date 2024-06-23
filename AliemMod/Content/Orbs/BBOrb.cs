
using AliemMod.Modules;
using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;

namespace AliemMod.Content.Orbs
{
    public class BBOrb : Orb
    {
        public Vector3 targetPosition;
        public float speed;

        public Vector3 attackOrigin;
        public float damageValue;
        public GameObject attacker;
        public bool isCrit;
        public ProcChainMask procChainMask;
        public float procCoefficient;

        private BulletAttack _backupBulletAttack;
        private bool _bulletHit;

        public override void Begin()
        {
            base.Begin();

            if (targetPosition == default)
            {
                targetPosition = target? target.transform.position : origin;
            }

            base.duration = Vector3.Distance(targetPosition, origin) / this.speed;

            EffectData effectData = new EffectData
            {
                origin = this.origin,
                start = this.targetPosition,
                genericFloat = base.duration
            };
            if (target != null) {
                effectData.SetHurtBoxReference(target);
            }
            EffectManager.SpawnEffect(Assets.BBOrbEffect, effectData, true);
        }

        public override void OnArrival()
        {
            if (this.target)
            {
                HealthComponent healthComponent = this.target.healthComponent;
                if (healthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = this.damageValue;
                    damageInfo.attacker = this.attacker;
                    damageInfo.inflictor = null;
                    damageInfo.force = Vector3.zero;
                    damageInfo.crit = this.isCrit;
                    damageInfo.procChainMask = this.procChainMask;
                    damageInfo.procCoefficient = this.procCoefficient;
                    damageInfo.position = this.target.transform.position;
                    //damageInfo.damageColorIndex = this.damageColorIndex;
                    damageInfo.damageType = DamageType.Generic;
                    healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
                }
            } 
            else if ((targetPosition != origin))
            {
                Firebullet();
            }

        }

        private void Firebullet()
        {
            if (_bulletHit)
                return;

            //todo move to state and do authority
            //if (_backupBulletAttack == null)
            //{
                /*_backupBulleAttack = */new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = targetPosition - attackOrigin,
                    origin = attackOrigin,
                    damage = damageValue,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = Vector3.Distance(origin, targetPosition),
                    force = 0,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0,
                    maxSpread = 0,
                    isCrit = isCrit,
                    owner = attacker,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = 0.3f,
                    tracerEffectPrefab = null,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = null,
                    //hitCallback = hitCallback
                }.Fire();
            //}

            //_backupBulletAttack.Fire();
        }

        private bool hitCallback(BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo)
        {
            _bulletHit = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            return _bulletHit;
        }
    }
}
