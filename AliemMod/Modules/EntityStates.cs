using System;
using System.Collections.Generic;
using System.Text;
using ModdedEntityStates.Aliem;
using Modules;

namespace Modules {
    internal static class EntityStates {
        public static void Init() {
            Content.AddEntityState(typeof(AliemCharacterMain));

            Content.AddEntityState(typeof(RayGun));
            Content.AddEntityState(typeof(RayGunBig));
            Content.AddEntityState(typeof(RayGunInputs));

            Content.AddEntityState(typeof(AliemLeapM2));
            Content.AddEntityState(typeof(AliemLeapM3));
            Content.AddEntityState(typeof(AliemRidingChomp));
            Content.AddEntityState(typeof(AliemRidingState));
            Content.AddEntityState(typeof(AliemBurrow));
            
            Content.AddEntityState(typeof(ThrowGrenade));
        }
    }
}
