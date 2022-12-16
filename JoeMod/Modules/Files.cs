//using R2API;
using BepInEx;
using System;
using System.IO;
using Zio.FileSystems;

namespace Modules {
    internal static class Files {

        public static SubFileSystem fileSystem;

        public static PluginInfo PluginInfo;
        
        internal static string assemblyDir {
            get {
                return System.IO.Path.GetDirectoryName(PluginInfo.Location);
            }
        }

        internal static string GetPath(string folderName, string fileName) {

            return Path.Combine(assemblyDir, folderName, fileName);
        }

        internal static void init(PluginInfo info) {
            PluginInfo = info;
        }
    }
    public static class SoundBank {
        //The folder where your SoundBanks are, it is required for them to be in a folder.
        public const string soundBankFolder = "SoundBanks";
        public const string soundBankFileName = "Tesla_Trooper.bnk";
        public const string soundBankName = "Tesla_Trooper";

        public static string SoundBankDirectory {
            get {
                return Path.Combine(Path.GetDirectoryName(Files.PluginInfo.Location), soundBankFolder);
            }
        }

        public static void Init() {
            AKRESULT akResult = AkSoundEngine.AddBasePath(SoundBankDirectory);
            //if (akResult == AKRESULT.AK_Success) {
            //    FacelessJoePlugin.Log.LogInfo($"Added bank base path : {SoundBankDirectory}");
            //} else {
            //    FacelessJoePlugin.Log.LogError(
            //        $"Error adding base path : {SoundBankDirectory} " +
            //        $"Error code : {akResult}");
            //}

            akResult = AkSoundEngine.LoadBank(soundBankFileName, out var _soundBankId);
            //if (akResult == AKRESULT.AK_Success) {
            //    FacelessJoePlugin.Log.LogInfo($"Added bank : {soundBankFileName}");
            //} else {
            //    FacelessJoePlugin.Log.LogError(
            //        $"Error loading bank : {soundBankFileName} " +
            //        $"Error code : {akResult}");
            //}
        }
    }
}