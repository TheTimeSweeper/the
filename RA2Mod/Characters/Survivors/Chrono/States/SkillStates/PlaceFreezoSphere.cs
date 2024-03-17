using EntityStates;
using RoR2;
using RA2Mod.Survivors.Chrono.Components;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RA2Mod.Survivors.Chrono.States
{
    public class PlaceFreezoSphere : BaseSkillState
    {
        public CameraTargetParams.CameraParamsOverrideHandle cameraOverride;
        public Vector3 originalPoint;
        public Transform origOrigin;

        public List<CharacterBody> freezeeBodies = new List<CharacterBody>();

        public static float freezeDuration => ChronoConfig.M3_Freezosphere_FreezeDuration.Value;
        public static float BaseDuration => ChronoConfig.M3_Freezosphere_CastDuration.Value;

        private ChronosphereProjection freezosphereProjection;
        private float sqrRadius;
        
        public override void OnEnter()
        {
            base.OnEnter();

            skillLocator.utility.DeductStock(1);

            freezosphereProjection = Object.Instantiate(ChronoAssets.chronosphereProjectionFreeze);
            freezosphereProjection.transform.position = originalPoint;
            freezosphereProjection.SetRadiusAndEnable(ChronoConfig.M3_Freezosphere_Radius.Value);
            freezosphereProjection.AnimateShader(true, 0, BaseDuration, false);
            freezosphereProjection.GetComponent<TeamFilter>().teamIndex = teamComponent.teamIndex;

            sqrRadius = ChronoConfig.M3_Freezosphere_Radius.Value * ChronoConfig.M3_Freezosphere_Radius.Value;

            if (isAuthority)
            {
                TeamMask filter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
                for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
                {
                    if (!filter.HasTeam(teamIndex))
                        continue;

                    GatherTeleportees(TeamComponent.GetTeamMembers(teamIndex));
                }
            }

            FreezeFreezees();
            
            Util.PlaySound("Play_ChronosphereSelectStart", freezosphereProjection.gameObject);
        }

        private void GatherTeleportees(ReadOnlyCollection<TeamComponent> teamComponents)
        {
            for (int i = 0; i < teamComponents.Count; i++)
            {
                if ((teamComponents[i].transform.position - originalPoint).sqrMagnitude < sqrRadius)
                {
                    if (teamComponents[i].TryGetComponent(out CharacterBody body))
                    {
                        if (body != characterBody && FriendlyFireManager.ShouldDirectHitProceed(body.healthComponent, teamComponent.teamIndex))
                        {
                            freezeeBodies.Add(body);
                        }
                    }
                }
            }
        }

        private void FreezeFreezees()
        {
            for (int i = 0; i < freezeeBodies.Count; i++)
            {
                TryFreeze(freezeeBodies[i]);
            }
        }

        private void TryFreeze(CharacterBody body)
        {
            if(body.TryGetComponent(out SetStateOnHurt setStateOnHurt))
            {
                if (setStateOnHurt.targetStateMachine)
                {
                    VanishingState frozenState = new VanishingState();
                    frozenState.freezeDuration = freezeDuration;
                    setStateOnHurt.targetStateMachine.SetInterruptState(frozenState, InterruptPriority.Frozen);
                }
                EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].SetNextState(new Idle());
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= BaseDuration && isAuthority)
            {
                base.outer.SetNextStateToMain();
            }
        }
        //protected override void PlayEnterSounds()
        //{
        //    Util.PlaySound("Play_ChronosphereSelectStart", chronosphereProjection.gameObject);
        //    Util.PlaySound("Play_ChronosphereSelectLoop", chronosphereProjection.gameObject);
        //}

        //protected override void PlayExitSounds()
        //{

        //    Util.PlaySound("Stop_ChronosphereSelectLoop", chronosphereProjection.gameObject);
        //    Util.PlaySound("Play_ChronosphereSelectEnd", chronosphereProjection.gameObject);
        //}

        public override void OnExit()
        {
            base.OnExit();

            freezosphereProjection.AnimateShader(false, 5 - BaseDuration - 0.5f, 0.5f, true);

            //skillLocator.utility.UnsetSkillOverride(this, ChronoAssets.cancelSKillDef, GenericSkill.SkillOverridePriority.Contextual);

            characterBody.aimOriginTransform = origOrigin;
            cameraTargetParams.RemoveParamsOverride(cameraOverride, 0.5f);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(originalPoint);
            for (int i = 0; i < freezeeBodies.Count; i++)
            {
                writer.Write(freezeeBodies[i].gameObject);
            }
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            originalPoint = reader.ReadVector3();
            while (reader.Position < reader.Length)
            {
                freezeeBodies.Add(reader.ReadGameObject().GetComponent<CharacterBody>());
            }
        }
    }
}