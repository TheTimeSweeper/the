﻿using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class CloseRangeKnife : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            hitboxGroupName = "Knife";
            
            damageType = DamageType.Generic;
            damageCoefficient = AliemConfig.M1_RayGun_Damage.Value * AliemConfig.M1_RayGun_CloseRangeKnife_Damage_Multiplier.Value;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            
            baseDuration = AliemConfig.M1_RayGun_CloseRangeKnife_Duration.Value;
            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.0f;
            attackEndPercentTime = 0.5f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.73f;

            hitStopDuration = 0.06f;
            attackRecoil = 0.5f;
            hitHopVelocity = 3f;

            swingSoundString = "";
            hitSoundString = "";
            muzzleString = "KnifeHitbox";
            playbackRateParam = "ShootGun.playbackRate";
            swingEffectPrefab = AliemAssets.knifeSwingEffect;
            hitEffectPrefab = AliemAssets.knifeImpactEffect;

            //impactSound = HenryAssets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAnimation("Gesture, Override", "Knife", "ShootGun.playbackRate", duration);
        }
    }
}