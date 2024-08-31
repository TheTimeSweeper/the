using AliemMod.Content.Survivors;
using RoR2;
using RoR2.Achievements;

namespace AliemMod.Content.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, 3, null)]
    public class AliemSlowMashAchievement : BaseAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "SlowMashAchievement" + AliemUnlockables.DevResetString;
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "SlowMashUnlockable" + AliemUnlockables.DevResetString;

        private class AverageTimeCounter
        {
            private float _timer;
            private float[] _times = new float[4];
            private int _timesIndex;

            public void incrementTimer(float deltaTime)
            {
                _timer += deltaTime;
            }
            public void addTime()
            {
                _times[_timesIndex % _times.Length] = _timer;
                _timesIndex++;
                _timer = 0;
            }
            public float GetAverage()
            {
                if (_timesIndex <= _times.Length)
                    return -1;
                float sum = 0;
                for (int i = 0; i < _times.Length; i++)
                {
                    sum += _times[i];
                }
                return sum / _times.Length;
            }
        }

        private AverageTimeCounter _mashTimeCounter = new AverageTimeCounter();
        private AverageTimeCounter _fireTimeCounter = new AverageTimeCounter();

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("AliemBody");
        }

        public override void OnBodyRequirementMet()
        {
            RoR2Application.onFixedUpdate += RoR2Application_onFixedUpdate;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onOnEnter += MashAndHoldInputs_onOnEnter;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onMash += MashAndHoldInputs_onMash;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onFire += MashAndHoldInputs_onFire;
        }

        public override void OnBodyRequirementBroken()
        {
            RoR2Application.onFixedUpdate -= RoR2Application_onFixedUpdate;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onOnEnter -= MashAndHoldInputs_onOnEnter;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onMash -= MashAndHoldInputs_onMash;
            ModdedEntityStates.Aliem.MashAndHoldInputs.onFire -= MashAndHoldInputs_onFire;
        }

        private void RoR2Application_onFixedUpdate()
        {
            if (_mashTimeCounter != null)
            {
                _mashTimeCounter.incrementTimer(UnityEngine.Time.fixedDeltaTime);
            }
            if (_fireTimeCounter != null)
            {
                _fireTimeCounter.incrementTimer(UnityEngine.Time.fixedDeltaTime);
            }
        }

        private void MashAndHoldInputs_onOnEnter()
        {
            _mashTimeCounter = new AverageTimeCounter();
            _fireTimeCounter = new AverageTimeCounter();
        }

        private void MashAndHoldInputs_onMash()
        {
            _mashTimeCounter.addTime();
            CheckAverages();
        }

        private void MashAndHoldInputs_onFire()
        {
            _fireTimeCounter.addTime();
            CheckAverages();
        }

        private void CheckAverages()
        {
            if (_mashTimeCounter == null)
                return;
            if (_fireTimeCounter == null)
                return;
            //Helpers.LogWarning($"mash average {_mashTimeCounter.GetAverage().ToString("0.000")} fire average {_fireTimeCounter.GetAverage().ToString("0.000")}");
            float _mashAverage = _mashTimeCounter.GetAverage();
            if (_mashAverage == -1)
                return;
            float _fireAverage = _fireTimeCounter.GetAverage();
            if (_fireAverage == -1)
                return;
            if (_mashAverage > _fireAverage * 2)
            {
                base.Grant();
            }
        }
    }
}
