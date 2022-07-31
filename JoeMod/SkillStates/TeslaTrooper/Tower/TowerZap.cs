using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;                                 
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper.Tower {

    public class TeamZap : TowerZap {

    }

    public class TeamZapCharge : TowerZap {

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
                inflictor = gameObject,
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
            PlayAnimation("weapon", "PrepZap", "prep.playbackRate", base.duration);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (lightningTarget == null && !hasFired) {
                TeslaTrackerComponent tracker = GetComponent<TeslaTrackerComponent>();
                if (tracker) {
                    lightningTarget = tracker.GetTrackingTarget();
                }
                if (lightningTarget == null) {
                    lightningTarget = lightningOrb.PickNextTarget(transform.position);
                }
                if (lightningTarget == null) {
                    base.outer.SetNextStateToMain();
                } else {
                    lightningOrb.target = lightningTarget;
                }
            }
        }

        //cast time is 1
        protected override void OnCastEnter() {

            if (!NetworkServer.active)
                return;

            string sound = ZapSound;
            if (lightningOrb.isCrit) sound = ZapSoundCrit;
            Util.PlaySound(sound, gameObject);

            if (lightningTarget == null)
                return;

            fireOrb();
        }

        protected virtual void fireOrb() {
            //todo: custom lightningorb
            for (int i = 0; i < 3; i++) {

                OrbManager.instance.AddOrb(lightningOrb);

                //Helpers.LogWarning("orb attacker " + lightningOrb.attacker);
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (!hasFired) {
                fireOrb();
                hasFired = true;
            }

            if (Modules.Config.TowerTargeting.Value) {
                GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent?.SetTowerLockedTarget(null);
            }
        }

        // Token: 0x0600419A RID: 16794 RVA: 0x0002F86B File Offset: 0x0002DA6B
        public override void OnSerialize(NetworkWriter writer) {

            writer.Write(HurtBoxReference.FromHurtBox(lightningTarget));
        }

        // Token: 0x0600419B RID: 16795 RVA: 0x0010A8CC File Offset: 0x00108ACC
        public override void OnDeserialize(NetworkReader reader) {

            this.lightningTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }

}
