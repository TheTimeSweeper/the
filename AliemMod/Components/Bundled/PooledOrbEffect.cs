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

        void Start()
        {
            _hasRunInitialStart = true;
            _trailLocalPosition = trailRenderer.transform.position;
        }

        void OnEnable()
        {
            if (_hasRunInitialStart)
            {
                //close enough right?
                effectComponent.Start();
                orbEffectComponent.Start();
                orbEffectComponent.age = 0;
            }
        }

        void Update()
        {
            //cap age so orbeffect doesn't destroy it
            orbEffectComponent.age = Mathf.Min(orbEffectComponent.age, orbEffectComponent.duration * 0.99f);
            if (orbEffectComponent.age == 0.99f)
            {
                AliemPoolManager.instance.returnOrbEffect(this);
            }
        }

        public void OnRent(EffectData effectData)
        {
            trailRenderer.emitting = false;
            trailRenderer.transform.SetParent(transform, false);
            trailRenderer.transform.localPosition = _trailLocalPosition;
            //todo pool transform.position, rotation = effectdata
            gameObject.SetActive(true);
            trailRenderer.emitting = true;
        }

        public void OnReturn()
        {
            trailRenderer.transform.SetParent(AliemPoolManager.instance.parent);
            gameObject.SetActive(false);
        }
    }
}
