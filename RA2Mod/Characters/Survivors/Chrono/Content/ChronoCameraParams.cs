using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoCameraParams
    {
        public static CharacterCameraParamsData chronosphereCamera = Modules.CameraParams.CreateCameraParams(
                2,
                new Vector3(0, 9, -17),
                90,
                0.1f,
                80);

        public static CharacterCameraParamsData sprintCamera = Modules.CameraParams.CreateCameraParams(
                2,
                new Vector3(0, 0, -15),
                90,
                0.1f,
                66);
    }
}
