using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoBuffTracker : MonoBehaviour
    {
        public delegate void BuffsChangedEvent(CharacterBody body);
        public BuffsChangedEvent onBuffsChanged;

        public CharacterBody body;

        void Awake()
        {
            body = GetComponent<CharacterBody>();
        }

        public void invokeBuffsChanged()
        {
            onBuffsChanged?.Invoke(body);
        }
    }
}