using EntityStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class AimChronosphere2 : AimChronosphereBase
    {
        public Vector3 originalPoint;
        
        private GameObject projectionGameObject => chronosphereProjectionController.projection;

        private float sqrRadius;
        ChronosphereProjectionController chronosphereProjectionController;

        private bool playedEnterSoundsHack;

        public override void OnEnter()
        {
            chronosphereProjectionController = GetComponent<ChronosphereProjectionController>();

            if (NetworkServer.active)
            {
                ChronosphereProjection chronosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjection);
                chronosphereProjection.transform.position = originalPoint;
                NetworkServer.Spawn(chronosphereProjection.gameObject);
                chronosphereProjection.RpcSetRadiusAndEnable(AimChronosphereBase.BaseRadius);
                chronosphereProjectionController.RpcSetProjection(chronosphereProjection.gameObject);
            }

            if (NetworkServer.active)
            {
                for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
                {
                    //RootTeleportees(TeamComponent.GetTeamMembers(teamIndex));
                }
            }

            sqrRadius = BaseRadius * BaseRadius;

            skillLocator.utility.SetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.OnEnter();

            viewRadius *= 0.5f;
        }

        private void RootTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        if (FriendlyFireManager.ShouldDirectHitProceed(body.healthComponent, teamComponent.teamIndex))
                        {
                            body.AddBuff(ChronoBuffs.chronoSphereRootDebuff);
                        }
                    }
                }
            }
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            base.UpdateTrajectoryInfo(out dest);

            Vector3 vector = dest.hitPoint - originalPoint;
            dest.finalRay.origin = originalPoint;
            dest.finalRay.direction = vector;
            dest.speedOverride = this.projectileBaseSpeed;
            dest.travelTime = this.projectileBaseSpeed / vector.magnitude;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!playedEnterSoundsHack && projectionGameObject != null)
            {;
                PlayEnterSounds();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (castSuccessful)
            {
                Util.PlaySound("Play_ChronosphereMove", projectionGameObject);
            }

            if (NetworkServer.active)
            {
                NetworkServer.Destroy(projectionGameObject);
            }

            skillLocator.utility.UnsetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new PlaceChronosphere2 {
                originalPoint = originalPoint, 
                BaseRadius = BaseRadius, 
                trajectoryPoint = currentTrajectoryInfo.hitPoint
            };
        }

        protected override void PlayEnterSounds()
        {
            playedEnterSoundsHack = true;
            Util.PlaySound("Play_ChronosphereSelectStart", projectionGameObject);
            Util.PlaySound("Play_ChronosphereSelectLoop", projectionGameObject);
        }
        
        protected override void PlayExitSounds()
        {
            Util.PlaySound("Stop_ChronosphereSelectLoop", projectionGameObject);
            Util.PlaySound("Play_ChronosphereSelectEnd", projectionGameObject);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(originalPoint);
        }
    }
}