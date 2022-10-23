using System;
using System.Collections.Generic;
using System.Text;
using Modules;

namespace Modules {
    internal static class EntityStates {
        public static void Init() {
            Content.AddEntityState(typeof(ModdedEntityStates.Aliem.AliemCharacterMain));

            Content.AddEntityState(typeof(ModdedEntityStates.Aliem.AliemLeap));
            Content.AddEntityState(typeof(ModdedEntityStates.Aliem.AliemRidingChomp));
            Content.AddEntityState(typeof(ModdedEntityStates.Aliem.AliemRidingState));

            Content.AddEntityState(typeof(ModdedEntityStates.Aliem.RayGun));
        }
    }
}
