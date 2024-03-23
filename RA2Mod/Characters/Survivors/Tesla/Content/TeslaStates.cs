using RA2Mod.Modules;
using RA2Mod.Survivors.Tesla.States;
using RA2Mod.General.States;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaStates { 
        public static void Init()
        {
            Content.AddEntityState(typeof(TeslaCharacterMain));

            Content.AddEntityState(typeof(Zap));

            Content.AddEntityState(typeof(ZapPunch));
            Content.AddEntityState(typeof(ZapPunchWithDeflect));
            Content.AddEntityState(typeof(ChargeZapPunch));
            Content.AddEntityState(typeof(FireChargedZapPunch));

            Content.AddEntityState(typeof(AimBigZap));
            Content.AddEntityState(typeof(BigZap));

            Content.AddEntityState(typeof(ShieldZapStart));
            Content.AddEntityState(typeof(ShieldZapCollectDamage));
            Content.AddEntityState(typeof(ShieldZapReleaseDamage));

            Content.AddEntityState(typeof(BlinkZap));

            Content.AddEntityState(typeof(DeployTeslaTower));
            Content.AddEntityState(typeof(DeployTeslaTowerScepter));

            Content.AddEntityState(typeof(Rest));
        }
    }
}
