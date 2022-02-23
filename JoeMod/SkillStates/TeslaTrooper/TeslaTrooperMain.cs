using EntityStates;
using HenryMod.Modules;
using RoR2;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper {
    public class TeslaTrooperMain : GenericCharacterMain {

        private int lastVoice = -1;

        public override void Update() {
            base.Update();

            if (Input.GetKeyDown(KeyCode.CapsLock)) {

                int rand = getRandomVoiceLine();
                while(rand == lastVoice) {
                    rand = getRandomVoiceLine();
                }

                Helpers.PlaySoundVoiceLine((TeslaVoiceLine)rand, gameObject);
                
                //Util.PlaySound("Play_itesatd", gameObject);
            }
        }

        private int getRandomVoiceLine() {
            int rand;
            if (!characterBody.outOfCombat) {
                rand = Random.Range(0, 5);
            } else {
                if (inputBank.moveVector != Vector3.zero) {
                    rand = Random.Range(5, 11);
                } else {
                    rand = Random.Range(11, 15);
                }
            }

            return rand;
        }
    }
}