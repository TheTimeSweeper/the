using EntityStates;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper.Tower {

    public class TowerIdleSearch : BaseSkillState {
        public static float SearchRange = 60;
        public static float BaseZapInterval = 2;
        public static float SpawnedBaseZapInterval = 1;

        //private EntityStateMachine _weaponStateMachine;
        private LightningOrb _lightningOrb;
        private HurtBox _lightningTarget;

        private TeslaTrackerComponent ownerTrackerComponent;

        private float currentZapInterval { get => (justSpawned ? SpawnedBaseZapInterval : BaseZapInterval) / attackSpeedStat; }
        public bool justSpawned;

        private float _cooldownTimer;
        
        public override void OnEnter() {
            base.OnEnter();

            ownerTrackerComponent = GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent;

            //_weaponStateMachine = EntityStateMachine.FindByCustomName(gameObject, "weapon");

            _lightningOrb = new LightningOrb {
                //origin = base.GetModelChildLocator().FindChild("Orb").position,
                //damageValue = damageStat,
                //isCrit = RollCrit(),
                //bouncesRemaining = 1,
                //damageCoefficientPerBounce = BounceDamageMultplier,
                //damageType = DamageType.SlowOnHit,
                //procCoefficient = 1f,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                bouncedObjects = new List<HealthComponent>(),
                //lightningType = LightningOrb.LightningType.Loader,
                //damageColorIndex = DamageColorIndex.Default,
                range = SearchRange,
                canBounceOnSameTarget = true,
                //speed = 1000
            };

            _cooldownTimer = 0;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (ownerTrackerComponent) {
                bool towerNear = Vector3.Distance(transform.position, ownerTrackerComponent.transform.position) < TeslaTowerControllerController.NearTowerRange;
                ownerTrackerComponent.SetTowerNear(towerNear);
            }

            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= currentZapInterval) {
                SearchTarget();
            }
        }

        private void SearchTarget() {

            if(ownerTrackerComponent) {
                _lightningTarget = ownerTrackerComponent.GetTowerTrackingTarget();
            }

            if (_lightningTarget == null) {
                _lightningTarget = _lightningOrb.PickNextTarget(transform.position);
            }

            if (_lightningTarget) {

                outer.SetNextState(new TowerZap {
                    lightningTarget = _lightningTarget,
                });
                _cooldownTimer = 0;
            }
        }

        public override void OnExit() {
            base.OnExit();
            ownerTrackerComponent?.SetTowerNear(false);
        }
    }

}
