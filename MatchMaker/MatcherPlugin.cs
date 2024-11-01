using BepInEx;
using MatcherMod.Survivors.Matcher;
using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace MatcherMod
{
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class MatcherPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.TheTimesweeper.Matcher";
        public const string MODNAME = "Match Maker";
        public const string MODVERSION = "0.2.0";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static MatcherPlugin instance;

        void Awake()
        {
            instance = this;
            Log.Init(Logger);

            //async load hopoo shader
            Modules.Materials.Init();

            Modules.Language.Init();

            new MatcherSurvivor().Initialize();

            new Modules.ContentPacks().Initialize();
        }
    }
}
