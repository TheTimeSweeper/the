using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public abstract class MashAndHoldInputs : BaseSkillState, IOffHandable {
		
		protected virtual string StateMachineName => isOffHanded ? "Weapon2": "Weapon";

        protected virtual EntityState initialMashState => newMashState;

		protected abstract EntityState newMashState { get; }
		protected virtual InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected abstract EntityState newHoldState { get; }
		protected virtual InterruptPriority holdInterruptPriority => InterruptPriority.Skill;

		protected virtual bool RepeatHoldState => true;

        public bool isOffHanded { get; set; }

        protected virtual float minHoldTime => 0.5f;
        protected virtual float minMashTime => 0.35f;

        private EntityStateMachine outputESM;

		private bool _holding;
		private bool _mashing;

		private float _holdTimer = 0;
		private float _mashTimer = -1;
		private int _mashes;

		private bool _hasFiredHold;

        public override void OnEnter() {
			base.OnEnter();
			outputESM = EntityStateMachine.FindByCustomName(gameObject, StateMachineName);

			if (initialMashState != null) {
				outputESM.SetInterruptState(SetHandedness(initialMashState), mashInterruptPriority);
			}

			_mashTimer = minMashTime;
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

			if (!_holding)
            {
                //timer counts down
                _mashTimer -= Time.deltaTime;

                //if the timer goes past zero (we haven't mashed again in the minimum time), we're not mashing
                if (_mashTimer < 0)
                    _mashing = false;

                if (GetSkillButton().justPressed)
                {
                    _mashes++;

                    //second time we mash within the right time, we are now mashing
                    if (!_mashing && _mashTimer > 0 && _mashes > 1)
                    {
                        _mashing = true;
                    }

                    //on each mash, start a short timer that counts down
                    _mashTimer = minMashTime;
                }
            }

            if (GetSkillButton().down) {
				_holdTimer += Time.fixedDeltaTime;

				_holding = _holdTimer > minHoldTime;
            } else {
				_holding = false;
				_holdTimer = 0;
            }

			if (_holding) {

				if (RepeatHoldState && _hasFiredHold) {
					outputESM.SetInterruptState(SetHandedness(newHoldState), holdInterruptPriority);
				}

				if (outputESM.CanInterruptState(holdInterruptPriority)) {
					_hasFiredHold = true;
					outputESM.SetInterruptState(SetHandedness(newHoldState), holdInterruptPriority);
				}
			} else if (_mashing) {
				outputESM.SetInterruptState(SetHandedness(newMashState), mashInterruptPriority);
			}
			if(!GetSkillButton().down && !_holding && !_mashing && _mashTimer < 0) {
				outer.SetNextStateToMain();
            }
		}

        private InputBankTest.ButtonState GetSkillButton()
        {
            return isOffHanded ? inputBank.skill2 : inputBank.skill1;
        }

        public EntityState SetHandedness(EntityState state)
        {
            if(state is IOffHandable)
            {
                (state as IOffHandable).isOffHanded = isOffHanded;
            } 
            return state;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
