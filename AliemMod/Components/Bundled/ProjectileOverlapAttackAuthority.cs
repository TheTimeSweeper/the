using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace AliemMod.Components.Bundled
{
    public class ProjectileOverlapAttackAuthority : ProjectileOverlapAttack {
        bool? authority = null;
        new public void FixedUpdate()
        {
            if(authority == null)
            {               //absolutely cursed
                authority = projectileController.isPrediction || (NetworkServer.active && projectileController.clientAuthorityOwner == null);
            }
            if(authority.Value){
                attack.Fire();
            }
        }
    }
}
