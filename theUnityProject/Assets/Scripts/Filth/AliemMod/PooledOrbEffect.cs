using RoR2.Orbs;
using RoR2;
using UnityEngine;

namespace AliemMod.Components.Bundled
{
    public class PooledOrbEffect : MonoBehaviour
    {
        public EffectComponent effectComponent;
        public OrbEffect orbEffectComponent;
        public TrailRenderer trailRenderer;
        private bool _hasRunInitialStart = false;
    }
}
