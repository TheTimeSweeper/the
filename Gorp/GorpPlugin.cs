using BepInEx;
using EntityStates.FalseSonBoss;
using GorpMod.Characters.Enemies.Gorp;
using GorpMod.Survivors.Gorp;
using RoR2;
using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace GorpMod
{
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class GorpPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.TheTimesweeper.Gorp";
        public const string MODNAME = "Gorp";
        public const string MODVERSION = "0.1.0";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static GorpPlugin instance;

        void Awake()
        {
            instance = this;
            Log.Init(Logger);

            //async load hopoo shader
            Modules.Materials.Init();

            Modules.Language.Init();

            new GorpCharacter().Initialize();

            new Modules.ContentPacks().Initialize();
        }

        [ConCommand(commandName = "gorp",
            flags = ConVarFlags.ExecuteOnServer,
            helpText = "It's gorpin time.")]
        private static void CCGorp(ConCommandArgs args)
        {

            var senderBody = args.GetSenderBody();
            InputBankTest inputBank = senderBody.inputBank;

            Ray ray = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

            Vector3 hit = senderBody.transform.position;
            const float yOffset = 0.3f;
            if (Util.CharacterRaycast(senderBody.gameObject, ray, out RaycastHit hitInfo, Mathf.Infinity, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
            {
                hit = hitInfo.point + (Vector3.up * yOffset);
            }

            var bossEncounter = UnityEngine.Object.Instantiate(GorpCharacter.BossEncounter, hit, Quaternion.identity);
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(bossEncounter);
            }
        }
    }
}