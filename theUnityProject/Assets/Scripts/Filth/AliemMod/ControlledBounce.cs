using UnityEngine;

namespace AliemMod.Components.Bundled {
    public class ControlledBounce: MonoBehaviour {

        [SerializeField]
        private Rigidbody rigidBody;

        [SerializeField]
        private float yBounce;

        private Vector3 _lastVelocity;

        private void FixedUpdate() {
            _lastVelocity = rigidBody.velocity;
        }

        private void OnCollisionEnter(Collision collision) {

            rigidBody.velocity = Vector3.Reflect(_lastVelocity, Vector3.up);
        }
    }
}
