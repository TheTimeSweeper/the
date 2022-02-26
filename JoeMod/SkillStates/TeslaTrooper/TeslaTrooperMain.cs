using EntityStates;
using HenryMod.Modules;
using RoR2;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper {
    public class TeslaTrooperMain : GenericCharacterMain {

        private int lastVoice = -1;

        private Animator cachedAnimator;

        public override void OnEnter() {
            base.OnEnter();
            cachedAnimator = GetModelAnimator();
        }

        public override void Update() {
            base.Update();

            cachedAnimator.SetBool("inCombat", !characterBody.outOfCombat);

            if (Input.GetKeyDown(KeyCode.CapsLock)) {

                playRandomvoiceLine();
                //Util.PlaySound("Play_itesatd", gameObject);
            }
        }

        private void playRandomvoiceLine() {
            int rand = getRandomVoiceLine();
            while (rand == lastVoice) {
                rand = getRandomVoiceLine();
            }

            Helpers.PlaySoundVoiceLine((TeslaVoiceLine)rand, gameObject);
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