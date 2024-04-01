using RoR2.UI;
using UnityEngine;

namespace HellDiverMod.General.Components.UI
{
    public abstract class CompoanionHUDManager<TComponent, TUI> : MonoBehaviour where TComponent : IHasCompanionUI<TUI> where TUI : ICompanionUI<TComponent>
    {
        public HUD hud { get; set; }

        protected abstract GameObject UIPrefab { get; }
        protected abstract string transformPath { get; }

        private TUI companionUI;

        private TComponent companionComponent;
        private GameObject cachedBodyObject;

        void Awake()
        {
            hud = GetComponent<HUD>();
        }

        void Update()
        {
            if (hud.targetBodyObject)
            {
                if (cachedBodyObject == null || cachedBodyObject != hud.targetBodyObject)
                {
                    cachedBodyObject = hud.targetBodyObject;
                    companionComponent = cachedBodyObject.GetComponent<TComponent>();

                    if (companionComponent != null)
                    {
                        if (companionUI == null)
                        {
                            InitCompanionUI();
                        }
                    }
                }

                if (companionComponent != null && companionComponent.allowUIUpdate)
                {
                    companionComponent.CompanionUI = companionUI;
                    companionUI.OnUIUpdate();
                }
            }
        }

        private void InitCompanionUI()
        {
            GameObject gob = Instantiate(UIPrefab, hud.transform.Find(transformPath));
            gob.transform.localPosition = Vector3.zero;
            companionUI = gob.GetComponent<TUI>();
            companionUI.OnInitialize(companionComponent);
        }
    }
}
