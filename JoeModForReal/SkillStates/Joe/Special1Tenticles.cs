using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Joe {
    public class Special1Tenticles : BaseTimedSkillState {

        public static float BaseDuration = 0.5f;

        public static float Range = 30;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, 0);
            if (NetworkServer.active) {
                GameObject gameObject = Object.Instantiate(Modules.Assets.TenticlesSpelledWrong, transform.position, Quaternion.identity);
                gameObject.GetComponent<TeamFilter>().teamIndex = characterBody.teamComponent.teamIndex;
                //gameObject.GetComponent<BuffWard>().Networkradius = 8f + 8f * (float)itemCount;
                NetworkServer.Spawn(gameObject);
            }

            base.PlayCrossfade("Fullbody, underried", "ded3", "castDed.playbackRate", duration, 0.2f);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}