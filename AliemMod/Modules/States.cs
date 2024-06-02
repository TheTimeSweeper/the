using System;
using System.Collections.Generic;
using System.Text;
using ModdedEntityStates.Aliem;
using Modules;

namespace Modules {
    internal static class States {
        public static void Init() {
            Content.AddEntityState(typeof(AliemCharacterMain));

            Content.AddEntityState(typeof(RayGunFire));
            Content.AddEntityState(typeof(RayGunFireUncharged));
            Content.AddEntityState(typeof(CloseRangeKnife));
            Content.AddEntityState(typeof(RayGunChargedFire));

            Content.AddEntityState(typeof(InputRayGun));
            Content.AddEntityState(typeof(ChargeRayGun));

            Content.AddEntityState(typeof(RayGunInstant));


            Content.AddEntityState(typeof(SwordFire));
            Content.AddEntityState(typeof(SwordFireCharged));
            Content.AddEntityState(typeof(SwordFireChargedDash));

            Content.AddEntityState(typeof(InputSword));
            Content.AddEntityState(typeof(ChargeSword));


            Content.AddEntityState(typeof(ShootRifleUncharged));
            Content.AddEntityState(typeof(ShootRifleCharged));

            Content.AddEntityState(typeof(InputRifle));
            Content.AddEntityState(typeof(ChargeRifle));


            Content.AddEntityState(typeof(ChargedLunarNeedleFire));


            Content.AddEntityState(typeof(AliemLeapM2));
            Content.AddEntityState(typeof(AliemLeapM3));
            Content.AddEntityState(typeof(AliemRidingChomp));
            Content.AddEntityState(typeof(AliemRidingState));
            Content.AddEntityState(typeof(EndRidingState));
            Content.AddEntityState(typeof(AliemBurrow));
            

            Content.AddEntityState(typeof(ThrowGrenade));
            Content.AddEntityState(typeof(ThrowGrenadeScepter));

            Content.AddEntityState(typeof(SwapSecondaryWeapon));
            Content.AddEntityState(typeof(SwapSecondaryWeaponScepter));
        }
    }
}
