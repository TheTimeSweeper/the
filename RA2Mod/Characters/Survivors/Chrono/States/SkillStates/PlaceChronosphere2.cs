using EntityStates;
using RA2Mod.General.States;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.States
{
    public class PlaceChronosphere2 : BaseSkillState
    {
        public static float teleportDelay => ChronoConfig.M3_Chronosphere_Delay.Value;

        public float BaseRadius;
        public Vector3 trajectoryPoint;
        public Vector3 originalPoint;

        private float sqrRadius;

        public List<CharacterBody> teamCharacterBodies = new List<CharacterBody>();

        private ChronosphereProjection chronosphereProjection;
        private bool hasMoved;

        public override void OnEnter()
        {
            base.OnEnter();

            skillLocator.utility.DeductStock(1);

            chronosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjection);
            chronosphereProjection.transform.position = trajectoryPoint;
            chronosphereProjection.SetRadiusAndEnable(BaseRadius * 0.5f);
            chronosphereProjection.AnimateShader(true, 0, teleportDelay, false);

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
            if (base.fixedAge > teleportDelay && !hasMoved)
            {
                hasMoved = true;


                outer.SetNextState(new WindDownState { windDownTime = 0.5f });
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            for (int i = 0; i < teamCharacterBodies.Count; i++)
            {
                TrySetBodyInvisible(teamCharacterBodies[i], false);
            }
            chronosphereProjection.AnimateShader(false, 0, 0.3f, true);
        }

        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        if (body.isFlying || !body.isChampion)
                        {
                            teamCharacterBodies.Add(body);
                        }
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

                TeleportHelper.TeleportBody(body, resultPoint);

                //TeleportBody(body, resultPoint);
            }
        }

        private static void TeleportBody(CharacterBody body, Vector3 resultPoint)
        {
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private void TrySetBodyInvisible(CharacterBody body, bool shouldInvis)
        {
            if (body == null)
                return;
            if (body.modelLocator == null)
                return;
            if (body.modelLocator.modelTransform == null)
                return;
            if (body.modelLocator.modelTransform.TryGetComponent(out CharacterModel model)){

                //model.invisibilityCount += shouldInvis ? 1 : -1;

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