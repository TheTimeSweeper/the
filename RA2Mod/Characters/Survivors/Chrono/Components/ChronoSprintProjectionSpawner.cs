using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoSprintProjectionSpawner : NetworkBehaviour
    {
        public ChronoProjectionMotor marker;
        
        [Server]
        public void SpawnProjectionServer(Vector3 position, Quaternion rotation)
        {
            marker = Object.Instantiate(ChronoAssets.markerPrefab, position, rotation, null);

            NetworkServer.Spawn(marker.gameObject);
            RpcSendMarker(marker.gameObject);
        }

        [ClientRpc]
        private void RpcSendMarker(GameObject markerObject)
        {
            marker = markerObject.GetComponent<ChronoProjectionMotor>();
        }

        [Server]
        public void DisposeMarkerServer()
        {
            NetworkServer.Destroy(marker.gameObject);
        }

        public void MoveMarkerAuthority(Vector3 deltaPosition)
        {
            if (marker.Motor.GroundingStatus.IsStableOnGround)
            {
                deltaPosition.y = Mathf.Max(deltaPosition.y, 0);
            }

            marker.SimpleMove(deltaPosition);

            if (NetworkServer.active)
            {
                RpcMove(deltaPosition);
            }
            else
            {
                CmdMove(deltaPosition);
            }
        }

        [Command]
        private void CmdMove(Vector3 deltaPosition)
        {
            marker.SimpleMove(deltaPosition);
        }

        [ClientRpc]
        private void RpcMove(Vector3 deltaPosition)
        {
            marker.SimpleMove(deltaPosition);
        }
    }
}