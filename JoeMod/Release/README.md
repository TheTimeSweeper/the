# Electrician In the Field
Adds the Tesla Trooper, who can construct Tesla Towers to help him fry enemies.
- Item displays and ragdoll
- Alt skills and skins with achievements
- Fully multiplayer compatible
- Mod Support
  - Ancient Scepter
  - BetterUI (proc coefficients)
  - Skills++
  - Aetherium (Item Displays)
  - CustomEmotesAPI
  - VRAPI
- Y'all remember Red Alert 2?
# SPREAD THE DOOM
Adds the Desolator, who spreads large spheres of radiation everywhere in his wake.  
- Item displays and ragdoll
- Fully multiplayer compatible
- Mod Support
  - Ancient Scepter
  - BetterUI (proc coefficients)
  - Skills++ (one skill lol)
  - Aetherium (Item Displays)
  - CustomEmotesAPI

[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/JoeMod/Release/_readme/CSS2.png)]()

Anything you'd like to say about the guys, ping me (`TheTimesweeper#5727`) on the ror2 modding discord or the enforcer discord (https://discord.gg/r5XTMFd4W7).  
___
## Overview
Based on their respective units from Red Alert 2, but with SkeletorChampion coming in and saying "nah I'm model them to fit RoR2, and look awesome, and my dong is enormous".

Tesla Trooper is a mid-range bruiser on his own, and an all-range monster when he builds his Tesla Tower.  
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/JoeMod/Release/_readme/zaps_combined.png)]()

Desolator is a walking powerhouse of radiation and area damage.  
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/JoeMod/Release/_readme/rad.png)]()

[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/JoeMod/Release/_readme/Icons.png)]()

## Credits
SkeletorChampion - Made the character models (which kicked off the mod. Without him, the mod wouldn't exist.), and grandmastery skin  
Violet Chaolan - wwise sound help  
SweeperSecret - icons,  <3  
Mr.Bones - mastery skins  
Jaysian - Lores  
SOM - animation help  
Moffein - consult, savior  
DeegerDill - consult  
Westwood Studios - sounds, inspiration  

Thanks to the lovely reception from the community, including all the great feedback and ideas c:

## Languages
If you'd like to translate to your language, check out the [language folder on Github](https://github.com/TheTimeSweeper/the/tree/master/JoeMod/Release/plugins/Language).  
Bazillion thanks to those that have, and in advance to those that may.

Currently Supported:
- English
- Ukrainian - by Damglador
- Spanish - by Juhnter
- Russian - by Nikto0o
- French - by Fyrebw
- BR Portuguese - by Kauzok
- Chinese (simplified) - by Rody, and FallenTroop

## Future Plans (that I may or may not get to)
- Alt skills
- Achievements
- custom lightningorb effects (help)
- improved animations (help)
- ~~Scepter and vr and all those fun stuffs~~
- ~~Desolator Alt Character~~

___
for no particular reason I made a cool skin for minecraft check it out   
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/theUnityProject/Assets/_Kniggas/TeslaTrooper/TeslaBundle/textures/MC/MCSkin.png)]()
___
## Changelog

`2.1.6`
- added failsafe fix for current incompatiblility with Shaman (must play default skin until he pulls my fix)
- added simplified chinese translation (thanks Rody and FallenTroop!)
- fixed Desolator scepter skills multiplayer issues
- fixed Desolator scepter alt special description displaying wrong damage

`2.1.5`
- added Brazilian Portuguese translation (thanks Kauzok!)
- added mastery skin for Desolator (thanks again, Mr.Bones)
  - google translated the name for other languages, sorry if something's wrong!
- haven't touched this project in several months so I hope nothing broke making this update!

`2.1.4`
 - added french translation (thanks Fyrebw!)

`2.1.3`
 - added russian translation (thanks Nikto0o!)
 - attempt to optimized desolator's big-ass specials by simplifying the hitboxes
   - *this makes the cube hitbox wildly inaccurate to the sphere visual so enjoy the extra range I suppose* 
 - removed joe

`2.1.2`
 - r2api split ass(emblies)
 - now that colorsapi is real, added a color to communicate tesla trooper charged ally attacks
 - added some missing text to language file

`2.1.1`
 - added Ukrainian translation (thanks Damglador!)
 - added Spanish translation (thanks Juhnter!)
 - fixed desolator disable config breaking both characters
 - fixed desolator irradiator projectile collisions being inconsistent
 - added buff icon for desolator deployed state
 - lowered desolator sounds
 - adjusted sound for desolator utility

