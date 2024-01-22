using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class MidPointBetweenManyTransforms : MonoBehaviour
    {
        [SerializeField]
        private Transform[] transforms;

        private float inverseLength;

        void Awake()
        {
            inverseLength = 1f / (transforms.Length - 1);
        }

        void Update()
        {
            for (int i = 1; i < transforms.Length - 1; i++)
            {
                transforms[i].position = Vector3.Lerp(this.transforms[0].position, this.transforms[transforms.Length - 1].position, i * inverseLength);
            }
        }
    }
}
