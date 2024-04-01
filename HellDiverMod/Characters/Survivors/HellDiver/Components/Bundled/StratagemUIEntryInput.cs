using System;
using UnityEngine;
using UnityEngine.UI;

namespace HellDiverMod.Survivors.HellDiver.Components.UI
{
    public class StratagemUIEntryInput : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private Sprite[] inputSprites;

        private Color _setColor;

        public void Reset()
        {
            _setColor = Color.gray;
        }

        public void Darken()
        {
            _setColor = Color.gray / 2;
        }

        public void Lighten()
        {
            _setColor = Color.white;
        }

        public void Hide()
        {
            _setColor = Color.clear;
        }

        void Update()
        {
            image.color = Color.Lerp(image.color, _setColor, 0.3f);
        }

        internal void InitArrow(StratagemInput stratagemInput)
        {
            image.sprite = inputSprites[(int)stratagemInput];
        }
    }
}
