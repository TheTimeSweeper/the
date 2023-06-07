using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ArrayTransferTool : MonoBehaviour {

    [SerializeField]
    private List<GameObject> objeys;

    [ContextMenu("send to rendererinfos")]
    public void sendRendererInfos() {

        CharacterModel charaModel = GetComponent<CharacterModel>();

        if (charaModel == null) {

            Debug.LogError("no charactermodel attached");
            return;
        }

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(charaModel, "grab rends");
#endif

        charaModel.baseRendererInfos = new CharacterModel.RendererInfo[objeys.Count];
        for (int i = 0; i < objeys.Count; i++) {

            Renderer rend = objeys[i].GetComponent<Renderer>();

            charaModel.baseRendererInfos[i] = new CharacterModel.RendererInfo {
                renderer = rend,
                defaultMaterial = rend.sharedMaterial,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
            };
        }

        objeys.Clear();
    }

    [ContextMenu("send to childLocator")]
    public void sendChildLocator() {

        ChildLocator childLocator = GetComponent<ChildLocator>();

        if (childLocator == null) {

            Debug.LogError("no ChildLocator attached");
            return;
        }

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(childLocator, "grab transes");
#endif

        int originalLength = childLocator.transformPairs.Length;
        Array.Resize(ref childLocator.transformPairs, childLocator.transformPairs.Length + objeys.Count);
        for (int i = 0; i < objeys.Count; i++) {

            childLocator.transformPairs[i + originalLength] = new ChildLocator.NameTransformPair {
                name = objeys[i].name,
                transform = objeys[i].transform,
            };
        }

        objeys.Clear();
    }

    [ContextMenu("dynamic boner")]
    public void sendToDynamicBones() {

        List<DynamicBone> dynamicBones = GetComponents<DynamicBone>().ToList();
        if(dynamicBones.Count <= 0) {
            Debug.LogWarning("No DynamicBone script to make copies of. Adding new.");
            dynamicBones.Add(gameObject.AddComponent<DynamicBone>());
        }

        Queue<GameObject> objeysQueue = new Queue<GameObject>(objeys);

        GameObject rootObject;
        int iter = 0;
        while (objeysQueue.Count > 0) {

            rootObject = objeysQueue.Dequeue();

            if (dynamicBones.Count <= iter) {
                dynamicBones.Add(gameObject.AddComponent<DynamicBone>());
#if UNITY_EDITOR
                Undo.RegisterCreatedObjectUndo(dynamicBones[iter], "grab transes");
#endif
            }

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(dynamicBones[iter], "grab transes");
            UnityEditorInternal.ComponentUtility.CopyComponent(dynamicBones[0]);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(dynamicBones[iter]);
#endif
            dynamicBones[iter].m_Root = rootObject.transform;
            iter++;
        }
    }

}
