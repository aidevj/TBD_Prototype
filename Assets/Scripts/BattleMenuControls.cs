using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuControls : MonoBehaviour {
	public List <Button> buttons;
	public Button buttonPrefab;
	private int currentButton;
	public GameObject canv;
	public GameObject player1;
	public GameObject player2;
	public GameObject enemy;

	private int currentPlayer;

	private int turn;

	//Reese placeholder
	private int reese_maxhp = 50;
	private int reese_curhp = 50;
	private int reese_maxap = 20;
	private int reese_curap = 20;

	private int reese_attack = 4;
	private int reese_defense = 3;
	private int reese_dexterity = 3; 
	private string reese_element = "none";

	//private string[] reese_attacks = {"Regular Attack", "Water Attack", "Heal"}; //would be a list of Ability Objects(?)
	private Ability[] player_attacks;


	//enemy stat placeholder
	//enemy1 = Fire Bird
	private int enemy1_maxhp = 30;
	private int enemy1_curhp = 70;
	private int enemy1_maxap = 15;
	private int enemy1_curap = 15;
					  
	private int enemy1_attack = 3;
	private int enemy1_defense = 3;
	private int enemy1_dexterity = 5; 
	private string enemy1_element = "fire";

	// Use this for initialization
	void Start () {
		player_attacks = player1.GetComponents<Ability>();


		currentPlayer = 0;
		currentButton = 0;

		CreateButtons();
		turn = 0;

	}

	// Update is called once per frame
	void Update () {
		//PLAYER TURN

			if (Input.GetKeyUp (KeyCode.R)) { //select next attack
				DeselectButton ();
				currentButton++;
				if (currentButton >= buttons.Count) {
					currentButton = 0;
				}
				SelectButton ();
			}

			if (Input.GetKeyUp (KeyCode.Q)) { //select previous attack
				DeselectButton ();
				currentButton--;
				if (currentButton < 0) {
					currentButton = buttons.Count - 1;
					;
				}
				SelectButton ();
			}

			//-----------------------------------------------------------CALL PLAYER ATTACK
			if (Input.GetKeyUp (KeyCode.Space)) { //confirm attack
				buttons [currentButton].onClick.Invoke ();
			}


			//-----------------------------------------------------------SWITCH USER
			if (Input.GetKeyUp (KeyCode.Return)) { 


				for (int i = 1; i < 4; i++)
					Destroy (GameObject.Find ("Button" + i));
				buttons.Clear ();

				if (currentPlayer == 0) {
					currentPlayer = 1;
					Debug.Log ("Switched from Reese to Alicea");
					player_attacks = player2.GetComponents<Ability> ();
				} else if (currentPlayer == 1) {
					currentPlayer = 0;
					Debug.Log ("Switched from Alicea to Reese");
					player_attacks = player1.GetComponents<Ability> ();
				}

				CreateButtons ();

			}

		
		//-----------------------------------------------------------ENEMY ATTACK CALL
		if(reese_curap <=0){

			CalcAttackOnAlly ();
		}

	}

	//-----------------------------------------------------------BUTTON SET UP
	public void CreateButtons(){

		//makes 3 buttons on it's own
		//ideally we'd pull in a Character's info to populate with their specific attacks
		for (int i = 1; i < 4; i++) {
			Button b1 = Instantiate (buttonPrefab) as Button;
			b1.name = "Button" + i;
			b1.transform.SetParent (canv.transform, false);
			b1.transform.position += new Vector3 (-420f, -30f * i, 0f);
			b1.GetComponentInChildren<Text> ().text = player_attacks[i-1].name;
			b1.onClick.AddListener (() => ConfirmAttack ());

			buttons.Add (b1);
		}

		SelectButton ();
	
	}


	//-----------------------------------------------------------BUTTON USE
	public void SelectButton(){

		ColorBlock cb = buttons [currentButton].colors;
		cb.normalColor = new Color(0.5F, 0.6F, 0.7F,1F);
		buttons [currentButton].colors = cb;
		buttons [currentButton].transform.position += new Vector3 (15f, 0f, 0f);
	}
	public void DeselectButton(){

		ColorBlock cb = buttons [currentButton].colors;
		cb.normalColor = Color.white;
		buttons [currentButton].colors = cb;
		buttons [currentButton].transform.position -= new Vector3 (15f, 0f, 0f);
	}

	public void ConfirmAttack(){

		//Debug.Log (buttons[currentButton].GetComponentInChildren<Text>().text + " CLICKED");

		//make sure the enemy is still alive before attacking
		if (enemy1_curhp > 0)
			CalcAttackOnEnemy (currentButton);
		else
			Debug.Log ("THERES NOTHING TO ATTACK");
	
	}
	//------------------------------------------------CALCULATIONS
	//-----------------------------------------------------------PLAYER ATTACK
	public void CalcAttackOnEnemy(int attackID){ 
		//should take in the enemy object when put together
		//currently pretending the enemy is in range

		//IF THE PLAYER CAN AFFORD THE MOVE
		if (reese_curap - player_attacks [attackID].cost >= 0) {
			float damage = 0;

			Debug.Log ("Reese used " + player_attacks [attackID].name);

			//CHECK FOR CRITICAL HIT
			if (CalcCrit (reese_dexterity)) {
				Debug.Log ("Critical Hit!!!");

				//calculate damage if critical
				damage = (float)((reese_attack * 1.5) - (float)(enemy1_defense / 5));

			} else {
				//calculate damage if NOT critical
				damage = (float)(reese_attack - (float)(enemy1_defense / 5));

			}

			//ADJUST DAMAGE BASED ON ELEMENT
			if (player_attacks [currentButton].element == "water" && enemy1_element == "fire") {
				Debug.Log ("ELEMENTAL ADVANTAGE X1.5");
				damage *= 1.5f;
			}


			//deduct enemy HP based on damage
			enemy1_curhp -= Mathf.FloorToInt (damage);

			//deduct Reese AP
			reese_curap -= player_attacks [attackID].cost;
			Debug.Log ("Reese AP: " + reese_curap);
			Debug.Log ("Damage Output: " + damage);
			Debug.Log ("Enemy HP: " + enemy1_curhp);
		} else {
			Debug.Log ("NO MORE AP");
		
		}

	}

	//-----------------------------------------------------------ENEMY ATTACK
	public void CalcAttackOnAlly(){

		if (enemy1_curap > 0){
			enemy1_curap -= 3;

			float damage = 0;

			Debug.Log ("ENEMY USED AN ATTACK");

			//CHECK FOR CRITICAL HIT
			if (CalcCrit (enemy1_dexterity)) {
				Debug.Log ("Critical Hit!!!");

				//calculate damage if critical
				damage = (float)((enemy1_attack * 1.5) - (float)(reese_defense / 5));

			} else {
				//calculate damage if NOT critical
				damage = (float)(enemy1_attack - (float)(reese_defense / 5));

			}
			reese_curhp -= Mathf.FloorToInt (damage);

			Debug.Log ("Enemy AP: " + enemy1_curap);
			Debug.Log ("Damage Output: " + damage);
			Debug.Log ("Player HP: " + reese_curhp);
		}
		else  {
			Debug.Log ("PLAYER TURN");
			reese_curap = 20;
			//turn = 0;
		} 
	
	}

	//-----------------------------------------------------------CRITICAL CALCULATION
	public bool CalcCrit(int userDex){
		float chance = Random.Range (0.0f, 100.0f);

		float threshold = (float)(10 + (float)userDex * 0.75);

		if (chance <= threshold)
			return true;
		else
			return false;
	}
}
