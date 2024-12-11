## Changelog

`1.2.4`
- fixed for damagesource update
- fixed cursed BBGun weapon
- sword projectile is client side again

`1.2.3`
- fixed incredible nullref spam from the latest update

`1.2.2`
- fixed fps issues

`1.2.1` nevermind DamageAPI is fixed
- returned chomp decapitating and broken achievements
- fixed missing footsteps
- dive can now be aimed downwards (this was in a previous patch idk which)

`1.2.0` half updated for sots
- damagetypes are disabled until r2api.damagetypes is fixed. for now this means:
    - chomp will not do decaptiate effect on kill
    - some achievements are removed, their skills unlocked by default
- sots fuckery. these issues are likely out of my control until gearbox fixes their issues
    - sword projectile is no longer client authoritative. it might feel like ass on client. still works though
    - hold to charge weapons now charge faster/slower based on fps
    - sword dash now goes much further/shoerter baed on fps
    - human machine gun charged shots shoot much faster/slower based on fps
    - close range knife attacks faster/slower based on fps
    - charactermotor onhitground changes have made burrowing inconsistent
    - wew
- wip BBGun weapon in cursed config untested for time. will be fixed/released with full patch

`1.1.0`
- added config to remove mashing, allowing to simply hold to shoot. this removes the ability to hold to charge of course
    - let me know if this causes any issues
- added new WIP weaopn in cursed config, BBGun
    - Ignore all the empty config entries haha
    - let me know if this causes any issues
  - *shoots a ton of bees, but they're optimized as fuck. if you have potato pc please reach out and let me know if it hurts your frames at all*

`1.0.1`
- fixed close range knife not activating always
- tweaked its hitboxes, added it to config
- fixed skins breaking when raindroplobotomy was installed

`1.0.0`
shout out to rob. this update wouldn't have happened if he didn't give the boy some attention
- added energy sword
- added human machine gun
- added sawed off
- added Weapon Swap specials, replacing secondary with a second primary
- added somwhat ror-friendly gup skin (thanks tsuyoikenko!) (config to set as default and change icon)
- solidified mash to shoot and hold to charge functionality for primaries
  - mash at pretty lenient pace and it will auto shoot at max attack speed
  - hold and it will charge up a charged attack
- secondary now fires charged version of primary at max charge, depending on selected primary
- updated most animations and added new ones (thanks rob!)
- a few nerfs for a few reasons. long story short I care about him now
- added configs for pretty much everything, available in Risk of Options
- added shooting while diving and riding
- added armor while diving and riding
- added a double jump which can cancel dive
- severely improved riding presentation
- added hitbox on popping out of burrowing
- added config ability to control movement of the enemy or ally AI you're riding. behind config cause it's probably buggy
- added proper AI
- added shank on close range ray gun (in reference to the original game)
- increased hitbox size of projectiles with respect to enemies but not world
- separated assetbundles n stuff
- added language support

`0.3.2`
- increased damage on ray gun
  - *projectile is harder to hit (especially in mp), so it can afford to do higher than commando-tier damage*
- increased chomp heal from 10% max health to 30%
- lowered chomp cooldown and damage
- chomp now has `BonusToLowHealth` damage type  

*these changes should hopefully make his kit flow a little better, and give some needed buffs*  
*(from what I know. If I buffed him too much let me know)*

`0.3.1`
- leap now only rides if you're holding the input 
  - config for always ride
  - you're welcome, contra c:
- slightly toned down the range on riding detecting
- readded hitbox to leap
- added scepter why not
- added another silly way to shoot raygun in cursed
- moved up aim origin so projectiles hit the floor less
- reordered so he's not in between tesla trooper and desolator
- fixed stats not being to vanilla standard

`0.3.0`
- fixed projectiles in multiplayer
- fixed riding error spam in multiplayer
- networked riding position for clients. 
  - *each player will see their proper riding position on their screen, but on other screens they will floating behind*
- can now ride allies, which gives them a movement buff
  - *was already giving enemies a movement buff to simulate panicking. simply applies to allies as well*
- chomp now heals for 10% of max health
- borrowed some sounds from the game for his projectiles' impacts
- added his upper and lower jaw to ragdoll

*likely (hopefully, barring bugs) my last update for this guy until I sometime later decide to revisit him*  
*have fun and thanks for havin fun c:*

`0.2.1`
- fixed another bug with ridden enemies dying causing lockups when certain mods were installed
- fixed character collider causing him to spawn through the floor out of the pod
  
`0.2.0`
- added item displays
- chomp now decapitates enemies if it kills them
- fixed bug with riding certain entities
- adjusted character collider to more suit his tiny body

`0.1.0`
- c: