using UnityEngine;
using UnityEngine.UI;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class CooldownTimerView : MonoBehaviour
    {
        [SerializeField]
        private Image filledImage;
        [SerializeField]
        private Transform handTransform;

        public void SetFill(float fill)
        {
            filledImage.fillAmount = fill;
            handTransform.localRotation = Quaternion.Euler(0, 0, -fill * 360f);
        }
    }
}
