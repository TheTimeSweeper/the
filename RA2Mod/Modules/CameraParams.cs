using HG.BlendableTypes;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RA2Mod.Modules
{
    internal static class CameraParams
    {
        internal static CharacterCameraParamsData CreateCameraParams(float pivotVerticalOffset, Vector3 idealPosition, float pitch = 70, float wallCushion = 0.1f, float fov = 0)
        {
            CharacterCameraParamsData newParams = new CharacterCameraParamsData();

            newParams.maxPitch = pitch;
            newParams.minPitch = -pitch;
            newParams.pivotVerticalOffset = pivotVerticalOffset;
            newParams.idealLocalCameraPos = idealPosition;
            newParams.wallCushion = wallCushion;
            if(fov != 0)
            {
                newParams.fov = new BlendableFloat
                {
                    value = fov,
                    alpha = 1
                };
            }

            return newParams;
        }

        internal static CameraTargetParams.CameraParamsOverrideHandle OverrideCameraParams(CameraTargetParams camParams, CharacterCameraParamsData paramsData, float transitionDuration = 0.5f)
        {
            CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
            {
                cameraParamsData = paramsData,
                priority = 1,
            };

            return camParams.AddParamsOverride(request, transitionDuration);
        }
    }
}
