using RoR2.Networking;
using UnityEngine;
namespace RA2Mod.Survivors.Chrono.Components
{
    public class BootlegCharacterNetworkTransform : CharacterNetworkTransform
    {
        [SerializeField]
        private ChronoProjectionMotor notCharacterMotor;

        new void ApplyCurrentSnapshot(float currentTime)
        {
            CharacterNetworkTransform.Snapshot snapshot = this.CalcCurrentSnapshot(currentTime, this.interpolationDelay);

            if (this.notCharacterMotor)
            {
                //this.notCharacterMotor.netIsGrounded = snapshot.isGrounded;
                //this.notCharacterMotor.netGroundNormal = snapshot.groundNormal;
                if (this.notCharacterMotor.Motor.enabled)
                {
                    this.notCharacterMotor.Motor.MoveCharacter(snapshot.position);
                }
                else
                {
                    this.notCharacterMotor.Motor.SetPosition(snapshot.position, true);
                }
            }
        }
    }
}