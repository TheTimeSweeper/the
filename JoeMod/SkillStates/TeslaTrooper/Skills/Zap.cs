using TeslaTrooper;
using EntityStates;
using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {

    public class Zap : BaseTimedSkillState
    {
        internal static event Action<bool, bool> onZapAuthority;
        internal static event Action<HurtBox> onZapAllyAuthority;

        #region Gameplay Values
        public static float DamageCoefficient = 1.2f;
        public static float BounceDamageMultplier = 0.69f;
        public static float ProcCoefficient = 0.8f;
        public static int OrbCasts = 3;
        public static float BounceDistance = 20;

        public static float BaseDuration = 0.9f;
        public static float BaseCastTime = 0.05f;
        #endregion

        public int skillsPlusCasts = 0;

        private List<HealthComponent> _bouncedObjectsList = new List<HealthComponent>();
        private bool _crit;

        private TeslaWeaponComponent _weaponComponent;
        private TeslaTrackerComponentZap _tracker;
        private HurtBox _targetHurtbox;
        private bool _attackingTeammate;
        
        private float _baseCastInterval = 0.1f;

        private Transform _muzzleTransform;
        private float _originSpacing = 0.22f;
        
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
                    return muzzlePosition + muzzleDirection * (_originSpacing * (_currentCasts));
                }
            }
        }


        private Vector3? originalMuzzlePosition = null;
        private Vector3 muzzlePosition {
            get {
                if (originalMuzzlePosition == null) {
                    originalMuzzlePosition = _muzzleTransform.position;
                }
                return originalMuzzlePosition.Value;
            }
        }

        private Vector3? originalMuzzleDirection= null;

        private Vector3 muzzleDirection{
            get {
                if (originalMuzzleDirection == null) {
                    originalMuzzleDirection = _muzzleTransform.right + _muzzleTransform.up - _muzzleTransform.forward;
                }
                return originalMuzzleDirection.Value;
            }
        }

        private ModdedLightningType GetModdedOrbType
        {
            get
            {
                if (_weaponComponent) {
                    if (_weaponComponent.hasTeslaCoil) {
                        return ModdedLightningType.Tesla;
                    }
                    return _weaponComponent.teslaSkinDef.ZapLightningType;
                }

                return ModdedLightningType.Ukulele;
            }
        }

        public LightningOrb.LightningType GetBounceOrbType {
            get {
                if (_weaponComponent) {
                    return _weaponComponent.teslaSkinDef.ZapBounceLightningType;
                }

                return LightningOrb.LightningType.MageLightning;
            }
        }
        #endregion

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastTime);

            _weaponComponent = GetComponent<TeslaWeaponComponent>();

            _tracker = GetComponent<TeslaTrackerComponentZap>();

            _muzzleTransform = Modules.VRCompat.GetModelChildLocator(this).FindChild("MuzzleGauntlet");
            if (_muzzleTransform == null)
                _muzzleTransform = transform;

            StartAimMode(2);

            //todo: joe crosscompat
            //base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);

            //todo: ishandout causing old sniper animation issues
            PlayCrossfade("Gesture Right Arm, Override", "HandOut", 0.05f);
            GetModelAnimator().SetBool("isHandOut", true);

            if (isAuthority) {
                if (_tracker) {

                    TeslaTrackerComponentZap.RangeTier trackingDistance = _tracker.GetTrackingTargetDistance();

                    _targetHurtbox = _tracker.GetTrackingTarget();

                    switch (trackingDistance) {
                        default:
                        case TeslaTrackerComponentZap.RangeTier.FURTHEST:
                            totalOrbCasts = OrbCasts - 2 + Mathf.RoundToInt(skillsPlusCasts * 0.334f);
                            break;
                        case TeslaTrackerComponentZap.RangeTier.MIDDLE:
                            totalOrbCasts = OrbCasts - 1 + Mathf.RoundToInt(skillsPlusCasts * 0.667f);
                            break;
                        case TeslaTrackerComponentZap.RangeTier.CLOSEST:
                            totalOrbCasts = OrbCasts + skillsPlusCasts;
                            break;
                    }

                    _attackingTeammate = _tracker.GetIsTargetingTeammate();

                    onZapAuthority?.Invoke(trackingDistance == TeslaTrackerComponentZap.RangeTier.CLOSEST, _attackingTeammate);
                    if (_attackingTeammate) {
                        onZapAllyAuthority?.Invoke(_targetHurtbox);
                    }

                } else {
                    _targetHurtbox = createOrb().PickNextTarget(transform.position);
                    totalOrbCasts = OrbCasts;
                }
            }

            if (_attackingTeammate) {
                //end move early
                duration *= 0.7f;
            }

            _crit = RollCrit();
        }

        public override void OnExit() {
            base.OnExit();

            GetModelAnimator().SetBool("isHandOut", false);
        }

        private PseudoLightningOrb createOrb() {
            return new PseudoLightningOrb {
                origin = GetOrbOrigin,
                damageValue = DamageCoefficient * damageStat,
                isCrit = _crit,
                bouncesRemaining = 1,
                damageCoefficientPerBounce = BounceDamageMultplier,
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                bouncedObjects = _bouncedObjectsList,
                lightningType = GetBounceOrbType,
                moddedLightningType = GetModdedOrbType,
                damageColorIndex = DamageColorIndex.Default,
                range = BounceDistance,
                speed = -1,
                target = _targetHurtbox
            };
        } 

        protected override void OnCastEnter()
        {
            if (_targetHurtbox) {

                PlayZapSound();
            }
        }

        protected override void OnCastFixedUpdate()
        {
            if (_targetHurtbox) {

                while (_currentCasts < totalOrbCasts && fixedAge > nextCastTime) {
                    PlayZapAnimation();
                    if (NetworkServer.active) {
                        FireZap();
                    }
                    _currentCasts++;
                    base.characterBody.AddSpreadBloom(0.32f);
                }
            }
        }

        private void FireZap() {

            if (_attackingTeammate) {
                FireZapTeammate();
                return;
            }

            PseudoLightningOrb _lightningOrb = createOrb();

            _lightningOrb.AddModdedDamageType(Modules.DamageTypes.Conductive);
            OrbManager.instance.AddOrb(_lightningOrb);
            ////happens after firing each orb to apply to their bounces only
            //_lightningOrb.moddedLightningType = ModdedLightningType.MageLightning;
        }

        private void FireZapTeammate() {

            HarmlessBuffOrb orb = new HarmlessBuffOrb {
                buffToApply = Modules.Buffs.conductiveBuffTeam,
                target = _targetHurtbox,
                origin = GetOrbOrigin,
                moddedLightningType = GetModdedOrbType,
            };
            OrbManager.instance.AddOrb(orb);
            //cancel additional casts
            _currentCasts = totalOrbCasts;
        }

        //friendly fire scrapped
        private void ModifyTeamLightningOrb(LightningOrb lightningOrb) {
            if (_attackingTeammate) {
                //lightningOrb.range = BounceDistance * 2;
                //lightningOrb.damageCoefficientPerBounce = 3f;
                lightningOrb.bouncesRemaining = 0;

                lightningOrb.damageValue *= 0.1f;
                lightningOrb.procCoefficient = 0;
            }
        }

        private void PlayZapAnimation() {
            EffectManager.SimpleMuzzleFlash(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"),
                                            gameObject,
                                            "MuzzleGauntlet",
                                            true);

            PlayAnimation("Gesture, Additive", "Shock");
            //PlayCrossfade("Gesture, Override", "Shock", 0.1f);
        }

        private void PlayZapSound() {
            string sound = "Play_itesatta";
            if (_crit) sound = "Play_zap_crit";// "Play_trooper_itesat2b_tesla_trooper_attack";
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