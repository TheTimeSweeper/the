
using UnityEngine;
namespace RA2Mod.Survivors.Tesla.Components
{
    public class HammerReparenter : MonoBehaviour
    {
        [SerializeField]
        private Transform hammer;

        [SerializeField]
        private Transform hand;
        [SerializeField]
        private Transform hip;

        public void Reparent(int parent)
        {

            hammer.SetParent(parent == 0 ? hand : hip);
            hammer.localPosition = Vector3.zero;
            hammer.localRotation = Quaternion.identity;
        }
    }
}