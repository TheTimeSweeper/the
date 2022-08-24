using JoeMod;
using R2API;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class BlinkZap : EntityStates.Huntress.BlinkState {

        #region Gameplay Values
        public static float DamageCoefficient = 5f;
        public static float ProcCoefficient = 1f;
        #endregion

        private TeslaTrackerComponent _tracker;
        private HurtBox _targetHurtbox;
        private CameraTargetParams.CameraParamsOverrideHandle _cameraOverrideHandle;

        public override void OnEnter() {

            _tracker = GetComponent<TeslaTrackerComponent>();
            _targetHurtbox = _tracker?.GetTrackingTarget();
            beginSoundString = "";
            endSoundString = "";
            speedCoefficient = 25f;

            base.OnEnter();

            characterMotor.Motor.ForceUnground();

            if (_targetHurtbox) {

                float distance = Vector3.Distance(_targetHurtbox.transform.position, transform.position);
                duration = distance * 0.9f / (speedCoefficient * moveSpeedStat);

                _cameraOverrideHandle = cameraTargetParams.AddParamsOverride(new CameraTargetParams.CameraParamsOverrideRequest {
                    cameraParamsData = GetBlinkCameraParams(distance*0.5f),
                    priority = 0.1f
                }, 0);

                FireLightningOrb();
            }

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, duration + 1.5f);
            }
        }
        public override Vector3 GetBlinkVector() {
            if (!_targetHurtbox) {
                return base.GetBlinkVector();
            }

            return Vector3.Normalize(_targetHurtbox.transform.position - transform.position);
        }

        public override void OnExit() {
            base.OnExit();

            cameraTargetParams.RemoveParamsOverride(_cameraOverrideHandle, 0.1f);
        }

        private CharacterCameraParamsData GetBlinkCameraParams(float distance) {

            CharacterCameraParamsData blinkParams = cameraTargetParams.cameraParams.data;

            blinkParams.idealLocalCameraPos.value = blinkParams.idealLocalCameraPos.value + new Vector3(0f, 1.5f, -distance);
            return blinkParams;
        }

        private void FireLightningOrb() {

            if (!_tracker.GetIsTargetingTeammate()) {

                PseudoLightningOrb orb = new PseudoLightningOrb {
                    origin = transform.position,
                    damageValue = DamageCoefficient * damageStat,
                    isCrit = base.RollCrit(),
                    bouncesRemaining = 0,
                    //damageCoefficientPerBounce = BounceDamageMultplier,
                    damageType = DamageType.Generic,
                    teamIndex = teamComponent.teamIndex,
                    attacker = gameObject,
                    procCoefficient = 1f,
                    bouncedObjects = new List<HealthComponent>(),
                    //lightningType = LightningOrb.LightningType.MageLightning,
                    moddedLightningType = ModdedLightningType.Ukulele,
                    damageColorIndex = DamageColorIndex.Default,
                    //range = BounceDistance,
                    speed = speedCoefficient * moveSpeedStat,
                    target = _targetHurtbox
                };

                DamageAPI.AddModdedDamageType(orb, Modules.DamageTypes.consumeConductive);
                DamageAPI.AddModdedDamageType(orb, Modules.DamageTypes.applyBlinkCooldown);

                OrbManager.instance.AddOrb(orb);
            }

            HarmlessBuffOrb orb2 = new HarmlessBuffOrb {
                target = _targetHurtbox,
                origin = transform.position,
                speed = speedCoefficient * moveSpeedStat
            };
            for (int i = 0; i < 2; i++) {
                OrbManager.instance.AddOrb(orb2);
            }
        }
    }
}