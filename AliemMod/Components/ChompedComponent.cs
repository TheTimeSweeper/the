using R2API;
using RoR2;
using UnityEngine;

namespace AliemMod.Components
{
    public class ChompedComponent : MonoBehaviour, RoR2.IOnKilledServerReceiver {

        private CharacterBody body;
        private ChildLocator childLocator;
        private Transform head;
        private int headIndex;

        public void OnKilledServer(DamageReport damageReport) {
            if (!head)
                return;

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, Modules.DamageTypes.Decapitating)) {
                
                EffectData effectData = new EffectData {
                    rootObject = body.gameObject,
                    modelChildIndex = (short)headIndex
                };
                
                EffectManager.SpawnEffect(Modules.AliemAssets.bloodEffect, effectData, true);
                head.gameObject.AddComponent<ShrinkComponent>();
            }
        }

        internal void init(CharacterBody body_) {
            body = body_;

            childLocator = body_.modelLocator.modelTransform.GetComponent<ChildLocator>();
            if (childLocator) {
                head = childLocator.FindChild("Head");
                headIndex = childLocator.FindChildIndex("Head");
                if (head == null) {
                    head = childLocator.FindChild("head");
                    headIndex = childLocator.FindChildIndex("head");
                }
            }
        }
    }
}
