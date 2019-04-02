// Aiden
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles the Player controls for units on the grid map.
/// Since the controlled unit changed, which one getting controlled is changed here.
/// </summary>
public class UnitController : MonoBehaviour {
	
	public float speed = 1f;

	private HUDManager HUDManagerScript;

	public HexGrid hexGrid;

	[HideInInspector]public List<Unit> units = new List<Unit> ();
	[HideInInspector]public List<Enemy> enemies = new List<Enemy> ();

	//public List<Unit> currentTurnUnits;						// list of what side is currently being controlled

	[SerializeField]private bool controllerOn = true;							// determines if controller is allowed to work, ***disable (false) on layover screens, etc.***

	public GameObject unitHolderObj;							// gameobject called "Units" that holds all the unit in play
	public GameObject enemyHolderObj;
	public Unit controlledUnit;									// the Unit controlled currently
	private Transform unitTransform;
	private int currentUnitIndex = 0;
    private int currentEnemyIndex = 0;

    public bool isPlayerTurn; 

	// Coordinates for pathfinding/movement
	[HideInInspector]public HexCoordinates initialCoord;														// first position of unit since last move apply or since switched to
	[HideInInspector]public Stack<HexCoordinates> currentPathWalked = new Stack<HexCoordinates>();					// Stack of current walked path
																							// NOTE: THIS SHOULD NEVER INCLUDE THE INITIAL COORD (at least not for now)
	[HideInInspector]public Stack<int> currentPathAPCost = new Stack<int>();	// keeps track of the AP costs during the  current path made

	[HideInInspector]public Stack<int> pathDamage = new Stack<int> ();		// keeps track of any damage accumulated during the path made

	public Unit target;										// target of action for currently controlled unit

	// Properties
	public Transform UnitTransform {
		get { return UnitTransform; }
	}

	public bool ControllerOn {
		set { controllerOn = value; }
		get { return controllerOn; }
	}

