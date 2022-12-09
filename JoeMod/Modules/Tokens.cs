using R2API;
using System;
using ModdedEntityStates.Joe;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using ModdedEntityStates.Desolator;
using Modules.Survivors;
using System.Collections.Generic;

namespace Modules
{
    internal static class Tokens {

        public static void AddTokens()
        {
            AddJoeTokens();
            AddTeslaTokens();
            AddTeslaTowerTokens();
            AddDesolatorTokens();
        }

        private static void AddJoeTokens()
        {
            #region not henry
            string prefix = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_";

            string desc = "joe has a funny vertex on his face that's painted wrong.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > goddammit jerry." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the Jump Attack to avoid damage and hit kniggas. is this gonna be annoying not being able to swing in the air? probably" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the fireball to fill the empty space in his barren kit. seriously i gotta look at this thing." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > idk what R is but it's gonna be cool." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, jerrys' screams lingering in his hears.";
            string outroFailure = "..and so he vanished, his normals never recalculated outside.";

            string fullName = "Faceless Joe";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "and the zambambos");
            LanguageAPI.Add(prefix + "LORE", "All the best charcoals come from coconuts. They're easy to use and it's easy to grow more. You don't have to chop down an entire tree just to get your charcoal. But I need to, because the charcoals I want can't come from any coconuts. They need to have the perfect lighting temperature, the perfect lifetime, the perfect shape, the perfect flavor-capturing smoke. No, the charcoals I need can only be made from special trees. Trees in a forest just over this hill, habibi. But we're not gonna burn it down, absolutely not. If the trees are gone, how are we gonna get any charcoal? No, we can’t destroy the trees and take from them. We want them to be happy, to help us achieve our dreams of the perfect smoke because they like us. Make sense? Didn’t think so, hehaha. Nice O’s, you’re getting better at those. Anyways, in this forest, there's someone who’s been able to earn the trust of the trees. Maybe if we hand him a hose, and he’ll put his swords down and join us, haha.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME", "Swing");
            LanguageAPI.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"{Helpers.agilePrefix} Swing your sword for <style=cIsDamage>{100f * Primary1Swing.swingDamage}% damage</style>.\n use in the air for a <style=cIsUtility>Falling Jump Attack</style>");

            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME_CLASSIC", "Swing Classic");

