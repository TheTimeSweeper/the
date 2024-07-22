using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace AliemMod.Content.Orbs
{
    public class BBOrbMissed : BBOrb, IOrbFixedUpdateBehavior
    {
        private BulletAttack _backupBulletAttack;
        private bool _bulletHit;

        private Vector3 _aimVectorSegment;
        private float _bulletDistance;
        private float _tim;
        private float _interval;
        private float _attempts;

        private const int SEGMENTS = 3;

        public override void Begin()
        {
            base.Begin();
            _aimVectorSegment = (targetPosition - attackOrigin) / SEGMENTS;
            _bulletDistance = _aimVectorSegment.magnitude;
            _interval = duration / SEGMENTS;
            _tim = _interval;
        }

        public void FixedUpdate()
        {
            _tim -= Time.fixedDeltaTime;

            if (_tim <= 0 && _attempts < SEGMENTS)
            {
                _tim = _interval;
                FireNextBullet();
                _attempts++;
            }
        }

        public override void OnArrival()
        {
            base.OnArrival();
            if(_attempts < SEGMENTS)
            {
                FireNextBullet();
            }
        }

        private void FireNextBullet()
        {
            Firebullet(attackOrigin + _aimVectorSegment * _attempts, attackOrigin + _aimVectorSegment * (_attempts + 1));
        }

        private void Firebullet(Vector3 bulletOrigin, Vector3 end)
        {
            if (_bulletHit)
                return;

            //todo move to state and do authority
            //if (_backupBulletAttack == null)
            //{
            /*_backupBulleAttack = */
            new BulletAttack
            {
                bulletCount = 1,
                aimVector = _aimVectorSegment,
                origin = bulletOrigin,
                damage = damageValue,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = _bulletDistance,
                force = 0,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = 0,
                maxSpread = 0,
                isCrit = isCrit,
                owner = attacker,
                smartCollision = false,
                procChainMask = default,
                procCoefficient = procCoefficient,
                radius = 0.3f,
                tracerEffectPrefab = null,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = null,
                hitCallback = hitCallback
            }.Fire();
            //}

            //_backupBulletAttack.Fire();
        }

        private bool hitCallback(BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo)
        {
            _bulletHit = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            return _bulletHit;
        }

        protected override void ReturnToPool()
        {
            _attempts = 0;
            AliemPoolManager.instance.ReturnBBOrbMissed(this);
        }
    }
}
