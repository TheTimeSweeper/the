using UnityEngine;

namespace GorpMod.Characters.Enemies.Gorp.Comopnents
{
    public class FunnyScale : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.localScale = Vector3.one * 2;
        }
    }
}
