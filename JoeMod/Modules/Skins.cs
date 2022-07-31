using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules
{
    internal static class Skins
    {
        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, GameObject root)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, root, null);
        }

        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, GameObject root, UnlockableDef unlockableDef)
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
                RendererInfos = new CharacterModel.RendererInfo[rendererInfos.Length],
                RootObject = root,
                UnlockableDef = unlockableDef
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
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

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        public struct SkinDefInfo
        {
            public SkinDef[] BaseSkins;
            public Sprite Icon;
            public string NameToken;
            public UnlockableDef UnlockableDef;
            public GameObject RootObject;
            public CharacterModel.RendererInfo[] RendererInfos;
            public SkinDef.MeshReplacement[] MeshReplacements;
            public SkinDef.GameObjectActivation[] GameObjectActivations;
            public SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            public SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            public string Name;
        }

        /// <summary>
        /// Plug in the names of all the GameObjects that are going to be activated/deactivated in any of your skins, and store this in a variable
        /// </summary>
        /// <returns>An ordered list of gameobjects to activate/deactivate</returns>
        public static List<GameObject> createAllActivatedGameObjectsList(ChildLocator childLocator, params string[] allChildren) {
            List<GameObject> allObjects = new List<GameObject>();

            for (int i = 0; i < allChildren.Length; i++) {
                allObjects.Add(childLocator.FindChildGameObject(allChildren[i]));
            }
            return allObjects;
        }

        /// <summary>
        /// Using the ActivatedGameObjects list, pass in the index of each gameobject to activate. Objects not passed in will deactivate.
        /// </summary>
        public static SkinDef.GameObjectActivation[] getGameObjectActivationsFromList(List<GameObject> allObjects, params int[] activatedChildren) {

            SkinDef.GameObjectActivation[] gameObjectActivations = new SkinDef.GameObjectActivation[allObjects.Count];

            for (int i = 0; i < allObjects.Count; i++) {
                gameObjectActivations[i] = new SkinDef.GameObjectActivation {
                    gameObject = allObjects[i],
                    shouldActivate = activatedChildren.Contains(i),
                };
            }

            return gameObjectActivations;
        }

        internal static SkinDef.MeshReplacement[] getMeshReplacements(CharacterModel.RendererInfo[] rendererinfos, params string[] meshes) {

            List<SkinDef.MeshReplacement> meshReplacements = new List<SkinDef.MeshReplacement>();

            for (int i = 0; i < rendererinfos.Length; i++) {
                if (string.IsNullOrEmpty(meshes[i]))
                    continue;

                meshReplacements.Add(
                new SkinDef.MeshReplacement {
                    renderer = rendererinfos[i].renderer,
                    mesh = Assets.LoadAsset<Mesh>(meshes[i])
                });
            }

            return meshReplacements.ToArray();
        }
    }
}