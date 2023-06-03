using RoR2.Projectile;
using UnityEngine;


namespace JoeModForReal.Components.Projectile {
    [RequireComponent(typeof(ProjectileDamage))]
    public class ProjectileDamageFalloff : MonoBehaviour {

        [SerializeField]
        private ProjectileDamage projectileDamage;

        [SerializeField]
        private float delayBeforeFalloff =  0;

        [SerializeField]
        private float falloffTime = 5;

        [SerializeField]
        private AnimationCurve falloffModel;

        private float? _initialProjectileDamage = null;
        private float _timer;

        void Reset() {
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        void FixedUpdate() {

            if (_timer >= delayBeforeFalloff) {

                if (_initialProjectileDamage == null) {
                    _initialProjectileDamage = projectileDamage.damage;
                }

                projectileDamage.damage = _initialProjectileDamage.Value * falloffModel.Evaluate((_timer - delayBeforeFalloff) / falloffTime);
            }

            _timer += Time.fixedDeltaTime;
        }
    }
}


