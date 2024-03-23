using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.General.Components {
    [System.Serializable]
    public class Recolor {

        public string recolorName;

        public Color[] colors = new Color[] { Color.red };
    }

    [System.Serializable]
    public class RecolorGroup {

        [SerializeField]
        private Renderer[] renderers;

        [SerializeField]
        private bool emission;

        private MaterialPropertyBlock[] _matProperties;

        public void fillPropertyBlocks() {

            _matProperties = new MaterialPropertyBlock[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) {
                _matProperties[i] = new MaterialPropertyBlock();
                renderers[i].GetPropertyBlock(_matProperties[i]);
            }
        }

        public void setColors(Color color) {
#if UNITY_EDITOR
            fillPropertyBlocks();
#endif
            for (int i = 0; i < _matProperties.Length; i++) {
                _matProperties[i].SetColor("_Color", color);

                if (emission)
                    _matProperties[i].SetColor("_EmissionColor", color);

                renderers[i].SetPropertyBlock(_matProperties[i]);
            }
        }
    }

    public class SkinRecolorController : MonoBehaviour {

        [SerializeField]
        private RecolorGroup[] recolorGroups;

        [Space, SerializeField]
        private Recolor[] recolors;

        public int currentColor { get; private set; } = -1;

        void Awake() {

            for (int i = 0; i < recolorGroups.Length; i++) {
                recolorGroups[i].fillPropertyBlocks();
            }

#if UNITY_EDITOR

            SetRecolor(recolorInt);

#endif
        }

        public void SetRecolor(Color[] colors) {

            for (int i = 0; i < recolorGroups.Length; i++) {

                if (i >= colors.Length) {
                    continue;
                }

                recolorGroups[i].setColors(colors[i]);
            }
        }

        public void SetRecolor(int i) {

            if (recolors[i].colors.Length < recolorGroups.Length) {
                Debug.LogError("not enough colors for this recolor");
            }
            SetRecolor(recolors[i].colors);
            currentColor = i;
        }

        public void SetRecolor(string name) {

            for (int i = 0; i < recolors.Length; i++) {

                if (recolors[i].recolorName == name) {
                    SetRecolor(i);
                    return;
                }
            }
        }

#if UNITY_EDITOR

        [Header("Editor"), SerializeField]
        private int recolorInt = 0;
        [SerializeField]
        private string recolorString = "cyan";

        [Space, SerializeField]
        private bool updateColor = false;

        [SerializeField]
        private Color[] testColors;

        [ContextMenu("SetColors")]
        private void Start() {
            SetRecolor(recolorInt);
        }
        private void Update() {
            if (updateColor) {
                SetRecolor(testColors);
            }
        }

        [ContextMenu("Cycle Colors Int")]
        private void CycleColorsInt() {
            recolorInt++;
            if (recolorInt >= recolors.Length)
                recolorInt = 0;
            SetRecolor(recolorInt);
        }

        [ContextMenu("SetRecolor int")]
        private void SetRecolorInt() {
            SetRecolor(recolorInt);
        }

        [ContextMenu("SetRecolor string")]
        private void SetRecolorString() {
            SetRecolor(recolorString);
        }


#endif
    }
}