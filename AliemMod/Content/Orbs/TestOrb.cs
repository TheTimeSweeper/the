using HG;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace AliemMod.Content.Orbs
{
    public class TestOrb : Orb
    {
        public Vector3 start;

        public override void Begin()
        {
            base.Begin();
            EffectData effectData = new EffectData
            {
                origin = this.origin,
                start = this.start,
                genericFloat = base.duration
            };
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/MageLightningOrbEffect"), effectData, true);
        }
    }
}
