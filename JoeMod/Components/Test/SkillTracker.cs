using RoR2;
using RoR2.Skills;
using UnityEngine;

public class SkillTracker : MonoBehaviour {

    private CharacterBody _characterBody;
    public GenericSkill lastActivatedSkill = null;
    public int lastActivatedSkillIndex = 0;

    public void Init(CharacterBody characterBody_) {
        _characterBody = characterBody_;
        _characterBody.onSkillActivatedServer += _characterBody_onSkillActivatedServer;
    }

    private void _characterBody_onSkillActivatedServer(GenericSkill genericSkill) {
        lastActivatedSkill = genericSkill;
        lastActivatedSkillIndex = _characterBody.skillLocator.GetSkillSlotIndex(genericSkill);
    }

    void OnDestroy() {
        _characterBody.onSkillActivatedServer -= _characterBody_onSkillActivatedServer;
    }
}

public class SkillStealController : MonoBehaviour {

    private CharacterBody _characterBody;
    public SkillDef StolenSkillDef;

    public void Start() {
        _characterBody = GetComponent<CharacterBody>();
        _characterBody.onSkillActivatedServer += _characterBody_onSkillActivatedServer;
    }

    private void _characterBody_onSkillActivatedServer(GenericSkill genericSkill) {
        if (genericSkill.skillDef == StolenSkillDef)
            genericSkill.UnsetSkillOverride(_characterBody.skillLocator, StolenSkillDef, GenericSkill.SkillOverridePriority.Replacement);
    }

    void OnDestroy() {
        _characterBody.onSkillActivatedServer -= _characterBody_onSkillActivatedServer;
    }
}