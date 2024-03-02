using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoSprintProjectionSpawner : NetworkBehaviour
    {
        public ChronoProjectionMotor marker;

        //private CharacterMaster master;

        //public void Start()
        //{
        //    master = GetComponent<CharacterBody>().master;
        //}

        [Server]
        public void SpawnProjectionServer(Vector3 position, Quaternion rotation)
        {
            marker = Object.Instantiate(ChronoAssets.markerPrefab, position, rotation, null);

            //NetworkConnection clientAuthorityOwner = null;
            //if (master != null && master.networkIdentity != null)
            //{
            //    clientAuthorityOwner = master.networkIdentity.clientAuthorityOwner;
            //}
            //if (clientAuthorityOwner != null)
            //{
                NetworkServer.Spawn(marker.gameObject);
                RpcSendMarker(marker.gameObject);
            //}
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

            marker.SimpleMove(deltaPosition, marker.transform.position + deltaPosition);

            if (NetworkServer.active)
            {
                RpcMove(deltaPosition, marker.transform.position + deltaPosition);
            }
            else
            {
                CmdMove(deltaPosition, marker.transform.position + deltaPosition);
            }
        }

        [Command]
        private void CmdMove(Vector3 deltaPosition, Vector3 finalPosition)
        {
            marker.SimpleMove(deltaPosition, finalPosition);
        }

        [ClientRpc]
        private void RpcMove(Vector3 deltaPosition, Vector3 finalPosition)
        {
            marker.SimpleMove(deltaPosition, finalPosition);
        }
    }
}