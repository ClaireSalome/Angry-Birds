using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager ;

public class CollisionSystem : FSystem {

	private Family _triggered2D = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered2D))) ;

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in _triggered2D) {
			Triggered2D tr = go.GetComponent<Triggered2D> ();
			Move mo = go.GetComponent<Move> ();

			foreach (GameObject target in tr.Targets) {
				// ground collision
				//quand le projectile ne bouge plus -> nouveau projectile créé donc ne plus toucher à la vitesse
				if (target.tag.Equals ("Ground") && mo.inMovement == true) {
					mo.groundContact = true;
					mo.vitesse.y =0f;
				}

				if (target.tag.Equals ("wood_struct")) {
					mo.vitesse.x = 0f;
				}
			}
		}
	}
}