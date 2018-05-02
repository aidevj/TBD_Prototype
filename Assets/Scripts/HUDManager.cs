using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	// Other script references
	[HideInInspector]
	public UnitController UnitControllerScript;

	// HUD Objects
	public GameObject activeUnitObj;
	Text activeUnitText;
	// ...

	void Start () {
		UnitControllerScript = GameObject.Find ("ControllerManager").GetComponent<UnitController> ();

		// assign all text
		activeUnitText = activeUnitObj.GetComponent<Text> ();

	}

	void Update () {}

	public void UpdateActiveUnitText(string newText) {
		activeUnitText.text = newText;
	}
}
