using GorpMod.Modules;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine;

namespace GorpMod.Enemies.Gorp.Content
{
    public static class CharacterAssets
    {
        public static GameObject GipMaster;

        public static void Init()
        {
            Asset.LoadAssetAsync<GameObject>(RoR2_DLC1_Gup.GipMaster_prefab, (geep) => GipMaster = geep);
        }
    }
}
