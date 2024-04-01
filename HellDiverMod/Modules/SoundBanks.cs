//using R2API;
using System.IO;

namespace HellDiverMod.Modules {
    public static class SoundBanks {
        //The folder where your SoundBanks are, it is required for them to be in a folder.
        public const string soundBankFolder = "SoundBanks";
        public const string soundBankFileName = "Tesla_Trooper.bnk";
        public const string soundBankName = "Tesla_Trooper";

        public static string SoundBankDirectory {
            get
            {
                string starng = Path.Combine(Path.GetDirectoryName(HellDiverPlugin.instance.Info.Location), soundBankFolder);
                return starng;
            }
        }

        public static void Init() {
            
            if (UnityEngine.Application.isBatchMode) return;


            AKRESULT akResult = AkSoundEngine.AddBasePath(SoundBankDirectory);
            //if (akResult == AKRESULT.AK_Success) {
            //    Log.Info($"Added bank base path : {SoundBankDirectory}");
            //} else {
            //    Log.Error(
            //        $"Error adding base path : {SoundBankDirectory} " +
            //        $"Error code : {akResult}");
            //}

            akResult = AkSoundEngine.LoadBank(soundBankFileName, out var _soundBankId);
            //if (akResult == AKRESULT.AK_Success) {
            //    Log.Info($"Added bank : {soundBankFileName}");
            //} else {
            //    Log.Error(
            //        $"Error loading bank : {soundBankFileName} " +
            //        $"Error code : {akResult}");
            //}
        }
    }
}