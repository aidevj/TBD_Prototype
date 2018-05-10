using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			LoadLevel (0);
		}
	}

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
