using UnityEngine;
using UnityEngine.UI;

public class TerrainEditor : MonoBehaviour {

	public Color[] colors;		// array of colors to be used
	public HexGrid hexGrid;		// reference to our hex grid

	private Color activeColor;	// currently selected color

	private int currentTool = 0;	// index of current used tool
	private TerrainType activeTerrainType = TerrainType.Impassible;	// default

	public Button[] buttons;	// drag in the buttons (size of 7)

	/*	TOOL INDECIES
	 * 0	Wall			(Will be not interactiable, default selected tool)
	 * 1	Rough terrain
	 * 2	Trench
	 * 3	Poison Gas
	 * 4	Swamp
	 * 5	Heat wave
	 * 6	Mist
	 */

	void Awake () {
		SelectColor (0);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			HandleInput ();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			hexGrid.EditCell (hit.point, activeColor, activeTerrainType);
		}
	}

	public void SelectColor(int index) {
		activeColor = colors [index];
	}

	/// <summary>
	/// Changes the tool according to the index provided. Called by the button presses in the terrain editor menu.
	/// </summary>
	/// <param name="index">Index of tool.</param>
	public void ChangeTool(int index) {
		currentTool = index;

		// disable selected button and enable the rest
		foreach (Button b in buttons) {
			b.interactable = true;
		}
		buttons [index].interactable = false;
	
		// set cooresponding color
		switch (index) {
		case 0:
			activeColor = colors [0];
			activeTerrainType = TerrainType.Impassible;
			break;
		case 1:
			activeColor = colors [1];
			activeTerrainType = TerrainType.RoughTerrain;
			break;
		case 2: 
			activeColor = colors [2];
			activeTerrainType = TerrainType.Trench;
			break;
		case 3:
			activeColor = colors [3];
			activeTerrainType = TerrainType.Poison;
			break;
		case 4:
			activeColor = colors [4];
			activeTerrainType = TerrainType.Swamp;
			break;
		case 5:
			activeColor = colors [5];
			activeTerrainType = TerrainType.HeatWave;
			break;
		case 6:
			activeColor = colors [6];
			activeTerrainType = TerrainType.Mist;
			break;
		}
	}


	// METHODS FOR DIFFERENT OPTIONS ------------------------------------------------

	public void PlaceWall(Vector3 location) {
		// spawn a block on top of this location

		// change terrain type on this hex

	}
}
