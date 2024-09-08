using RA2Mod.Modules;
using RA2Mod.Survivors.Desolator.Achievements;
using RA2Mod.Survivors.Desolator.States;
using System;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorTokens
    {
        public static void Init()
        {
            AddDesolatorTokens();
        }

        private static void AddDesolatorTokens()
        {
            string prefix = DesolatorSurvivor.TOKEN_PREFIX;

            string desc = "The Desolator is a walking powerhouse of radiation and area of effect.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                        + "< ! > Rad-Cannon applies two stacks of Radiation, helping other abilities deal more damage." + Environment.NewLine + Environment.NewLine
                        + "< ! > Scorched Earth is a simple heavy damage dealer, especially on an enemy with a lot of stacks of Radiation." + Environment.NewLine + Environment.NewLine
                        + "< ! > Use the movement speed from Reactor to get you out of a pinch, but you can instead use its weakening properties to help deal extra damage." + Environment.NewLine + Environment.NewLine
                        + "< ! > Spread the Doom leaves you vulnerable to attack, but you're rewarded for staying deployed longer to ramp up damage and radiation." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, behind him, an oasis of death.";
            string outroFailure = "..and so he vanished, there goes the neighborhood.";

            string fullName = "Desolator";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Here Comes the Sun");

            #region lore

            LanguageAPI.Add(prefix + "LORE",
                "<i>In collaboration with [REDACTED], the Ministry of Information presents...</i>\n" +
                "<i>DESOLATOR: REVOLUTIONARY HERO AGAINST TYRANNY!</i>\n" +
                "\n" +
                "The film sputters, its age showing like rust on metal, but it churns on tape nonetheless. It's played for a ragged bunker in which the blast-door is welded shut and lined to the edge with every available object in the room; books, chairs, the bed. Alone sits a jittery man, watching.\n" +
                "\n" +
                "<i>DESOLATOR, once a poor innocent boy from the slums who grew up with nothing but HATE from the tyrannical government! Sought a way to free himself and his comrades from the barbarians!\n" +
                "He himself organized our GLORIOUS REVOLUTION! Fighting side by side with the men on the frontlines to ensure that no battle was lost!\n" +
                "But eventually, he was GREIVOUSLY WOUNDED and using the power of experimental technology stolen from our OPPRESSORS, he was REBUILT into the hero we know him today!</i>\n" +
                "\n" +
                "The bunker trembles, as its only illumination swings back and forth in response. His past had caught up to him, whose work would come haunt him.\n" +
                "None know the truth of his actions; his job was to cover it up after all. But such a job required such mental tenacity to endure painting over the facts.\n" +
                "Screams could be heard outside the bunker, only to be drowned into what sounded like sludge.\n" +
                "\n" +
                "<i>DESOLATOR returned to the front of battle! rescuing all his fellow compatriots-</i>\n" +
                "'They were massacred.'\n" +
                "<i>lest they bear the same burden as him!-</i>\n" +
                "'You bared our brothers in arms as much suffering as you did to the enemy.'\n" +
                "<i>DESOLATOR AND HIS COMRADE'S REVOLUTION SAVED OUR PLANET, IN WHICH HE BUILT THE SOCIETY WE HAVE TODAY!</i>\n" +
                "'All you left was a red river of bodies for a cause you didn't believe, the saviour of the revolution had the perfect excuse.'\n" +
                "\n" +
                "But before he could continue watching the tape, the frantic man felt his skin crawl, and sweat build. He was here.\n" +
                "Before another thought can be processed, the door glows an iridescent green and begins to melt. Bolts that held the door shut, that could withstand nuclear fallout, melted like butter against a hot knife. The remains of the barricade liquidated across the floor, lumps of metal and furniture; burning, melded into a radioactive soup. It's him, it's-\n" +
                "\n" +
                "<i>DESOLATOR!</i>\n" +
                "\n" +
                "The film solemnly rings; before sputtering what's left of the record. Eventually dissolving like the door before it.\n" +
                "Before words could be exchanged, the dying man's melting flesh began to split and slip off his body, akin to slow cooked meat. His eyes became hazy, taking one last look at the radioactive presence as he spoke the last words the man would ever hear.\n" +
                "\n" +
                "'Shh, the end is near.'\n" +
                "\n" +
                "Brain sloshing, muscle dripping to reveal the skeleton underneath, as even the atomic structure of bone started liquifying. Soon, the room itself was becoming unstable; the last things to leave were the heavy, steel-boot footsteps and heavy breathing. His respiration being the only thing human about him.");
            #endregion lore
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Desecration");
            //LanguageAPI.Add(prefix + "NOD_SKIN_NAME", "Brotherhood");
            //LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Minecraft");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Radiation");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"Desolator's attacks deal more damage to <style=cIsHealing>Irradiated</style> targets: {Tokens.DamageText($"+{DesolatorSurvivor.DamageMultiplierPerIrradiatedStack}x")} per stack of <style=cIsHealing>Radiation</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_NAME", "Rad-Cannon");
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_DESCRIPTION", $"<style=cIsHealing>Irradiating</style>. Shoot an enemy with a beam of radiation for {Tokens.DamageValueText(RadBeam.DamageCoefficient)}.");// Reduces armor by {Tokens.UtilityText($"{DesolatorSurvivor.ArmorShredAmount}")} for {Tokens.UtilityText($"{DesolatorSurvivor.ArmorShredDuration} seconds")}.");

            LanguageAPI.Add("KEYWORD_RADIATION_PRIMARY", Tokens.KeywordText("Irradiating", $"Inflicts 2 stacks of <style=cIsHealing>Radiation</style>, each dealing {Tokens.DamageText($"{DesolatorSurvivor.DotDamage * 2 * RadBeam.RadDamageMultiplier * 100}% damage per second")} for {Tokens.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}."));
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGBEAM_NAME", "Scorched Earth");
            LanguageAPI.Add(prefix + "SECONDARY_BIGBEAM_DESCRIPTION",
                $"<style=cIsHealing>Irradiating</style>. Blast an area for {Tokens.DamageValueText(AimBigRadBeam.BlastDamageCoefficient)}, and cover the area in radiation for {Tokens.UtilityText($"{AimBigRadBeam.DotZoneLifetime} seconds")}. " +
                $"Enemies in contact take {Tokens.DamageText($"{AimBigRadBeam.PoolDamageCoefficient * 100}% damage twice per second")}");

            LanguageAPI.Add("KEYWORD_RADIATION_SECONDARY", Tokens.KeywordText("Irradiating", $"Initial Blast: Inflicts {Tokens.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Tokens.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}.\n" +
                $"Lingering Area: Each tick inflicts {Tokens.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 0.5f * 100}% damage per second")} for {Tokens.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}."));
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_AURA_NAME", "Reactor");
            LanguageAPI.Add(prefix + "UTILITY_AURA_DESCRIPTION", $"For {Tokens.UtilityText($"{RadiationAura.BuffDuration} seconds")}, {Tokens.DamageText("Weaken")} all nearby enemies, and gain a boost to {Tokens.UtilityText("move speed")} and {Tokens.UtilityText("armor")}");
            #endregion

            #region Special
            //default
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_NAME", "Spread the Doom");
            string specialDesc =
                $"<style=cIsHealing>Irradiating</style>. Deploy your Rad-Cannon into the ground. Gain {Tokens.UtilityText($"barrier")} for each enemy nearby. {Tokens.UtilityText($"Every 3 seconds")}, pump a pool of radiation that lasts for {Tokens.UtilityText($"7 seconds")}, dealing {Tokens.DamageText($"{DeployIrradiate.DamageCoefficient * 100}% damage per second")}.";
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_DEPLOY_NAME", "There Goes the Neighborhood");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_DEPLOY_DESCRIPTION", specialDesc + Tokens.ScepterDescription($"Double Area. 1.5x Faster ticks per second."));

            //cancel
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_CANCEL_NAME", "Cancel");
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_CANCEL_DESCRIPTION", "Stop Spreading the Doom");

            //alt
            bool fun = UnityEngine.Random.value <= 0.1f;
            string name = "Irradiators";
            string name2 = "Glow Sticks";
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_NAME", name);

            specialDesc =
                $"<style=cIsHealing>Irradiating</style>. Throw {Tokens.UtilityText("up to 2")} {name} which cover a large area in radiation for {Tokens.UtilityText($"11 seconds")}, dealing {Tokens.DamageText($"{ThrowIrradiator.DamageCoefficient * 100}% damage per second")}.";
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_DESCRIPTION", specialDesc);

            string specialDescScepter = specialDesc + Tokens.ScepterDescription($"Explodes on expiration for {Tokens.DamageValueText(ScepterThrowIrradiator.finalExplosionDamageCoefficient)}.");

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_NAME", "Unstable " + name);
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_DESCRIPTION", specialDescScepter);

            //alt fun
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_NAME_FUN", name2);
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_DESCRIPTION_FUN", specialDesc.Replace(name, name2));

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_NAME_FUN", "Unstable " + name2);
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_DESCRIPTION_FUN", specialDescScepter.Replace(name, name2));


            LanguageAPI.Add("KEYWORD_RADIATION_SPECIAL", Tokens.KeywordText("Irradiating", $"Each tick inflicts {Tokens.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 0.7f * 100}% damage per second")} for {Tokens.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}."));
            #endregion

            #region Achievements
            LanguageAPI.Add(Tokens.GetAchievementNameToken(DesolatorMasteryAchievement.identifier),       $"{fullName}: Mastery");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(DesolatorMasteryAchievement.identifier), $"As {fullName}, beat the game or obliterate on Monsoon.");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(DesolatorGrandMasteryAchievement.identifier), $"{fullName}: Grand Mastery");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(DesolatorGrandMasteryAchievement.identifier), $"As {fullName}, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");

            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", $"Irradiators");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", $"Find and activate the 3 irradiators in stage 3.");

            #endregion
        }
    }
}