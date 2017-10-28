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
				if (target.tag.Equals ("Ground")) {
					mo.groundContact = true;
					mo.vitesse.y =0f;
				}
			}
		}
	}
}