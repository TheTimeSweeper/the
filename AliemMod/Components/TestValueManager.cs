using System;
using UnityEngine;

public class TestValueManager : MonoBehaviour {

    //how do doing attributes
    //[debugfloat("valuename", KeyCode.U, KeyCode.J, 5)] on any static value elsewhere
    //would be neat

    private float _tim;
    private float _holdTime = 0.4f;    

    //compiler flags when
    private bool _testingEnabled => Modules.AliemConfig.Debug;

    public static float value1 = -60f;
    public static float value2 = 25f;

    void Update() {
        if (!_testingEnabled)
            return;

        if (!Input.GetKey(KeyCode.LeftAlt))
            return;

        manageTestValue(ref value1, "pitch", KeyCode.Keypad7, KeyCode.Keypad4, 10f);
        manageTestValue(ref value2, "forwardSpeed", KeyCode.Keypad8, KeyCode.Keypad5, 1);
        //manageTestValue(ref value2, "deflect damage", KeyCode.Keypad8, KeyCode.Keypad5, 0.1f);
        //manageTestValue(ref value3, "end time", KeyCode.Keypad9, KeyCode.Keypad6, 0.01f);
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
