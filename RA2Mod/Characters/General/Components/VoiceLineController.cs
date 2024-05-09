using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.General.Components
{
    [System.Serializable]
    public class VoiceLineContext
    {
        //[SerializeField]
        public string prefix;

        //[SerializeField]
        public int attackSounds, selectSounds, moveSounds;

        public VoiceLineContext(string prefix, int attackSounds, int selectSounds, int moveSounds)
        {
            this.prefix = prefix;
            this.attackSounds = attackSounds;
            this.selectSounds = selectSounds;
            this.moveSounds = moveSounds;
        }

        public string RandomMoveVoice()
        {
            return $"Play_Voice_{prefix}_Move_{Random.Range(1, moveSounds + 1)}";
        }

        public string RandomSelectVoice()
        {
            return $"Play_Voice_{prefix}_Select_{Random.Range(1, selectSounds + 1)}";
        }

        public string RandomAttackVoiceline()
        {
            return $"Play_Voice_{prefix}_Attack_{Random.Range(1, attackSounds + 1)}";
        }
        public string RandomDeathVoiceline()
        {
            return $"Play_Voice_{prefix}_Death_{Random.Range(1, attackSounds + 1)}";
        }
    }

    public class VoiceLineInLobby : MonoBehaviour //VoiceLineController
    {
        public VoiceLineContext voiceLineContext;

        /*protected override*/ void Awake()
        {
            if (GeneralConfig.VoiceInLobby.Value)
            {
                //to network the voice in lobby I have to fucking add a networkidentity and fucking somehow make this authority
                //task for future dumbass me
                //PlayVoicelineAuthority(voiceLineContext.RandomSelectVoice());
                Util.PlaySound(voiceLineContext.RandomSelectVoice(), gameObject);
            }
        }
    }

    public class VoiceLineController : NetworkBehaviour
    {
        public VoiceLineContext voiceLineContext;
        private CharacterBody _body;
        private bool _authority;

        protected virtual void Start()
        {
            _body = GetComponent<CharacterBody>();
            _authority = Util.HasEffectiveAuthority(gameObject);
            
            //if (GeneralConfig.VoiceOnSpawn.Value)
            //{
            //    PlayVoicelineAuthority(voiceLineContext.RandomSelectVoice());
            //}
        }

        protected virtual void Update()
        {
            if (Modules.Config.GetKeyPressed(GeneralConfig.VoiceKey.Value))
            {
                if (_body == null || !Util.HasEffectiveAuthority(gameObject))
                    return;

                PlayRandomVoiceLineAuthority();
            }
        }

        protected virtual void PlayRandomVoiceLineAuthority()
        {
            string sound;

            if (_body.outOfCombat)
            {
                if (_body.isSprinting)
                {
                    sound = voiceLineContext.RandomMoveVoice();
                }
                else
                {
                    sound = voiceLineContext.RandomSelectVoice();
                }
            }
            else
            {
                sound = voiceLineContext.RandomAttackVoiceline();
            }

            PlayVoicelineAuthority(sound);
        }

        public void PlayVoicelineAuthority(string sound)
        {
            Util.PlaySound(sound, gameObject);

            if (NetworkServer.active)
            {
                RpcSound(sound);
            }
            else
            {
                CmdSound(sound);
            }
        }

        [Command]
        private void CmdSound(string sound)
        {
            Util.PlaySound(sound, gameObject);
        }

        [ClientRpc]
        private void RpcSound(string sound)
        {
            Util.PlaySound(sound, gameObject);
        }
    }
}