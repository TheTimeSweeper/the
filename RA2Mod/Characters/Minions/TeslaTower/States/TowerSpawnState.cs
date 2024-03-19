using EntityStates;
using RoR2;

namespace RA2Mod.Minions.TeslaTower.States
{

    public class TowerSpawnState : BaseState
    {
        public static float TowerSpawnDuration = 1.5f;
        protected float duration;

        public override void OnEnter()
        {
            duration = TowerSpawnDuration / attackSpeedStat;

            //I am still fucking exploding
            Util.PlaySound("Play_buliding_uplace", gameObject);
            PlayAnimation("Body", "ConstructionComplete", "construct.playbackRate", duration);
            base.OnEnter();

            characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            TeslaTowerControllerController towerController = characterBody.master.minionOwnership?.ownerMaster.GetBodyObject()?.GetComponent<TeslaTowerControllerController>();

            if (towerController)
            {
                towerController.addTower(gameObject);

                //characterBody.masterObject.GetComponent<Deployable>()?.onUndeploy.AddListener(() => {
                //    towerController.removeTower(gameObject);
                //});
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {

                outer.SetNextState(new TowerIdleSearch
                {
                    justSpawned = true
                });
            }
        }
    }

}
