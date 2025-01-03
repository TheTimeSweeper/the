﻿using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using RoR2.Audio;

namespace KatamariMod.Survivors.Katamari.States
{
    public class KatamariCharacterMain : GenericCharacterMain
    {
        private OverlapAttack overlapAttack;

        private Vector3 velocity => characterMotor.velocity;
        private HitBoxGroup hitBoxGroup;
        private bool wasAttacking;

        public override void OnEnter()
        {
            base.OnEnter();

            hitBoxGroup = base.FindHitBoxGroup("Katamari");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            float speed = velocity.magnitude;
            float rootMotion = characterMotor.rootMotion.magnitude;
            if(Input.GetKey(KeyCode.G)) Log.Warning(rootMotion);

            if (speed >= KatamariConfig.passive_speedAttackThreshold.Value)
            {
                if (!wasAttacking)
                {
                    InitAttack(speed);
                }
                bool hit = overlapAttack.Fire();
                if (hit)
                {
                    Vector3 recoilVelocity = Vector3.Cross(Vector3.Cross(GetAimRay().direction, Vector3.up), Vector3.down);
                    recoilVelocity = -recoilVelocity + Vector3.up * 0.5f;
                    characterMotor.velocity = recoilVelocity * KatamariConfig.impactBounceMultiplier.Value;
                }
            }

            wasAttacking = speed >= KatamariConfig.passive_speedAttackThreshold.Value;
        }

        private void InitAttack(float speed)
        {
            overlapAttack = new OverlapAttack();
            overlapAttack.attacker = base.gameObject;
            overlapAttack.damage = characterBody.damage * KatamariConfig.passive_speedAttackMultiplier.Value * speed;
            overlapAttack.damageColorIndex = DamageColorIndex.Default;
            overlapAttack.damageType = DamageType.Generic;
            overlapAttack.forceVector = velocity;
            overlapAttack.hitBoxGroup = this.hitBoxGroup;
            overlapAttack.hitEffectPrefab = null;
            NetworkSoundEventDef networkSoundEventDef = null;
            overlapAttack.impactSound = ((networkSoundEventDef != null) ? networkSoundEventDef.index : NetworkSoundEventIndex.Invalid);
            overlapAttack.inflictor = base.gameObject;
            overlapAttack.isCrit = base.RollCrit();
            overlapAttack.procChainMask = default(ProcChainMask);
            overlapAttack.pushAwayForce = 1000;
            overlapAttack.procCoefficient = 1;
            overlapAttack.teamIndex = base.GetTeam();
        }
    
    }
}
