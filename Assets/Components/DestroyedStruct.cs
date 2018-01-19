using UnityEngine;

public class DestroyedStruct : MonoBehaviour {

	//compte le nombre de structures détruites
	public int nb_destroyed_struct = 0;

	//niveau à charger à la fin
	public string level_to_load = "";

	//limite en dessous de laquelle la structure est considérée comme détruite
	public float destroyed_treshold = -2.5f;

	//nombre de tirs effectués par le joueur
	public int nb_shoot = 0 ;

	//nombre de tirs minimum nécessaire
	public int nb_min_shoot = 1;

	// indique si le niveau est fini (pour affichage des niveaux dans le menu)
	public bool ended_lvl = false ;
}