//using R2API;
using Zio.FileSystems;
using System.Linq;
using System;

namespace Modules {

    internal static class Language {

        public static SubFileSystem fileSystem;

        internal static string languageRoot => System.IO.Path.Combine(Files.assemblyDir, "Language");

        internal static string TokensOutput = "";

        public static void PrintOutput(string preface = "") {
            Helpers.LogWarning($"{preface}\n{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}");
            TokensOutput = "";
        }

        public static void Add(string token, string text) {
            Language.TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }

        public static void HookRegisterLanguageTokens() {
            On.RoR2.Language.SetFolders += fixme;

            return;
        }

        //Credits to Moffein for this credit
        //Credits to Anreol for this code
        private static void fixme(On.RoR2.Language.orig_SetFolders orig, RoR2.Language self, System.Collections.Generic.IEnumerable<string> newFolders) {
            if (System.IO.Directory.Exists(Language.languageRoot)) {
                var dirs = System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(Language.languageRoot), self.name);
                orig(self, newFolders.Union(dirs));
                return;
            }
            orig(self, newFolders);
        }
    }
}