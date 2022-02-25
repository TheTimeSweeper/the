using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recolor {

    public string recolorName;

    public Color mainColor = Color.red;

    public Color offColor = Color.red/2;
}

    [System.Serializable]
public class RecolorGroup {

    [SerializeField]
    private Renderer[] renderers;

    private MaterialPropertyBlock[] _matProperties;

    public void fillPropertieBlocks() {

        _matProperties = new MaterialPropertyBlock[renderers.Length];
        for (int i = 0; i < renderers.Length; i++) {
            _matProperties[i] = new MaterialPropertyBlock();
            renderers[i].GetPropertyBlock(_matProperties[i]);
        }
    }

    public void setColors(Color color) {

        for (int i = 0; i < _matProperties.Length; i++) {
            _matProperties[i].SetColor("_Color", color);
            renderers[i].SetPropertyBlock(_matProperties[i]);
        }
    }
}

public class SkinRecolorController : MonoBehaviour { 

    [SerializeField]
    private RecolorGroup mainRecolor;

    [SerializeField]
    private RecolorGroup offRecolor;

    [Space, SerializeField]
    private Recolor[] recolors;

    void Awake() {
        mainRecolor.fillPropertieBlocks();
        offRecolor.fillPropertieBlocks();
    }

    public void SetRecolor(Color mainColor) => SetRecolor(mainColor, mainColor / 2);
    public void SetRecolor (Color mainColor, Color offColor) {
        if (offColor == Color.black)
            offColor = mainColor / 2;

        mainRecolor.setColors(mainColor);
        offRecolor.setColors(offColor);
    }

    public void SetRecolor(int i) {
        SetRecolor(recolors[i].mainColor, recolors[i].offColor);
    }

    public void SetRecolor(string name) {

        for (int i = 0; i < recolors.Length; i++) {

            if(recolors[i].recolorName == name) {
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
    private Color mainColor = Color.white;
    [SerializeField]
    private Color offColor = Color.white;

    [ContextMenu("SetColors")]
    private void Start() {
        SetRecolor(0);
    }
    private void Update() {
        if (updateColor) {
            SetRecolor(mainColor, offColor);
        }
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
