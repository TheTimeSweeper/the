
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator.Components {
    public class SpawnProjectileOnAnimation : MonoBehaviour {

        [SerializeField]
        private CharacterModel characterModel;

        public void SpawnProjectile() {
            //todo desomove
            //EntityStateMachine.FindByCustomName(characterModel.body.gameObject, "Weapon").SetNextState(new ModdedEntityStates.Desolator.EmoteRadiationProjectile());
        }
    }
}
