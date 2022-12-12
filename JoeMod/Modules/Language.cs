//using R2API;
using Zio.FileSystems;
using System.Linq;

namespace Modules {
    internal static class Language {
        public static SubFileSystem fileSystem;
        internal static string languageRoot => System.IO.Path.Combine(Language.assemblyDir, "language");

        internal static string assemblyDir {
            get {
                return System.IO.Path.GetDirectoryName(FacelessJoePlugin.PluginInfo.Location);
            }
        }

        public static void RegisterLanguageTokens() {
            On.RoR2.Language.SetFolders += fixme;

            return;
        }

        //Credits to Anreol for this code
        private static void fixme(On.RoR2.Language.orig_SetFolders orig, RoR2.Language self, System.Collections.Generic.IEnumerable<string> newFolders) {
            if (System.IO.Directory.Exists(Language.languageRoot)) {
                var dirs = System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(Language.languageRoot), self.name);
                orig(self, newFolders.Union(dirs));
                return;
            }
            orig(self, newFolders);
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