using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem {
    public class MashAndHoldInputs : BaseSkillState {

		protected virtual string StateMachineName => "Weapon";

		protected virtual EntityState initialMashState => null;

		protected virtual EntityState mashState => null;
		protected virtual InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected virtual EntityState holdState => null;
		protected virtual InterruptPriority holdInterruptPriority => InterruptPriority.Skill;

		protected virtual bool RepeatHoldState => true;

		private EntityStateMachine outputESM;
		private float minHoldTime = 1f;
		private float minMashTime = 0.35f;

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
				outputESM.SetInterruptState(initialMashState, mashInterruptPriority);
			}

			_mashTimer = minMashTime;
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

			if (!_holding) {
				//timer counts down
				_mashTimer -= Time.deltaTime;

				//if the timer goes past zero (we haven't mashed again in the minimum time), we're not mashing
				if (_mashTimer < 0)
					_mashing = false;

				if (inputBank.skill1.justPressed) {
					_mashes++;

					//second time we mash within the right time, we are now mashing
					if (!_mashing && _mashTimer > 0 && _mashes > 1) {
						_mashing = true;
                    }

					//on each mash, start a short timer that counts down
					_mashTimer = minMashTime;
				}
			}

            if (inputBank.skill1.down) {
				_holdTimer += Time.fixedDeltaTime;

				_holding = _holdTimer > minHoldTime;
            } else {
				_holding = false;
				_holdTimer = 0;
            }

			if (_holding) {

				if (RepeatHoldState && _hasFiredHold) {
					outputESM.SetInterruptState(holdState, holdInterruptPriority);
				}

				if (outputESM.CanInterruptState(holdInterruptPriority)) {
					_hasFiredHold = true;
					outputESM.SetInterruptState(holdState, holdInterruptPriority);
				}
			} else if (_mashing) {
				outputESM.SetInterruptState(mashState, mashInterruptPriority);
			}
			if(!inputBank.skill1.down && !_holding && !_mashing && _mashTimer < 0) {
				outer.SetNextStateToMain();
            }
		}

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
