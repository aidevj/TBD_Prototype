// Aiden
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Script for the HexGrid object. Handles creating and trangulating the meshes of the cells of the grid.
/// Also sets coordinates and writes them on grid.
/// </summary>
public class HexGrid : MonoBehaviour {

	// debugging options
	public bool ShowCoordinates = true;
	public bool LogHexTouches = true;

	public UnitController ControllerScript; 				// Reference to the controller script in order to track unit movement. Found in Start()
	public HUDManager HUDScript;

	private Unit currentUnit;

	public List<Unit> units;								// references to the units and enemies in UnitController
	public List<Enemy> enemies;

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	public Color targetColor;

	Canvas gridCanvas;
	HexMesh hexMesh;

	HexCell[] cells;                                        // Array of hexCells in the Grid

    // hex type stuff
    public Color defaultColor = Color.white;
	public Color activeColor = Color.cyan; 					// "touched"
	public GameObject wallPrefab;
	public GameObject mistFXPrefab;
	public GameObject poisonFXPrefab;

	private HexCoordinates lastCoords;
	private string lastCoordinatesAsString = "";

	public bool HexClickEnabled = true;		// cahnged in gamemanager

	// Read-only cells
	public HexCell[] Cells {
		get { return cells; }
	}

	void Awake () {
		cells = new HexCell[height * width];

		gridCanvas = GetComponentInChildren<Canvas> ();
		hexMesh = GetComponentInChildren<HexMesh> ();

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start(){
		// after the grid has awoken
		hexMesh.Triangulate (cells);

		units = ControllerScript.units;
		enemies = ControllerScript.enemies;

        // set default colors
        defaultColor = new Color(1f, 1f, 1f, 0.25f);


	}

	void Update () {
		// Must get current unit in update to update when current unit is changes
		currentUnit = ControllerScript.controlledUnit.GetComponent<Unit>();


        // hex click should be enabled during: PLAYER CONTROL MODE, FREE ROAM CAM MODE, and TERRAIN EDITOR
		if (HexClickEnabled == true) {
			// get position of unit for movement
			TouchCell (currentUnit.transform.position);

			// handle target selection
            // Hover (player control mode)
                // since only Melee functionality so far, restrict selection be radial within 1 hex of the player
            
            // Click
            // Hover (free cam mode)
                 // same as terrain editor
			if (Input.GetMouseButtonDown (0)) {
				Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (inputRay, out hit)) {
					SelectTarget (hit.point);
				}
			}
		}
	}

	/// <summary>
	/// Selects a target on a position on the grid
	/// </summary>
	public void SelectTarget(Vector3 position) {
		position = transform.InverseTransformPoint(position);

		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		//if (LogHexTouches) Debug.Log("Touched at: " + coordinates.ToString());

		int index = HexCoordinates.GetIndexOfCoordinate(coordinates, width);
		HexCell cell = cells[index];

		// if there is an enemy there, assign as target
		if (cell.IsOccupied () == true && cell.occupant.tag == "Enemy") {
			ControllerScript.target = cell.occupant;
			cell.color = targetColor;

			hexMesh.Triangulate (cells);
		} else {
			Debug.LogWarning ("No enemy to target.");
		}


	}

