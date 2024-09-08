using RA2Mod.Modules;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaBuffs
    {
        public static BuffDef zapShieldBuff;

        public static BuffDef conductiveBuffTeam;
        public static BuffDef conductiveBuffTeamGrace;

        public static void Init(AssetBundle assetBundle)
        {
            zapShieldBuff = Content.CreateAndAddBuff(
                "Tesla Barrier",
                null,
                Color.cyan,
                false,
                false);
            ContentPacks.asyncLoadCoroutines.Add(Asset.LoadBuffIconAsync(zapShieldBuff, "RoR2/Base/Common/texBuffGenericShield.tif"));

            conductiveBuffTeam = Content.CreateAndAddBuff(
                "Charged",
                null,
                Color.cyan,
                false,
                false);
            ContentPacks.asyncLoadCoroutines.Add(Asset.LoadBuffIconAsync(conductiveBuffTeam, "RoR2/Base/ShockNearby/texBuffTeslaIcon.tif"));
            
            conductiveBuffTeamGrace = Content.CreateAndAddBuff(
                "Charged2",
                null,
                Color.blue,
                false,
                false);
            ContentPacks.asyncLoadCoroutines.Add(Asset.LoadBuffIconAsync(conductiveBuffTeamGrace, "RoR2/Base/ShockNearby/texBuffTeslaIcon.tif"));
        }
    }
}
