using RA2Mod.Minions.TeslaTower;
using RA2Mod.Minions.TeslaTower.States;
using RA2Mod.Modules;
using RA2Mod.Survivors.Tesla.Achievements;
using RA2Mod.Survivors.Tesla.States;
using System;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaTokens
    {
        public static void Init()
        {
            TrooperTokens();
            TowerTokens();
            CompatTokens();
        }

        private static void TrooperTokens()
        {
            string prefix = TeslaTrooperSurvivor.TOKEN_PREFIX;

            string desc = "The Tesla Trooper is a close-mid-range bruiser, who can construct Tesla Towers to empower his combat potential.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                        + "< ! > Use Tesla Gauntlet close range to deal the most damage. The reticle will reflect this" + Environment.NewLine + Environment.NewLine
                        + "< ! > Use 2000 Volts to control crowds, and to command your tower to wipe crowds" + Environment.NewLine + Environment.NewLine
                        + "< ! > You benefit from being closer to enemies, use Charging Up to assist with this." + Environment.NewLine + Environment.NewLine
                        + "< ! > The Tesla Tower's largest damage output is through empowering your secondary skill!" + Environment.NewLine + Environment.NewLine;

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

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Gauntlet");

            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"{Tokens.UtilityText("Charging")}. Zap targeted units with a bolt of electricity for {Tokens.DamageText($"{Zap.DamageCoefficient * 100}% damage")}. Casts {Tokens.UtilityText($"up to 3 bolts")} at {Tokens.UtilityText($"close range")}.");

            LanguageAPI.Add(prefix + "KEYWORD_CHARGED", $"<style=cKeywordName>Charging</style><style=cSub>A charged ally has their next attack {Tokens.UtilityText("shocking")} and damage boosted by {Tokens.DamageText($"{TeslaConfig.M1_Zap_ConductiveAllyBoost.Value}x")}");

            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Tesla Knuckles");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", $"Punch enemies for {Tokens.DamageValueText(ZapPunch.DefaultDamageCoefficient)}, and zap enemies in a cone for {Tokens.DamageValueText(ZapPunch.DefaultDamageCoefficient * ZapPunch.OrbDamageMultiplier)}. {Tokens.UtilityText("Deflects Projectiles")}.");

            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION",
                $"{Tokens.UtilityText("Stunning.")} Create an electric blast in a large area for {Tokens.DamageValueText(BigZap.DamageCoefficient)}. " +
                $"While near a {Tokens.UtilityText("Tesla Tower")}, perform an {Tokens.UtilityText("empowered, shocking")} version for {Tokens.DamageValueText(TowerBigZap.DamageCoefficient)}.");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAPPUNCH_NAME", "Charged Fist");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAPPUNCH_DESCRIPTION", $"Hold to charge a punch for {Tokens.DamageText("80%-800% damage")}, and zap enemies in a cone for {Tokens.DamageText("40%-400% damage")}. " +
                $"While near a {Tokens.UtilityText("Tesla Tower")}, replace zap cone with a long-range {Tokens.UtilityText("shocking")} beam for {Tokens.DamageText("120%-1200% damage")}");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_NAME", "Charging Up");
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_DESCRIPTION",
                $"For {Tokens.UtilityText($"{ShieldZapCollectDamage.ShieldBuffDuration} seconds")}, " +
                $"{Tokens.UtilityText(TeslaConfig.UtilityDamageAbsorption * 100 + "% of damage")} is negated, and {Tokens.UtilityText("absorbed")}, " +
                $"After which, {Tokens.DamageText("blast")} in a wide area {Tokens.DamageText("based on damage absorbed")}.");

            LanguageAPI.Add(prefix + "UTILITY_BLINK_NAME", "Surging Forward");
            LanguageAPI.Add(prefix + "UTILITY_BLINK_DESCRIPTION",
                $"{Tokens.UtilityText("Stunning.")} Become a {Tokens.UtilityText("bolt of electricity")} and surge toward a targeted enemy, dealing {Tokens.DamageValueText(BlinkZap.DamageCoefficient)}. {Tokens.DamageText("Can be casted again")} within a short window. Cannot target the same enemy twice.");

            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            string target = TeslaConfig.M4_Tower_Targeting.Value ? "targeted" : "nearby";
            string specialDesc =
                $"Construct a Tesla Tower for {Tokens.UtilityText($"{TowerLifetime.LifeDuration} seconds")} that zaps {target} units for {Tokens.DamageText($"3x{TowerZap.DamageCoefficient * 100}% damage")}. " +
                $"Enhances Secondary skill.";
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_NAME", "Tesla Network");
            LanguageAPI.Add(prefix + "SPECIAL_SCEPTER_TOWER_DESCRIPTION", specialDesc + Tokens.ScepterDescription($"Lowered Cooldown, Additional Stock, Tower Zaps up to {TowerZapMulti.extraZaps} simultaneous targets"));
            #endregion

            #region Achievements

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaMasteryAchievement.identifier), $"{fullName}: Mastery");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaMasteryAchievement.identifier), $"As {fullName}, beat the game or obliterate on Monsoon.");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaGrandMasteryAchievement.identifier),           $"{fullName}: Grand Mastery");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaGrandMasteryAchievement.identifier), $"As {fullName}, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaTowerBigZapAchievement.identifier),        $"Big Zap");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaTowerBigZapAchievement.identifier), $"As {fullName}, command your Tesla Tower to use 2000 Volts on 10 enemies at once");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaAllyZapAchievement.identifier),        $"Ally Zap");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaAllyZapAchievement.identifier), $"As {fullName}, charge an ally with Tesla Gauntlet 3 times in one run. (ally must attack with the charge in order to be charged again)");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaShieldZapKillAchievement.identifier), $"Shield Zap");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaShieldZapKillAchievement.identifier), $"As {fullName}, defeat a boss monster using Charging Up");

            //scrapped because boring
            //LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaZapCloseRangeAchievement.identifier),        $"Close Zap");
            //LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaZapCloseRangeAchievement.identifier), $"As {fullName}, zap an enemy at close range 20 times in a row");

            LanguageAPI.Add(Tokens.GetAchievementNameToken(TeslaRepairTowerAchievement.identifier), $"Our Power");
            LanguageAPI.Add(Tokens.GetAchievementDescriptionToken(TeslaRepairTowerAchievement.identifier), $"Repair a tesla coil with any source of power (Fuel Array, Royal Capacitor, Unstable Tesla Coil, Charged Perforator, Ukelele, Genesis Loop, etc).");

            #endregion
        }

        private static void TowerTokens()
        {
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
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", $"Zap nearby units for {Tokens.DamageText($"3x{TowerZap.DamageCoefficient}")}.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts (Tower)");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"{Tokens.UtilityText("Shocking")}. Performs an ${Tokens.DamageText("empowered")} blast at Tesla Trooper's target for {Tokens.DamageValueText(TowerBigZap.DamageCoefficient)}");
            #endregion
        }

        private static void CompatTokens()
        {
            string prefix = TeslaTrooperSurvivor.TOKEN_PREFIX;

            LanguageAPI.Add("TESLA_PRIMARY_ZAP_UPGRADE_DESCRIPTION", "Every 2 levels, <style=cIsUtility>+1</style> Close-Range Bolt");
            LanguageAPI.Add("TESLA_SECONDARY_BIGZAP_UPGRADE_DESCRIPTION", "<style=cIsUtility>+10%</style> Area, <style=cIsDamage>+10%</style> Damage");
            LanguageAPI.Add("TESLA_UTILITY_SHIELDZAP_UPGRADE_DESCRIPTION", "<style=cIsUtility>+0.5</style> second buff time");
            LanguageAPI.Add("TESLA_SPECIAL_TOWER_UPGRADE_DESCRIPTION", "<style=cIsUtility>+1</style> second lifetime, <style=cIsUtility>additional stock</style> every 3 levels");
            LanguageAPI.Add("TESLA_SPECIAL_SCEPTER_TOWER_UPGRADE_DESCRIPTION", "<style=cIsUtility>+1</style> second lifetime, <style=cIsUtility>additional stock</style> every 3 levels");


            LanguageAPI.Add(prefix + "PROC_BOLTS", "Bolts");
            LanguageAPI.Add(prefix + "PROC_BOLT", "Bolt");
            LanguageAPI.Add(prefix + "PROC_BLAST", "Blast");
            LanguageAPI.Add(prefix + "PROC_FIST", "Fist");
            LanguageAPI.Add(prefix + "PROC_BEAM", "Charged Beam");
        }
    }
}
