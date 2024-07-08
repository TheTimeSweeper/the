using AliemMod.Components.Bundled;
using AliemMod.Content.Orbs;
using AliemMod.Modules;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AliemMod.Content
{
    public class AliemPoolManager
    {
        public static AliemPoolManager instance { get; private set; }

        private Queue<BBOrb> _bbOrbPool = new Queue<BBOrb>();
        private Queue<BBOrbMissed> _bbOrbMissedPool = new Queue<BBOrbMissed>();
        private Queue<PooledOrbEffect> _orbEffectPool = new Queue<PooledOrbEffect>();
        private List<PooledOrbEffect> _activeOrbEffects = new List<PooledOrbEffect>();

        public Transform parent;

        public AliemPoolManager(Transform parent) {
            instance = this;
            this.parent = parent;
            Hook();
        }

        private void Hook()
        {
            IL.RoR2.EffectManager.SpawnEffect_EffectIndex_EffectData_bool += EffectManager_SpawnEffect_EffectIndex_EffectData_bool;
        }

        private void EffectManager_SpawnEffect_EffectIndex_EffectData_bool(MonoMod.Cil.ILContext il)
        {
            //todo IL 00CD if effectdef.prefab == Assets.BBOrbEffect return aliempoolmanager.instance.rentorbeffect
        }

        #region orbs
        public BBOrb RentBBOrb()
        {
            if(_bbOrbPool.Count > 0)
            {
                return _bbOrbPool.Dequeue();
            }
            return new BBOrb();
        }

        public void ReturnBBOrb(BBOrb bbOrb)
        {
            _bbOrbPool.Enqueue(bbOrb);
        }

        public BBOrbMissed RentBBOrbMissed()
        {
            if (_bbOrbMissedPool.Count > 0)
            {
                return _bbOrbMissedPool.Dequeue();
            }
            return new BBOrbMissed();
        }

        public void ReturnBBOrbMissed(BBOrbMissed bbOrbMissed)
        {
            _bbOrbMissedPool.Enqueue(bbOrbMissed);
        }
        #endregion orbs

        #region here we goo

        public EffectComponent RentOrbEffect(EffectData effectData)
        {
            PooledOrbEffect pooledEffect;
            if (_orbEffectPool.Count > 0)
            {
                pooledEffect = _orbEffectPool.Dequeue();
            }
            else
            {
                pooledEffect = UnityEngine.Object.Instantiate(Assets.BBOrbEffect).GetComponent<PooledOrbEffect>();
            }
            pooledEffect.OnRent(effectData);
            return pooledEffect.effectComponent;
        }

        public void returnOrbEffect(PooledOrbEffect pooledEffect)
        {
            pooledEffect.OnReturn();
            _orbEffectPool.Enqueue(pooledEffect);
        }

        #endregion here we goo
    }
}
