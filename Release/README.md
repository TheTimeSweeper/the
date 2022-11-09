# Electrician In the Field
- Adds the Tesla Trooper, who can construct Tesla Towers to help him fry enemies.
- Fully fleshed out
  - Item displays and ragdoll
  - Fully animated (but by me so y'know)
- Fully multiplayer compatible
- Mod Support
  - Ancient Scepter
  - Skills++
  - Aetherium (Item Displays)
  - VRAPI
  - CustomEmotesAPI
- Y'all remember Red Alert 2?

[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/Release/readme/CSS.png)]()

Anything you'd like to say about the guy, ping me (`TheTimesweeper#5727`) on the ror2 modding discord or the enforcer discord (https://discord.gg/r5XTMFd4W7). I'll be taking a close look at feedback.
___
## Overview
Based on Tesla Trooper from Red Alert 2, but with SkeletorChampion coming in and saying "nah I'm model him to fit RoR2, and look awesome, and my dong is enormous".  
Tesla Trooper is a mid-range bruiser on his own, and an all-range monster when he builds his Tesla Tower.  

[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/Release/readme/zaps_combined.png)]()
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/theUnityProject/Assets/_Kniggas/TeslaTrooper/TeslaBundle/Icons/texIconTeslaTrooper.png)]()

## Credits
SkeletorChampion - Made the character model, which kicked off the mod. Without him, the mod wouldn't exist.  
Violet Chaolan - wwise sound help  
SweeperSecret - icons,  <3  
Mr.Bones - mastery skin  
Jaysian - Lore  
Moffein - consult, savior  
DeegerDill - consult  
Westwood Studios - sounds, inspiration  
  
Thanks to the lovely reception from the community, including all the great feedback and ideas c:

## Future Plans (that I may or may not get to)
- Alt skills
- Achievements
- custom lightningorb effects (help)
- improved animations (help)
- ~~Scepter and vr and all those fun stuffs~~
- Desolator Alt Character

___
for no particular reason I made a cool skin for minecraft check it out   
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/theUnityProject/Assets/_Kniggas/TeslaTrooper/TeslaBundle/textures/MC/MCSkin.png)]()
___
## Changelog
`1.3.2`
 - fixed eclipse not saving progress

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
  