using System;
using System.Collections.Generic;
using System.IO;

namespace AliemMod.Modules
{
    internal static class LanguageAPI
    {
        public static void Add(string token, string text)
        {
            Modules.Language.Add(token, text);
        }
    }

    internal static class Language
    {
        public static string TokensOutput = "";

        public static bool usingLanguageFolder = true;

        public static bool printingEnabled = false;

        public static void Init()
        {
            if (usingLanguageFolder)
            {
                RoR2.Language.collectLanguageRootFolders += Language_collectLanguageRootFolders;
            }
        }

        private static void Language_collectLanguageRootFolders(List<string> obj)
        {
            string path = Path.Combine(Path.GetDirectoryName(AliemPlugin.instance.Info.Location), "Language");
            if (Directory.Exists(path))
            {
                obj.Add(path);
            }
        }

        public static void Add(string token, string text)
        {
            //if (!usingLanguageFolder)
            //{
                R2API.LanguageAPI.Add(token, text);
            //}

            if (!printingEnabled) return;

            //add a token formatted to language file
            TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }

        public static void PrintOutput(string fileName = "")
        {
            if (!printingEnabled) return;

            //wrap all tokens in a properly formatted language file
            string strings = $"{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}";

            //spit out language dump in console for copy paste if you want
            Helpers.LogWarning($"{fileName}: \n{strings}");

            //write a language file next to your mod. must have a folder called Language next to your mod dll.
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Path.Combine(Directory.GetParent(AliemPlugin.instance.Info.Location).FullName, "Language", "english", fileName);
                File.WriteAllText(path, strings);
            }

            //empty the output each time this is printed, so you can print multiple language files
            TokensOutput = "";
        }
    }
}