`2.1.0`
 - added sit emotes for both men
 - added language support. thanks to Damglador for pushing for it, and thanks to Moffein and Anreol for the code
   - if you'd like to translate to your language, check out the [language folder on Github](https://github.com/TheTimeSweeper/the/tree/master/JoeMod/Release/plugins/Language)
 - made desolator back pack tube thing change color with recolors
 - fixed "voice line in css" config for playing wrong voice lines for desolator
 - fixed not being able to play voice lines while using certain abliites
 - finally came out of the past and separated assetbundles and soundbank from dll

`2.0.1`
 - fixed utility broken

`2.0.0`
 - released new character Desolator!
 - added new alt secondary skill for tesla trooper, expanding on alt m1 in cursed config 
 - added achievement for alt m1. still in cursed for now
 - config has been reorganized. you should probably just delete existing
 - finished tower item displays for sotv items
 - fixed level growth stats not being to the vanilla standard
 - lowered distance scaling on sounds
 - added head hurtbox proper
 - small tweaks to the russion section of lore (thanks Damglador)

`1.3.2`
 - fixed eclipse not saving progress
 - attempt fix to targeting just not wanting to target sometimes

`1.3.1`
 - fixed emoteapi rig

`1.3.0`
 - added rig for EmoteAPI. happy now?
 - freed alt Util from cursed config and added as a proper skill variant
   - improved reticle targeting, separate from m1 targeting
   - icon and sound
   - unlockable by achievement
   - *haven't tested much in multiplayer but I'm pretty sure it should work fine?*
 - added additional property for scepter: tower now zaps multiple enemies at once
   - *kept multiple towers by the rule of cool*
   - *it's probably way overtuned now, so I'll maybe dial it back in the future. for now, have fun c:*
 - finally fixed malachite aspect destroying his and his towers' bones
 - adjusted m1 reticle to help more clearly read the 3 different tiers of range
 - added secret beta config

`1.2.1`
 - fixed conflict with ttgl mod and vrapi

`1.2.0`
 - new Grand Mastery skin! Thanks as always to the lovely SkeletorChampion
   - comes with a unique tower
   - comes with a few custom effects
 - holy shit VR
   - zaps with right hand, build tower with left hand
   - all skins supported
 - Added Lore by Jaysian, thanks!
 - bumped up damage of cursed config alt primary
   - *if it's not gonna make sense may as well be strong*
 - Added new heavy WIP Alt Utility in cursed config: Surging Forward
   - *not really sure where I was going with this one but turned out kinda fun so y not*
 - fixed tower blocking its own projectiles, mainly ATG missiles

`1.1.1` buncha tweaks
- ally zap no longer does damage, fixing pennies exploit
- zapping allies with m1 now ends the move earlier
  - *so accidentally hitting allies doesn't eat up a full duration m1*
- m1 zap now travels instantly
- slightly lowered m1 attack duration
  - *not enough to affect any balance concerns, just to hopefully feel a little smoother*
- fixed m1 not blooming crosshair for clients
- lowered lingering m2 cast time
- added very WIP m1 alt skill under cursed config

`1.1.0`
- Added proper Mastery skin, complete with a unique tower
  - *thanks Mr.Bones!*
- Added Scepter Upgrade
  - ~~*but by accident I did exactly lysate cell, so I'm open to any better ideas*~~
- Limited lysate cell to 1 additional tower, similar to engi
  - *stacking simultaneous towers turned out way too strong for a green rarity item*  
  - *truthfully it should be 0 but I want the opportunity for multiple towers in some capacity*  
  - *unlimited stacking behavior can be reverted in config*
- Fixed Utility's cooldown to start after the move is done
- Fixed missing tower sounds in multiplayer
- lowered sound distance so they don't dominate the battlefield
  - *let me know if they're too quiet now*
- Removed dependency on FixPluginTypesSerialization

`1.0.2`
- accidentally cranked up m1 distance way too high woops

`1.0.1`
- fixed tower getting taken by void infestors
- bumped up tracking range to help deal with xi construct
- adjusted m1 visual to make separate arcs a little more visible
- added config to disable tower item displays if you find them too silly
- added Aetherium item displays
- added item displays just for Tinker's Satchel mustaches 

`1.0.0`
- c:
  