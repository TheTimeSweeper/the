using BepInEx.Logging;
using System;
using System.Security;
using System.Security.Permissions;

namespace RA2Mod
{
    internal static class Log
    {
        private static ManualLogSource _logSource;
        
        public static DateTime _startTime = default(DateTime);

        private static string timesLog = "";
        private static string funnyLog = "";

        internal static void Init(ManualLogSource logSource)
        {
            _logSource = logSource;
        }

        internal static void Debug(object data) => _logSource.LogDebug(data);
        internal static void Error(object data) => _logSource.LogError(data);

        internal static void ErrorAssetBundle(string assetName, string bundleName) =>
            Log.Error($"failed to load asset, {assetName}, because it does not exist in asset bundle, {bundleName}");        
        internal static void Fatal(object data) => _logSource.LogFatal(data);
        internal static void Info(object data) => _logSource.LogInfo(data);
        internal static void Message(object data) => _logSource.LogMessage(data);
        internal static void Warning(object data) => _logSource.LogWarning(data);
        internal static void WarningNull(string name, object objecte)
        {
            Log.Warning($"{name} is {objecte!= null}");
        }
        internal static void WarningDebug(string format, params object[] data) => DebugWarning(format, data);
        internal static void DebugWarning(string format, params object[] data)
        {
            if (General.GeneralConfig.Debug.Value)
            {
                _logSource.LogWarning(string.Format(format, data));
            }
        }
        internal static void CurrentTime(string funny)
        {
            if (General.GeneralConfig.Debug.Value)
            {
                funnyLog += "\n" + funny;
                TimeSpan timeSpan = DateTime.Now - _startTime;
                string milliseconds = "\n" + timeSpan.TotalSeconds.ToString("0.0000");
                timesLog += milliseconds;
                _logSource.LogWarning($"{funny}{milliseconds}");
            }
        }

        internal static void AllTimes()
        {
            if (!string.IsNullOrEmpty(timesLog))
            {
                Log.Warning(timesLog);
                Log.Warning(funnyLog);
            }
        }
    }
}