using System;
using System.Collections.Generic;
using Modules;

public enum TeslaVoiceLine {
    attack_2000Volts, //0
    attack_HesFried,
    attack_CompletingCircuit,
    attack_Juice,
    attack_CommencingShock, //4

    move_GoingToSource,
    move_MovingOut,
    move_YesComrade,
    move_SurgingForward,
    move_Electrician,
    move_RubberShoes, //10

    select_TeslaSuit,
    select_ChargingUp,
    select_Electrodes,
    select_CheckingConnection //14
}

public static class Helpers {
    public const string agilePrefix = "<style=cIsUtility>Agile.</style> ";

    public static string DamageText(string text) {
        return $"<style=cIsDamage>{text}</style>";
    }
    public static string DamageValueText(float value) {
        return $"<style=cIsDamage>{value * 100}% damage</style>";
    }

    public static string UtilityText(string text) {
        return $"<style=cIsUtility>{text}</style>";
    }

    public static string RedText(string text) => HealthText(text);
    public static string HealthText(string text) {
        return $"<style=cIsHealth>{text}</style>";
    }

    public static string ScepterDescription(string desc) {
        return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
    }

    public static T[] Append<T>(ref T[] array, List<T> list) {
        var orig = array.Length;
        var added = list.Count;
        Array.Resize<T>(ref array, orig + added);
        list.CopyTo(array, orig);
        return array;
    }

    public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);

    public static bool verbose = false;
    public static void Log(object message, bool chat = false) {

        if (Config.Debug && verbose) {
            FacelessJoePlugin.Log.LogMessage(message);

            if (chat) {
                RoR2.Chat.AddMessage(message.ToString());
            }
        }
    }

    public static void LogWarning(object message, bool chat = false) {

        if (Config.Debug) {
            FacelessJoePlugin.Log.LogWarning(message);

            if (chat) {
                RoR2.Chat.AddMessage(message.ToString());
            }
        }
    }


    public static uint PlaySoundVoiceLine(TeslaVoiceLine line, UnityEngine.GameObject gameObject) {
        string voiceLineString;
        switch (line) {
            case TeslaVoiceLine.attack_2000Volts:
                voiceLineString = "Play_itesata";
                break;
            case TeslaVoiceLine.attack_HesFried:
                voiceLineString = "Play_itesatb";
                break;
            case TeslaVoiceLine.attack_CompletingCircuit:
                voiceLineString = "Play_itesatc";
                break;
            case TeslaVoiceLine.attack_Juice:
                voiceLineString = "Play_itesatd";
                break;
            case TeslaVoiceLine.attack_CommencingShock:
                voiceLineString = "Play_itesate";
                break;
            case TeslaVoiceLine.move_GoingToSource:
                voiceLineString = "Play_itesmoa";
                break;
            case TeslaVoiceLine.move_MovingOut:
                voiceLineString = "Play_itesmob";
                break;
            case TeslaVoiceLine.move_YesComrade:
                voiceLineString = "Play_itesmoc";
                break;
            case TeslaVoiceLine.move_SurgingForward:
                voiceLineString = "Play_itesmod";
                break;
            default:
            case TeslaVoiceLine.move_Electrician:
                voiceLineString = "Play_itesmoe";
                break;
            case TeslaVoiceLine.move_RubberShoes:
                voiceLineString = "Play_itesmof";
                break;
            case TeslaVoiceLine.select_TeslaSuit:
                voiceLineString = "Play_itesea";
                break;
            case TeslaVoiceLine.select_ChargingUp:
                voiceLineString = "Play_iteseb";
                break;
            case TeslaVoiceLine.select_Electrodes:
                voiceLineString = "Play_itesec";
                break;
            case TeslaVoiceLine.select_CheckingConnection:
                voiceLineString = "Play_itesed";
                break;
        }
        UnityEngine.Debug.LogWarning("uh " + voiceLineString);

        return RoR2.Util.PlaySound(voiceLineString, gameObject, "Volume_TeslaVoice", 100);
    }
}

public static class ArrayHelper {
    public static T[] Append<T>(ref T[] array, List<T> list) {
        var orig = array.Length;
        var added = list.Count;
        Array.Resize<T>(ref array, orig + added);
        list.CopyTo(array, orig);
        return array;
    }

    public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);
}