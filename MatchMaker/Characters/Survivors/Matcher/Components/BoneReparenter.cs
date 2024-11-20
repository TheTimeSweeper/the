using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class BoneReparenter : MonoBehaviour
    {
        [System.Serializable]
        public class Reparenter
        {
            public Transform objectToParent;
            public string parameter;
            public Transform parent1;
            public Transform parent2;

            private bool _parented;

            public void CheckParent(Animator animator)
            {
                bool shouldParent = animator.GetFloat(parameter) > 0.5f;

                if (shouldParent == _parented)
                    return;

                _parented = shouldParent;

                objectToParent.parent = shouldParent ? parent2 : parent1;
                objectToParent.transform.localRotation = Quaternion.identity;
                objectToParent.transform.localPosition = Vector3.zero;
            }
        }

        [SerializeField]
        private Reparenter[] reparenters;

        [SerializeField]
        private Animator _animator;

        private void Update()
        {
            for (int i = 0; i < reparenters.Length; i++)
            {
                reparenters[i].CheckParent(_animator);
            }
        }
    }
}
