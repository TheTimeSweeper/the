using AliemMod.Content.Orbs;
using EntityStates;
using RoR2.Orbs;
using ModdedEntityStates.Aliem;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.SendMouseEvents;
using System.ComponentModel;

namespace AliemMod.Content.SkillStates.Aliem.BBGun
{
    internal class FireBBGun : BaseSkillState, IOffHandable
    {
        public float baseDamageCoefficient = AliemConfig.M1_BBGun_Damage.Value;
        public float baseProcCoefficient = AliemConfig.M1_BBGun_ProcCoefficient.Value;
        public float maxSpread = AliemConfig.M1_BBGun_Spread.Value;
        public float spreadYawScale = AliemConfig.bbgunSpreadYaw.Value;
        public float range => AliemConfig.BBGunRange.Value;
        public float fireInterval = AliemConfig.M1_BBGun_Interval.Value * 0.02f;

        public int bsPerFrame = AliemConfig.bbgunBaseBs.Value;
        public string muzzleString => isOffHanded? "BlasterMuzzle.R" : "BlasterMuzzle";

        public bool isOffHanded { get; set; }

        private Ray _currentRay;
        private bool _currentCrit;
        private float _updateRayInterval = 0.1f;

        private Transform _muzzleTransform;

        private float _fireTim = 0;
        private float _rayTim = 0;
        private float _interval;

        public override void OnEnter()
        {
            base.OnEnter();
            _muzzleTransform = FindModelChild(muzzleString);
            _interval = fireInterval * bsPerFrame;

            base.PlayAnimation("Gesture, Override", "ShootLoop");
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootLoop");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootLoop");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(_rayTim <= 0)
            {
                _rayTim += _updateRayInterval;
                _currentRay = GetAimRay();
                _currentCrit = base.RollCrit();
            }

            int bCount = 0;
            while(_fireTim <= 0)
            {
                bCount += bsPerFrame;
                _fireTim += _interval;
            }

            if(bCount > 0)
            {
                FireBs(bCount, _currentRay.direction);
            }

            _rayTim -= Time.fixedDeltaTime;
            _fireTim -= Time.fixedDeltaTime;

            if(!GetSkillButton().down && isAuthority){
                base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            PlayAnimation(0.5f);

        }

        public void PlayAnimation(float duration)
        {
            base.PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", duration);
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
            Vector3[] randomDirections = new Vector3[BCount];
            Vector3 up = Vector3.up;
            Vector3 axis = Vector3.Cross(up, aimVector);
            int num = 0;

            while ((long)num < (long)((ulong)this.bsPerFrame))
            {
                float x = UnityEngine.Random.Range(0, this.maxSpread);
                float z = UnityEngine.Random.Range(0f, 360f);
                Vector3 vector = Quaternion.Euler(0f, 0f, z) * (Quaternion.Euler(x, 0f, 0f) * Vector3.forward);
                float y = vector.y;
                vector.y = 0f;
                float angle = (Mathf.Atan2(vector.z, vector.x) * 57.29578f - 90f) * this.spreadYawScale;
                float angle2 = Mathf.Atan2(y, vector.magnitude) * 57.29578f;
                randomDirections[num] = Quaternion.AngleAxis(angle, up) * (Quaternion.AngleAxis(angle2, axis) * aimVector);
                num++;
            }

            for (int i = 0; i < BCount; i++)
            {
                FireSingleB(randomDirections[i]);
            }
        }

        private void FireSingleB(Vector3 aimVector)
        {
            HurtBox hitTarget = BootlegCharacterRaycastSingle(gameObject, new Ray(_currentRay.origin, aimVector), out RaycastHit hitInfo, range, BulletAttack.defaultStopperMask, QueryTriggerInteraction.UseGlobal);

            OrbManager.instance.AddOrb(new BBOrb()
            {
                targetPosition = hitInfo.point,
                speed = UnityEngine.Random.Range(AliemConfig.bbgunMinSpeed.Value, AliemConfig.bbgunMaxSpeed.Value),
                attackOrigin = _currentRay.origin,
                damageValue = damageStat * baseDamageCoefficient,
                attacker = gameObject,
                isCrit = _currentCrit,
                procChainMask = default(ProcChainMask),
                procCoefficient = baseProcCoefficient,
                origin = _muzzleTransform.position,
                target = hitTarget
            });
        }

        private HurtBox BootlegCharacterRaycastSingle(GameObject bodyObject, Ray ray, out RaycastHit hitInfo, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            RaycastHit[] hits = Physics.SphereCastAll(ray, AliemConfig.bbgunRadius.Value, maxDistance, layerMask, queryTriggerInteraction);
            return CopyPasteCodeJustToAvoidOneGetComponent(bodyObject, ray, maxDistance, hits, out hitInfo);
        }

        private HurtBox CopyPasteCodeJustToAvoidOneGetComponent(GameObject bodyObject, Ray ray, float maxDistance, RaycastHit[] hits, out RaycastHit hitInfo)
        {
            int eligibleHit = -1;
            float iterDistance = float.PositiveInfinity;
            HurtBox hurtBox = null;
            for (int i = 0; i < hits.Length; i++)
            {
                float distance = hits[i].distance;
                if (distance < iterDistance)
                {
                    hurtBox = (hits[i].collider).GetComponent<HurtBox>();
                    if ((bool)hurtBox)
                    {
                        HealthComponent healthComponent = hurtBox.healthComponent;
                        if ((bool)healthComponent && healthComponent.gameObject == bodyObject)
                        {
                            continue;
                        }
                    }
                    hitInfo = hits[i];
                    if (distance == 0f)
                    {
                        hitInfo.point = ray.origin;
                        return hurtBox;
                    }
                    eligibleHit = i;
                    iterDistance = distance;
                }
            }
            if (eligibleHit == -1)
            {
                hitInfo = new RaycastHit { point = ray.origin + ray.direction* maxDistance};
                return null;
            }
            hitInfo = hits[eligibleHit];
            return hurtBox;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
