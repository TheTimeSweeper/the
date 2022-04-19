using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;                                 
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper.Tower {

    public class TeamZap : TowerZap {

    }

    public class TeamCharge : TowerZap {

    }

    public class TowerZap: BaseTimedSkillState
    {
        public static float DamageCoefficient = 2.3f;
        public static float ProcCoefficient = 1f;
        
        public static float BaseDuration = 1f;
        public static float BaseStartCastTime = 1;

        public LightningOrb lightningOrb;
        public HurtBox lightningTarget;

        public static string PrepSound = "Play_tower_btespow_tesla_tower_prep";
        public static string ZapSound = "Play_tower_btesat1a_tesla_tower_attack";
        public static string ZapSoundCrit = "Play_tower_btesat2a_tesla_tower_attack";

        protected string zapSound = ZapSound;
        protected string zapSoundCrit = ZapSoundCrit;

        public override void OnEnter()
        {
            base.OnEnter();
            Helpers.LogWarning(this.GetType().ToString() + " onenter");
            // is this redundant cause the cast time is the end and I could just do an onexit kinda thing?
            InitDurationValues(BaseDuration, BaseStartCastTime);

            bool tesla = GetComponent<TowerWeaponComponent>().hasTeslaCoil;

            lightningOrb = new LightningOrb {
                origin = base.GetModelChildLocator().FindChild("Orb").position,
                damageValue = DamageCoefficient * damageStat,
                isCrit = RollCrit(),
                //bouncesRemaining = 1,
                //damageCoefficientPerBounce = BounceDamageMultplier,
                //damageType = DamageType.SlowOnHit,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                bouncedObjects = new List<HealthComponent>(),
                lightningType = tesla? LightningOrb.LightningType.Tesla : LightningOrb.LightningType.Loader,
                damageColorIndex = DamageColorIndex.Default,
                //range = SearchRange,
                canBounceOnSameTarget = true,
                speed = 1000
            };

            lightningOrb.target = lightningTarget;
            
            Util.PlaySound(PrepSound, gameObject);
            PlayCrossfade("weapon", "PrepZap", "prep.playbackRate", base.duration, 0.1f);
            //playanimation or however I'm going to do the glow going up the pole
                //and the orb glowing
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (lightningTarget == null && !hasFired) {
                TeslaTrackerComponent tracker = GetComponent<TeslaTrackerComponent>();
                if (tracker) {
                    lightningTarget = tracker.GetTrackingTarget();
                }
                if(lightningTarget == null) {
                    base.outer.SetNextStateToMain();
                }
            }
        }

        protected override void OnCastEnter() {

            if (!NetworkServer.active)
                return;

            string sound = ZapSound;
            if (lightningOrb.isCrit) sound = ZapSoundCrit;
            Util.PlaySound(sound, gameObject);

            if (lightningTarget == null)
                return;

            //todo: custom lightningorb
            fireOrb();
        }

        protected virtual void fireOrb() {

            //todo: custom lightningorb
            for (int i = 0; i < 3; i++) {

                OrbManager.instance.AddOrb(lightningOrb);
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (!hasFired) {
                fireOrb();
                hasFired = true;
            }

            GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent?.SetTowerLockedTarget(null);
        }
    }

}
