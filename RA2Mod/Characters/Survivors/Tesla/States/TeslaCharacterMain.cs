using BepInEx.Configuration;
using EntityStates;
using RA2Mod.General;
using RA2Mod.General.States;
using RA2Mod.Modules;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{
    public class TeslaCharacterMain : GenericCharacterMain
    {
        private Animator cachedAnimator;

        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            cachedAnimator = GetModelAnimator();
        }

        public override void Update()
        {
            base.Update();
            bool combat = !characterBody.outOfCombat;

            cachedAnimator.SetBool("inCombat", combat);

            if (isAuthority && characterMotor.isGrounded)
            {
                CheckEmote<Rest>(GeneralConfig.RestKeybind.Value);
            }
        }

        private void FindLocalUser()
        {
            if (localUser == null)
            {
                if (characterBody)
                {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList)
                    {
                        if (lu.cachedBody == characterBody)
                        {
                            localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        private void CheckEmote<T>(KeyboardShortcut keybind) where T : EntityState, new()
        {
            if (Config.GetKeyPressed(keybind))
            {
                FindLocalUser();

                if (localUser != null && !localUser.isUIFocused)
                {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }
    }
}