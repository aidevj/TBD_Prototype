// Aiden
using UnityEngine;
using UnityEngine.Collections;
using System.Collections.Generic;

public enum Status { Alive, Dead };

/// <summary>
/// Base Class for any Unit (ally) in the Game
/// </summary>
public class Unit : MonoBehaviour{

	public HexCoordinates currentCoord;

	// Status Attributes
    public string name;

    public int currentHP;
    public int maxHP;

    public int currentAP;
    public int maxAP;

	public Status status;

    // Cost to move per normal grid hex
    public int moveCost;

    public string weaponType;
    public string weaponAff;

    // Base stats
    public int attack;
    public int defense;
    public int dexterity;
    public int endurance;
    public int intelligence;

	// List of available moves
	public Ability[] movelist;


	// Properties
	#region
	public string Name {
		get { return name; }
	}

	#endregion

	void Start() {
		currentCoord = HexCoordinates.FromPosition (transform.position);

	}

}
