using System;
using System.Collections.Generic;
using System.Text;
using ModdedEntityStates.Aliem;
using Modules;

namespace Modules {
    internal static class EntityStates {
        public static void Init() {
            Content.AddEntityState(typeof(AliemCharacterMain));

            Content.AddEntityState(typeof(RayGunFire));
            Content.AddEntityState(typeof(RayGunFireUncharged));
            Content.AddEntityState(typeof(RayGunChargedFire));
            Content.AddEntityState(typeof(RayGunInputs));
            Content.AddEntityState(typeof(RayGunInstant));

            Content.AddEntityState(typeof(SwordCharging));
            Content.AddEntityState(typeof(SwordFire));
            Content.AddEntityState(typeof(SwordFireCharged));
            Content.AddEntityState(typeof(SwordFireChargedDash));
            Content.AddEntityState(typeof(SwordInputs));

            Content.AddEntityState(typeof(AliemLeapM2));
            Content.AddEntityState(typeof(AliemLeapM3));
            Content.AddEntityState(typeof(AliemRidingChomp));
            Content.AddEntityState(typeof(AliemRidingState));
            Content.AddEntityState(typeof(EndRidingState));
            Content.AddEntityState(typeof(AliemBurrow));
            
            Content.AddEntityState(typeof(ThrowGrenade));
            Content.AddEntityState(typeof(ScepterThrowGrenade));
        }
    }
}
