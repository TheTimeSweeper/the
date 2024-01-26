﻿using EntityStates;
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

        public List<CharacterBody> teamCharacterBodies = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();

            sqrRadius = BaseRadius * BaseRadius;

            if (isAuthority)
            {
                for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
                {
                    GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
                }
            }

            MoveTeleportees();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge > Time.fixedDeltaTime * 2)
            {
                outer.SetNextState(new WindDownState { windDownTime = 1f });
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            for (int i = 0; i < teamCharacterBodies.Count; i++)
            {
                TrySetBodyInvisible(teamCharacterBodies[i], false);
            }
        }

        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        teamCharacterBodies.Add(body);
                    }
                }
            }
        }

        private void MoveTeleportees()
        {
            trajectoryPoint += Vector3.up * 1;
            for (int i = 0; i < teamCharacterBodies.Count; i++)
            {
                CharacterBody body = teamCharacterBodies[i];
                if (body == null)
                    continue;
                Vector3 originalPosition = body.gameObject.transform.position;
                Vector3 relativeDistance = (originalPosition - originalPoint) * 0.5f;

                TrySetBodyInvisible(body, true);

                Vector3 resultPoint;
                if (Physics.Linecast(trajectoryPoint, trajectoryPoint + relativeDistance, out RaycastHit info, LayerIndex.world.mask))
                {
                    resultPoint = info.point;
                }
                else
                {
                    resultPoint = trajectoryPoint + relativeDistance;
                }

                if (body.TryGetComponent(out CharacterMotor motor))
                {
                    motor.Motor.SetPosition(resultPoint);
                }
                else if (body.TryGetComponent(out RigidbodyMotor rbmotor))
                {
                    rbmotor.rigid.MovePosition(resultPoint);
                }
                else
                {
                    body.gameObject.transform.position = resultPoint;
                }
            }
        }

        private void TrySetBodyInvisible(CharacterBody body, bool shouldInvis)
        {
            var model = body.modelLocator?.modelTransform?.GetComponent<CharacterModel>();
            if (model)
            {
                model.invisibilityCount += shouldInvis ? 1 : -1;

                if (shouldInvis == false)
                {
                    TeleportOutController.AddTPOutEffect(model, 1f, 0f, 0.2f);
                }
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(originalPoint);
            writer.Write(trajectoryPoint);
            for (int i = 0; i < teamCharacterBodies.Count; i++)
            {
                writer.Write(teamCharacterBodies[i].gameObject);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
            trajectoryPoint = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                teamCharacterBodies.Add(reader.ReadGameObject().GetComponent<CharacterBody>());
            }
        }
    }
}