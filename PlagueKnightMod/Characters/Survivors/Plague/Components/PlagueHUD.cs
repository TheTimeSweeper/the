using UnityEngine;
using RoR2.UI;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueHUD : MonoBehaviour
    {
        public HUD hud { get; set; }

        private PlagueBombSelectUI bombSelectUI;

        private PlagueBombSelectorController plagueBombSelectorController;
        private GameObject cachedBodyObject;

        void Awake()
        {
            hud = GetComponent<HUD>();
        }

        void Update()
        {
            if(hud.targetBodyObject)
            {
                if(cachedBodyObject == null || cachedBodyObject != hud.targetBodyObject)
                {
                    cachedBodyObject = hud.targetBodyObject;
                    plagueBombSelectorController = cachedBodyObject.GetComponent<PlagueBombSelectorController>();

                    if (plagueBombSelectorController)
                    {
                        if (bombSelectUI == null)
                        {
                            InitBombSelectUI();
                        }
                    }
                }

                if (plagueBombSelectorController && plagueBombSelectorController.initialized)
                {
                    plagueBombSelectorController.bombSelectUI = bombSelectUI;
                    bombSelectUI.UpdateGrids(plagueBombSelectorController.powderGenericSkills.genericSkills, plagueBombSelectorController.casingGenericSkills.genericSkills);
                }
            }
        }
        
        private void InitBombSelectUI()
        {
            bombSelectUI = GameObject.Instantiate<PlagueBombSelectUI>(PlagueAssets.bombSelectUI, hud.mainUIPanel.transform);
            bombSelectUI.transform.localPosition = Vector3.zero;
            bombSelectUI.plagueBombSelectorController = plagueBombSelectorController;
            bombSelectUI.Show(false);
        }
    }
}