﻿using UnityEngine;
using FYFY;
using System.Collections;

public class TrajectoireSystem : FSystem {
	//système pour calculer les vecteurs vitesses sur les axes
	//en fonction de angle et vitesse souhaités


	// _projectile = famille des entités ayant le composant Move
	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in _projectile) {

			Move mo = go.GetComponent<Move> ();
			DataProjectile dp = go.GetComponent<DataProjectile> ();
			Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();

			float dt = Time.deltaTime;

			if (mo.inMovement == true) {

//				go.transform.position += mo.vitesse * dt + ((mo.earth_gravity / dp.masse) * Mathf.Pow (dt, 2)) / 2f;
//
//				mo.vitesse += 0.5f * (2 * mo.earth_gravity / dp.masse) * dt; 

				//si le projectile sort du champ de la caméra
				if (go.transform.position.x > 3f * Camera.main.orthographicSize) {
					mo.inMovement = false;
					mo.new_projectile = true;
				}

				//equations du cours
				float delta_x =  (mo.vitesse.x * dt);
				float delta_y = 0f;
				// coeff frottement solide
				float mu = 0.5f;

				//si le projectile n'a pas touché le sol
				if (mo.groundContact == false) {
					delta_y = (mo.vitesse.y * dt) + (dp.masse * mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
					mo.vitesse.y += dp.masse * mo.earth_gravity.y * dt;
					go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.y*Mathf.Rad2Deg );
				} 
				else {
                    //le projectile a touché le sol -> force de frottement
                    delta_x += mu * (dp.masse * mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
                    mo.vitesse.x += mu * mo.earth_gravity.y * dt;
					go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.x*Mathf.Rad2Deg );
					// si la vitesse est nulle, le projectile ne bouge plus
					if (mo.vitesse.x <= 0f && mo.vitesse.y <= 0f) {
						mo.inMovement = false;
						//faire une pause pour l'affichage
						int i = 0 ;
						while (i < 100000) {
							i++;
						}
						//new Wait().Start();
						mo.new_projectile = true;
					}
				}
					
				go.transform.position += new Vector3 (delta_x, delta_y, 0f);

			}

//			Vector2 movement = new Vector2 (Mathf.Cos (mo.angle*Mathf.PI/180), Mathf.Sin (mo.angle*Mathf.PI/180));
//			dp.set_calculated_velocity(movement * mo.vitesse);

			// angle = cos-1(x)

		}

	}

//	public class Wait : MonoBehaviour
//	{
//		private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));
//
//		public void Start(){
//			Debug.Log ("ci");
//			StartCoroutine(resetPro());
//		}
//
//		IEnumerator resetPro() {
//			Debug.Log ("first");
//			Move mo = _projectile.First ().GetComponent<Move> ();
//			yield return new WaitForSeconds(3);
//			Debug.Log ("Coucou");
//			mo.new_projectile = true;
//		}
//
//
//	}
}