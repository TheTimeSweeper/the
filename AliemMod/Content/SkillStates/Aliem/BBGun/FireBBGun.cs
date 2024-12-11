using AliemMod.Content.Orbs;
using EntityStates;
using RoR2.Orbs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.SendMouseEvents;
using System.ComponentModel;
using AliemMod.Content;

namespace ModdedEntityStates.Aliem
{
    public class FireBBGun : BaseSkillState, IOffHandable, IChannelingSkill
    {
        public float baseDamageCoefficient = AliemConfig.M1_BBGun_Damage.Value;
        public float maxSpread = AliemConfig.M1_BBGun_Spread.Value;
        public float spreadYawScale = AliemConfig.bbgunSpreadYaw.Value;
        public float range => AliemConfig.BBGunRange.Value;
        public float baseFireInterval = AliemConfig.M1_BBGun_Interval.Value * 0.02f;

        public int bsPerFrame = AliemConfig.bbgunBaseBs.Value;
        public string muzzleString => isOffHanded ? "BlasterMuzzle.R" : "BlasterMuzzle";

        public bool isOffHanded { get; set; }

        private Ray _currentRay;
        private bool _currentCrit;
        private float _updateRayInterval = 0.2f;

        private Transform _muzzleTransform;

        private float _fireTim = 0;
        private float _rayTim = 0;
        private float _interval;

        public void StopChanneling()
        {
            if(isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _muzzleTransform = FindModelChild(muzzleString);
            _interval = baseFireInterval * bsPerFrame / attackSpeedStat;

            PlayAnimation("Gesture, Override", "ShootLoop", "ShootGun.playbackRate", _interval * 58.82f);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootLoop");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootLoop");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _rayTim -= Time.deltaTime;
            _fireTim -= Time.deltaTime;

            if (_rayTim <= 0)
            {
                _rayTim += _updateRayInterval;
                onRayUpdate();
            }

            int bCount = 0;
            while (_fireTim <= 0)
            {
                bCount += bsPerFrame;
                _fireTim += _interval;
            }

            if (bCount > 0)
            {
                FireBs(bCount, _currentRay.direction);
                characterBody.OnSkillActivated(activatorSkillSlot);
            }
        }

        private void onRayUpdate()
        {
            _currentRay = GetAimRay();
            _currentCrit = RollCrit();
            _interval = baseFireInterval * bsPerFrame / characterBody.attackSpeed;
            StartAimMode();
        }

        public override void OnExit()
        {
            base.OnExit();

            PlayAnimation(0.5f);

        }

        public void PlayAnimation(float duration)
        {
            PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", duration);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGun");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGun");
        }

        private InputBankTest.ButtonState GetSkillButton()
        {
            return isOffHanded ? inputBank.skill2 : inputBank.skill1;
        }

        //stolen from bulletattack.fire
        private void FireBs(int BCount, Vector3 aimVector)
        {
            Util.PlaySound("Play_AliemBB", gameObject);

            Vector3 up = Vector3.up;
            Vector3 axis = Vector3.Cross(up, aimVector);
            int num = 0;

            for (int i = 0; i < BCount; i++)
            {
                float x = UnityEngine.Random.Range(0, maxSpread);
                float z = UnityEngine.Random.Range(0f, 360f);
                Vector3 vector = Quaternion.Euler(0f, 0f, z) * (Quaternion.Euler(x, 0f, 0f) * Vector3.forward);
                float y = vector.y;
                vector.y = 0f;
                float angle = (Mathf.Atan2(vector.z, vector.x) * 57.29578f - 90f) * spreadYawScale;
                float angle2 = Mathf.Atan2(y, vector.magnitude) * 57.29578f;
                FireSingleB(Quaternion.AngleAxis(angle, up) * (Quaternion.AngleAxis(angle2, axis) * aimVector));
            }
        }

        private void FireSingleB(Vector3 aimVector)
        {
            HurtBox hitTarget = BootlegCharacterRaycastSingle(gameObject, new Ray(_currentRay.origin, aimVector), out Vector3 hitPoint, range, BulletAttack.defaultStopperMask, QueryTriggerInteraction.UseGlobal);

            BBOrb orb = hitTarget ? AliemPoolManager.instance.RentBBOrb() : AliemPoolManager.instance.RentBBOrbMissed();

            orb.targetPosition = hitPoint;
            orb.speed = UnityEngine.Random.Range(AliemConfig.bbgunMinSpeed.Value, AliemConfig.bbgunMaxSpeed.Value);
            orb.attackOrigin = _currentRay.origin;
            orb.damageValue = damageStat * baseDamageCoefficient;
            orb.attacker = gameObject;
            orb.isCrit = _currentCrit;
            orb.origin = _muzzleTransform.position;
            orb.target = hitTarget;
            orb.damageTypeCombo = DamageTypeCombo.GenericPrimary;

            OrbManager.instance.AddOrb(orb);
        }

        private HurtBox BootlegCharacterRaycastSingle(GameObject bodyObject, Ray ray, out Vector3 hitPoint, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            RaycastHit[] hits = Physics.SphereCastAll(ray, AliemConfig.bbgunRadius.Value, maxDistance, layerMask, queryTriggerInteraction);
            return CopyPasteCodeJustToAvoidOneGetComponent(bodyObject, ray, maxDistance, hits, out hitPoint);
        }

        private HurtBox CopyPasteCodeJustToAvoidOneGetComponent(GameObject bodyObject, Ray ray, float maxDistance, RaycastHit[] hits, out Vector3 hitPoint)
        {
            int eligibleHit = -1;
            float iterDistance = float.PositiveInfinity;
            float maxHitDistance = 0;
            HurtBox checkedHurtbox = null;
            HurtBox finalHurtbox = null;
            for (int i = 0; i < hits.Length; i++)
            {
                float distance = hits[i].distance;
                if (distance > maxHitDistance)
                {
                    maxHitDistance = distance;
                }
                if (distance < iterDistance)
                {
                    checkedHurtbox = hits[i].collider.GetComponent<HurtBox>();
                    if ((bool)checkedHurtbox)
                    {
                        HealthComponent healthComponent = checkedHurtbox.healthComponent;
                        if (healthComponent)
                        {
                            if (!FriendlyFireManager.ShouldDirectHitProceed(healthComponent, teamComponent.teamIndex))
                            {
                                continue;
                            }
                            eligibleHit = i;
                            finalHurtbox = checkedHurtbox;
                            if (distance == 0f)
                            {
                                hitPoint = ray.origin;
                                return finalHurtbox;
                            }
                        }
                    }
                    iterDistance = distance;
                }
            }
            if (eligibleHit == -1)
            {
                maxDistance = maxHitDistance == 0 ? maxDistance : maxHitDistance;
                hitPoint = ray.origin + ray.direction * maxDistance;
                return null;
            }
            hitPoint = hits[eligibleHit].point;
            return finalHurtbox;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
