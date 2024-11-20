using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class MatcherViewController : MonoBehaviour
    {
        [System.Serializable]
        public class RendererRevealer
        {
            public Renderer renderer;
            private MaterialPropertyBlock _block;
            private float _timer;

            public void Init()
            {
                _block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(_block);
            }

            public void StartReveal(float time)
            {
                _timer = time;
                renderer.gameObject.SetActive(true);
            }

            public void Update()
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;

                    if(_timer < 0)
                    {
                        renderer.gameObject.SetActive(false);
                    }
                }                
            }
        }

        [SerializeField]
        private RendererRevealer swordRenderer;
        [SerializeField]
        private RendererRevealer staffRenderer;

        void Update()
        {
            swordRenderer.Update();
            staffRenderer.Update();
        }

        public void RevealSword(float time)
        {
            swordRenderer.StartReveal(time);
        }
        public void RevealStaff(float time) 
        {
            staffRenderer.StartReveal(time);
        }

    }
}
