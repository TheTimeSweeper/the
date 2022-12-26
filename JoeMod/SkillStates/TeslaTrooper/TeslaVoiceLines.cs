using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {
    public class TeslaVoiceLines : EntityState {
        public override void Update() {
            base.Update();

            if (isAuthority && Modules.Config.GetKeyPressed(Modules.Config.voiceKey)) {

                playRandomvoiceLine();
            }
        }

        protected virtual void playRandomvoiceLine(string prefix = "Play_") {
            string sound;
            if (characterBody.outOfCombat) {
                if (characterBody.isSprinting) {
                    sound = $"{prefix}Voiceline_Move";
                } else {
                    sound = $"{prefix}Voiceline_Select";
                }
            } else {
                sound = $"{prefix}Voiceline_Attack";
            }

            RoR2.Util.PlaySound(sound, gameObject);
        }

        //reinventing the wheel wwise already does
        //private int getRandomVoiceLine() {
        //    int rand;
        //    if (!characterBody.outOfCombat) {
        //        rand = Random.Range(0, 5);
        //        Helpers.LogWarning("speak combat");
        //    } else {
        //        if (inputBank.moveVector != Vector3.zero) {
        //            rand = Random.Range(5, 11);
        //        } else {
        //            rand = Random.Range(11, 15);

        //            Helpers.LogWarning("speak out combat not moving");
        //        }
        //    }

        //    return rand;
        //}
    }
}