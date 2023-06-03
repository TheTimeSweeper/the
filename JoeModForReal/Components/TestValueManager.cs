using ModdedEntityStates.TeslaTrooper;
using UnityEngine;

internal class TestValueManager : MonoBehaviour {

    //how do doing attributes
    //[debugfloat("valuename", KeyCode.U, KeyCode.J, 5)] on any static value elsewhere
    //would be neat
    //public static float bloob = 0.7f;

    private float _tim;
    private float _holdTime = 0.4f;    

    //compiler flags when
    private bool _testingEnabled => Modules.Config.Debug;
    public static float fireballDamage = 5.5f;
    public static float swingDamage = 1.8f;
    
    public static float tenticleArmor = 50f;
    public static float tenticleMove = 0.5f;
    public static float swrodBeam = 1.4f;
    
    public static float dashArmor = 150f;

    public static float beems = 10f;

    void Update() {
        if (!_testingEnabled)
            return;

        if (!Input.GetKey(KeyCode.LeftAlt))
            return;

        manageTestValue(ref fireballDamage, "1 fireball dam", KeyCode.T, KeyCode.G, 0.1f);

        manageTestValue(ref swingDamage, "2 primary dam", KeyCode.Y, KeyCode.H, 0.1f);

        manageTestValue(ref tenticleArmor, "3 tenticle armor", KeyCode.U, KeyCode.J, 1f);
        manageTestValue(ref tenticleMove, "4 tenticle move", KeyCode.I, KeyCode.K, 0.1f);
        manageTestValue(ref swrodBeam, "5 swrodbeam", KeyCode.O, KeyCode.L, 0.1f);

        manageTestValue(ref dashArmor, "6 dash armor", KeyCode.P, KeyCode.Semicolon, 10f);

        manageTestValue(ref beems, "7 beems", KeyCode.LeftBracket, KeyCode.RightBracket, 1f);
    }

    private void manageTestValue(ref float value, string valueName, KeyCode upKey, KeyCode downKey, float incrementAmount) {

        if (Input.GetKeyDown(upKey)) {

            value = setTestValue(value + incrementAmount, valueName);
        }

        if (Input.GetKeyDown(downKey)) {
            
            value = setTestValue(value - incrementAmount, valueName);
        }


        if (Input.GetKey(upKey) || Input.GetKey(downKey)) {

            float amount = incrementAmount * (Input.GetKey(upKey) ? 1 : -1);

            _tim += Time.deltaTime;

            if (_tim > _holdTime) {
                _tim = _holdTime - 0.02f; 
                value = setTestValue(value + amount, valueName);
            }
        }


        if (Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey)) {
            _tim = 0;
        }

    }

    private float setTestValue(float value, string print) {
        Helpers.LogWarning($"{print}: {value.ToString("0.000")}");
        return value;
    }
}
