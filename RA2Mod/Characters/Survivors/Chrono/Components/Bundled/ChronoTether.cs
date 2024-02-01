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

        [SerializeField]
        private float disposeTime;

        [SerializeField]
        private bool disposeImmediately;

        private MaterialPropertyBlock block = new MaterialPropertyBlock();

        private float endTimer = -1;
        private float startAlpha;

        void Awake()
        {
            startAlpha = rend.material.GetFloat("_AlphaBoost");
            if (disposeImmediately)
                Dispose();
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
            if (endTimer >= disposeTime)
            {
                Destroy(gameObject);
                return;
            }

            rend.GetPropertyBlock(block);
            block.SetFloat("_AlphaBoost", Mathf.Lerp(startAlpha, 0, endTimer / disposeTime));
            rend.SetPropertyBlock(block);
        }
    }
}
