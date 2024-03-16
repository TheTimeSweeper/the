
using RoR2;
using UnityEngine;

public class SpawnProjectileOnAnimation : MonoBehaviour {

    [SerializeField]
    private CharacterModel characterModel;

    public void SpawnProjectile() {
        EntityStateMachine.FindByCustomName(characterModel.body.gameObject, "Weapon").SetNextState(new ModdedEntityStates.Desolator.EmoteRadiationProjectile());
    }
}
