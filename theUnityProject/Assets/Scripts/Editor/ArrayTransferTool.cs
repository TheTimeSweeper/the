using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTransferTool : MonoBehaviour
{

    [SerializeField]
    private Renderer[] objeys;

    [ContextMenu("send")]
    public void sendObjects() {

        CharacterModel charaModel = GetComponent<CharacterModel>();

        if (charaModel == null) {

            Debug.LogError("no charactermodel attached");
            return;
        }

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(charaModel, "grab rends");
#endif

        charaModel.baseRendererInfos = new CharacterModel.RendererInfo[objeys.Length];
        for (int i = 0; i < objeys.Length; i++) {
            charaModel.baseRendererInfos[i] = new CharacterModel.RendererInfo {
                renderer = objeys[i],
                defaultMaterial = objeys[i].sharedMaterial,
            };
        }

        objeys = new Renderer[0];
    }

}
