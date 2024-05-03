using RA2Mod.Modules;
using RA2Mod.Survivors.Desolator.States;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorStates
    {
        public static void Init()
        {
            Content.AddEntityState(typeof(DesolatorCharacterMain));

            Content.AddEntityState(typeof(RadBeam));
            Content.AddEntityState(typeof(AimBigRadBeam));

            Content.AddEntityState(typeof(RadiationAura));

            Content.AddEntityState(typeof(DeployEnter));
            Content.AddEntityState(typeof(DeployIrradiate));
            Content.AddEntityState(typeof(DeployCancel));

            Content.AddEntityState(typeof(ScepterDeployEnter));
            Content.AddEntityState(typeof(ScepterDeployIrradiate));

            Content.AddEntityState(typeof(ThrowIrradiator));
            Content.AddEntityState(typeof(ScepterThrowIrradiator));

            Content.AddEntityState(typeof(EmoteRadiationProjectile));
        }
    }
}