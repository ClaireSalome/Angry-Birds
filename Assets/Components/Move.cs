using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	//composant pour récupérer les entrées du joueur concernant l'angle et la vitesse

	// gravite
	public Vector3 earth_gravity = new Vector3 (0,-9.81f,0);

	//position initiale
	public Vector3 init_position = new Vector3(-8f, -2.8f,0);

	//vitesse entrée par l'utilisateur
	public Vector3 vitesse_init = new Vector3 (2.5f, 4.5f, 0f);

	// vecteur vitesse
	public Vector3 vitesse = new Vector3(2.5f, 4.5f, 0);

	//pour le declenchement du tir
	public bool inMovement = false ;

	//pour les frottements du sol
	public bool groundContact = false ;

	//pour un nouveau tir
	public bool new_projectile = false ;

	public int numOfTrajectoryPoints = 20;

	public ArrayList trajectoryPoints = new ArrayList();
}