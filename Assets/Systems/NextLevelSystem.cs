using UnityEngine;
using FYFY;
using System.Collections;

public class NextLevelSystem : FSystem {

	private GameObject[] structures = GameObject.FindGameObjectsWithTag("wood_struct");


	//pour compter le nombre de structures détruites
	private Family dest = FamilyManager.getFamily (new AllOfComponents(typeof(DestroyedStruct)));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		GameObject nextLvl = dest.First ();
		TotalScore ts = GameObject.FindGameObjectWithTag ("total").GetComponent<TotalScore> ();

		foreach(GameObject go in structures) {

			Destroyed d = go.GetComponent<Destroyed> ();
			if(d.destroyed == false && d.transform.position.y < nextLvl.GetComponent<DestroyedStruct> ().destroyed_treshold) {
				d.destroyed = true;
				nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct++;
				// mise a jour du score
				ts.score_total += 2;
			}
		}

		if (nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct == structures.Length) {
			//NIVEAU TERMINE : affichage d'un message
			GameObject winText = GameObject.FindGameObjectWithTag("win") ;
			winText.GetComponent<Canvas>().sortingOrder = 2;
			nextLvl.AddComponent<ChangeLevel> ();

		}
	
	}
}

// pour avoir un délai entre la réussite du niveau et le changement de scène
public class ChangeLevel : MonoBehaviour
{
	private Family dest = FamilyManager.getFamily (new AllOfComponents(typeof(DestroyedStruct)));

	public void Start(){
		StartCoroutine(waitForIt());
	}

	IEnumerator waitForIt() {
		GameObject nextLvl = dest.First ();
		yield return new WaitForSeconds(3f);
		GameObjectManager.loadScene (nextLvl.GetComponent<DestroyedStruct>().level_to_load);
	}


}