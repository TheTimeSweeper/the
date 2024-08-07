﻿using AliemMod.Modules;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public class SwapSecondaryWeaponScepter : SwapSecondaryWeapon
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active)
            {
                characterBody.AddBuff(Buffs.attackSpeedBuff);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(Buffs.attackSpeedBuff);
            }
        }
    }
}
