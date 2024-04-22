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
- custom lightningorb effects
- improved animations (help)
- ~~Scepter and vr and all those fun stuffs~~
- ~~Desolator Alt Character~~
- Driver compat
- Chrono Legionnaire, G.I.
- Conscript, Prisma Trooper

___
for no particular reason I made a cool skin for minecraft check it out   
[![](https://raw.githubusercontent.com/TheTimeSweeper/the/master/theUnityProject/Assets/_Kniggas/TeslaTrooper/TeslaBundle/textures/MC/MCSkin.png)]()
___
## Changelog
see Changelog tab for more  

`2.2.3`
- fixed desolator scepter resetting when zetaspects is installed
  - *skill will still reset if a buff is lost while in special because of code jank on scepter's end. full fix to completely workaround all this coming at some point.*
- fixed desolator rad cannon not spinning when deployed

`2.2.2`
- fixed Tesla Trooper Surging Forward while rooted locking you in purgatory 
- fixed Tesla Trooper M2 conflict with autosprint, only a year after it was reported
- Desolator passive now counts stacks from SS2U Nucleator dot

`2.2.1`
- forgot to update text to the changes of last patch woops
 
`2.2.0`
- tesla trooper
  - passive: ally buff multiplier 1.3 -> 1.1
  - passive: ally buff shock duration 1 - 3.5
  - secondary (empowered): 1200% -> 1500%  
    - *He gets a lot of feedback that he's very strong, this may surprise you, because he also gets a lot of feedback that he's weak.*  
    - *I suspect a giant culprit is those people don't know to buff the tower before using secondary, missing out on a lot of his potential damage (1200% * 1.3 = 1560%)*  
    - *These changes remove this aspect from being a necessity, in exchange buffing a bit of its utility when you do decide to use it*  
    - *If you liked that aspect, configs will come at some point so you can revert these changes*
  - primary: animation plays on each bolt  
    - *as well as the tower aspect, I suspect the people are also missing out on tripling close-range primary damage. more feedback on doing multiple hits should help with this.*
  - Utility: sound now plays when absorbing damage, increasing in pitch based on damage absorbed  
    - *thanks rob for the suggestion. I agree with all your others as well so they'll come at some point*
    - *except when you said it does a whole lot of nothing. late game it's a free 1800% with a giant aoe ya goon*
  - secondary (empowered): now commands Starstorm 2 Shock Drones as well
  - fix potential nullref spam on tracking component
- Desolator
  - fixed a bug where sceptered special reverts to regular special after using it
    - *I assume. I could not reproduce it so if it still happens to you, let me know and give a log*
  - he'll get some more love I promise
  