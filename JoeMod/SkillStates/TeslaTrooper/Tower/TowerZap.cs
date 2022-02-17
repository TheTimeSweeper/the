using HenryMod.ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;
using RoR2.Orbs;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower
{
    public class TowerZap: BaseTimedSkillState
    {
        public static float DamageCoefficient = 690;
        public static float ProcCoefficient = 1f;

        public static float BaseDuration = 0.69f;
        public static float BaseStartCastTime = 1;
        
        public LightningOrb lightningOrb;
        public HurtBox lightningTarget;

        public override void OnEnter()
        {
            base.OnEnter();

            // is this redundant cause the cast time is the end and I could just do an onexit kinda thing?
            InitDurationValues(BaseDuration, BaseStartCastTime);

            lightningOrb.target = lightningTarget;
            lightningOrb.damageValue = DamageCoefficient * base.damageStat;
            
            PlaySoundAuthority("Play_tower_btesat1a_tesla_tower_attack");
            //playanimation or however I'm going to do the glow going up the pole
                //and the orb glowing
        }

        protected override void OnCastEnter()
        {
            //todo: see if this means you can't repeat hits
            //todo: custom lightningorb
            if (lightningTarget == null)
                return;

            OrbManager.instance.AddOrb(lightningOrb);

            string sound = "Play_tower_btesat1a_tesla_tower_attack";
            if (lightningOrb.isCrit) sound = "Play_tower_btesat1b_tesla_tower_attack";

            PlaySoundAuthority(sound);
            //playsound zap
        }
    }

}
