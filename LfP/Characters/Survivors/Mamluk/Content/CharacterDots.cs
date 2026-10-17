using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public class CharacterDots
    {
        public static DotController.DotIndex comboFinisherDot;

        public static void Init()
        {
            comboFinisherDot = DotAPI.RegisterDotDef(new DotController.DotDef
            {
                interval = 0.25f,//dot will tick 4 times per second
                damageCoefficient = 0.5f, //dot will tick for 50% damage each tick
                damageColorIndex = DamageColorIndex.Poison,
                associatedBuff = CharacterBuffs.comboFinisherDotBuff
            });
        }
    }
}
