using System;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueBombColorizer : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] renderers;

        internal void Colorize(Material material)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                Log.Warning(renderers[i]);
                renderers[i].sharedMaterial = material;
            }
        }
    }
}
