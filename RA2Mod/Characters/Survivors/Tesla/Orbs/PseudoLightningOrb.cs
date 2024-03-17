using RoR2;
using RoR2.Orbs;
using TeslaTrooper;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Orbs
{
    public class PseudoLightningOrb : LightningOrb
    {

        public ModdedLightningType moddedLightningType;

        // Token: 0x060040C4 RID: 16580 RVA: 0x0010BF94 File Offset: 0x0010A194
        public override void Begin()
        {

            if (speed <= 0)
            {
                duration = 0.0001f;
            }
            else
            {
                duration = distanceToTarget / speed;
            }

            GameObject effect = null;
            switch (moddedLightningType)
            {
                case ModdedLightningType.Ukulele:
                    effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect");
                    break;
                case ModdedLightningType.Tesla:
                    effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/TeslaOrbEffect");
                    break;
                case ModdedLightningType.BFG:
                    effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/BeamSphereOrbEffect");
                    break;
                case ModdedLightningType.Loader:
                    effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect");
                    break;
                case ModdedLightningType.MageLightning:
                    effect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/MageLightningOrbEffect");
                    break;
                case ModdedLightningType.Nod:
                    effect = TeslaAssets.TeslaLightningOrbEffectRed;
                    break;
                case ModdedLightningType.NodMage:
                    effect = TeslaAssets.TeslaMageLightningOrbEffectRed;
                    break;
                case ModdedLightningType.NodMageThick:
                    effect = TeslaAssets.TeslaMageLightningOrbEffectRedThick;
                    break;

            }
            EffectData effectData = new EffectData
            {
                origin = origin,
                genericFloat = duration
            };
            effectData.SetHurtBoxReference(target);
            EffectManager.SpawnEffect(effect, effectData, true);
        }
    }
}

namespace TeslaTrooper
{
    public enum ModdedLightningType
    {
        Ukulele,
        Tesla,
        BFG,
        Loader,
        MageLightning,
        Nod,
        NodMage,
        NodMageThick
    }
}
