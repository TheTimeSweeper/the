using EntityStates;
using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper
{
    public class Zap : BaseTimedSkillState
    {
        //todo less damage on allies
        //and more bounce range
        #region Gameplay Values
        public static float DamageCoefficient = 1f;
        public static float BounceDamageMultplier = 0.69f;
        public static float ProcCoefficient = 0.8f;
        public static int OrbCasts = 3;
        public static float BounceDistance = 20;

        public static float BaseDuration = 1f;
        public static float BaseCastTime = 0.05f;//todo anim

        public static float nearDist1 = 0.4f;
        public static float nearDist2 = 0.7f;
        #endregion

        private float shotSpread = 3;

        public int skillsPlusCasts = 0;

        private List<HealthComponent> _bouncedObjectsList = new List<HealthComponent>();
        private bool _crit;

        private TeslaWeaponComponent _weaponComponent;
        private TeslaTrackerComponent _tracker;
        private HurtBox _targetHurtbox;
        private bool _attackingTeammate;

        private float _baseCastInterval = 0.05f;

        private Transform _muzzleTransform;
        private float _originSpacing = 0.1f;

        private int _currentCasts;

        private int totalOrbCasts;

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

                    return _muzzleTransform.position + _muzzleTransform.forward * (-_originSpacing * (_currentCasts % 3));
                }
            }
        }

        private LightningOrb.LightningType GetOrbType
        {
            get
            {
                if (_weaponComponent && _weaponComponent.hasTeslaCoil) {
                    return LightningOrb.LightningType.Tesla;
                }

                switch (_currentCasts)
                {
                    case 0:
                        return LightningOrb.LightningType.Ukulele;
                    case 1:
                        return LightningOrb.LightningType.Ukulele;
                    case 2:
                        return LightningOrb.LightningType.Ukulele;
                    default:
                    case 3:
                        return LightningOrb.LightningType.Ukulele;
                }
            }
        }
        #endregion

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastTime);

            _weaponComponent = GetComponent<TeslaWeaponComponent>();

            _tracker = GetComponent<TeslaTrackerComponent>();

            _muzzleTransform = GetModelChildLocator().FindChild("MuzzleGauntlet");
            if (_muzzleTransform == null)
                _muzzleTransform = transform;

            StartAimMode(2);

            //todo: joe crosscompat
            //base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);
            GetModelAnimator().SetBool("isHandOut", true);
            if (isAuthority) {
                if (_tracker) {
                    _targetHurtbox = _tracker.GetTrackingTarget();

                    switch (_tracker.GetTrackingTargetDistance()) {
                        default:
                        case TeslaTrackerComponent.RangeTier.FURTHEST:
                            totalOrbCasts = OrbCasts - 2 + Mathf.FloorToInt(skillsPlusCasts * 0.334f);
                            break;
                        case TeslaTrackerComponent.RangeTier.MIDDLE:
                            totalOrbCasts = OrbCasts - 1 + Mathf.FloorToInt(skillsPlusCasts * 0.667f);
                            break;
                        case TeslaTrackerComponent.RangeTier.CLOSEST:
                            totalOrbCasts = OrbCasts + skillsPlusCasts;
                            break;
                    }

                    _attackingTeammate = _tracker.GetIsTargetingTeammate();
                    //if(_attackingTeammate) {
                    //    totalOrbCasts = 3;
                    //}

                } else {
                    _targetHurtbox = createOrb().PickNextTarget(transform.position);
                    totalOrbCasts = OrbCasts;
                }
            }

            _crit = RollCrit();
        }

        public override void OnExit() {
            base.OnExit();

            GetModelAnimator().SetBool("isHandOut", false);
        }

        private LightningOrb createOrb() {
            return new LightningOrb {
                origin = transform.position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = _crit,
                bouncesRemaining = 1,
                damageCoefficientPerBounce = BounceDamageMultplier,
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                bouncedObjects = _bouncedObjectsList,
                lightningType = LightningOrb.LightningType.Ukulele,
                damageColorIndex = DamageColorIndex.Default,
                range = BounceDistance,
                speed = 690,
            };
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
            
            if (NetworkServer.active && _targetHurtbox) {
                while (_currentCasts < totalOrbCasts && fixedAge > nextCastTime) {
                    FireZap();
                    _currentCasts++;
                }
            }
        }

        private void FireZap() {

            LightningOrb _lightningOrb = createOrb();
            _lightningOrb.origin = GetOrbOrigin;
            _lightningOrb.lightningType = GetOrbType;
            _lightningOrb.target = _targetHurtbox;
            //apply conduct on first cast only
            if (FacelessJoePlugin.conductiveMechanic) {
                _lightningOrb.AddModdedDamageType(Modules.DamageTypes.conductive);
            }
            ModifyTeamLightningOrb(_lightningOrb);
            OrbManager.instance.AddOrb(_lightningOrb);
            //happens after firing to apply to bounces only
            _lightningOrb.lightningType = LightningOrb.LightningType.MageLightning;

            base.characterBody.AddSpreadBloom(TestValueManager.value1);
        }

        private void ModifyTeamLightningOrb(LightningOrb lightningOrb) {
            if (_attackingTeammate) {
                //lightningOrb.range = BounceDistance * 2;
                //lightningOrb.damageCoefficientPerBounce = 3f;
                lightningOrb.bouncesRemaining = 0;

                lightningOrb.damageValue *= 0.1f;
                lightningOrb.procCoefficient = 0;
            }
        }

        private void PlayZap()
        {
            EffectManager.SimpleMuzzleFlash(Modules.Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"),
                                            gameObject,
                                            "MuzzleGauntlet",
                                            true);
            PlayAnimation("Gesture, Additive", "Shock");

            string sound = "Play_itesatta";
            if (_crit) sound = "Play_trooper_itesat2b_tesla_trooper_attack";
            //sound = EntityStates.Mage.Weapon.FireLaserbolt.attackString;

            Util.PlaySound(sound, gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        // Token: 0x0600419A RID: 16794 RVA: 0x0002F86B File Offset: 0x0002DA6B
        public override void OnSerialize(NetworkWriter writer) {

            writer.Write(HurtBoxReference.FromHurtBox(this._targetHurtbox));
            writer.Write(_attackingTeammate);
            writer.Write(totalOrbCasts);
        }

        // Token: 0x0600419B RID: 16795 RVA: 0x0010A8CC File Offset: 0x00108ACC
        public override void OnDeserialize(NetworkReader reader) {

            this._targetHurtbox = reader.ReadHurtBoxReference().ResolveHurtBox();
            this._attackingTeammate = reader.ReadBoolean();
            this.totalOrbCasts = reader.ReadInt32();
        }
    }
}