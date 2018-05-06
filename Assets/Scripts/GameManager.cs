using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
	PlayerTurn,
	EnemyTurn,
	Paused
}

public class GameManager : MonoBehaviour {

	// Refernces to other manager scripts, drag in inspector
	public GameObject ControllerManagerObject;
	public GameObject UICanvasObject;
	private UnitController UnitControllerManagerScript;
	private HUDManager HUDManagerScript;

	// Gameplay properties
	public GameState currentGameState;
	public GameState lastGameState;


	void Start() {
		UnitControllerManagerScript = ControllerManagerObject.GetComponent<UnitController> ();
		HUDManagerScript = UICanvasObject.GetComponent<HUDManager> ();
	}


	// Menu loading methods

	/// <summary>
	/// Opens the terrain editor menu and pauses the gameplay.
	/// </summary>
	public void OpenTerrainEditor() {
		SceneManager.LoadScene (3, LoadSceneMode.Additive);
		UnitControllerManagerScript.DisableController ();
		lastGameState = currentGameState;
		currentGameState = GameState.Paused;

	}

	public void CloseTerrainEditor() {
		//SceneManager.UnloadSceneAsync (3);
		//UnitControllerManagerScript.EnableController ();
		//currentGameState = lastGameState;

	}


}
