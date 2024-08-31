using AliemMod.Components.Bundled;
using AliemMod.Content.Orbs;
using AliemMod.Modules;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
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

        public Transform instanceTransform;

        public AliemPoolManager(Transform parent) {
            instance = this;
            instanceTransform = parent;
            Hook();
        }

        private void Hook()
        {
            if (AliemConfig.M1_BBGun_VFXPooled.Value)
            {
                IL.RoR2.EffectManager.SpawnEffect_EffectIndex_EffectData_bool += EffectManager_SpawnEffect_EffectIndex_EffectData_bool;
            }
        }

        private void EffectManager_SpawnEffect_EffectIndex_EffectData_bool(MonoMod.Cil.ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
                
             cursor.GotoNext(MoveType.Before,
                 Instruction => Instruction.MatchCall<UnityEngine.Object>("Instantiate")
                 );
            cursor.RemoveRange(2);
            cursor.EmitDelegate<Func<GameObject, Vector3, Quaternion, EffectComponent>>((gob, origin, rotation) =>
            {
                if (gob == AliemAssets.BBOrbEffect)
                {
                    return instance.RentOrbEffect(origin, rotation);
                }

                return UnityEngine.Object.Instantiate(gob, origin, rotation).GetComponent<EffectComponent>();
            });

            //ight I'ma just try remove
            //cursor.GotoNext(MoveType.After,
            //    Instruction => Instruction.MatchCallvirt<EffectDef>("get_prefab"),
            //    Instruction => Instruction.MatchLdloc(3)
            //    );

            //Helpers.LogWarning(cursor);
            //ILLabel afterGetPrefabLabel = cursor.MarkLabel();

            //cursor.Index = 0;
            //cursor.GotoNext(MoveType.Before, Instruction => Instruction.MatchCallvirt<GameObject>("GetComponent"));
            //ILLabel getComponentLabel = cursor.MarkLabel();

            //cursor.Index = 0;

            //cursor.GotoNext(MoveType.After,
            //    Instruction => Instruction.MatchCallvirt<EffectDef>("get_prefab")
            //    );
            //cursor.EmitDelegate<Func<GameObject, bool>>((gob) => { 
            //    return gob == Assets.BBOrbEffect; 
            //});
            //cursor.Emit(OpCodes.Brtrue, afterGetPrefabLabel);
            //cursor.Emit(OpCodes.Ldloc_3);
            //cursor.EmitDelegate<Func<EffectData, EffectComponent>>((effectData) => {
            //    return AliemPoolManager.instance.RentOrbEffect(effectData);
            //});
            //cursor.Emit(OpCodes.Br, getComponentLabel);

            //Helpers.LogWarning(cursor);
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

        public EffectComponent RentOrbEffect(Vector3 origin, Quaternion rotation)
        {
            PooledOrbEffect pooledEffect;
            if (_orbEffectPool.Count > 0)
            {
                pooledEffect = _orbEffectPool.Dequeue();
            }
            else
            {
                pooledEffect = UnityEngine.Object.Instantiate(AliemAssets.BBOrbEffect).GetComponent<PooledOrbEffect>();
                pooledEffect.transform.parent = instanceTransform;
            }
            pooledEffect.OnRent(origin, rotation);
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
