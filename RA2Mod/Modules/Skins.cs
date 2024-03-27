using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RA2Mod.Modules
{
    internal static class Skins
    {
        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] defaultRendererInfos, GameObject root, UnlockableDef unlockableDef = null)
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = new CharacterModel.RendererInfo[defaultRendererInfos.Length],
                RootObject = root,
                UnlockableDef = unlockableDef
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            defaultRendererInfos.CopyTo(skinDefInfo.RendererInfos, 0);
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            PopulateSkinDef(defaultRendererInfos, skinDefInfo, skinDef);

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        internal static SkinDef GetCurrentSkinDef(CharacterBody characterBody)
        {
            return SkinCatalog.GetBodySkinDef(characterBody.bodyIndex, (int)characterBody.skinIndex);
        }

        public static T CreateSkinDef<T>(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] defaultRendererInfos, GameObject root, UnlockableDef unlockableDef = null) where T : SkinDef
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = new CharacterModel.RendererInfo[defaultRendererInfos.Length],
                RootObject = root,
                UnlockableDef = unlockableDef
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            T skinDef = ScriptableObject.CreateInstance<T>();

            PopulateSkinDef(defaultRendererInfos, skinDefInfo, skinDef);

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void PopulateSkinDef(CharacterModel.RendererInfo[] rendererInfos, SkinDefInfo skinDefInfo, SkinDef skinDef)
        {
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            rendererInfos.CopyTo(skinDefInfo.RendererInfos, 0);
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;
        }

        /// <summary>
        /// creates a new skindef that has the original skindef in its baseSkins
        /// </summary>
        /// <param name="originalSkinDef"></param>
        /// <returns></returns>
        public static T DuplicateScepterSkinDef<T>(T originalSkinDef, string newName = "_SCEPTER") where T : SkinDef
        {
            //why do we need to do this again?
            On.RoR2.SkinDef.Awake += DoNothing;

            T newSkinDef = ScriptableObject.CreateInstance<T>();
            newSkinDef.baseSkins = new SkinDef[] { originalSkinDef };
            newSkinDef.icon = originalSkinDef.icon;
            newSkinDef.unlockableDef = originalSkinDef.unlockableDef;
            newSkinDef.rootObject = originalSkinDef.rootObject;
            newSkinDef.rendererInfos = originalSkinDef.rendererInfos;
            newSkinDef.gameObjectActivations = originalSkinDef.gameObjectActivations;
            newSkinDef.meshReplacements = originalSkinDef.meshReplacements;
            newSkinDef.projectileGhostReplacements = originalSkinDef.projectileGhostReplacements;
            newSkinDef.minionSkinReplacements = originalSkinDef.minionSkinReplacements;
            newSkinDef.nameToken = originalSkinDef.nameToken + newName;
            newSkinDef.name = originalSkinDef.name + newName;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return newSkinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }

        private static CharacterModel.RendererInfo[] getRendererMaterials(CharacterModel.RendererInfo[] defaultRenderers, params Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            for (int i = 0; i < newRendererInfos.Length; i++)
            {
                try
                {
                    newRendererInfos[i].defaultMaterial = materials[i];
                }
                catch
                {
                    Log.Error("error adding skin rendererinfo material. make sure you're not passing in too many");
                }
            }

            return newRendererInfos;
        }
        /// <summary>
        /// pass in strings for mesh assets in your bundle. pass the same amount and order based on your rendererinfos, filling with null as needed
        /// <code>
        /// myskindef.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
        ///    "meshHenrySword",
        ///    null,
        ///    "meshHenry");
        /// </code>
        /// </summary>
        /// <param name="assetBundle">your skindef's rendererinfos to access the renderers</param>
        /// <param name="defaultRendererInfos">your skindef's rendererinfos to access the renderers</param>
        /// <param name="meshes">name of the mesh assets in your project</param>
        /// <returns></returns>
        internal static SkinDef.MeshReplacement[] GetMeshReplacements(this AssetBundle assetBundle, CharacterModel.RendererInfo[] defaultRendererInfos, params string[] meshes)
        {

            List<SkinDef.MeshReplacement> meshReplacements = new List<SkinDef.MeshReplacement>();

            for (int i = 0; i < defaultRendererInfos.Length; i++)
            {
                if (string.IsNullOrEmpty(meshes[i]))
                    continue;

                meshReplacements.Add(
                new SkinDef.MeshReplacement
                {
                    renderer = defaultRendererInfos[i].renderer,
                    mesh = assetBundle.LoadAsset<Mesh>(meshes[i])
                });
            }

            return meshReplacements.ToArray();
        }

        /// <summary>
        /// Plug in the names of all the GameObjects that are going to be activated/deactivated in any of your skins, and store this in a variable
        /// </summary>
        /// <returns>An ordered list of gameobjects to activate/deactivate</returns>
        internal static List<GameObject> CreateAllActivatedGameObjectsList(ChildLocator childLocator, params string[] allChildren)
        {
            List<GameObject> allObjects = new List<GameObject>();

            for (int i = 0; i < allChildren.Length; i++)
            {
                allObjects.Add(childLocator.FindChildGameObject(allChildren[i]));
            }
            return allObjects;
        }

        /// <summary>
        /// Using the ActivatedGameObjects list, pass in the index of each gameobject to activate. Objects not passed in will deactivate.
        /// </summary>
        internal static SkinDef.GameObjectActivation[] GetGameObjectActivationsFromList(List<GameObject> allObjects, params int[] activatedChildren)
        {

            SkinDef.GameObjectActivation[] gameObjectActivations = new SkinDef.GameObjectActivation[allObjects.Count];

            for (int i = 0; i < allObjects.Count; i++)
            {
                gameObjectActivations[i] = new SkinDef.GameObjectActivation
                {
                    gameObject = allObjects[i],
                    shouldActivate = activatedChildren.Contains(i),
                };
            }

            return gameObjectActivations;
        }

        internal static Sprite CreateRecolorIcon(Color color)
        {
            var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);

            var fillColorArray = tex.GetPixels();
            for (int i = 0; i < fillColorArray.Length; i++)
            {
                fillColorArray[i] = color;
            }
            tex.SetPixels(fillColorArray);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
        }
    }
}