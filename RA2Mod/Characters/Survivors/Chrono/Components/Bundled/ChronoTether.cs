using System.Collections;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoTether : MonoBehaviour
    {
        [SerializeField]
        private Transform tetherPoint;
        
        [SerializeField]
        private Renderer rend;

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        float endTimer = -1;

        void Awake()
        {
            rend.GetPropertyBlock(block);
        }

        public void SetTetherPoint(Vector3 position)
        {
            tetherPoint.position = position;
        }

        public void Dispose()
        {
            endTimer = 0;
        }

        void Update()
        {
            if (endTimer < 0)
                return;
            endTimer += Time.deltaTime;
            if (endTimer >= 0.5f)
            {
                Destroy(gameObject);
                return;
            }

            block.SetFloat("_alphaBoost", endTimer * 2);
            rend.SetPropertyBlock(block);
        }
    }
}
