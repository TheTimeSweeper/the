using RoR2;
using UnityEngine;

namespace RA2Mod.General.Components
{
    public class VoiceLines:MonoBehaviour
    {
        [SerializeField]
        public string prefix = "Play_";

        private CharacterBody _body;
        private bool _authority;

        void Awake()
        {
            _body = GetComponent<CharacterBody>();
            _authority = Util.HasEffectiveAuthority(gameObject);
        }

        void Update()
        {
            if (!_authority || _body == null)
                return;

            if (Modules.Config.GetKeyPressed(GeneralConfig.VoiceKey))
            {
                playRandomvoiceLine();
            }
        }

        protected virtual void playRandomvoiceLine()
        {
            string sound;
            if (_body.outOfCombat)
            {
                if (_body.isSprinting)
                {
                    sound = $"{prefix}Voiceline_Move";
                }
                else
                {
                    sound = $"{prefix}Voiceline_Select";
                }
            }
            else
            {
                sound = $"{prefix}Voiceline_Attack";
            }

            RoR2.Util.PlaySound(sound, gameObject);
        }
    }
}