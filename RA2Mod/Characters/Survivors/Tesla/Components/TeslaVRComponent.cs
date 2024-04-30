using UnityEngine;
using RoR2;
using VRAPI;
using RoR2.Skills;
using RA2Mod.Survivors.Tesla;
using RA2Mod.General.Components;

public class TeslaVRComponent : MonoBehaviour {

    private CharacterBody body;

    void Awake () {
        body = GetComponent<CharacterBody>();
    }

    void Start() {
        onHandPairSet();
    }

    private void onHandPairSet() {

        if (body.baseNameToken != TeslaTrooperSurvivor.TOKEN_PREFIX + "NAME")
            return;

        SkillDef color = body.skillLocator?.FindSkill("Recolor")?.skillDef;
        if (color) {
            MotionControls.dominantHand.transform.GetComponentInChildren<SkinRecolorController>().SetRecolor(color.skillName.ToLowerInvariant());
            MotionControls.nonDominantHand.transform.GetComponentInChildren<SkinRecolorController>().SetRecolor(color.skillName.ToLowerInvariant());
        }

        //ghetto gameobjectactivations
        ChildLocator handChildLocator = MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();

        switch (body.skinIndex) {
            default:
            case (uint)0://default
                handChildLocator.FindChildGameObject("MeshEmission").SetActive(false);
                break;
            case (uint)1://mastery
                handChildLocator.FindChildGameObject("MeshEmission").SetActive(true);
                break;
            case (uint)2://nod
                MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>().FindChildGameObject("MeshArmorColor").SetActive(false);
                handChildLocator.FindChildGameObject("MeshEmission").SetActive(true);
                break;
            case (uint)3://minecraft
                handChildLocator.FindChildGameObject("MeshEmission").SetActive(false);
                break;
        }
    }
}



