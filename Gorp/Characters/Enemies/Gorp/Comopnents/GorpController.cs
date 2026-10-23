using EntityStates;
using EntityStates.Gup;
using GorpMod.Enemies.Gorp.Content;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GorpMod.Characters.Enemies.Gorp.Comopnents
{
    public class GorpController : NetworkBehaviour, IOnTakeDamageServerReceiver
    {
        private Transform _armatureTransform;
        private CharacterBody _body;

        private int Thresholds => CharacterConfig.HealthThresholds;

        private int currentThreshold;

        private float monchInterval => CharacterConfig.MonchInterval;
        private float _monchTimer;

        private float startHealth;
        private float currentHealthFraction => _body.healthComponent.combinedHealth / startHealth;

        void Start()
        {
            _armatureTransform = GetComponent<ModelLocator>().modelTransform.Find("mdlGupArmature");
            _body = GetComponent<CharacterBody>();
            startHealth = _body.healthComponent.fullCombinedHealth;
            if (NetworkServer.active)
            {
                HealthComponent.onCharacterHealServer += HealthComponent_onCharacterHealServer;
                CheckCurrentHealthThresholdServer();
            }
            UpdateScaleInternal(Thresholds - 1);
        }

        void OnDestroy()
        {
            HealthComponent.onCharacterHealServer -= HealthComponent_onCharacterHealServer;
        }

        private void HealthComponent_onCharacterHealServer(HealthComponent arg1, float arg2, ProcChainMask arg3)
        {
            if (arg1 != _body.healthComponent)
                return;

            CheckCurrentHealthThresholdServer();

            RpcUpdateScale(currentThreshold);
        }

        public void OnTakeDamageServer(DamageReport damageReport)
        {
            CheckCurrentHealthThresholdServer();

            RpcUpdateScale(currentThreshold);
        }

        private void CheckCurrentHealthThresholdServer()
        {
            float lastThreshold = currentThreshold;
            while ((currentHealthFraction) > ((float)(currentThreshold + 1) / Thresholds))
            {
                currentThreshold++;
            }
            while ((currentHealthFraction) < ((float)(currentThreshold) / Thresholds))
            {
                currentThreshold--;
            }

            if (currentThreshold != lastThreshold)
            {
                if (NetworkServer.active)
                {
                    UpdateThresholdServer();
                }
            }
        }

        private void UpdateThresholdServer()
        {
            Log.Warning("reached threshold " + currentThreshold);
            int buffCount = _body.GetBuffCount(CharacterBuffs.GorpThresholdBuff);
            while (buffCount < currentThreshold)
            {
                buffCount++;
                _body.AddBuff(CharacterBuffs.GorpThresholdBuff);
            }
            while (buffCount > currentThreshold)
            {
                buffCount--;
                _body.RemoveBuff(CharacterBuffs.GorpThresholdBuff);
            }

            //boostcharacterscale version
            //problem: shrinks too slowly, I wanted it sudden
            //problem?: mortar and death gips inherit final scale
            //int startingBoosts = _body.inventory.GetItemCountEffective(DLC4Content.Items.BoostCharacterScale);
            //int startingReduces = _body.inventory.GetItemCountEffective(DLC4Content.Items.ReduceCharacterScale);
            //int desiredboosts;
            //int desiredReduces;
            //if (finalScale > 1)
            //{
            //    desiredboosts = Mathf.RoundToInt((finalScale * 100) - 100);
            //    desiredReduces = 0;
            //} 
            //else if (finalScale == 1)
            //{
            //    desiredboosts = 0;
            //    desiredReduces = 0;
            //}
            //else
            //{
            //    desiredReduces = Mathf.RoundToInt(100 - (finalScale * 100));
            //    desiredboosts = 0;
            //}

            //SyncItemCount(_body.inventory, DLC4Content.Items.BoostCharacterScale, startingBoosts, desiredboosts);
            //SyncItemCount(_body.inventory, DLC4Content.Items.ReduceCharacterScale, startingReduces, desiredReduces);
        }

        [ClientRpc]
        private void RpcUpdateScale(int currentThreshold)
        {
            UpdateScaleInternal(currentThreshold);
        }

        private void UpdateScaleInternal(int currentThreshold)
        {
            float t = (float)currentThreshold / Thresholds;

            //square root any growth past 1 such that it is harder to get out of control
            if(t > 1)
            {
                t = Mathf.Sqrt(t);
            }

            float finalScale = Mathf.LerpUnclamped(CharacterConfig.Size_MinHealth, CharacterConfig.Size_MaxHealth, t);
            _armatureTransform.localScale = Vector3.one * finalScale;
        }

        public static void SyncItemCount(Inventory inventory, ItemDef itemDef, int currentCount, int desiredCount)
        {

            if(currentCount < desiredCount)
            {
                inventory.GiveItemPermanent(itemDef, desiredCount - currentCount);
            }

            if(currentCount > desiredCount)
            {
                inventory.RemoveItemPermanent(itemDef, currentCount - desiredCount);
            }
        }

        void FixedUpdate()
        {
            if (!NetworkServer.active)
                return;

            if (!_body.healthComponent.alive)
                return;

            _monchTimer += Time.deltaTime;
            if (_monchTimer <= monchInterval)
            {
                return;
            }
            _monchTimer = 0;

            var bodies = CharacterBody.readOnlyInstancesList;
            for (int i = bodies.Count - 1; i >= 0; i--)
            {
                if (!GorpCharacter.IsGup(bodies[i]) || bodies[i] == _body)
                    continue;

                float monchDistance = Mathf.LerpUnclamped(CharacterConfig.MonchDistance_MinHealth, CharacterConfig.MonchDistance_MaxHealth, currentThreshold / Thresholds);
                if (Vector3.SqrMagnitude(bodies[i].transform.position - transform.position) < monchDistance * monchDistance)
                {
                    float healthBeforeMonch = _body.healthComponent.combinedHealth;
                    float healing = bodies[i].healthComponent.health;

                    if(healthBeforeMonch + healing > startHealth * CharacterConfig.MaxAbsorbtionSize)
                    {
                        CheckCurrentHealthThresholdServer();

                        RpcUpdateScale(currentThreshold);
                        continue;
                    }
                    float remainder = _body.healthComponent.combinedHealth + healing - _body.healthComponent.fullCombinedHealth;
                    _body.healthComponent.Heal(healing, default);

                    //boosthp gives you 10% starting max health
                    _body.inventory.GiveItemPermanent(RoR2Content.Items.BoostHp, Mathf.RoundToInt(remainder / (startHealth * 0.1f)));

                    bodies[i].GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(GipDeath));
                    bodies[i].master.TrueKill();

                    CheckCurrentHealthThresholdServer();

                    RpcUpdateScale(currentThreshold);
                }
            }
        }
    }
}
