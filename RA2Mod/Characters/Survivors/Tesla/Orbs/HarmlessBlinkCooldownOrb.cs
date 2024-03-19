using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Orbs
{
    public class HarmlessBlinkCooldownOrb : Orb
    {
        public GameObject ownerGameObject = null;
        public ModdedLightningType moddedLightningType = ModdedLightningType.Ukulele;
        public float speed = -1;

        public override void Begin()
        {
            base.Begin();

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

        public override void OnArrival()
        {

            if (target && ownerGameObject)
            {
                ownerGameObject.GetComponent<TeslaTrackerComponent>().AddCooldownTarget(target.healthComponent);
            }
        }
    }
}
