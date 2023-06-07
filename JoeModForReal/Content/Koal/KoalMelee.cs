using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Koal {

    public class KoalPrimary11 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary1", "Primary.playbackRate");
        }
    }

    public class KoalPrimary12 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary2", "Primary.playbackRate");
        }
    }

    public class KoalPrimary13 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary3", "Primary.playbackRate");
        }
    }

    public class KoalSecondary11 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary1", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary12 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary2", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary13 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary3", "Secondary.playbackRate");
        }
    }


    public class KoalPrimary21 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary2", "Primary.playbackRate");
        }
    }

    public class KoalPrimary22 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary2", "Primary.playbackRate");
        }
    }

    public class KoalPrimary23 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary2", "Primary.playbackRate");
        }
    }

    public class KoalSecondary21 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary2", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary22 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary2", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary23 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary2", "Secondary.playbackRate");
        }
    }


    public class KoalPrimary31 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary3", "Primary.playbackRate");
        }
    }

    public class KoalPrimary32 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary3", "Primary.playbackRate");
        }
    }

    public class KoalPrimary33 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("primary3", "Primary.playbackRate");
        }
    }

    public class KoalSecondary31 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary3", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary32 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary3", "Secondary.playbackRate");
        }
    }

    public class KoalSecondary33 : KoalMelee {
        protected override void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            base.PlayAttackAnimation("secondary3", "Secondary.playbackRate");
        }
    }
}


namespace ModdedEntityStates.Koal {
    public class KoalMelee : BaseMeleeAttackButEpic {
        public override void OnEnter() {

            this.hitboxName = "primary1Group";

            base.damageType = DamageType.Generic;
            base.damageCoefficient = 2;
            base.procCoefficient = 1f;
            base.pushForce = 100f;
            base.bonusForce = Vector3.zero;

            base.baseDuration = 0.96f;
            base.attackStartTime = 0.05f;
            base.attackEndTime = 0.31f;
            base.baseEarlyExitTime = 0.38f;

            base.hitStopDuration = 0;// .069f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 2f;

            base.swingSoundString = "play_joe_whoosh";
            base.hitSoundString = "";
            base.muzzleString = "notMercSlash";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = Modules.Assets.MercSwordSlash;
            base.hitEffectPrefab = Modules.Assets.MercImpactEffect;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.FleshSliceSound.index;

            base.OnEnter();
        }

        protected virtual void PlayAttackAnimation(string animationState, string playbackRateParameter) {
            PlayCrossfade("Gesture, Additive", animationState, playbackRateParameter, duration, 0.04f);
            PlayCrossfade("Gesture, Override", animationState, playbackRateParameter, duration, 0.04f);
        }

        protected override void PlayAttackAnimation() {
            PlayAttackAnimation("primary1", "Primary.playbackRate");
        }
    }
}
