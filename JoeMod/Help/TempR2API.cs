using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace R2API {
    public static class LanguageAPI {

        public static void Add(string string1, string string2) {
            
        }
    }

    public static class PrefabAPI {
        public static GameObject InstantiateClone(this GameObject prefab, string newName, bool network = false) {
            GameObject instance = Object.Instantiate(prefab);
            instance.name = newName;
            return instance;
        }
    }

    public static class SoundAPI {
        public static class SoundBanks {

            public static void Add(byte[] bank) {

            }
        }
    }

}
