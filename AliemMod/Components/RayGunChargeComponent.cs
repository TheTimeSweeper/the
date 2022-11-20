using RoR2;
using UnityEngine;

namespace AliemMod.Components {
    public class RayGunChargeComponent: MonoBehaviour {

        public static float maxCharge = 1;
        public static float minCharge = 0.2f;

        private float charge = minCharge;
        private CharacterBody characterBody;

        void Start() {
            characterBody = GetComponent<CharacterBody>();
        }

        void FixedUpdate() {
            charge += Time.fixedDeltaTime * characterBody.attackSpeed;
        }

        public float RedeemCharge() {

            float redeemedCharge = Mathf.Min(charge, maxCharge);
            charge = minCharge;

            Helpers.LogWarning("redeeming " + redeemedCharge);
            return redeemedCharge;
        }
    }
}
