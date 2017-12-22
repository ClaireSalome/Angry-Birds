using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager ;
using FYFY_plugins.TriggerManager ;

public class CollisionSystem : FSystem {


	private Family _triggered2D = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered2D))) ;
	private Family _incollision = FamilyManager.getFamily(new AllOfComponents(typeof(InCollision2D)));

	//récupérer le score total pour les récompenses
	private TotalScore total = GameObject.FindGameObjectWithTag("total").GetComponent<TotalScore>() ;

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		// gestion des collisions avec le sol et les structures
		foreach (GameObject go in _incollision) {
			InCollision2D col = go.GetComponent<InCollision2D> ();

			Move mo = go.GetComponent<Move> ();

			foreach (GameObject target in col.Targets) {
				// ground collision
				// quand le projectile ne bouge plus -> nouveau projectile créé donc ne plus toucher à la vitesse
				if (target != null && target.tag.Equals ("Ground") && mo.inMovement == true) {
					mo.groundContact = true;
					mo.vitesse.y = 0f;
				}

				//mo.inMovement == true pour éviter modification de vitesse quand on remet un nouveau projectile
				if (target.tag.Equals ("wood_struct") && mo.inMovement == true) {
					Rigidbody2D dP = go.GetComponent<Rigidbody2D> ();
					Rigidbody2D rB = target.GetComponent<Rigidbody2D> ();
					mo.vitesse.x  = (mo.vitesse.x*(dP.mass - rB.mass)) / (dP.mass + rB.mass);
					mo.vitesse.y  = (mo.vitesse.y*(dP.mass - rB.mass)) / (dP.mass + rB.mass);
				}

				if (target != null && target.tag.Equals ("reward")) {
					Collect co = target.GetComponent<Collect> ();
					total.score_total += co.reward;
					GameObjectManager.unbind (target);
					GameObject.Destroy (target);
				}

			}
		}

		// détection des récompenses
		foreach (GameObject go in _triggered2D) {
			Triggered2D tr = go.GetComponent<Triggered2D> ();
			foreach (GameObject target in tr.Targets){

				if (target.tag.Equals ("reward")) {
					Collect co = target.GetComponent<Collect> ();
					total.score_total += co.reward;
					GameObjectManager.unbind (target);
					GameObject.Destroy (target);
				}
			}
		}
	}
}
