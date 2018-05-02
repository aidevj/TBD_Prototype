//Aiden
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = 1.5f;

	void Start () {
		
	}

	void Update () {
		transform.Translate (Input.GetAxis ("Horizontal_Camera") * speed, Input.GetAxis ("Vertical_Camera"), 0);
	}
}
