﻿// Aiden
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for the HexGrid object. Handles creating and trangulating the meshes of the cells of the grid.
/// Also sets coordinates and writes them on grid.
/// </summary>
public class HexGrid : MonoBehaviour {

	// debugging options
	public bool ShowCoordinates = true;
	public bool LogHexTouches = true;

	private UnitController ControllerScript; 				// Reference to the controller script in order to track unit movement. Found in Start()
	private Unit currentUnit;

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	Canvas gridCanvas;
	HexMesh hexMesh;

	HexCell[] cells;										// Array of hexCells in the Grid

	public Color defaultColor = Color.white;
	public Color activeColor = Color.cyan; 					// "touched"

	private string lastCoordinatesAsString = "";

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

		// get reference to ControllerManager's UnitController script
		ControllerScript = GameObject.Find("ControllerManager").GetComponent<UnitController>();

	}

	void Update () {
		// Call color changing TouchCell stuff to change at position of the unit
		currentUnit = ControllerScript.controlledUnit.GetComponent<Unit>();
		TouchCell (currentUnit.transform.position);

	}
		

	// use for player movement!!!
	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);

		// to know where we are touching, we have to convert the touch position to hex coordinates
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		if (coordinates.ToString() != lastCoordinatesAsString) {	// only do if its different
			if (LogHexTouches) Debug.Log("Walked on: " + coordinates.ToString());

			int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
			HexCell cell = cells[index];
			cell.color = activeColor;
			hexMesh.Triangulate(cells);
		}

		lastCoordinatesAsString = coordinates.ToString ();
	}

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);

		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		if (LogHexTouches) Debug.Log("Touched at: " + coordinates.ToString());

		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);


		lastCoordinatesAsString = coordinates.ToString ();
	}

	/// <summary>
	/// Resets the color of the grid to white.
	/// IMPORTANT: Do NOT use to reset grid walking paths, as this will clear structure and natural feature colors assigned on terrain.
	/// </summary>
	void ResetGridColorToBlank() {
		foreach (HexCell cell in cells) {
			cell.color = defaultColor;
		}
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
		cell.color = defaultColor;

		// instantiate the labels and show cell coordinates
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

}

// reference: http://catlikecodig.com/unity/tutorials/hex-map/part-1/