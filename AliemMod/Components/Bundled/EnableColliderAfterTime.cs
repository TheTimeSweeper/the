using UnityEngine;

namespace AliemMod.Components.Bundled
{
    [DefaultExecutionOrder(10)]
    public class EnableColliderAfterTime : MonoBehaviour
    {
        [SerializeField]
        private float time = 0.5f;

        [SerializeField]
        private Collider collider;

        [SerializeField]
        private bool shouldEnable;

        private bool _done;

        void FixedUpdate()
        {
            time -= Time.fixedDeltaTime;
            if (!_done && time <= 0)
            {
                _done = true;
                collider.enabled = shouldEnable;
            }
        }

        void Start()
        {
            collider.enabled = !shouldEnable;
        }
    }
}
