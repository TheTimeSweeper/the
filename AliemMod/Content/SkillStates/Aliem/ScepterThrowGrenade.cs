namespace ModdedEntityStates.Aliem {
    public class ScepterThrowGrenade : ThrowGrenade {

		new public static float DamageCoefficient = 24;

		protected override void ModifyState() {
			base.projectilePrefab = Modules.Projectiles.GrenadeProjectileScepter;
			base.damageCoefficient = DamageCoefficient;
		}
    }
}