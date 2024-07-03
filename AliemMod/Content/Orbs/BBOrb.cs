
using AliemMod.Modules;
using RoR2;
using RoR2.Orbs;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static RoR2.BulletAttack;

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
            //if (target != null) {
            //    effectData.SetHurtBoxReference(target);
            //}
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
        }
    }
}
