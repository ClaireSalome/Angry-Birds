using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	//composant pour les données concernant le déplacement du projectile

	// gravite
	public Vector3 earth_gravity = new Vector3 (0,-9.81f,0);

	//position initiale
	public Vector3 init_position = new Vector3(-9f, -3f,0);

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

	//affichage de la trajectoire pour les premiers niveaux
	public int numOfTrajectoryPoints = 30; 

	public ArrayList trajectoryPoints = new ArrayList();

	// pour modifier la trajectoire une seule fois quand on touche un object wood
	public ArrayList idStructure = new ArrayList ();


	// pour les pierres à fissurer : on ne fissure que la première touchée
	public bool stone_touched = false;

}