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

	[SerializeField]
	private List<GameObject> units = new List<GameObject> ();	// list of controllable units

	public bool controllerOn = true;							// determines if controller is allowed to work, ***disable (false) on layover screens, etc.***

	public GameObject unitHolderObj;							// object that holds the the unit gameobjects called "Units"
	public GameObject controlledUnit;							// the Unit to be controlled currently
	private Transform unitTransform;
	private int currentUnitIndex = 0;							// the current index in the list of units
		

	// Properties
	public Transform UnitTransform {
		get { return UnitTransform; }
	}

	void Start () {
		HUDManagerScript = GameObject.Find ("UICanvas").GetComponent<HUDManager> ();

		unitTransform = controlledUnit.transform;

		// Populate the Units list
		foreach (Transform child in unitHolderObj.transform) {
			units.Add (child.gameObject);
		}

		HUDManagerScript.UpdateActiveUnitText(controlledUnit.GetComponent<Unit>().Name);
	}

	// Change the Controlled unit to which these controls will now apply
	public void CycleUnit() {
		currentUnitIndex++;
		if (currentUnitIndex > units.Count - 1)
			currentUnitIndex = 0;

		controlledUnit = units[currentUnitIndex];
		unitTransform = controlledUnit.transform;

		// change UI
		HUDManagerScript.UpdateActiveUnitText(controlledUnit.GetComponent<Unit>().Name);
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
