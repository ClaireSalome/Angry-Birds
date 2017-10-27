using UnityEngine;

public class Move : MonoBehaviour {
	//composant pour récupérer les entrées du joueur concernant l'angle et la vitesse

	// gravite
	public Vector3 earth_gravity = new Vector3 (0,-9.81f,0);

	// vecteur vitesse
	public Vector3 vitesse = new Vector3(3f, 5f, 0);

	//pour le declenchement du tir
	public bool inMovement = false ;

	/*
	 * angle de la trajectoire
	 */
//	public int angle = 30;

}