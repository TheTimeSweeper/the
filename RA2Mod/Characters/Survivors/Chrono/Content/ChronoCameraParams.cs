using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoCameraParams
    {
        public static CharacterCameraParamsData chronosphereCamera => Modules.CameraParams.CreateCameraParams(
                2,
                new Vector3(0, ChronoConfig.y.Value, ChronoConfig.z.Value),
                90,
                0.1f,
                ChronoConfig.fov.Value);

        public static CharacterCameraParamsData sprintCamera => Modules.CameraParams.CreateCameraParams(
                2,
                new Vector3(0, ChronoConfig.y2.Value, ChronoConfig.z2.Value),
                90,
                0.1f,
                ChronoConfig.fov2.Value);

        public static void Init()
        {
            //chronosphereCamera = Modules.CameraParams.CreateCameraParams(
            //    2,
            //    new Vector3(0, 10, -10),
            //    90,
            //    0.1f,
            //    100);
        }
    }
}
