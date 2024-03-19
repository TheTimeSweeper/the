using EntityStates;
using RA2Mod.Survivors.Tesla;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Minions.TeslaTower.States
{

    public class TowerIdleSearch : BaseSkillState
    {
        public static float SearchRange = 60;
        public static float BaseZapInterval = 2;
        public static float SpawnedBaseZapInterval = 1;

        //private EntityStateMachine _weaponStateMachine;
        private LightningOrb _lightningOrb;
        private HurtBox _lightningTarget;

        private TeslaTrackerComponentZap ownerTrackerComponent;

        private float currentZapInterval;
        public bool justSpawned;

        private float _cooldownTimer;

        public override void OnEnter()
        {
            base.OnEnter();

            ownerTrackerComponent = GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent;

            //_weaponStateMachine = EntityStateMachine.FindByCustomName(gameObject, "weapon");

            _lightningOrb = new LightningOrb
            {
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                bouncedObjects = new List<HealthComponent>(),
                range = SearchRange,
                canBounceOnSameTarget = true,
            };

            _cooldownTimer = 0;
            currentZapInterval = (justSpawned ? SpawnedBaseZapInterval : BaseZapInterval) / attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= currentZapInterval)
            {
                SearchTarget();
            }
        }

        private void SearchTarget()
        {

            if (TeslaConfig.M4_Tower_Targeting.Value && ownerTrackerComponent)
            {
                _lightningTarget = ownerTrackerComponent.GetTowerTrackingTarget();
                ownerTrackerComponent.SetTowerLockedTarget(_lightningTarget?.healthComponent);
            }

            if (_lightningTarget == null)
            {
                _lightningTarget = _lightningOrb.PickNextTarget(transform.position);
            }

            if (_lightningTarget)
            {
                TowerZap towerZapState = GetNextStateType();
                towerZapState.lightningTarget = _lightningTarget;
                outer.SetNextState(towerZapState);
                _cooldownTimer = 0;
            }
        }

        protected virtual TowerZap GetNextStateType()
        {
            return new TowerZap();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

}
