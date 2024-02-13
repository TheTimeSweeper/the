using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.General.Components
{
    public class ProjectileSpawnDotzoneChildren : MonoBehaviour
    {
        [SerializeField]
        private ProjectileController parentProjectile;

        [SerializeField]
        private HitBoxGroup parenthitboxGroup;

        [SerializeField]
        public DotZoneChild childPrefab;

        [SerializeField]
        private Transform[] childProjectiles;

        private void Start()
        {
            if (!NetworkServer.active)
                return;

            parenthitboxGroup.hitBoxes = new HitBox[childProjectiles.Length];

            for (int i = 0; i < childProjectiles.Length; i++)
            {
                //var child = 
                DotZoneChild Proje = Instantiate(childPrefab, childProjectiles[i].position, childProjectiles[i].rotation);
                Proje.projectileController.owner = parentProjectile.owner;
                Proje.projectileController.procChainMask = parentProjectile.procChainMask;
                Proje.projectileController.procCoefficient = parentProjectile.procCoefficient;
                Proje.projectileController.teamFilter.teamIndex = parentProjectile.teamFilter.teamIndex;
                parenthitboxGroup.hitBoxes[i] = Proje.hitbox;
                NetworkServer.Spawn(Proje.gameObject);
            }
        }

        // Token: 0x06004361 RID: 17249 RVA: 0x001183F8 File Offset: 0x001165F8
        protected Quaternion GetRandomChildRollPitch()
        {
            Quaternion lhs = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360), Vector3.forward);
            Quaternion rhs = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360), Vector3.left);
            return lhs * rhs;
        }
    }
}