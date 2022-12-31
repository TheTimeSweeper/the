//using R2API;
using JoeModForReal;
using System.IO;

namespace Modules {
    public static class SoundBanks {
        //The folder where your SoundBanks are, it is required for them to be in a folder.
        public const string soundBankFolder = "SoundBanks";
        public const string soundBankFileName = "joe.bnk";

        public static string SoundBankDirectory {
            get {
                return Path.Combine(Path.GetDirectoryName(Files.PluginInfo.Location), soundBankFolder);
            }
        }

        public static void Init() {
            AKRESULT akResult = AkSoundEngine.AddBasePath(SoundBankDirectory);
            if (akResult == AKRESULT.AK_Success) {
                FacelessJoePlugin.Log.LogMessage($"Added bank base path : {SoundBankDirectory}");
            } else {
                FacelessJoePlugin.Log.LogError(
                    $"Error adding base path : {SoundBankDirectory} " +
                    $"Error code : {akResult}");
            }

            akResult = AkSoundEngine.LoadBank(soundBankFileName, out var _soundBankId);
            if (akResult == AKRESULT.AK_Success) {
                FacelessJoePlugin.Log.LogMessage($"Added bank : {soundBankFileName}");
            } else {
                FacelessJoePlugin.Log.LogError(
                    $"Error loading bank : {soundBankFileName} " +
                    $"Error code : {akResult}");
            }
        }
    }
}