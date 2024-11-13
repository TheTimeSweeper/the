using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Networking;

namespace MatcherMod.Modules.UI
{
    public abstract class CompanionHUDManager<TComponent, TUI> : MonoBehaviour where TComponent : IHasCompanionUI<TUI> where TUI : ICompanionUI<TComponent>
    {
        public HUD hud { get; set; }

        public abstract GameObject UIPrefab { get; }
        public abstract bool LocalUserOnly { get; }
        /// <summary>
        /// starts at HUDSimple(Clone). first child is MainContainer
        /// </summary>
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
                    if(companionUI != null)
                    {
                        companionUI.OnBodyLost();
                        companionUI = default;
                    }

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

                if (companionUI != null && companionComponent != null && companionComponent.allowUIUpdate)
                {
                    companionComponent.CompanionUI = companionUI;
                    companionUI.OnUIUpdate();
                }
            }
        }

        protected virtual void InitCompanionUI()
        {
            if (LocalUserOnly)
            {
                PlayerCharacterMasterController controller = hud.targetMaster.GetComponent<PlayerCharacterMasterController>();
                if (controller == null)
                    return;
                NetworkIdentity identity = controller.networkUserObject.GetComponent<NetworkIdentity>();
                if (identity == null)
                    return;
                if (!identity.isLocalPlayer)
                    return;
            }

            GameObject gob = Instantiate(UIPrefab, hud.transform.Find(transformPath));
            gob.transform.localPosition = Vector3.zero;
            companionUI = gob.GetComponent<TUI>();
            companionUI.OnInitialize(companionComponent, hud);
        }
    }
}
