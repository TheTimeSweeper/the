using ModdedEntityStates.TeslaTrooper;
using UnityEngine;

public class TestValueManager : MonoBehaviour {

    //how do doing attributes
    //[debugfloat("valuename", KeyCode.U, KeyCode.J, 5)] on any static value elsewhere
    //would be neat
    //public static float bloob = 0.7f;

    private float _tim;
    private float _holdTime = 0.4f;    

    //compiler flags when
    private bool _testingEnabled => Modules.Config.Debug;



    //public static float vertOff = 1.37f;
    //public static float aimorigin = 2.5f;
    //public static float camerapivot = 2.5f;
    //public static float bak = -12;

    public static float value1 = 0.1f;

    void Update() {
        if (!_testingEnabled)
            return;

        if (!Input.GetKey(KeyCode.LeftAlt))
            return;

        //manageTestValue(ref EntityStates.Joe.ThrowBoom.lowGravMultiplier, "bomb Grav", KeyCode.Keypad7, KeyCode.Keypad4, 0.02f);
        //manageTestValue(ref EntityStates.Joe.ThrowBoom.smallhopVelocity, "bomb hop", KeyCode.Keypad8, KeyCode.Keypad5, 0.05f);

        //manageTestValue(ref BigZap.keep_bigsexyeffect, "effect", KeyCode.Alpha2, KeyCode.Alpha1, 1f);

        //manageTestValue(ref vertOff, "verticaloffset", KeyCode.Keypad7, KeyCode.Keypad4, 1f);
        //manageTestValue(ref aimorigin, "aimorigin", KeyCode.Keypad8, KeyCode.Keypad5, 1f);
        //manageTestValue(ref camerapivot, "aimorigin", KeyCode.Keypad9, KeyCode.Keypad6, 1f);
        //manageTestValue(ref bak, "aimorigin", KeyCode.Keypad3, KeyCode.KeypadPeriod, 1f);
        manageTestValue(ref value1, "test value 1", KeyCode.Keypad7, KeyCode.Keypad4, 0.02f);
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
                _tim = _holdTime - 0.05f; 
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
