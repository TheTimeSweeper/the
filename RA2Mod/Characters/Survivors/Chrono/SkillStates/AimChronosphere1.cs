using EntityStates;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class AimChronosphere1 : AimChronosphereBase
    {
        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new PlaceChronosphere2 { originalPoint = point };
        }
    }
}