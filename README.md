# wew what a mess
See [here](https://github.com/TheTimeSweeper/RoR2RedAlert) for the latest development on Tesla Trooper, Desolator, and now Chrono Legionnaire (and GI and Conscript and)

If you would like to work on any of these guys, let me know and I'll split them into their own repo.

If you want to build any of these mods directly from here, uhhhh
1. import thunderkit with these settings
 
|Configuration | Set To | Why |
|---|-|---|
|Check Unity Version | On | |
|Disable Assembly Updater | On | |
|Post Processing Unity Package Installer | Off | already done in manifest |
|Assembly Publicizer | On | |
|MMHook Generator | Off | we're not building the mod from the editor so we don't need extra bloat |
|Import assemblies | On |  |
|Import Project Settings | Off | should already be imported but if it's not for you turn this on |
|Set Deferred Shading | On |  |
|Create Game Package | On |  |
|Import Addressable Catalog | On | addressable browser is dope |
|Configure Addressable Graphics Settings | On |  |
|Ensure RoR2 Thunderstore Source | Off | |
|Install BepInEx| Off |  |
|R2API Submodule Installer | Off | ah yes who wouldn't want 28 packages slowing down compiling, playing, and building? |
|Install RoR2 Compatible Unity Multiplayer HLAPI | Off | we're not weaving in editor so we don't need this |
|Install RoR2 Editor Kit | Off |  |
|the rest | On |  |

2. hope and pray. I haven't tested this so let me know