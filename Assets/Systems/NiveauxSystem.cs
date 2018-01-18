using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class NiveauxSystem : FSystem {


	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true;

	Button lvl_1 = GameObject.Find ("lvl1").GetComponent<Button> ();
	Button lvl_2 = GameObject.Find ("lvl2").GetComponent<Button> ();
	Button lvl_3 = GameObject.Find ("lvl3").GetComponent<Button> ();
	Button lvl_4 = GameObject.Find ("lvl4").GetComponent<Button> ();
	Button lvl_5 = GameObject.Find ("lvl5").GetComponent<Button> ();
	Button lvl_6 = GameObject.Find ("lvl6").GetComponent<Button> ();
	Button lvl_7 = GameObject.Find ("lvl7").GetComponent<Button> ();
	Button lvl_8 = GameObject.Find ("lvl8").GetComponent<Button> ();
	Button lvl_9 = GameObject.Find ("lvl9").GetComponent<Button> ();
	Button lvl_10 = GameObject.Find ("lvl10").GetComponent<Button> ();
	Button lvl_11 = GameObject.Find ("lvl11").GetComponent<Button> ();
	Button lvl_12 = GameObject.Find ("lvl12").GetComponent<Button> ();

	Button accueil = GameObject.Find("accueil").GetComponent<Button>();

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
	
		if (addEvent) {

			accueil.onClick.AddListener (home);

			//pour charger les niveaux
			//TODO griser les menus non accessibles tant qu'on n'a pas fini certains niveaux
			lvl_1.onClick.AddListener (level1);
			lvl_2.onClick.AddListener (level2);
			lvl_3.onClick.AddListener (level3);
			lvl_4.onClick.AddListener (level4);
			lvl_5.onClick.AddListener (level5);
			lvl_6.onClick.AddListener (level6);
			lvl_7.onClick.AddListener (level7);
			lvl_8.onClick.AddListener (level8);
			lvl_9.onClick.AddListener (level9);
			lvl_10.onClick.AddListener (level10);
			lvl_11.onClick.AddListener (level11);
			lvl_12.onClick.AddListener (level12);

			addEvent = false;
		}
	
	}

	public void home(){
		GameObjectManager.loadScene ("menu");
	}

	private void level1(){

		GameObjectManager.loadScene("niveau1");
	}

	private void level2(){

		GameObjectManager.loadScene("niveau2");
	}

	private void level3(){

		GameObjectManager.loadScene("niveau3");
	}

	private void level4(){

		GameObjectManager.loadScene("niveau4");
	}

	private void level5(){

		GameObjectManager.loadScene("niveau5");
	}

	private void level6(){

		GameObjectManager.loadScene("niveau6");
	}

	private void level7(){

		GameObjectManager.loadScene("niveau7");
	}

	private void level8(){

		GameObjectManager.loadScene("niveau8");
	}

	private void level9(){

		GameObjectManager.loadScene("niveau9");
	}

	private void level10(){

		GameObjectManager.loadScene("niveau10");
	}

	private void level11(){

		GameObjectManager.loadScene("niveau11");
	}

	private void level12(){

		GameObjectManager.loadScene("niveau12");
	}

}