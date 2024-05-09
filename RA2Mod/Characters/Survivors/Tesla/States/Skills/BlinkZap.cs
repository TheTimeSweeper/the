using EntityStates;
using R2API;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RA2Mod.Survivors.Tesla.Orbs;
using RA2Mod.General.States;

namespace RA2Mod.Survivors.Tesla.States
{
    public class BlinkZap : EntityStates.Huntress.BlinkState
    {
        #region Gameplay Values
        public static float DamageCoefficient = 3f;
        public static float ProcCoefficient = 1f;
        #endregion

        private TeslaWeaponComponent _weaponComponent;
        private TeslaTrackerComponentDash _tracker;
        private HurtBox _targetHurtbox;
        private CameraTargetParams.CameraParamsOverrideHandle _cameraOverrideHandle;

        private ModdedLightningType GetModdedOrbType
        {
            get
            {
                if (_weaponComponent)
                {
                    if (_weaponComponent.hasTeslaCoil)
                    {
                        return ModdedLightningType.Tesla;
                    }
                    return _weaponComponent.teslaSkinDef.ZapLightningType;
                }

                return ModdedLightningType.Ukulele;
            }
        }

        public override void OnEnter()
        {
            _weaponComponent = GetComponent<TeslaWeaponComponent>();

            _tracker = GetComponent<TeslaTrackerComponentDash>();
            _targetHurtbox = _tracker?.GetTrackingTarget();
            beginSoundString = "";
            endSoundString = "";
            speedCoefficient = 25f;

            base.OnEnter();

            characterMotor.Motor.ForceUnground();

            if (_targetHurtbox)
            {

                float distance = Vector3.Distance(_targetHurtbox.transform.position, transform.position);
                duration = distance * 0.8f / (speedCoefficient * moveSpeedStat);
                duration = Mathf.Min(duration, 1);

                _cameraOverrideHandle = cameraTargetParams.AddParamsOverride(new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = GetBlinkCameraParams(distance * 0.5f),
                    priority = 0.1f
                }, 0);

                FireLightningOrb();
            }

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, duration + 1.0f);
            }

            Util.PlaySound("Play_trooper_blink", gameObject);
        }
        public override Vector3 GetBlinkVector()
        {
            if (!_targetHurtbox)
            {
                return base.GetBlinkVector();
            }

            return Vector3.Normalize(_targetHurtbox.transform.position - transform.position);
        }

        public override void OnExit()
        {
            base.OnExit();

            cameraTargetParams.RemoveParamsOverride(_cameraOverrideHandle, 0.1f);

            WindDownState windDownState = new WindDownState();
            windDownState.windDownTime = 0.2f;
            EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetInterruptState(windDownState, InterruptPriority.Any);
        }

        private CharacterCameraParamsData GetBlinkCameraParams(float distance)
        {
            CharacterCameraParamsData blinkParams = cameraTargetParams.cameraParams.data;

            blinkParams.idealLocalCameraPos.value = blinkParams.idealLocalCameraPos.value + new Vector3(0f, 1.5f, -distance);
            return blinkParams;
        }

        private void FireLightningOrb()
        {
            if (!_tracker.GetIsTargetingTeammate())
            {
                PseudoLightningOrb orb = new PseudoLightningOrb
                {
                    origin = transform.position,
                    damageValue = DamageCoefficient * damageStat,
                    isCrit = RollCrit(),
                    bouncesRemaining = 0,
                    //damageCoefficientPerBounce = BounceDamageMultplier,
                    damageType = DamageType.Stun1s,
                    teamIndex = teamComponent.teamIndex,
                    attacker = gameObject,
                    procCoefficient = 1f,
                    bouncedObjects = new List<HealthComponent>(),
                    //lightningType = LightningOrb.LightningType.MageLightning,
                    moddedLightningType = GetModdedOrbType,
                    damageColorIndex = DamageColorIndex.Default,
                    //range = BounceDistance,
                    speed = speedCoefficient * moveSpeedStat,
                    target = _targetHurtbox
                };

                //DamageAPI.AddModdedDamageType(orb, Modules.DamageTypes.ShockMed);

                OrbManager.instance.AddOrb(orb);
            }

            HarmlessBlinkCooldownOrb orb2 = new HarmlessBlinkCooldownOrb
            {
                target = _targetHurtbox,
                origin = transform.position,
                ownerGameObject = gameObject,
                speed = speedCoefficient * moveSpeedStat,
                moddedLightningType = GetModdedOrbType,
            };
            for (int i = 0; i < 2; i++)
            {
                OrbManager.instance.AddOrb(orb2);
            }
        }
    }
}