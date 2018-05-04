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
	public List<Unit> units = new List<Unit> ();		// ******Cahnge to this later to access the data from units directly

	public bool controllerOn = true;					// determines if controller is allowed to work, ***disable (false) on layover screens, etc.***

	public GameObject unitHolderObj;					// gameobject called "Units" that holds all the unit in play
	public Unit controlledUnit;							// the Unit to be controlled currently
	private Transform unitTransform;
	private int currentUnitIndex = 0;					// the current index in the list of units
		

	// Properties
	public Transform UnitTransform {
		get { return UnitTransform; }
	}

	void Start () {
		HUDManagerScript = GameObject.Find ("UICanvas").GetComponent<HUDManager> ();

		// Populate the Units list
		foreach (Transform child in unitHolderObj.transform) {
			units.Add (child.gameObject.GetComponent<Unit>());
		}

		// Default the first in the list to the current controlled unit
		controlledUnit = units[0];
		unitTransform = controlledUnit.transform;
		HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);
	}

	/// Change the Controlled unit to which these controls will now apply
	public void CycleUnit() {
		currentUnitIndex++;
		if (currentUnitIndex > units.Count - 1)
			currentUnitIndex = 0;

		controlledUnit = units[currentUnitIndex];
		unitTransform = controlledUnit.transform;

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
	}

	public void EnableController() {
		controllerOn = true;
	}
}
