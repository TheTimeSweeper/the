using ModdedEntityStates.Genji;

namespace JoeModForReal.Content.Survivors {
    public static class GenjiStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ThrowShuriken));
            Modules.Content.AddEntityState(typeof(ThrowShurikenAlt));
            Modules.Content.AddEntityState(typeof(Deflect));
            Modules.Content.AddEntityState(typeof(Dash));
            Modules.Content.AddEntityState(typeof(DragonBlade));
            Modules.Content.AddEntityState(typeof(DragonBladeIntro));
            Modules.Content.AddEntityState(typeof(DragonBladeOutro));
            Modules.Content.AddEntityState(typeof(SwingBlade));
        }
    }
}