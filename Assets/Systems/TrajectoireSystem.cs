using UnityEngine;
using FYFY;

public class TrajectoireSystem : FSystem {
	//système pour calculer les vecteurs vitesses sur les axes
	//en fonction de angle et vitesse souhaités


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

			Move mo = go.GetComponent<Move> ();
			DataProjectile dp = go.GetComponent<DataProjectile> ();
			Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();

			float dt = Time.deltaTime;

			go.transform.position += mo.vitesse*dt + ((mo.earth_gravity/rb.mass)* Mathf.Pow(dt,2))/2f;

            //TODO: mettre a jour la vitesse??
            mo.vitesse += 0.5f * (2 * mo.earth_gravity / rb.mass) * dt; 
//			Vector2 movement = new Vector2 (Mathf.Cos (mo.angle*Mathf.PI/180), Mathf.Sin (mo.angle*Mathf.PI/180));
//			dp.set_calculated_velocity(movement * mo.vitesse);



		}

	}
}