	/// <summary>
	/// Used for unit movement on the cells
	/// </summary>
	/// <param name="position">Position of unit.</param>
	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);

		// to know where we are touching, we have to convert the touch position to hex coordinates
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);


		#region Checks upon Cell Change
		if (coordinates.ToString() != lastCoordinatesAsString) {	
			if (LogHexTouches) Debug.Log("Walked on: " + coordinates.ToString());

			int index = HexCoordinates.GetIndexOfCoordinate(coordinates, width);
			HexCell cell = cells[index];
			cell.color = activeColor;
			hexMesh.Triangulate(cells);

			OccupyCell(index, ControllerScript.controlledUnit);

			// unoccupy the last walked cell (top of stack) before pusnhing the new one (only if there is a last)
			if (ControllerScript.currentPathWalked.Count > 0) {
				UnoccupyCell(HexCoordinates.GetIndexOfCoordinate(ControllerScript.currentPathWalked.Peek(), width));
				// Subtract AP per move (done here so it doesnt subtract until after moved at least once)
				int thisAPCost = ControllerScript.controlledUnit.moveCost * cell.APCostMultiplier;
				ControllerScript.currentPathAPCost.Push(thisAPCost);
				ControllerScript.controlledUnit.currentAP -= thisAPCost;
				HUDScript.UpdateAPBar();

				// Push damages
				if (cell.terrainType == TerrainType.Poison) {
					ControllerScript.pathDamage.Push(5);
					ControllerScript.controlledUnit.currentHP -= 5;
					if (ControllerScript.controlledUnit.currentHP <= 0) {
						ControllerScript.controlledUnit.currentHP = 1;
					}

					HUDScript.UpdateHPBar();
				}
				else {
					ControllerScript.pathDamage.Push(0);
				}

			}
			// Push this new coord to the controller's current path stack
			ControllerScript.currentPathWalked.Push(coordinates);


		}
		#endregion

		lastCoordinatesAsString = coordinates.ToString ();
	}

    /// <summary>
    /// Used to change the color of the cells being targeted by the unit.
    /// </summary>
    /// <param name="position">Targeting position of hex.</param>
    void TargetCell(Vector3 position)
    {
        // TO DO: 
    }

	/// <summary>
	/// Modify's a cell from its position.
	/// Changes the color and terrain type.
	/// To be called from TerrainEditor.cs
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="color">Color.</param>
	/// <param name="type">Type.</param>
	public void EditCell (Vector3 position, Color color, TerrainType type) {
		position = transform.InverseTransformPoint(position);

		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		if (LogHexTouches) Debug.Log("Touched at: " + coordinates.ToString());

		int index = HexCoordinates.GetIndexOfCoordinate(coordinates, width);
		HexCell cell = cells[index];

		// if the same, dont do anything
		//if (cell.terrainType == type) return; // dont do this for now since walk path colors can override the terrain type color

		// change cell properties
		cell.color = color;
		cell.terrainType = type;

		if (type == TerrainType.Impassible) {
			GameObject newWall = (GameObject)Instantiate (wallPrefab, cell.transform.position, Quaternion.identity);
			newWall.transform.parent = cell.gameObject.transform;
		}

		// edit AP cost multipliers
		if (type == TerrainType.RoughTerrain) {
			cell.APCostMultiplier = 2;
		}

		hexMesh.Triangulate(cells);
	}

	/// <summary>
	/// Resets the color of the grid to white.
	/// IMPORTANT: Do NOT use to reset grid walking paths, as this will clear structure and natural feature colors assigned on terrain.
	/// </summary>
	public void ResetGridColorToBlank() {
		foreach (HexCell cell in cells) {
			cell.color = defaultColor;
		}
		hexMesh.Triangulate(cells);
	}

	/// <summary>
	/// Restores the grid INCLUDING the terrain assigned to it.
	/// Erases walking paths etc.
	/// </summary>
	void RestoreGrid() {
		// TO DO
	}

	/// <summary>
	/// Creates cells at the given positions with all defaults.
	/// </summary>
	/// <param name="x">The x position.</param>
	/// <param name="z">The z position.</param>
	/// <param name="i">Index of the cell out of all.</param>
	void CreateCell (int x, int z, int i) {
		Vector3 position;
		// space them out according to radius and offset
		position.x = ( x + z * 0.5f  - z / 2 ) * (HexMetrics.innerRadius * 2f); //position.x = x * 10f; // <-- non shifted
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f); //position.z = z * 10f;

		// assign cell to the current index and assign its values
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

		// default attributes of cell
		cell.color = defaultColor;
		cell.terrainType = TerrainType.Normal;

		// instantiate the labels and show cell coordinates
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}



	/// <summary>
	/// Makes the isOccupied attribute of the specified cell true.
	/// </summary>
	/// <param name="index">Index.</param>
	public void OccupyCell(int index, Unit newOccupant) {
		cells [index].occupant = newOccupant;
	}

	public void UnoccupyCell(int index) {
		cells [index].occupant = null;
	}



}

// reference: http://catlikecodig.com/unity/tutorials/hex-map/part-1/