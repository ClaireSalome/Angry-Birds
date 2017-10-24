using UnityEngine;
using FYFY;

public class MoveProSystem : FSystem {
	//Système pour soumettre le projectile aux forces 


	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in _projectile) {

			Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();
			DataProjectile dp = go.GetComponent<DataProjectile> ();

			//au clic gauche de la souris
			if (Input.GetKey (KeyCode.Mouse0).Equals (true)) {
				rb.velocity = dp.get_calculated_velocity ();
				rb.isKinematic = false;
			}
				

		}
	}
}