using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
	PlayerTurn,
	EnemyTurn,
	Paused
}

/// <summary>
/// Handles and oversees processes of the Grid Scene and the battle gameplay that takes place within.
/// Handles opening and closing menus in the screen such as the Terrain Editor, and sets the Game State accordingly.
/// </summary>
public class GameManager : MonoBehaviour {

	// Refernces to other manager scripts (drag in inspector)
	public UnitController UnitControllerManagerScript;
	public HUDManager HUDManagerScript;
	public TerrainEditor TerrainEditorScript;

	// Gameplay properties
	public GameState currentGameState;
	public GameState lastGameState;

	public bool terrainEditorOpen;


	void Start() {
		// Terrain Editor should be inactive from the start
		TerrainEditorScript.gameObject.SetActive (false);
		terrainEditorOpen = false;

		// ignore collisions between layer 0 and 8
		Physics.IgnoreLayerCollision(0,8);
	}

	void Update() {
		// toggle terrain editor menu
		if (Input.GetKeyDown (KeyCode.T)) {
			
			if (terrainEditorOpen == true) {
				CloseTerrainEditor ();
			}
			else {
				OpenTerrainEditor ();
			}
		}
	}


	// Menu loading methods

	/// <summary>
	/// Opens the terrain editor menu and pauses the gameplay.
	/// </summary>
	public void OpenTerrainEditor() {
		TerrainEditorScript.gameObject.SetActive (true);
		terrainEditorOpen = true;

		UnitControllerManagerScript.ControllerOn = false;
		lastGameState = currentGameState;
		currentGameState = GameState.Paused;

	}

	public void CloseTerrainEditor() {
		TerrainEditorScript.gameObject.SetActive (false);
		terrainEditorOpen = false;

		UnitControllerManagerScript.ControllerOn = true;
		currentGameState = lastGameState;

	}


}
