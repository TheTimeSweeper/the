using GorpMod.Survivors.Gorp.Content;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorpMod.Characters.Enemies.Gorp.Comopnents
{
    public class GorpController : MonoBehaviour, IOnTakeDamageServerReceiver
    {
        private Transform _mdlTransform;
        private CharacterBody _body;

        private float Thresholds = CharacterConfig.HealthThresholds;

        private float lastThresholdReached;

        void Start()
        {
            _mdlTransform = GetComponent<ModelLocator>().modelTransform;
            _body = GetComponent<CharacterBody>();
        }

        public void OnTakeDamageServer(DamageReport damageReport)
        {
            float lastLastThersholdReached = lastThresholdReached;
            Log.Warning("hit");
            Log.Warning((1 - _body.healthComponent.combinedHealthFraction));
            Log.Warning((lastThresholdReached + 1) / Thresholds);
            while ((1 - _body.healthComponent.combinedHealthFraction) > ((lastThresholdReached + 1) / Thresholds))
            {
                Log.Warning("reached");
                lastThresholdReached++;
            }

            if (lastThresholdReached != lastLastThersholdReached)
            {
                UpdateThreshold();
            }
        }

        private void UpdateThreshold()
        {
            int buffCount = _body.GetBuffCount(CharacterBuffs.GorpHealthBuff);
            while (buffCount < lastThresholdReached)
            {
                buffCount++;
                _body.AddBuff(CharacterBuffs.GorpHealthBuff);
            }

            _mdlTransform.localScale = Vector3.one * Mathf.Lerp(CharacterConfig.Size_MaxHealth, CharacterConfig.Size_MinHealth, (lastThresholdReached / Thresholds));
        }
    }
}
