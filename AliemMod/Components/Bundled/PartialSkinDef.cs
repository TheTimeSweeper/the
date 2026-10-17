using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AliemMod.Components.Bundled
{
    public class PartialSkinDef : SkinDef
    {
        CharacterModel.RendererInfo[] rendererInfosBuffer;
        new public void Apply(GameObject characterModelObject)
        {
            //not registered in SkinCatalog, so it doesn't get migrated to SkinDefParams for us
            if (skinDefParams == null && !skinDefParamsAddress.RuntimeKeyIsValid())
            {
                skinDefParams = SkinDefParams.FromSkinDef(this);
            }

            Bake();
            if (runtimeSkin == null)
            {
                Helpers.LogWarning($"{name} failed to bake. partial skin apply failed");
                return;
            }

            CharacterModel characterModel = characterModelObject.GetComponent<CharacterModel>();
            if (characterModel == null)
            {
                Helpers.LogWarning("no charactermodel. partial skin apply failed");
                return;
            }

            rendererInfosBuffer = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            for (int i = 0; i < characterModel.baseRendererInfos.Length; i++)
            {
                rendererInfosBuffer[i] = characterModel.baseRendererInfos[i];
            }

            for (int i = 0; i < rendererInfosBuffer.Length; i++)
            {
                CharacterModel.RendererInfo rendererInfo = rendererInfosBuffer[i];
                for (int j = 0; j < runtimeSkin.rendererInfoTemplates.Length; j++)
                {
                    //Helpers.LogWarning($"{rendererInfo.renderer} {rendererInfos[j].renderer} matched {rendererInfo.renderer == rendererInfos[j].renderer}");
                    
                    if (Util.BuildPrefabTransformPath(characterModel.transform, rendererInfo.renderer.transform) ==
                        runtimeSkin.rendererInfoTemplates[j].transformPath)
                    {
                        rendererInfosBuffer[i] = runtimeSkin.rendererInfoTemplates[j].data;
                        rendererInfosBuffer[i].renderer = characterModel.transform.Find(runtimeSkin.rendererInfoTemplates[j].transformPath).GetComponent<Renderer>();
                    }
                }
            }

            //heh
            characterModel.gameObjectActivationTransforms.Clear();

            //SkinDef.Apply passes null loadedMeshes and RuntimeSkin.ApplyAsync NREs on any mesh replacement, so drive it ourselves
            //all our references are direct so this finishes without actually waiting on anything
            IEnumerator apply = runtimeSkin.ApplyAsync(characterModelObject, new List<AssetReferenceT<Material>>(), new List<AssetReferenceT<Mesh>>(), new List<AssetReferenceT<GameObject>>());
            while (apply.MoveNext()) { }

            characterModel.baseRendererInfos = rendererInfosBuffer;
        }
    }
}
