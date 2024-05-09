using EntityStates;
using UnityEngine;

namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerSell : GenericCharacterDeath
    {

        private float deathDuration;

        public override void OnEnter()
        {
            base.OnEnter();

            if (characterBody.master.minionOwnership && characterBody.master.minionOwnership.ownerMaster.GetBodyObject())
            {
                TeslaTowerControllerController towerController = characterBody.master.minionOwnership.ownerMaster.GetBodyObject().GetComponent<TeslaTowerControllerController>();

                if (towerController)
                {
                    towerController.removeTower(gameObject);

                    //characterBody.masterObject.GetComponent<Deployable>()?.onUndeploy.AddListener(() => {
                    //    towerController.removeTower(gameObject);
                    //});
                }
            }

        }
    

        // Token: 0x06004324 RID: 17188 RVA: 0x0011196C File Offset: 0x0010FB6C
        public override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
        {
            Animator modelAnimator = GetModelAnimator();
            if (modelAnimator)
            {
                int layerIndex = modelAnimator.GetLayerIndex("Body");
                modelAnimator.PlayInFixedTime("Death", layerIndex);
                modelAnimator.Update(0f);
                deathDuration = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > deathDuration)
            {
                DestroySelf();
                return;
            }
        }

        protected virtual void DestroySelf()
        {
            Destroy(gameObject);
        }

        public override bool shouldAutoDestroy
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06004327 RID: 17191 RVA: 0x0002C851 File Offset: 0x0002AA51
        public override void OnExit()
        {
            DestroyModel();
            base.OnExit();
        }
    }

}
