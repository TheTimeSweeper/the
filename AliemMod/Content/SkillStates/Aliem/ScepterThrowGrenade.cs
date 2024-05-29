using AliemMod.Content;

namespace ModdedEntityStates.Aliem {
    public class ScepterThrowGrenade : ThrowGrenade {

		new public static float DamageCoefficient => AliemConfig.M4_GrenadeDamage.Value * 2;

		protected override void ModifyState() {
			base.projectilePrefab = Modules.Projectiles.GrenadeProjectileScepter;
			base.damageCoefficient = DamageCoefficient;
		}
    }
}