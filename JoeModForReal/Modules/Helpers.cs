using System;
using System.Collections.Generic;
using System.Linq;
using Modules;
using RoR2;

internal static class Helpers {
    public const string agilePrefix = "<style=cIsUtility>Agile.</style>";

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

    internal static string KeywordText(string keyword, string sub) {
        return $"<style=cKeywordName>{keyword}</style><style=cSub>{sub}</style>";
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
            JoeModForReal.FacelessJoePlugin.Log.LogMessage(message);

            if (chat) {
                RoR2.Chat.AddMessage(message.ToString());
            }
        }
    }

    public static void LogWarning(object message, bool chat = false) {

        if (Config.Debug) {
            JoeModForReal.FacelessJoePlugin.Log.LogWarning(message);

            if (chat) {
                RoR2.Chat.AddMessage(message.ToString());
            }
        }
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

    internal static string KeywordText(string v, object p) {
        throw new NotImplementedException();
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