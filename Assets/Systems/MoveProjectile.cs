using UnityEngine;
using FYFY;

public class MoveProjectile : FSystem {

	// _projectile = famille des entités ayant le composant Move
	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));
	private int cpt = 0;

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

			float speed = mo.vitesse_init - (mo.speed_decrease*cpt);
			int angle = mo.angle;

			if (speed != 0) {
				float new_x = speed * Mathf.Cos (angle) * Time.deltaTime;

				float new_y = ((-Physics.gravity.y / 2) *
				             (Mathf.Pow (new_x, 2) / (Mathf.Pow (speed * Mathf.Cos (angle), 2)))) +
				             (new_x * Mathf.Tan (angle));


				tr.position += new Vector3 (Mathf.Abs (new_x), new_y);
			}

			cpt++;

			//TODO
			/*
			 * QUand on lance un object avec vitesse v et angle alpha, quelle est la distance en fonction du temps ?
			 * v = d/t -> d = v*t à chaque instant (mouvement uniforme)
			 * et l'angle ?
			 * 
			 */
		}

	}
}