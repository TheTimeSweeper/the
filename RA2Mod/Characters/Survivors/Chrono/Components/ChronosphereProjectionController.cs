using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronosphereProjectionController : NetworkBehaviour
    {
        [SyncVar]
        public GameObject projection;

        [ClientRpc]
        public void RpcSetProjection(GameObject projectionGameObject)
        {
            projection = projectionGameObject;
        }
    }
}