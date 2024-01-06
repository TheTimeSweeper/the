using UnityEngine;

public class FuckinMaterialComponent : MonoBehaviour {
    Renderer[] rends;

    public float strenght;
    public float exponent;

    MaterialPropertyBlock matblock;
    void Awake() {
        matblock = new MaterialPropertyBlock();
        rends = GetComponentsInChildren<Renderer>();

    }

    void Update() {
        if (Input.GetKey(KeyCode.G)) {
            for (int i = 0; i < rends.Length; i++) {
                Renderer rend = rends[i];

                rend.GetPropertyBlock(matblock);
                matblock.SetFloat("_SpecularStrength", strenght);
                matblock.SetFloat("_SpecularExponent", exponent);
                rend.SetPropertyBlock(matblock);
            }
        }
    }
}
