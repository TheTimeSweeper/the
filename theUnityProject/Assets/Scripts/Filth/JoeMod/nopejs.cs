using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoeModForReal.Components.Projectile {
}

namespace FacelessJoe {
    public class nopejs : MonoBehaviour {
        [SerializeField]
        private Vector3 rotation = new Vector3(0, 1, 0);

        void FixedUpdate() {
            transform.Rotate(rotation);
        }
    }
}
