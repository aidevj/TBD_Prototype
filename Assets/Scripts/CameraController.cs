//Aiden
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = 1.5f;

	void Start () {
		
	}

	void Update () {
		transform.Translate (Input.GetAxis ("Horizontal") * speed, Input.GetAxis ("Vertical"), 0);
	}
}
