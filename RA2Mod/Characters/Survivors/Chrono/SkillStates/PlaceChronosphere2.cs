using EntityStates;
using RA2Mod.Modules.BaseStates;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class PlaceChronosphere2 : BaseSkillState
    {
        public float BaseRadius;
        public Vector3 trajectoryPoint;
        public Vector3 originalPoint;

        private float sqrRadius;

        public List<CharacterBody> characterBodies = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();

            sqrRadius = BaseRadius * BaseRadius;

            for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
            {
                GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
            }

            MoveTeleportees();

            outer.SetNextState(new WindDownState { windDownTime = 1f });
        }

        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        characterBodies.Add(body);
                    }
                }
            }
        }

        private void MoveTeleportees()
        {
            trajectoryPoint += Vector3.up * 1;
            for (int i = 0; i < characterBodies.Count; i++)
            {
                if (characterBodies[i] == null)
                    continue;
                if (NetworkServer.active)
                {
                    //characterBodies[i].RemoveBuff(ChronoBuffs.chronoSphereRootDebuff);
                }
                Vector3 originalPosition = characterBodies[i].gameObject.transform.position;
                Vector3 relativeDistance = (originalPosition - originalPoint) * 0.5f;

                Vector3 resultPoint;
                if (Physics.Linecast(trajectoryPoint, trajectoryPoint + relativeDistance, out RaycastHit info, LayerIndex.world.mask))
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

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            Log.Warning($"writing originalPoint {originalPoint}");
            writer.Write(originalPoint);
            Log.Warning($"writing trajectoryPoint {trajectoryPoint}");
            writer.Write(trajectoryPoint);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
            Log.Warning($"reading originalPoint {originalPoint}");
            trajectoryPoint = reader.ReadVector3();
            Log.Warning($"reading trajectoryPoint {trajectoryPoint}");
        }
    }
}