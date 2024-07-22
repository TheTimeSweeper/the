using AliemMod.Content;
using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;

namespace AliemMod.Components.Bundled
{
    public class PooledOrbEffect : MonoBehaviour
    {
        public EffectComponent effectComponent;
        public OrbEffect orbEffectComponent;
        public TrailRenderer trailRenderer;
        private bool _hasRunInitialStart = false;
        private Vector3 _trailLocalPosition;
        
        private bool _pendingRestart;

        void Start()
        {
            _hasRunInitialStart = true;
            _trailLocalPosition = trailRenderer.transform.localPosition;
        }

        public void OnRent(Vector3 origin, Quaternion rotation)
        {
            transform.position = origin;
            transform.rotation = rotation;

            trailRenderer.emitting = false;
            trailRenderer.transform.parent = transform;
            trailRenderer.transform.localPosition = _trailLocalPosition;

            gameObject.SetActive(true);
        }

        void FixedUpdate()
        {
            if (_pendingRestart)
            {
                trailRenderer.emitting = true;
                _pendingRestart = false;
                //close enough right?
                effectComponent.Start();
                orbEffectComponent.Start();
            }
        }

        public void OnReturn()
        {
            trailRenderer.transform.SetParent(AliemPoolManager.instance.instanceTransform, true);
            gameObject.SetActive(false);

            _pendingRestart = true;
            effectComponent.didResolveReferencedHurtBox = false;
            orbEffectComponent.age = 0;
        }

        void Update()
        {
            //cap age so orbeffect doesn't destroy it
            orbEffectComponent.age = Mathf.Min(orbEffectComponent.age, orbEffectComponent.duration * 0.99f);
            if (orbEffectComponent.age >= orbEffectComponent.duration * 0.98f)
            {
                AliemPoolManager.instance.returnOrbEffect(this);
            }
        }
    }
}
