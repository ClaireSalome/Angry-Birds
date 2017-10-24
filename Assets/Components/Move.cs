using UnityEngine;

public class Move : MonoBehaviour {
	//composant pour récupérer les entrées du joueur concernant l'angle et la vitesse


	/*
	 * vitesse à laquelle le projectile est lancé 
	 */
	//public float vitesse = 8.0f ;

	public Vector3 earth_gravity = new Vector3 (0,-9.81f,0);

	/*
	 * angle de la trajectoire
	 */
	public int angle = 30;

	public Vector3 vitesse = new Vector3(2.5f, 4.5f, 0);

}