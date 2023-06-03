using System;
using System.Collections.Generic;
using System.Text;
using ModdedEntityStates;
using ModdedEntityStates.Joe;
using Modules;

namespace Modules {
    internal static class EntityStates {
        public static void Init() {

            #region joe

            Content.AddEntityState(typeof(WindDownState));

            //prim
            Content.AddEntityState(typeof(JoeMain));

            Content.AddEntityState(typeof(Primary1Swing));
            Content.AddEntityState(typeof(Primary1JumpSwingFall));
            Content.AddEntityState(typeof(Primary1JumpSwingLand));

            Content.AddEntityState(typeof(PrimaryScepter1Swing));
            Content.AddEntityState(typeof(PrimaryScepter1JumpSwingFall));
            Content.AddEntityState(typeof(PrimaryScepter1JumpSwingLand));

            Content.AddEntityState(typeof(PrimaryStupidSwing));

            Content.AddEntityState(typeof(ThrowBoom));
            Content.AddEntityState(typeof(ThroBoomButCoolerQuestionMaark));

            //sec
            Content.AddEntityState(typeof(Secondary1Fireball));

            //ut
            Content.AddEntityState(typeof(Utility1Dash));
            Content.AddEntityState(typeof(Utility1ChargeMeleeDash));
            Content.AddEntityState(typeof(Utility1MeleeDashAttack));

            Content.AddEntityState(typeof(Special1Tenticles));
            #endregion
        }
    }
}
