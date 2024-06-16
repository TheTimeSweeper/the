using AliemMod.Content;
using AliemMod.Modules;

namespace ModdedEntityStates.Aliem
{
    public class ThrowGrenadeScepter : ThrowGrenade {

		new public static float DamageCoefficient => AliemConfig.M4_GrenadeDamage.Value * 2;

		protected override void ModifyState() {
			base.projectilePrefab = Projectiles.GrenadeProjectileScepter;
			base.damageCoefficient = DamageCoefficient;
		}
    }
}