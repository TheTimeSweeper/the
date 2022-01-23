using EntityStates;
using HenryMod.ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.ModdedEntityStates.Joe
{
    public class Zap : BaseTimedSkillState {

        #region Gameplay Values
        public static float DamageCoefficient = 1.2f;
        public static float BounceDamageMultplier = 0.69f;
        public static float ProcCoefficient = 1f;
        public static int TotalCasts = 3;
                
        public static float BaseDuration = 1f;
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
            get => (_baseCastInterval * _currentCasts + castStartTime) / base.attackSpeedStat;
        }
        #region cast iterations
        private Vector3 GetOrbOrigin
        {
            get {
                if(_muzzleTransform == null)
                {
                    return base.transform.position;
                } 
                else
                {
                    switch (_currentCasts)
                    {
                        default:
                        case 0:
                            return _muzzleTransform.position;
                        case 1:
                            return _muzzleTransform.position + _muzzleTransform.right * _originSpacing;
                        case 2:
                            return _muzzleTransform.position - _muzzleTransform.right * _originSpacing;
                    }
                }
            }
        }

        private LightningOrb.LightningType GetOrbColor
        {
            get => LightningOrb.LightningType.Ukulele;// _currentCasts == 0 ? LightningOrb.LightningType.Ukulele : LightningOrb.LightningType.Tesla;
        }
        #endregion

        public override void OnEnter()
        {
            base.OnEnter();
            SetDurationValues(BaseDuration, BaseCastTime);

            this._tracker = base.GetComponent<TotallyOriginalTrackerComponent>();

            this._muzzleTransform = base.GetModelChildLocator().FindChild("GauntletMuzzle");

            base.StartAimMode(2);

            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);

            if (_tracker)
            {
                _targetHurtbox = _tracker.GetTrackingTarget();
            }
            else
            {
                _targetHurtbox = _lightningOrb.PickNextTarget(this.transform.position);
            }

            if (!_targetHurtbox)
            {
                duration = 0.1f;
                return;
            }

            _lightningOrb = new LightningOrb
            {
                origin = base.transform.position,
                damageValue = Zap.DamageCoefficient * base.damageStat,
                isCrit = base.RollCrit(),
                bouncesRemaining = 1,
                damageCoefficientPerBounce = BounceDamageMultplier,
                damageType = DamageType.SlowOnHit,
                teamIndex = this.teamComponent.teamIndex,
                attacker = base.gameObject,
                procCoefficient = 1f,
                bouncedObjects = new List<HealthComponent>(),
                lightningType = LightningOrb.LightningType.Ukulele,
                damageColorIndex = DamageColorIndex.Default,
                range = 35f
            };

            _lightningOrb.target = _targetHurtbox;
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();
            PlayZap();
        }

        protected override void OnCastFixedUpdate()
        {
            base.OnCastFixedUpdate();

            while (_currentCasts < TotalCasts && base.fixedAge > nextCastTime)
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
            }
        }

        private void PlayZap()
        {
            //play sound
            //muzzle flash on gauntlet
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}