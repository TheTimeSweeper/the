using RoR2;
using UnityEngine;

namespace JoeModForReal.Content.Survivors {
    public class UltimateBuildup : MonoBehaviour, IOnDamageDealtServerReceiver {

        public delegate void DamageForUltimateEvent(float damage);
        public DamageForUltimateEvent OnDamageForUltimate;

        public CharacterBody body;

        public void Awake() {
            body = GetComponent<CharacterBody>();
        }

        public void OnDamageDealtServer(DamageReport damageReport) {
            trackDamage(damageReport.damageDealt);
        }

        public void trackDamage(float damage) {
            if (body.damage == 0)
                return;
            OnDamageForUltimate?.Invoke(damage / body.damage);
        }
    } 
}