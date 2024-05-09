using EntityStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.States
{
    public class AimChronosphere2 : AimChronosphereBase
    {
        public Vector3 originalPoint;
        public CameraTargetParams.CameraParamsOverrideHandle cameraOverride;
        public Transform origOrigin;
        
        private ChronosphereProjection chronosphereProjection;

        private float sqrRadius;

        public List<CharacterBody> teleporteeBodies = new List<CharacterBody>();

        public override void OnEnter()
        {
            chronosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjection);
            chronosphereProjection.transform.position = originalPoint;
            chronosphereProjection.SetRadiusAndEnable(AimChronosphereBase.BaseRadius);
            chronosphereProjection.AnimateShader(true, 0, 1, false);

            //if (isAuthority)
            //{
            //    for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
            //    {
            //        GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
            //    }
            //}

            //if (NetworkServer.active)
            //{
            //    RootTeleportees();
            //}

            sqrRadius = BaseRadius * BaseRadius;

            skillLocator.utility.SetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.OnEnter();

            viewRadius *= 0.5f;
        }
        
        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        if (FriendlyFireManager.ShouldDirectHitProceed(body.healthComponent, teamComponent.teamIndex))
                        {
                            teleporteeBodies.Add(body);
                        }
                    }
                }
            }
        }

        private void RootTeleportees()
        {
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                teleporteeBodies[i].AddBuff(ChronoBuffs.chronosphereRootDebuff);
            }
        }

        private void UnRootTeleportees()
        {
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                teleporteeBodies[i].RemoveBuff(ChronoBuffs.chronosphereRootDebuff);
            }
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            base.UpdateTrajectoryInfo(out dest);

            Vector3 vector = dest.hitPoint - originalPoint;
            dest.finalRay = new Ray(originalPoint, vector.normalized);
            dest.speedOverride = this.projectileBaseSpeed;
            dest.travelTime = vector.magnitude / this.projectileBaseSpeed;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (castSuccessful)
            {
                Util.PlaySound("Play_Chronosphere_Move", gameObject);
            }

            //if (NetworkServer.active)
            //{
            //    UnRootTeleportees();
            //}

            chronosphereProjection.AnimateShader(false, castSuccessful ? PlaceChronosphere2.teleportDelay : 0, castSuccessful ? 0.3f : 0.2f, true);

            skillLocator.utility.UnsetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);
            
            characterBody.aimOriginTransform = origOrigin;
            cameraTargetParams.RemoveParamsOverride(cameraOverride, 0.5f);
        }

        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new PlaceChronosphere2 {
                originalPoint = originalPoint,
                BaseRadius = BaseRadius,
                trajectoryPoint = currentTrajectoryInfo.hitPoint,
                //teamCharacterBodies = teleporteeBodies
            };
        }

        protected override void PlayEnterSounds()
        {
            Util.PlaySound("Play_Chronosphere_StartLoop", chronosphereProjection.gameObject);
        }
        
        protected override void PlayExitSounds()
        {
            Util.PlaySound("Stop_Chronosphere_Loop", chronosphereProjection.gameObject);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(originalPoint);
            for (int i = 0; i < teleporteeBodies.Count; i++)
            {
                writer.Write(teleporteeBodies[i].gameObject);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                teleporteeBodies.Add(reader.ReadGameObject().GetComponent<CharacterBody>());
            }
        }
    }
}