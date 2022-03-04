using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace R2API {
    public static class LanguageAPI {

        private static readonly Dictionary<string, string> LanguageDict = new Dictionary<string, string>();

        private const string genericLanguage = "generic";

        public static void Add(string key, string value) {

            if (!LanguageDict.ContainsKey(key)) {
                LanguageDict.Add(key, value);
            }
        }

        internal static void LanguageAwake() {

            On.RoR2.Language.GetLocalizedStringByToken += Language_GetLocalizedStringByToken;
            On.RoR2.Language.TokenIsRegistered += Language_TokenIsRegistered;
        }


        private static bool Language_TokenIsRegistered(On.RoR2.Language.orig_TokenIsRegistered orig, Language self, string token) {
            var languagename = self.name;
            if (LanguageDict.ContainsKey(token)) {
                return true;
            }
            return orig(self, token);
        }

        private static string Language_GetLocalizedStringByToken(On.RoR2.Language.orig_GetLocalizedStringByToken orig, Language self, string token) {
            var languagename = self.name;
            if (LanguageDict.ContainsKey(token)) {
                return LanguageDict[token];
            }
            return orig(self, token);
        }
    }

    public static class PrefabAPI {
        private static GameObject _parent;

        public static GameObject InstantiateClone(this GameObject prefab, string newName, bool network = false) {
            GameObject instance = Object.Instantiate(prefab, GetParent().transform);
            instance.name = newName;
            return instance;
        }


        private static GameObject GetParent() {
            if (!_parent) {
                _parent = new GameObject("ModdedPrefabs");
                Object.DontDestroyOnLoad(_parent);
                _parent.SetActive(false);

                On.RoR2.Util.IsPrefab += (orig, obj) => {
                    if (obj.transform.parent && obj.transform.parent.gameObject.name == "ModdedPrefabs") return true;
                    return orig(obj);
                };
            }

            return _parent;
        }
    }

    public static class SoundAPI {
        public static class SoundBanks {

            public static void Add(byte[] bank) {

            }
        }
    }

    public static class LoadoutAPI {

        public static void AddSkillDef(SkillDef skillDef) {
            Modules.ContentPacks.skillDefs.Add(skillDef);
        }

        public static void AddSkill(Type type) {
            Modules.ContentPacks.entityStates.Add(type);
        }

        public static Sprite CreateSkinIcon(Color top, Color right, Color bottom, Color left) {
           
            return CreateSkinIcon(top, right, bottom, left, new Color(0.6f, 0.6f, 0.6f));
        }

        public static Sprite CreateSkinIcon(Color top, Color right, Color bottom, Color left, Color line) {

            Texture2D tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            new IconTexJob {
                Top = top,
                Bottom = bottom,
                Right = right,
                Left = left,
                Line = line,
                TexOutput = tex.GetRawTextureData<Color32>()
            }.Schedule(16384, 1).Complete();
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
        }


        private struct IconTexJob : IJobParallelFor {

            [ReadOnly]
            public Color32 Top;

            [ReadOnly]
            public Color32 Right;

            [ReadOnly]
            public Color32 Bottom;

            [ReadOnly]
            public Color32 Left;

            [ReadOnly]
            public Color32 Line;

            public NativeArray<Color32> TexOutput;

            public void Execute(int index) {
                int x = index % 128 - 64;
                int y = index / 128 - 64;

                if (Math.Abs(Math.Abs(y) - Math.Abs(x)) <= 2) {
                    TexOutput[index] = Line;
                    return;
                }
                if (y > x && y > -x) {
                    TexOutput[index] = Top;
                    return;
                }
                if (y < x && y < -x) {
                    TexOutput[index] = Bottom;
                    return;
                }
                if (y > x && y < -x) {
                    TexOutput[index] = Left;
                    return;
                }
                if (y < x && y > -x) {
                    TexOutput[index] = Right;
                }
            }
        }
    }

}
