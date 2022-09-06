using System;
using System.Collections.Generic;
using System.Linq;
using Modules;
using RoR2;

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

internal static class Helpers {
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
    public static void LogVerbose(object message, bool chat = false) {

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

    public static string GetVoiceLineString(TeslaVoiceLine line) {
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
                voiceLineString = "Play_itessea";
                break;
            case TeslaVoiceLine.select_ChargingUp:
                voiceLineString = "Play_itesseb";
                break;
            case TeslaVoiceLine.select_Electrodes:
                voiceLineString = "Play_itessec";
                break;
            case TeslaVoiceLine.select_CheckingConnection:
                voiceLineString = "Play_itessed";
                break;
        }

        return voiceLineString;
    }

    //credit to tiler2 https://github.com/ThinkInvis/RoR2-TILER2/blob/3e2a6d4105417de06abb3ef3f85da844170abf8a/StaticModules/MiscUtil.cs#L455
    /// <summary>
    /// Returns a list of enemy TeamComponents given an ally team (to ignore while friendly fire is off) and a list of ignored teams (to ignore under all circumstances).
    /// </summary>
    /// <param name="allyIndex">The team to ignore if friendly fire is off.</param>
    /// <param name="ignore">Additional teams to always ignore.</param>
    /// <returns>A list of all TeamComponents that match the provided team constraints.</returns>
    public static List<TeamComponent> GatherEnemies(TeamIndex allyIndex, params TeamIndex[] ignore) {
        var retv = new List<TeamComponent>();
        bool isFF = FriendlyFireManager.friendlyFireMode != FriendlyFireManager.FriendlyFireMode.Off;
        var scan = ((TeamIndex[])Enum.GetValues(typeof(TeamIndex))).Except(ignore);
        foreach (var ind in scan) {
            if (isFF || allyIndex != ind)
                retv.AddRange(TeamComponent.GetTeamMembers(ind));
        }
        return retv;
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