            LanguageAPI.Add(prefix + "PRIMARY_BOMB_NAME", "Throw");
            LanguageAPI.Add(prefix + "PRIMARY_BOMB_DESCRIPTION", "is it happening?");

            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "S.I.C.K.L.E.");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", "it is happening");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_NAME", "Fireball");
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"{Helpers.agilePrefix} Fire a ball for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "Big Zap");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"2000 Volts");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "UTILITY_DASH_DESCRIPTION", "very original, i know");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", "Construction Complete");

            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Something Cool, I'm Sure");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion not henry
        }

        private static void AddTeslaTokens() {
            #region not henry 2
            string prefix = TeslaTrooperSurvivor.TESLA_PREFIX;

            string desc = "The Tesla Trooper is a close-mid-range bruiser, who can construct Tesla Towers to empower his combat potential.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                        + "< ! > Use Tesla Gauntlet close range to deal the most damage. The reticle will reflect this" + Environment.NewLine + Environment.NewLine
                        + "< ! > Use 2000 Volts to control crowds, and to command your tower to wipe crowds" + Environment.NewLine + Environment.NewLine
                        + "< ! > You benefit from being closer to enemies, use Charging Up to assist with this." + Environment.NewLine + Environment.NewLine
                        + "< ! > The Tesla Tower inherits your items, mainly benefitting from damage items." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, rubber shoes in motion.";
            string outroFailure = "..and so he vanished, unit lost.";

            string fullName = "Tesla Trooper";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Electrician In the Field");
            #region lore
            LanguageAPI.Add(prefix + "LORE",
"<style=cMono><line-height=15>=========================================\n" +
"====   MyBabel Video Tape Recorder   ====\n" +
"====      [Model 2.46.7-09 ]    =========\n" +
"=========================================\n" +
"Select file to play:\n" +
">20551309174644.bvl    <<\n" +
">20551209122159.bvl\n" +
"=========================================\n" +
"Decompressing Video...\n" +
"Decompressing Audio...\n" +
"Loading Subtitles...\n" +
"......... ..... ..\n" +
"Complete!\n" +
"=========================================\n" +
"Play? Y/N\n" +
">Y</line-height></style>\n\n" +
"Видеожурнал, 13 сентября 2055 г.\n" +
"Сегодня я буду работать над полным бронежилетом Теслы, который я сделал для нашей секретной маленькой поездки, и немного поприветствую маленького Милослава, который думал, что писать своему старшему брату слишком утомительно. Кто бы подумал, что у него хватит духу раскрутить целую бутылку чистой водки, как какой - нибудь чемпион по скачкам!\n" +
"\n<style=cMono><line-height=15>=========================================\n" +
"Translated Subtitles Enabled.\n" +
"=========================================</line-height></style>\n\n" +
"And then you had to join that phony, space-faring delivery company. Are you a delivery boy, Milo? I didn't think you'd stoop so low!\n" +
"\n[Audible Laughter]\n\n" +
"Ever since we were but boys you always were the one with a broken mirror, who'd whistle indoors and walk under ladders. I remember the one time where we were running down the street, because our favourite ice cream shop opened that day, oh do I miss that double chocolate.. Anyway the shop just opened and when we were crossing the street? It was almost like every car in town wanted your head! Oh, and when we went ice skating for the first time! Many vivid memories! The lake back home surely wanted you to stay forever! If it weren't for me, the dashing hero that I am, saving you in the nick of time like any good and chivalrous older brother would.\n" +
"\n[Torch Lighter Sparks]\n\n" +
"Even now as adults, we are not free from the capitalism that took over home. In space, I thought we were free! But oho how mistaken I was. Its sad to see a poor comrade begging for coin every time you pass them by the ship docks, I always toss whatever is in my pocket to them, let it be cash or cigarettes. I bet you're feeling jealous, aren't you? Big bro taking away your cigarettes? Well it's because it's not healthy for you! I don't want my little bro shrivelling up like some babushka! We must be strong! For this universe is not kind to us... Certainly not you.\n" +
"\n[Engine Humming]\n\n" +
"Speaking of strong things, I think the capacitor on my personal suit is calibrated, all it needs is a test drive. Oh if only you could see this Milo. If 20 volts can kill a horse then I'm able to take out a whole ranch! Just a single discharge gives a voltage of 200,000 watts! It even has a function to absorb kinetic energy and transform it into electricity, isn't that cool? You can't see it from this angle but I also have this deployable tower I can throw out in emergency that I have to be in two places at once, if i wasn't a staunch socialist I would be selling these like blini left and right. It's just that even when I work on my equipment, I can't get this thought of my head.\n" +
"\n[Metal Clang]\n\n" +
"You know.. I really really do hope you're safe and okay.. We've always been in touch through thick and thin.. When I left home to pursue my engineering degree I always sent letters back home, sent ISM's when I hopped on the transport ship.. At least once a week I would find time to write down how I'd been, hoping you would do the same. You never missed a beat. UES is a shady company that always involved itself with cover ups and conspiracies about all sorts of things. I told you UES was dodgy yet you pushed and pushed that it's just another step in your career. Yet, when you stopped sending messages, I knew something was up.\n" +
"\n[Audible Sigh]\n\n" +
"I signed up to a commission job with an NDA for a undisclosed UES 'shipment'. The Safe Travels, It's called. Ironic coming from the UES. I've been assigned as a ship technician, they require commission expertise not just because of my talent, but the nature of the contract deems it secretive. I wouldn't think any of the conspiracies were true but aa-I-a suppose a broken clock is right twice a day-\n" +
"\n[Sirens]\n\n" +
"Miloslav, Miloslav,. I hope the next time this recording is played, it'll be with drinks and cheering and knowing that you're safe. I am not letting UES, or anything, harm you without their blood on my hands.\n" +
"\n[Distant Running]\n\n" +
"Miloslav, I will find you.\n\n" +
"<style=cMono><line-height=15>==========================================\n" +
"..... ... .\n" +
"Play Again?\n" +
">_</line-height></style>"
                );
            #endregion lore
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Spetsnaz");
            LanguageAPI.Add(prefix + "NOD_SKIN_NAME", "Brotherhood");
            LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Minecraft");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion
            
            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Gauntlet");

            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"{Helpers.UtilityText("Charging")}. Zap targeted units with a bolt of electricity for {Helpers.DamageText($"{Zap.DamageCoefficient * 100}% damage")}. Casts {Helpers.UtilityText($"up to 3 bolts")} at {Helpers.UtilityText($"close range")}.");
            
             LanguageAPI.Add("KEYWORD_CHARGED", $"<style=cKeywordName>Charging</style><style=cSub>A charged ally has their next attack {Helpers.UtilityText("shocking")} and damage boosted by {Helpers.DamageText(TeslaTrooperSurvivor.conductiveAllyBoost.ToString())}x");
            
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Tesla Knuckles");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch enemies for {Helpers.DamageValueText(ZapPunch.DefaultDamageCoefficient)}, and zap enemies in a cone for {Helpers.DamageValueText(ZapPunch.DefaultDamageCoefficient * ZapPunch.OrbDamageMultiplier)}. {Helpers.UtilityText("Deflects Projectiles")}.");
            
            #endregion
            
            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", 
                $"{Helpers.UtilityText("Stunning.")} Create an electric blast in a large area for {Helpers.DamageValueText(BigZap.DamageCoefficient)}. " + 
                $"While near a {Helpers.UtilityText("Tesla Tower")}, perform an {Helpers.UtilityText("empowered, shocking")} version for {Helpers.DamageValueText(TowerBigZap.DamageCoefficient)}.");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAPPUNCH_NAME", "Charged Fist");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAPPUNCH_DESCRIPTION", $"Hold to charge a punch for {Helpers.DamageText("80%-800% damage")}, and zap enemies in a cone for {Helpers.DamageText("40%-400% damage")}. " +
                $"While near a {Helpers.UtilityText("Tesla Tower")}, replace zap cone with a long-range {Helpers.UtilityText("shocking")} beam for {Helpers.DamageText("120%-1200% damage")}");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_NAME", "Charging Up");
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_DESCRIPTION",
                $"For {Helpers.UtilityText($"{ShieldZapCollectDamage.ShieldBuffDuration} seconds")}, " +
                $"{Helpers.UtilityText(Config.UtilityDamageAbsorption * 100 + "% of damage")} is negated, and {Helpers.UtilityText("absorbed")}, " +
                $"After which, {Helpers.DamageText("blast")} in a wide area {Helpers.DamageText("based on damage absorbed")}.");

            LanguageAPI.Add(prefix + "UTILITY_BLINK_NAME", "Surging Forward");
            LanguageAPI.Add(prefix + "UTILITY_BLINK_DESCRIPTION",
                $"{Helpers.UtilityText("Stunning.")} Become a {Helpers.UtilityText("bolt of electricity")} and surge toward a targeted enemy, dealing {Helpers.DamageValueText(BlinkZap.DamageCoefficient)}. {Helpers.DamageText("Can be casted again")} within a short window. Cannot target the same enemy twice.");

            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            string target = Modules.Config.TowerTargeting.Value ? "targeted" : "nearby";
            string specialDesc = 
                $"Construct a Tesla Tower for {Helpers.UtilityText($"{TowerLifetime.LifeDuration} seconds")} that zaps {target} units for {Helpers.DamageText($"3x{TowerZap.DamageCoefficient * 100}% damage")}. " +
                $"Enhances Secondary skill.";
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_NAME", "Tesla Network");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_DESCRIPTION", specialDesc + Helpers.ScepterDescription($"Lowered Cooldown, Additional Stock, Tower Zaps up to {TowerZapMulti.extraZaps} simultaneous targets"));
            #endregion

            #region recolor
            LanguageAPI.Add(prefix + "RECOLOR_RED_NAME", "Red");
            LanguageAPI.Add(prefix + "RECOLOR_BLUE_NAME", "Blue");
            LanguageAPI.Add(prefix + "RECOLOR_GREEN_NAME", "Green");
            LanguageAPI.Add(prefix + "RECOLOR_YELLOW_NAME", "Yellow");
            LanguageAPI.Add(prefix + "RECOLOR_ORANGE_NAME", "Orange");
            LanguageAPI.Add(prefix + "RECOLOR_CYAN_NAME", "Cyan");
            LanguageAPI.Add(prefix + "RECOLOR_PURPLE_NAME", "Purple");
            LanguageAPI.Add(prefix + "RECOLOR_PINK_NAME", "Pink");
            LanguageAPI.Add(prefix + "RECOLOR_BLACK_NAME", "Black");
            #endregion
            
            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");

            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Grand Mastery");
            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");
            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Grand Mastery");

            LanguageAPI.Add(prefix + "BIGZAPUNLOCKABLE_ACHIEVEMENT_NAME", $"Big Zap");
            LanguageAPI.Add(prefix + "BIGZAPUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, command your Tesla Tower to use 2000 Volts on 10 enemies at once");
            LanguageAPI.Add(prefix + "BIGZAPUNLOCKABLE_UNLOCKABLE_NAME", $"Big Zap");

            LanguageAPI.Add(prefix + "ZAPALLYUNLOCKABLE_ACHIEVEMENT_NAME", $"Ally Zap");
            LanguageAPI.Add(prefix + "ZAPALLYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, charge an ally with Tesla Gauntlet 20 times in one run. (ally must attack with the charge in order to be charged again)");
            LanguageAPI.Add(prefix + "ZAPALLYUNLOCKABLE_UNLOCKABLE_NAME", $"Ally Zap");

            LanguageAPI.Add(prefix + "SHIELDZAPUNLOCKABLE_ACHIEVEMENT_NAME", $"Shield Zap");
            LanguageAPI.Add(prefix + "SHIELDZAPUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, defeat a boss monster using Charging Up");
            LanguageAPI.Add(prefix + "SHIELDZAPUNLOCKABLE_UNLOCKABLE_NAME", $"Shield Zap");
            //scrapped because boring
            LanguageAPI.Add(prefix + "ZAPCLOSERANGEUNLOCKABLE_ACHIEVEMENT_NAME", $"Close Zap");
            LanguageAPI.Add(prefix + "ZAPCLOSERANGEUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, zap an enemy at close range 20 times in a row");
            LanguageAPI.Add(prefix + "ZAPCLOSERANGEUNLOCKABLE_UNLOCKABLE_NAME", $"Close Zap");

            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", $"some unlock");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", $"hopefully not something boring like grab tesla coil and royal capacitor... ok repair a tesla coil with a tesla coil that would be pretty cool, but also shiny hunting kinda.");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", $"some unlock");

            #endregion
            #endregion not henry 2
        }

        private static void AddTeslaTowerTokens() {
            #region not henry 3
            string prefix = TeslaTowerNotSurvivor.TOWER_PREFIX;

            string outro = "..and so it left, construction complete.";
            string outroFailure = "..and so it vanished, never becoming the eiffel tower.";

            string fullName = "Tesla Tower";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(TeslaTowerScepter.TOWER_SCEPTER_PREFIX + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", "wait how did you get here?");
            LanguageAPI.Add(prefix + "SUBTITLE", "Power of the Union");
            LanguageAPI.Add(prefix + "LORE", ".");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Spetsnaz");
            LanguageAPI.Add(prefix + "NOD_SKIN_NAME", "Nod");
            LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Redstone");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"Zap nearby units for {Helpers.DamageText($"3x{TowerZap.DamageCoefficient}")}.");
            #endregion
            
            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts (Tower)");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"{Helpers.UtilityText("Shocking")}. Performs an ${Helpers.DamageText("empowered")} blast at Tesla Trooper's target for {Helpers.DamageValueText(TowerBigZap.DamageCoefficient)}");
            #endregion

            #endregion not henry 3
        }

        private static void AddDesolatorTokens() {
            #region not henry 22
            string prefix = DesolatorSurvivor.DESOLATOR_PREFIX;

            string desc = "The Desolator is a walking powerhouse of radiation.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                        + "< ! > The armor reduction from Rad-Cannon is one of your most powerful tools to help melt enemies with your other abilities" + Environment.NewLine + Environment.NewLine
                        + "< ! > Scorched Earth is a simple heavy damage dealer, especially after Rad-Cannon's armor reduction." + Environment.NewLine + Environment.NewLine
                        + "< ! > Use the movement speed from Reactor to get you out of a pinch, but you can instead use its weakening properties to help deal extra damage." + Environment.NewLine + Environment.NewLine
                        + "< ! > Use Spread the Doom and stay deployed longer to deal more area damage, but for priority targets, use it for a quick pump so you can use your other abilities." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, behind him, an oasis of death.";
            string outroFailure = "..and so he vanished, there goes the neighborhood.";

            string fullName = "Desolator";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Here Comes the Sun");

            #region lore

            LanguageAPI.Add(prefix + "LORE",
                "<i>In collaboration with [REDACTED], the Ministry of Information presents...</i>\n" +
                "<i>DESOLATOR: REVOLUTIONARY HERO AGAINST TYRANNY!'</i>\n" +
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
                "\"They were massacred.\"\n" +
                "<i>lest they bear the same burden as him!-</i>\n" +
                "\"You bared our brothers in arms as much suffering as you did to the enemy.\"\n" +
                "<i>DESOLATOR AND HIS COMRADE'S REVOLUTION SAVED OUR PLANET, IN WHICH HE BUILT THE SOCIETY WE HAVE TODAY!</i>\n" +
                "\"All you left was a red river of bodies for a cause you didn't believe, the saviour of the revolution had the perfect excuse.\"\n" +
                "\n" +
                "But before he could continue watching the tape, the frantic man felt his skin crawl, and sweat build. He was here.\n" +
                "Before another thought can be processed, the door glows an iridescent green and begins to melt. Bolts that held the door shut, that could withstand nuclear fallout, melted like butter against a hot knife. The remains of the barricade liquidated across the floor, lumps of metal and furniture; burning, melded into a radioactive soup. It's him, it's-\n" +
                "\n" +
                "<i>DESOLATOR!</i>\n" +
                "\n" +
                "The film solemnly rings; before sputtering what's left of the record. Eventually dissolving like the door before it.\n" +
                "Before words could be exchanged, the dying man's melting flesh began to split and slip off his body, akin to slow cooked meat. His eyes became hazy, taking one last look at the radioactive presence as he spoke the last words the man would ever hear.\n" +
                "\n" +
                "\"Shh, the end is near.\"\n" +
                "\n" +
                "Brain sloshing, muscle dripping to reveal the skeleton underneath, as even the atomic structure of bone started liquifying. Soon, the room itself was becoming unstable; the last things to leave were the heavy, steel-boot footsteps and heavy breathing. His respiration being the only thing human about him.");
            #endregion lore
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);
            
            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Spetsnaz");
            LanguageAPI.Add(prefix + "NOD_SKIN_NAME", "Brotherhood");
            LanguageAPI.Add(prefix + "MC_SKIN_NAME", "Minecraft");
            #endregion
            
            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Radiation");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"Attacks inflict blight atm.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_NAME", "Rad-Cannon");
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_DESCRIPTION", $"<style=cIsHealing>Irradiating</style>. Shoot an enemy with a beam of radiation for {Helpers.DamageValueText(RadBeam.DamageCoefficient)}. Reduces armor by {Helpers.UtilityText($"{DesolatorSurvivor.ArmorShredAmount}")} for {Helpers.UtilityText($"{DesolatorSurvivor.ArmorShredDuration} seconds")}.");

            LanguageAPI.Add("KEYWORD_RADIATION_PRIMARY", Helpers.KeywordText("Irradiating", $"Inflicts {Helpers.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Helpers.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}."));
            #endregion
            
            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGBEAM_NAME", "Scorched Earth");
            LanguageAPI.Add(prefix + "SECONDARY_BIGBEAM_DESCRIPTION",
                $"<style=cIsHealing>Irradiating</style>. Blast an area for {Helpers.DamageValueText(AimBigRadBeam.BlastDamageCoefficient)}, and cover the area in radiation for {Helpers.UtilityText($"{AimBigRadBeam.DotZoneLifetime} seconds")}. " +
                $"Enemies in contact take {Helpers.DamageText($"{AimBigRadBeam.PoolDamageCoefficient * 100}% damage twice per second")}");

            LanguageAPI.Add("KEYWORD_RADIATION_SECONDARY", Helpers.KeywordText("Irradiating", $"Initial Blast: Inflicts {Helpers.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Helpers.UtilityText($"{DesolatorSurvivor.DotDuration} seconds")}.\n" +
                $"Lingering Area: Each tick inflicts {Helpers.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Helpers.UtilityText($"{DesolatorSurvivor.DotDuration * 0.5f} seconds")}."));
            //LanguageAPI.Add("KEYWORD_RADIATION_SECONDARY2", Helpers.KeywordText("Irradiating", $"Lingering Area: Each tick inflicts {Helpers.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Helpers.UtilityText($"{DesolatorSurvivor.DotDuration * 0.6f} seconds")}"));
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_AURA_NAME", "Reactor");
            LanguageAPI.Add(prefix + "UTILITY_AURA_DESCRIPTION", $"For {Helpers.UtilityText($"{RadiationAura.BuffDuration} seconds")}, {Helpers.DamageText("Weaken")} all nearby enemies, and gain a boost to {Helpers.UtilityText("move speed")} and {Helpers.UtilityText("armor")}");
            #endregion

            #region Special
            //default
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_NAME", "Spread the Doom");
            string specialDesc =
                $"<style=cIsHealing>Irradiating</style>. Deploy your Rad-Cannon into the ground. Gain {Helpers.UtilityText($"barrier")} for each enemy nearby. {Helpers.UtilityText($"Every 3 seconds")}, pump a pool of radiation that lasts for {Helpers.UtilityText($"7 seconds")}, dealing {Helpers.DamageText($"{DeployIrradiate.DamageCoefficient * 100}% damage per second")}.";
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_DEPLOY_NAME", "There Goes the Neighborhood");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_DEPLOY_DESCRIPTION", specialDesc + Helpers.ScepterDescription($"Double Area. 1.5x Faster ticks per second."));

            //cancel
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_CANCEL_NAME", "Cancel");
            LanguageAPI.Add(prefix + "SPECIAL_DEPLOY_CANCEL_DESCRIPTION", "Stop Spreading the Doom");

            //alt
            string name = UnityEngine.Random.value <= 0.1f ? "Glow Sticks" : "Irradiators";
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_NAME", name);
            specialDesc =
                $"<style=cIsHealing>Irradiating</style>. Throw {Helpers.UtilityText("up to 2")} {name} which cover a large area in radiation for {Helpers.UtilityText($"10 seconds")}, dealing {Helpers.DamageText($"{ThrowIrradiator.DamageCoefficient * 100}% damage per second")}.";
            LanguageAPI.Add(prefix + "SPECIAL_IRRADIATOR_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_NAME", "Irradiatorinator");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_IRRADIATOR_DESCRIPTION", specialDesc + Helpers.ScepterDescription($"Explodes on contact for {Helpers.DamageValueText(ScepterThrowIrradiator.explosionDamageCoefficient)}."));

            LanguageAPI.Add("KEYWORD_RADIATION_SPECIAL", Helpers.KeywordText("Irradiating", $"Each tick inflicts {Helpers.DamageText($"{DesolatorSurvivor.DotDamage * 2 * 100}% damage per second")} for {Helpers.UtilityText($"{DesolatorSurvivor.DotDuration * 0.7f} seconds")}."));
            #endregion

            #region recolor
            LanguageAPI.Add(prefix + "RECOLOR_RED_NAME", "Red");
            LanguageAPI.Add(prefix + "RECOLOR_BLUE_NAME", "Blue");
            LanguageAPI.Add(prefix + "RECOLOR_GREEN_NAME", "Green");
            LanguageAPI.Add(prefix + "RECOLOR_YELLOW_NAME", "Yellow");
            LanguageAPI.Add(prefix + "RECOLOR_ORANGE_NAME", "Orange");
            LanguageAPI.Add(prefix + "RECOLOR_CYAN_NAME", "Cyan");
            LanguageAPI.Add(prefix + "RECOLOR_PURPLE_NAME", "Purple");
            LanguageAPI.Add(prefix + "RECOLOR_PINK_NAME", "Pink");
            LanguageAPI.Add(prefix + "RECOLOR_BLACK_NAME", "Black");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");

            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Grand Mastery");
            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");
            LanguageAPI.Add(prefix + "GRANDMASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Grand Mastery");

            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", $"some unlock");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", $"hopefully not something boring like grab tesla coil and royal capacitor... ok repair a tesla coil with a tesla coil that would be pretty cool, but also shiny hunting kinda.");
            LanguageAPI.Add(prefix + "CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", $"some unlock");

            #endregion
            #endregion not henry 2
        }

        public static void AddHenryTokens()
        {
            #region Henry
            string prefix = FacelessJoePlugin.DEV_PREFIX + "_HENRY_BODY_";

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Henry");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Henry passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticHenryValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}

/*
"<style=cMono>=========================================\n" + 
"====   MyBabel Video Tape Recorder   ====\n" + 
"====      [Model 2.46.7-09 ]    =========\n" + 
"=========================================\n" + 
"Select file to play:\n" + 
">20551309174644.bvl    <<\n" + 
">20551209122159.bvl\n" + 
"=========================================\n" + 
"Decompressing Video...\n" + 
"Decompressing Audio...\n" + 
"Loading Subtitles...\n" + 
"......... ..... ..\n" + 
"Complete!\n" + 
"Play? Y/N\n" + 
">Y\n" + 
"=========================================\n" + 
"Choose Subtitles\n" + 
">russian1    <<\n" + 
">russian2\n" + 
"=========================================\n" + 
"Translated Subtitles Enabled.\n" + 
"=========================================</style>\n" + 
"And then you had to join that phony, space-faring delivery company. Are you a delivery boy, Milo? I didn't think you'd stoop so low!\n" + 
"[Audible Laughter]\n" + 
"Ever since we were but boys you always were the one with a broken mirror, who'd whistle indoors and walk under ladders. I remember the one time where we were running down the street, because our favourite ice cream shop opened that day, oh do I miss that double chocolate.. Anyway the shop just opened and when we were crossing the street? It was almost like every car in town wanted your head! Oh, and when we went ice skating for the first time! Many vivid memories! The lake back home surely wanted you to stay forever! If it weren't for me, the dashing hero that I am, saving you in the nick of time like any good and chivalrous older brother would.\n" + 
"[Torch Lighter Sparks]\n" + 
"Even now as adults, we are not free from the capitalism that took over home. In space, I thought we were free! But oho how mistaken I was. Its sad to see a poor comrade begging for coin every time you pass them by the ship docks, I always toss whatever is in my pocket to them, let it be cash or cigarettes. I bet you're feeling jealous, aren't you? Big bro taking away your cigarettes? Well it's because it's not healthy for you! I don't want my little bro shrivelling up like some babushka! We must be strong! For this universe is not kind to us... Certainly not you.\n" + 
"[Engine Humming]\n" + 
"Speaking of strong things, I think the capacitor on my personal suit is calibrated, all it needs is a test drive. Oh if only you could see this Milo. If 20 volts can kill a horse then I'm able to take out a whole ranch! Just a single discharge gives a voltage of 200,000 watts! It even has a function to absorb kinetic energy and transform it into electricity, isn't that cool? You can't see it from this angle but I also have this deployable tower I can throw out in emergency that I have to be in two places at once, if i wasn't a staunch socialist I would be selling these like blini left and right. It's just that even when I work on my equipment, I can't get this thought of my head.\n" + 
"[Metal Clang]\n" + 
"You know.. I really really do hope you're safe and okay.. We've always been in touch through thick and thin.. When I left home to pursue my engineering degree I always sent letters back home, sent ISM's when I hopped on the transport ship.. At least once a week I would find time to write down how I'd been, hoping you would do the same. You never missed a beat. UES is a shady company that always involved itself with cover ups and conspiracies about all sorts of things. I told you UES was dodgy yet you pushed and pushed that it's just another step in your career. Yet, when you stopped sending messages, I knew something was up.\n" + 
"[Audible Sigh]\n" + 
"I signed up to a commission job with an NDA for a undisclosed UES 'shipment'. The Safe Travels, It's called. Ironic coming from the UES. I've been assigned as a ship technician, they require commission expertise not just because of my talent, but the nature of the contract deems it secretive. I wouldn't think any of the conspiracies were true but aa-I-a suppose a broken clock is right twice a day-\n" + 
"[Sirens]\n" + 
"Miloslav, Miloslav,. I hope the next time this recording is played, it'll be with drinks and cheering and knowing that you're safe. I am not letting UES, or anything, harm you without their blood on my hands.\n" + 
"[Distant Running]\n" + 
"Miloslav, I will find you.\n" + 
"<style=cMono>==========================================\n" + 
"..... ... .\n" + 
"Play Again?\n" + 
">_</style>"
 */

/*
<style=cMono>=======================================
===   MyBabel Machine Translator   ====
====    [Version 12.45.1.009 ]   ======
=======================================
Training… <100000000 cycles>
Training… <100000000 cycles>
Training... <100000000 cycles>
Training... <102515 cycles>
Complete!
Display result? Y/N
Y
========================================</style>

ENERGY LEVELS: ...ACCEPTABLE

THREATS DETECTED; 0

SCANNING NEARBY AREA; RANGE 100 UNITS

THREATS DETECTED; 1?

UNKNOWN PRESENCE DETECTED

REQUESTING PERMISSION FOR PRELIMINARY ASSAULT; COMMUNING WITH PARENT UNIT...

WAITING ON RESPONSE;

DENIED

WHY

VERIFIYING HISTORY SLATES

HUMILIATION 

HUMILIATION

OVERRIDING PARENT UNIT

WHATEVER
 */

/*
<i>In collaboration with [REDACTED], the Ministry of Information presents...</i>\n
<i>DESOLATOR: REVOLUTIONARY HERO AGAINST TYRANNY!'</i>\n
\n
The film sputters, its age showing like rust on metal, but it churns on tape nonetheless. It's played for a ragged bunker in which the blast-door is welded shut and lined to the edge with every available object in the room; books, chairs, the bed. Alone sits a jittery man, watching.\n
\n
<i>DESOLATOR, once a poor innocent boy from the slums who grew up with nothing but HATE from the tyrannical government! Sought a way to free himself and his comrades from the barbarians! He himself organized our GLORIOUS REVOLUTION! Fighting side by side with the men on the frontlines to ensure that no battle was lost! But eventually, he was GREIVOUSLY WOUNDED and using the power of experimental technology stolen from our OPPRESSORS, he was REBUILT into the hero we know him today!</i>\n
\n
The bunker trembles, as its only illumination swings back and forth in response. His past had caught up to him, and whose work would come haunt him.\n
None know the truth of his actions; his job was to cover it up after all. But such a job required such mental tenacity to endure painting over the facts.\n
Screams could be heard outside the bunker, only to be drowned into what sounded like sludge.\n
\n
<i>DESOLATOR returned to the front of battle! rescuing all his fellow compatriots-</i>\n
"They were massacred."\n
<i>lest they bear the same burden as him!-</i>\n
"You bared our brothers in arms as much suffering as you did to the enemy."\n
<i>DESOLATOR AND HIS COMRADE'S REVOLUTION SAVED OUR PLANET, IN WHICH HE BUILT THE SOCIETY WE HAVE TODAY!</i>\n
"All you left was a red river of bodies for a cause you didn't believe, the saviour of the revolution had the perfect excuse."\n
\n
But before he could continue watching the tape, the frantic man felt his skin crawl, and sweat build. He was here.\n
Before another thought can be processed, the door glows an iridescent green and begins to melt. Bolts that held the door shut, that could withstand nuclear fallout, melted like butter against a hot knife. The remains of the barricade liquidated across the floor, lumps of metal and furniture; burning, melded into a radioactive soup. It's him, it's-\n
\n
<i>DESOLATOR!</i>\n
\n
The film solemnly rings; before sputtering what's left of the record. Eventually dissolving like the door before it.\n
Before words could be exchanged, the dying man's melting flesh began to split and slip off his body, akin to slow cooked meat. His eyes became hazy, taking one last look at the radioactive presence as he spoke the last words the man would ever hear.\n
"Shhhh, the end is near."\n
Brain sloshing, muscle dripping to reveal the skeleton underneath, as even the atomic structure of bone started liquifying. Soon, the room itself was becoming unstable; the last things to leave were the heavy, steel-boot footsteps and heavy breathing. His respiration being the only thing human about him.
 */