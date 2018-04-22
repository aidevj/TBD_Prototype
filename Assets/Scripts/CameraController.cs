//Aiden
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = 1f;

	void Start () {
		
	}

	void Update () {
		transform.Translate (Input.GetAxis ("Horizontal") * speed, Input.GetAxis ("Vertical"), 0);
	}
}
