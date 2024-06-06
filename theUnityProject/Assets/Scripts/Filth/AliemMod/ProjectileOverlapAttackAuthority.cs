using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace AliemMod.Components.Bundled {
    public class ProjectileOverlapAttackAuthority : ProjectileOverlapAttack {

        new public void FixedUpdate() {
            bool authorityquestionmark = false;
            if (projectileController.clientAuthorityOwner != null || projectileController.clientAuthorityOwner == null && NetworkServer.active) {
                authorityquestionmark = true;
            }
            Debug.LogWarning(authorityquestionmark);
        }
    }
}
