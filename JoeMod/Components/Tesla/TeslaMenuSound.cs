using UnityEngine;

public class TeslaMenuSound : MonoBehaviour {

    //if you're reading this and know the not retarded way to play a sound in css let me know please
    void OnEnable() {
        RoR2.Util.PlaySound("Play_Tesla_lobby", gameObject);

        if (Modules.Config.VoiceInLobby.Value) {

            RoR2.Util.PlaySound("Play_VoiceLine", gameObject);
        }
    }
}