	void Start () {
		HUDManagerScript = GameObject.Find ("UICanvas").GetComponent<HUDManager> ();

        isPlayerTurn = true;

		// Populate the Units list
		foreach (Transform child in unitHolderObj.transform) {
			units.Add (child.gameObject.GetComponent<Unit>());
		}

		// Populate Enemies list
		foreach (Transform child in enemyHolderObj.transform) {
			enemies.Add (child.gameObject.GetComponent<Enemy> ());
		}

		// occupy cells with units from the start
		foreach (Unit u in units) {
			hexGrid.OccupyCell (HexCoordinates.GetIndexOfCoordinate (u.currentCoord, hexGrid.width), u);
		}
		foreach (Enemy e in enemies) {
			hexGrid.OccupyCell (HexCoordinates.GetIndexOfCoordinate (e.currentCoord, hexGrid.width), e);
		}

		// Default the first in the list to the current controlled unit
		controlledUnit = units[0];
		controlledUnit.gameObject.layer = 0;
		unitTransform = controlledUnit.transform;

		HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);
		HUDManagerScript.UpdateHPBar();
		HUDManagerScript.UpdateAPBar();
	}
    

	/// Cycles through controllable units (skips over dead units)
	public void CycleUnit() {

        if(isPlayerTurn)
        {
            // Apply latest path first if there is one
            if (currentPathWalked.Count != 0)
                ApplyMove();

            // set the coordinate of the unit from which you are switching to occupied
            hexGrid.OccupyCell(HexCoordinates.GetIndexOfCoordinate(GetCurrentCoordinate(), hexGrid.width), controlledUnit);

            // Now increment to next unit in list

            // Skip if dead
            do
            {
                currentUnitIndex++;
                if (currentUnitIndex > units.Count - 1) // loop through list
                    currentUnitIndex = 0;
            } while (units[currentUnitIndex].status == Status.Dead);


            if (currentUnitIndex > units.Count - 1) // loop through list
                currentUnitIndex = 0;

			ChangeControlledUnit (units[currentUnitIndex]);
      
            // Change all of the unit's layers to 8 before changing the current to 0
            foreach (Unit u in units)
            {
                u.gameObject.layer = 8;
            }
            controlledUnit.gameObject.layer = 0;

        }

        if(!isPlayerTurn)
        {
            // Apply latest path first if there is one
            if (currentPathWalked.Count != 0)
                ApplyMove();

            // set the coordinate of the unit from which you are switching to occupied
            hexGrid.OccupyCell(HexCoordinates.GetIndexOfCoordinate(GetCurrentCoordinate(), hexGrid.width), controlledUnit);

            // Now increment to next unit in list
            // Skip if dead
            do
            {
                currentEnemyIndex++;
                if (currentEnemyIndex > enemies.Count - 1) // loop through list
                    currentEnemyIndex = 0;
            } while (enemies[currentEnemyIndex].status == Status.Dead);


            if (currentEnemyIndex > enemies.Count - 1) // loop through list
                currentEnemyIndex = 0;

			ChangeControlledUnit (enemies [currentEnemyIndex]);

            // Change all of the unit's layers to 8 before changing the current to 0
            foreach (Unit e in enemies)
            {
                e.gameObject.layer = 8;
            }
            controlledUnit.gameObject.layer = 0;

            
        }
    }

    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn; // switch turns
        currentUnitIndex = 0;
        currentEnemyIndex = 0;

        if(isPlayerTurn)
        {
			ChangeControlledUnit(units[0]);

            foreach (Unit u in units)
            {
                u.currentAP = u.maxAP;
            }

        }
        if(!isPlayerTurn)
        {
			ChangeControlledUnit (enemies [0]);
            foreach (Enemy e in enemies)
            {
                e.currentAP = e.maxAP;
            }
        }
    }

	void Update () {
        HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);

        // Movement
        if (controllerOn){
            unitTransform.Translate(Input.GetAxis("Horizontal_Player") * speed, Input.GetAxis("Vertical_Player"), 0);
            
            //
        }

		// if character runs out of AP, disable controller
		if (controlledUnit.currentAP <= 0)
			controllerOn = false;

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
	/// Confirms the last path movement and consumes the appropriate amount of AP <-- this is handled in TouchCell() of HexGrid.cs
	/// according to the size of the currentPathWalked stack
	/// </summary>
	public void ApplyMove() {
		// clears current stack of moves
		pathDamage.Clear();
		currentPathAPCost.Clear ();
		currentPathWalked.Clear ();

		// set NEW initial coord
		initialCoord = GetCurrentCoordinate();

		HUDManagerScript.UpdateHPBar();
		HUDManagerScript.UpdateAPBar();

		// TO DO: clear the grid colors for walking (USE RESTORE GRID LATER)
		hexGrid.ResetGridColorToBlank();
	}

	public void UndoMove(){
		// return unit position to the initial coordinate position (cell's transform.position)
		unitTransform.position = new Vector3 (
			hexGrid.Cells[HexCoordinates.GetIndexOfCoordinate(initialCoord, hexGrid.width)].transform.position.x, 
			unitTransform.position.y, 
			hexGrid.Cells[HexCoordinates.GetIndexOfCoordinate(initialCoord, hexGrid.width)].transform.position.z
		);

		// set new initial coord after moving
		initialCoord = GetCurrentCoordinate();

		// restore lost HP and AP
		while (pathDamage.Count > 0) {
			controlledUnit.currentHP += pathDamage.Pop();
		}
		while (currentPathAPCost.Count > 0) {
			controlledUnit.currentAP += currentPathAPCost.Pop();
		}

		currentPathWalked.Clear ();

		HUDManagerScript.UpdateHPBar();
		HUDManagerScript.UpdateAPBar();

		// TO DO: clear the grid colors for walking (USE RESTORE GRID LATER)
		hexGrid.ResetGridColorToBlank();
	}

	/// <summary>
	/// Checkes if there are units adjacent to the currently controlled unit.
	/// ONLY APPLIES TO CHECKING FOR ENEMIES RIGHT NOW.
	/// </summary>
	/// <returns><c>true</c> units are adjacent <c>false</c> no adjacent units.</returns>
	public bool AreThereAdjacentUnits() {
		// store surrounding hexes in a list
		List<HexCoordinates> surroundingHexes = new List<HexCoordinates>();
		HexCoordinates current = GetCurrentCoordinate ();
		surroundingHexes.Add (new HexCoordinates(current.X + 1, current.Z));
		surroundingHexes.Add (new HexCoordinates(current.X, current.Z + 1));
		surroundingHexes.Add (new HexCoordinates(current.X - 1, current.Z));
		surroundingHexes.Add (new HexCoordinates(current.X, current.Z - 1));
		surroundingHexes.Add (new HexCoordinates(current.X - 1, current.Z + 1));
		surroundingHexes.Add (new HexCoordinates(current.X + 1, current.Z - 1));

		// if there are enemies, return true
		foreach (HexCoordinates h in surroundingHexes) {
			// if any adjacent cells are occupied, return true
			int index = HexCoordinates.GetIndexOfCoordinate(h, hexGrid.width);
			if (index >= 0) {	// needed to avoid index out of range errors for edge hexes
				if (hexGrid.Cells [index].IsOccupied() == true) {
					return true;
				}
			}

		}
		return false;
	}

	/// <summary>
	/// Helper function to get the current HexCoord
	/// </summary>
	/// <returns>The current location.</returns>
	HexCoordinates GetCurrentCoordinate() {
		return HexCoordinates.FromPosition(unitTransform.position);
	}

	/// <summary>
	/// Helper function to change the controlled Unit and assign all the necesary values.
	/// Call when cycling through units or switching turns, etc.
	/// Sets the controlledUnity, the unitTransform, and the initialCoord.
	/// </summary>
	/// <param name="changeTo">Unit to change the controlled unit to.</param>
	private void ChangeControlledUnit(Unit newUnit){
		controlledUnit = newUnit;
		unitTransform = controlledUnit.transform;
		initialCoord = GetCurrentCoordinate();

		// Update UI with Current Unit data
		HUDManagerScript.UpdateActiveUnitText(controlledUnit.Name);
		HUDManagerScript.UpdateHPBar();
		HUDManagerScript.UpdateAPBar();
		// CHANGE CORRESPONDING UNITS ACTIONS LIST HERE
	}

		
}
