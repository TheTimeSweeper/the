using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronosphereProjection : NetworkBehaviour
    {
        public bool overrideEnable;
        public float overrideValue;
        public float overrideValue2 = 0.2f;

        [SerializeField]
        public Renderer[] sphereRenderers;

        [SerializeField]
        private ObjectScaleCurve sphere;

        [SerializeField]
        private LoopSoundDef loopSoundDef;

        private MaterialPropertyBlock block = new MaterialPropertyBlock();
        private float endTimer = -1;

        private float fromValue;
        private float toValue;
        private float animateTime;
        private float delayTime;
        private bool destroyOnEnd;

        void Awake()
        {
            for (int i = 0; i < sphereRenderers.Length; i++)
            {
                sphereRenderers[i].GetPropertyBlock(block);
                float offset = Mathf.Lerp(fromValue, toValue, (endTimer - delayTime) / animateTime);
                block.SetVector("_MainTex_ST", new Vector4(1, 0.2f, 0, -0.38f));
                sphereRenderers[i].SetPropertyBlock(block);
            }
        }

        public void SetRadiusAndEnable(float radius)
        {
            sphere.transform.localScale = Vector3.one * radius * 2;
            //sphere.enabled = true;
            if (loopSoundDef)
            {
                Util.PlaySound(loopSoundDef.startSoundName, gameObject);
            }
        }

        public void AnimateShader(bool shouldShow, float delay = 0, float showTime = 1, bool destroyonEnd = false)
        {
            fromValue = shouldShow ? 0f : 0.6f;
            toValue = shouldShow ? 0.5f : 0.99f;
            animateTime = showTime;
            delayTime = delay; 
            destroyOnEnd = destroyonEnd;

            endTimer = 0;

            if (destroyonEnd && loopSoundDef)
            {
                Util.PlaySound(loopSoundDef.stopSoundName, gameObject);
            }
        }


        void Update()
        {
            if (overrideEnable)
            {
                for (int i = 0; i < sphereRenderers.Length; i++)
                {
                    sphereRenderers[i].GetPropertyBlock(block);
                    float offset = Mathf.Lerp(fromValue, toValue, (endTimer - delayTime) / animateTime);
                    block.SetVector("_MainTex_ST", new Vector4(1, overrideValue2, 0, overrideValue));
                    sphereRenderers[i].SetPropertyBlock(block);
                }
            }

            if (endTimer < 0)
                return;

            endTimer += Time.deltaTime;
            if (endTimer >= animateTime + delayTime)
            {
                if (destroyOnEnd)
                {
                    Destroy(gameObject); 
                } 
                else 
                {
                    endTimer = -1;
                }
                return;
            }

            for (int i = 0; i < sphereRenderers.Length; i++)
            {
                sphereRenderers[i].GetPropertyBlock(block);
                float offset = Mathf.Lerp(fromValue, toValue, (endTimer - delayTime) / animateTime);
                block.SetVector("_MainTex_ST", new Vector4(1, 0.2f, 0, offset) );
                sphereRenderers[i].SetPropertyBlock(block);
            }
        }
    }
}