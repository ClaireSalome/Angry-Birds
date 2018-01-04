using UnityEngine;

public class DestroyedStruct : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	public int nb_destroyed_struct = 0;

	public string level_to_load = "";

	public float destroyed_treshold = -2.5f;

}