using ModdedEntityStates.Joe;
using ModdedEntityStates.BaseStates;
using System.Collections.Generic;
using System;
using ModdedEntityStates;
using ModdedEntityStates.TeslaTrooper.Tower;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.Desolator;

namespace Modules
{
    internal static class States
    {
        public static void RegisterStates() {
            #region joe
            //prim
            Content.AddEntityState(typeof(JoeMain));

            Content.AddEntityState(typeof(Primary1Swing));
            Content.AddEntityState(typeof(Primary1JumpSwingFall));
            Content.AddEntityState(typeof(Primary1JumpSwingLand));

            Content.AddEntityState(typeof(PrimaryStupidSwing));

            Content.AddEntityState(typeof(ThrowBoom));
            Content.AddEntityState(typeof(ThroBoomButCoolerQuestionMaark));

            //sec
            Content.AddEntityState(typeof(Secondary1Fireball));
            #endregion

            Content.AddEntityState(typeof(WindDownState));
            
            #region tesla trooper
            Content.AddEntityState(typeof(TeslaTrooperMain));

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

            Content.AddEntityState(typeof(TeslaVoiceLines));
            #endregion tesla trooper

            #region tesla tower

            Content.AddEntityState(typeof(TowerSpawnState));
            Content.AddEntityState(typeof(TowerIdleSearch));

            Content.AddEntityState(typeof(TowerLifetime));
            Content.AddEntityState(typeof(TowerUndeploy));
            Content.AddEntityState(typeof(TowerSell));

            Content.AddEntityState(typeof(TowerZap));
            Content.AddEntityState(typeof(TowerBigZap));
            Content.AddEntityState(typeof(TowerBigZapGauntlet));

            //scepter
            Content.AddEntityState(typeof(TowerSpawnStateScepter));
            Content.AddEntityState(typeof(TowerIdleSearchScepter));

            Content.AddEntityState(typeof(TowerZapMulti));
            #endregion tesla tower

            #region desolator
            if (FacelessJoePlugin.Desolator) {
                Content.AddEntityState(typeof(DesolatorMain));

                Content.AddEntityState(typeof(RadBeam));
                Content.AddEntityState(typeof(AimBigRadBeam));

                Content.AddEntityState(typeof(RadiationAura));

                Content.AddEntityState(typeof(DeployEnter));
                Content.AddEntityState(typeof(DeployIrradiate));
                Content.AddEntityState(typeof(DeployCancel));

                Content.AddEntityState(typeof(ThrowIrradiator));

                Content.AddEntityState(typeof(EmoteRadiationProjectile));

                Content.AddEntityState(typeof(DesolatorVoiceLines));
            }
            #endregion desolator
        }
    }
}