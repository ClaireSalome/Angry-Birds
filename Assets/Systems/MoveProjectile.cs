using UnityEngine;
using FYFY;

public class MoveProjectile : FSystem {

	// _projectile = famille des entités ayant le composant Move
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
			Transform tr = go.GetComponent<Transform> ();
			Move mo = go.GetComponent<Move> ();
			Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();

			Vector3 pos_init = tr.position;
			Vector3 mousePoint = Vector3.zero;

			if(Input.GetMouseButtonDown(0).Equals(true) ){
				mousePoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				mousePoint.z = 0f;
				tr.position = mousePoint ;
			}

			// quand on relache le bouton droit de la souris
			if (Input.GetMouseButtonUp(0).Equals(true) ){
				// normalement
				// devrait créer la bonne direction
				rb.velocity= new Vector2 (mo.vitesse_init * Mathf.Cos (mo.angle*Mathf.PI/180) , mo.vitesse_init * Mathf.Sin (mo.angle*Mathf.PI/180));
				// le projectile est de nouveau soumis aux forces physiques
				rb.isKinematic = false;

			}
		}

	}
}