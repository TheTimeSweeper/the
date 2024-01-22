using EntityStates;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class PlaceChronosphere2 : AimChronosphereBase
    {
        public Vector3 originalPoint;

        public ChronosphereProjection chronosphereProjection;

        public List<CharacterBody> characterBodies = new List<CharacterBody>();

        private float sqrRadius;

        public override void OnEnter()
        {
            chronosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjection);
            chronosphereProjection.transform.position = originalPoint;
            NetworkServer.Spawn(chronosphereProjection.gameObject);
            chronosphereProjection.SetRadiusAndEnable(AimChronosphereBase.BaseRadius);

            sqrRadius = BaseRadius * BaseRadius;

            for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
            {
                GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
            }

            skillLocator.utility.SetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);

            base.OnEnter();

            viewRadius *= 0.5f;
            Log.Warning(viewRadius);
        }

        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if(teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        characterBodies.Add(body); 

                        if (FriendlyFireManager.ShouldDirectHitProceed(body.healthComponent, teamComponent.teamIndex))
                        {
                            body.AddBuff(ChronoBuffs.chronoSphereRootDebuff);
                        }
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (castSuccessful)
            {
                Util.PlaySound("Play_ChronosphereMove", chronosphereProjection.gameObject);

                MoveTeleportees(currentTrajectoryInfo.hitPoint);
            }
            NetworkServer.Destroy(chronosphereProjection.gameObject);

            skillLocator.utility.UnsetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        private void MoveTeleportees(Vector3 trajectoryPoint)
        {
            trajectoryPoint += Vector3.up * 1;
            for (int i = 0; i < characterBodies.Count; i++)
            {
                if (characterBodies[i] == null)
                    continue;
                
                characterBodies[i].RemoveBuff(ChronoBuffs.chronoSphereRootDebuff);

                Vector3 originalPosition = characterBodies[i].gameObject.transform.position;
                Vector3 relativeDistance = (originalPosition - originalPoint)*0.5f;

                Vector3 resultPoint;
                if (Physics.Linecast(trajectoryPoint, trajectoryPoint + relativeDistance, out RaycastHit info, LayerIndex.world.collisionMask))
                {
                    resultPoint = info.point;
                }
                else
                {
                    resultPoint = trajectoryPoint + relativeDistance;
                }

                if (characterBodies[i].TryGetComponent(out CharacterMotor motor))
                {
                    motor.Motor.SetPosition(resultPoint);
                }
                else if (characterBodies[i].TryGetComponent(out RigidbodyMotor rbmotor))
                {
                    rbmotor.rigid.MovePosition(resultPoint);
                }
                else
                {
                    characterBodies[i].gameObject.transform.position = resultPoint;
                }
            }
        }

        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new WindDownState { windDownTime = 1f};
        }

        protected override void PlayEnterSounds()
        {
            Util.PlaySound("Play_ChronosphereSelectStart", chronosphereProjection.gameObject);
            Util.PlaySound("Play_ChronosphereSelectLoop", chronosphereProjection.gameObject);
        }
        
        protected override void PlayExitSounds()
        {
            Util.PlaySound("Stop_ChronosphereSelectLoop", chronosphereProjection.gameObject);
            Util.PlaySound("Play_ChronosphereSelectEnd", chronosphereProjection.gameObject);
        }
    }
}