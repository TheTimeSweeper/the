using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using RA2Mod.Survivors.Tesla.Orbs;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Tesla;

namespace RA2Mod.Minions.TeslaTower.States
{

    public class TowerZap : BaseTimedSkillState
    {

        public override float TimedBaseDuration => GetBaseDuration();

        public override float TimedBaseCastStartPercentTime => 1;

        public static float DamageCoefficient = 2.3f;
        public static float ProcCoefficient = 1f;

        public static float BaseDuration = 1f;

        public LightningOrb lightningOrb;
        public HurtBox lightningTarget;
        public int zaps = 3;

        public static string PrepSound = "Play_tower_btespow_tesla_tower_prep";
        public static string ZapSound = "Play_tower_btesat1a_tesla_tower_attack";
        public static string ZapSoundCrit = "Play_tower_btesat1a_tesla_tower_attack";//"Play_tower_btesat2a_tesla_tower_attack"; taken for alt m2

        protected string zapSound = ZapSound;
        protected string zapSoundCrit = ZapSoundCrit;
        protected bool crit;

        private TowerWeaponComponent _towerWeaponComponent;
        private Transform _originTransform;

        public ModdedLightningType GetOrbType
        {
            get
            {

                if (_towerWeaponComponent)
                {
                    if (_towerWeaponComponent.hasTeslaCoil)
                        return ModdedLightningType.Tesla;

                    return _towerWeaponComponent.towerSkinDef.ZapLightningType;
                }

                return ModdedLightningType.Loader;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _towerWeaponComponent = GetComponent<TowerWeaponComponent>();

            _originTransform = FindModelChild("Orb");
            //if (_originTransform == null) {
            //    _originTransform = base.FindModelChild("Muzzle");
            //}
            //if (_originTransform == null) {
            //    _originTransform = base.FindModelChild("muzzle");
            //}
            if (_originTransform == null)
            {
                _originTransform = transform;
            }

            crit = RollCrit();
            lightningOrb = new PseudoLightningOrb
            {
                origin = _originTransform.position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = crit,
                //bouncesRemaining = 1,
                //damageCoefficientPerBounce = BounceDamageMultplier,
                //damageType = DamageType.SlowOnHit,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                inflictor = gameObject,
                procCoefficient = 1f,
                bouncedObjects = new List<HealthComponent>(),
                moddedLightningType = GetOrbType,
                damageColorIndex = DamageColorIndex.Default,
                range = TowerIdleSearch.SearchRange,
                canBounceOnSameTarget = true,
                speed = -1
            };

            lightningOrb.target = lightningTarget;

            PlayPrep();
        }

        protected virtual float GetBaseDuration()
        {
            return BaseDuration;
        }

        protected virtual void PlayPrep()
        {
            Util.PlaySound(PrepSound, gameObject);
            PlayCrossfade("weapon", "PrepZap", "prep.playbackRate", duration, 0.1f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (lightningTarget == null && !hasFired)
            {
                //attaching a tracker to the tower for playing as the tower
                //or for future proper ai
                TeslaTrackerComponentZap tracker = GetComponent<TeslaTrackerComponentZap>();
                if (tracker)
                {
                    lightningTarget = tracker.GetTrackingTarget();
                }
                if (lightningTarget == null)
                {
                    lightningTarget = lightningOrb.PickNextTarget(transform.position);
                }
                if (lightningTarget == null)
                {
                    outer.SetNextStateToMain();
                }
                else
                {
                    lightningOrb.target = lightningTarget;
                }
            }
        }

        //cast time is 1
        protected override void OnCastEnter()
        {

            ModifySound();

            Util.PlaySound(lightningOrb.isCrit ? zapSoundCrit : zapSound, gameObject);

            if (!NetworkServer.active)
                return;

            if (lightningTarget == null)
                return;

            fireOrb();
        }

        protected virtual void ModifySound() { }

        protected virtual void fireOrb()
        {
            //todo: custom lightningorb
            for (int i = 0; i < zaps; i++)
            {
                lightningOrb.origin = _originTransform.position;
                OrbManager.instance.AddOrb(lightningOrb);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!hasFired && fixedAge > duration * 0.5f)
            {
                OnCastEnter();
                hasFired = true;
            }

            if (TeslaConfig.M4_Tower_Targeting.Value)
            {
                GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent?.SetTowerLockedTarget(null);
            }
        }

        // Token: 0x0600419A RID: 16794 RVA: 0x0002F86B File Offset: 0x0002DA6B
        public override void OnSerialize(NetworkWriter writer)
        {

            writer.Write(HurtBoxReference.FromHurtBox(lightningTarget));
        }

        // Token: 0x0600419B RID: 16795 RVA: 0x0010A8CC File Offset: 0x00108ACC
        public override void OnDeserialize(NetworkReader reader)
        {

            lightningTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }

}
