using RoR2;
using UnityEngine;

namespace RA2Mod.General.Components
{
    public class MenuSoundComponent : MonoBehaviour
    {
        public string sound = "";

        protected virtual void OnEnable()
        {
            Util.PlaySound(sound, gameObject);
        }
    }
}