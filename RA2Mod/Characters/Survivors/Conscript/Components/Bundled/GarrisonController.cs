using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RA2Mod.Survivors.Conscript.Components.Bundled
{
    class GarrisonController : MonoBehaviour
    {
        public void OnInteract (RoR2.Interactor interactor)
        {
            if(interactor.TryGetComponent(out RoR2.SkillLocator skillLocator)){
                skillLocator.ApplyAmmoPack();
            }
        }
    }
}
