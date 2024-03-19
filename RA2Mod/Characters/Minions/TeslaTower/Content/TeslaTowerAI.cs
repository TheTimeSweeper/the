using RA2Mod.Modules;
using RoR2.CharacterAI;
using System;
using UnityEngine;

namespace RA2Mod.Minions.TeslaTower
{
    public class TeslaTowerAI
    {
        public static void Init(GameObject bodyPrefab, string masterName, Action<GameObject> OnComplete)
        {
            ContentPacks.asyncLoadCoroutines.Add(Prefabs.CreateBlankMasterPrefabAsync(bodyPrefab, masterName, (result) => {

                GameObject master = result;

                BaseAI baseAI = master.GetComponent<BaseAI>();
                baseAI.aimVectorDampTime = 0.01f;
                baseAI.aimVectorMaxSpeed = 360;

                //TestShootAi(master);

                //TestSprintAI(master);

                //CreateAI(master);

                OnComplete(master);
            }));
        }
    }
}