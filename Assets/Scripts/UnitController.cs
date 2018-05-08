// Aiden
using UnityEngine;
using UnityEngine.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the Player controls for units on the grid map.
/// Since the controlled unit changed, which one getting controlled is changed here.
/// </summary>
public class UnitController : MonoBehaviour {
	
	public float speed = 1f;

	private HUDManager HUDManagerScript;

	//[HideInInspector] 
	public List<Unit> units = new List<Unit> ();

	private bool controllerOn = true;					// determines if controller is allowed to work, ***disable (false) on layover screens, etc.***

	public GameObject unitHolderObj;					// gameobject called "Units" that holds all the unit in play
	public Unit controlledUnit;							// the Unit to be controlled currently
	private Transform unitTransform;
	private int currentUnitIndex = 0;					// the current index in the list of units

	// Coordinates for pathfinding/movement
	public HexCoordinates initialCoord;										// first position of unit since last move apply or since switched to
	public Stack<HexCoordinates> currentPath = new Stack<HexCoordinates>();					// Stack of current walked path
		// NOTE: THIS SHOULD NEVER INCLUDE THE INITIAL COORD (at least not for now)

	// Properties
	public Transform UnitTransform {
		get { return UnitTransform; }
	}

	public bool ControllerOn {
		set { controllerOn = value; }
		get { return controllerOn; }
	}

	void Start () {
		// Get reference to HUD Manager script
		HUDManagerScript = GameObject.Find ("UICanvas").GetComponent<HUDManager> ();

		// Populate the Units list
		foreach (Transform child in unitHolderObj.transform) {
			units.Add (child.gameObject.GetComponent<Unit>());
		}

		// Default the first in the list to the current controlled unit
		controlledUnit = units[0];
		controlledUnit.gameObject.layer = 0;		// CURRENT CONTROLLED UNIT MUST BE ON LAYER 0, OTHERS MUST BE ON 8
		unitTransform = controlledUnit.transform;
		HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);
	}

	/// Change the Controlled unit to which these controls will now apply
	public void CycleUnit() {
		// Apply latest path first if there is one
		if (currentPath.Count != 0 )
			ApplyMove(currentPath.Count);

		currentUnitIndex++;
		if (currentUnitIndex > units.Count - 1)
			currentUnitIndex = 0;

		controlledUnit = units[currentUnitIndex];
		unitTransform = controlledUnit.transform;

		// set initial coord
		initialCoord = HexCoordinates.FromPosition(unitTransform.position);


		// change all of the unit's layers to 8 before changing the current to 0
		foreach (Unit u in units) {
			u.gameObject.layer = 8;
		}
		controlledUnit.gameObject.layer = 0;

		// change UI
		HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);
	}

	void Update () {
		if (controllerOn)
			unitTransform.Translate (Input.GetAxis ("Horizontal_Player") * speed, Input.GetAxis ("Vertical_Player"), 0);
	}

	// When the terrain editor is open, disable units and mobility
	public void DisableController() {
		controllerOn = false;
		Debug.Log ("Controller DISABLED");
	}

	public void EnableController() {
		controllerOn = true;
		Debug.Log ("Controller ENABLED");
	}

	// MOVEMENT---------------------------------------------------------

	/// <summary>
	/// Confirms the last path movement and consumes the appropriate amount of AP
	/// according to the size of the currentPath stack
	/// </summary>
	/// <param name="numHexTravelled">Number hex travelled.</param>
	void ApplyMove(int numHexTravelled) {
		controlledUnit.currentAP -= numHexTravelled * controlledUnit.moveCost;

		// clears current stack of moves
		currentPath.Clear ();

		// set NEW initial coord
		// TO DO
	}

	void UndoMove(){
		// TO DO: return unit position to the initial coordinate position (cell's transform.position)
		//			clear currentPath stack
	}
		
}
