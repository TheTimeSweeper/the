using EntityStates;
using HenryMod.ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper
{
    public class Zap : BaseTimedSkillState
    {

        //todo less damage on allies
        //and more bounce range
        #region Gameplay Values
        public static float DamageCoefficient = 1.2f;
        public static float BounceDamageMultplier = 0.69f;
        public static float ProcCoefficient = 1f;
        public static int OrbCasts = 3;
        public static float BounceDistance = 20;

        public static float BaseDuration = 1.2f;
        public static float BaseCastTime = 0.05f;//todo anim
        #endregion

        private LightningOrb _lightningOrb;
        private TotallyOriginalTrackerComponent _tracker;
        private HurtBox _targetHurtbox;

        private float _baseCastInterval = 0.05f;

        private Transform _muzzleTransform;
        private float _originSpacing = 0.069f;

        private int _currentCasts;

        private float nextCastTime
        {
            get => (_baseCastInterval * _currentCasts + castStartTime) / attackSpeedStat;
        }
        #region orb iterations
        private Vector3 GetOrbOrigin
        {
            get
            {
                if (_muzzleTransform == null)
                {
                    return transform.position;
                }
                else
                {
                    switch (_currentCasts)
                    {
                        case 0:
                            return _muzzleTransform.position + _muzzleTransform.forward * _originSpacing;
                        case 1:
                            return _muzzleTransform.position - _muzzleTransform.forward * _originSpacing;
                        default:
                        case 2:
                            return _muzzleTransform.position;
                    }
                }
            }
        }

        private LightningOrb.LightningType GetOrbColor
        {
            get
            {
                switch (_currentCasts)
                {
                    default:
                    case 0:
                        return LightningOrb.LightningType.Ukulele;
                    case 1:
                        return LightningOrb.LightningType.Ukulele;
                    case 2:
                        return LightningOrb.LightningType.Ukulele;
                }
            }
        }
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastTime);

            _tracker = GetComponent<TotallyOriginalTrackerComponent>();

            _muzzleTransform = GetModelChildLocator().FindChild("MuzzleGauntlet");

            StartAimMode(2);

            //todo: joe crosscompat
            //base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);

            //todo incombat
            PlayAnimation("Gesture, Override", "HandOut");
            if (_tracker)
            {
                _targetHurtbox = _tracker.GetTrackingTarget();
            }
            else
            {
                _targetHurtbox = _lightningOrb.PickNextTarget(transform.position);
            }

            if (!_targetHurtbox)
            {
                duration = 0.1f;
                //base.outer.SetNextStateToMain();
                return;
            }
            _lightningOrb = new LightningOrb
            {
                origin = transform.position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = RollCrit(),
                bouncesRemaining = 1,
                damageCoefficientPerBounce = BounceDamageMultplier,
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                bouncedObjects = new List<HealthComponent>(),
                lightningType = LightningOrb.LightningType.Ukulele,
                damageColorIndex = DamageColorIndex.Default,
                range = BounceDistance,
                speed = 690,
            };

            _lightningOrb.target = _targetHurtbox;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            if (_targetHurtbox)
            {
                PlayZap();
            }
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            while (_currentCasts < OrbCasts && fixedAge > nextCastTime)
            {
                FireZap();
                _currentCasts++;
            }
        }

        private void FireZap()
        {
            if (_targetHurtbox)
            {
                _lightningOrb.origin = GetOrbOrigin;
                _lightningOrb.lightningType = GetOrbColor;
                OrbManager.instance.AddOrb(_lightningOrb);
                //happens after firing to apply to spreads only
                _lightningOrb.lightningType = LightningOrb.LightningType.MageLightning;
            }
        }

        private void PlayZap()
        {
            //muzzle flash on gauntle
            PlayAnimation("Gesture, Additive", "Shock");

            string sound = "Play_trooper_itesat2a_tesla_trooper_attack";
            if (_lightningOrb.isCrit) sound = "Play_trooper_itesat2b_tesla_trooper_attack";

            //sound = EntityStates.Mage.Weapon.FireLaserbolt.attackString;
            //todo sound wwise random sound
                //oh wait the alt sound was the powered up one I think. 
            //but not random pitch heh
            PlaySoundAuthority(sound);

            //god that was beautiful to my heart
            //but hurtful my brain
            //todo sound: actual audio engineering. too much high frequency there
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}