using RoR2;
using UnityEngine;

namespace AliemMod.Components.Bundled
{
    public class PartialSkinDef : SkinDef
    {
        CharacterModel.RendererInfo[] rendererInfosBuffer;
        new public void Apply(GameObject characterModelObject)
        {
            Bake();

            CharacterModel characterModel = characterModelObject.GetComponent<CharacterModel>();
            if (characterModel == null)
            {
                Helpers.LogWarning("no charactermodel. partial skin apply failed");
                return;
            }

            rendererInfosBuffer = characterModel.baseRendererInfos;

            for (int i = 0; i < rendererInfosBuffer.Length; i++)
            {
                CharacterModel.RendererInfo rendererInfo = rendererInfosBuffer[i];
                for (int j = 0; j < runtimeSkin.rendererInfoTemplates.Length; j++)
                {
                    //Helpers.LogWarning($"{rendererInfo.renderer} {rendererInfos[j].renderer} matched {rendererInfo.renderer == rendererInfos[j].renderer}");
                    
                    if (Util.BuildPrefabTransformPath(characterModel.transform, rendererInfo.renderer.transform) ==
                        runtimeSkin.rendererInfoTemplates[j].path)
                    {
                        rendererInfosBuffer[i] = runtimeSkin.rendererInfoTemplates[j].data;
                        rendererInfosBuffer[i].renderer = characterModel.transform.Find(runtimeSkin.rendererInfoTemplates[j].path).GetComponent<Renderer>();
                    }
                }
            }

            //heh
            characterModel.gameObjectActivationTransforms.Clear();

            runtimeSkin.Apply(characterModelObject);

            characterModel.baseRendererInfos = rendererInfosBuffer;
        }
    }
}
