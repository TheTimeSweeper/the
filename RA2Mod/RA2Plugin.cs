using BepInEx;
using RA2Mod.Survivors.Chrono;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using RA2Mod.General;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RA2Mod
{
    [BepInDependency("com.johnedwa.RTAutoSprintEx", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class RA2Plugin : BaseUnityPlugin
    {
        public const string MODUID = "com.thetimesweeper.ra2mod";
        public const string MODNAME = "RA2Mod";
        public const string MODVERSION = "0.0.0";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static RA2Plugin instance;

        public static bool testAsyncLoading = true;

        void Start()
        {
            Modules.SoundBanks.Init();
        }

        void Awake()
        {
            instance = this;
            
            Log.Init(Logger);
            GeneralConfig.Init();
            Log.CurrentTime("START " + (testAsyncLoading? "async" : "sync"));

            Modules.Language.Init();

            new ChronoSurvivor().Initialize();

            new Modules.ContentPacks().Initialize();

            On.RoR2.CameraTargetParams.CalcParams += CameraTargetParams_CalcParams;
            On.RoR2.CharacterCameraParamsData.Blend += CharacterCameraParamsData_Blend;
        }

        bool onehookonly;

        private void CharacterCameraParamsData_Blend(On.RoR2.CharacterCameraParamsData.orig_Blend orig, ref CharacterCameraParamsData src, ref CharacterCameraParamsData dest, float alpha)
        {
            if (onehookonly)
            {
                HG.BlendableTypes.BlendableFloat srcfov = new HG.BlendableTypes.BlendableFloat { alpha = src.minPitch.alpha, value = src.minPitch.value };
                HG.BlendableTypes.BlendableFloat destfov = new HG.BlendableTypes.BlendableFloat { alpha = dest.minPitch.alpha, value = dest.minPitch.value };

                string log = $"preblend srcfov {srcfov.value}, {srcfov.alpha} : destfov {destfov.value}, {destfov.alpha}";
                HG.BlendableTypes.BlendableFloat.Blend(srcfov, ref destfov, alpha);
                Log.Warning(log + $" blend srcfov {srcfov.value}, {srcfov.alpha} : destfov {destfov.value}, {destfov.alpha}");
            }
            orig(ref src, ref dest, alpha);

            if (onehookonly)
            {
                //Log.Warning($"srcfov {src.fov.value}, {src.fov.alpha} : destfov {dest.fov.value}, {dest.fov.alpha}");
            }
        }

        private void CameraTargetParams_CalcParams(On.RoR2.CameraTargetParams.orig_CalcParams orig, CameraTargetParams self, out CharacterCameraParamsData dest)
        {
            if (self.TryGetComponent(out CharacterBody body) && body.name.Contains("RA2ChronoBody") && self.cameraParams && UnityEngine.Input.GetKey(UnityEngine.KeyCode.G))
            {
                //Log.Warning(self.cameraParams.data.fov.value);
                onehookonly = true;
            }
            orig(self, out dest);

            if (onehookonly)
            {
                //Log.Warning($"currentfov {self.currentCameraParamsData.fov.value}, {self.currentCameraParamsData.fov.alpha}");
            }
            onehookonly = false;
        }
    }
}
