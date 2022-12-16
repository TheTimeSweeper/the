//using R2API;
using Zio.FileSystems;
using System.Linq;

namespace Modules {

    internal static class Language {

        public static SubFileSystem fileSystem;

        internal static string languageRoot => System.IO.Path.Combine(Files.assemblyDir, "Language");

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