using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RA2Mod.Survivors.Chrono.Components {

    public class CooldownTimerView : MonoBehaviour {
        [SerializeField]
        private Image filledImage;
        [SerializeField]
        private Transform handTransform;

        public void SetFill(float fill) {
            filledImage.fillAmount = fill;
            handTransform.localRotation = Quaternion.Euler(0, 0, -fill * 360f);
        }

        private void Update() {
            float amoutn = filledImage.fillAmount + Time.deltaTime;
            if (amoutn > 1)
                amoutn -= 1;
            SetFill(amoutn);
        }
    }
}