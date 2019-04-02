//Aiden
using UnityEngine;

public enum CamMode {
    PlayerFollow,
    FreeRoam
}

public class CameraController : MonoBehaviour {

	public float speed = 1.5f;
    public CamMode currentCamMode = CamMode.PlayerFollow;

    // Smooth follow cam attributes
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;
    public UnitController unitController;

    // Player HUD reference
    private GameObject ActionsHUD;

    void Start()
    {
        ActionsHUD = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update() {

        // Check for automatic changes of camera type
        //// if FREE ROAM CAM CONTROLS (L1+LS/Arrow Keys) are pressed, change to freeRoam mode
            // to do: event handler for input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            currentCamMode = CamMode.PlayerFollow;
        }

        //// if PLAYER MOVEMENT CONTROLS (LS/WASD) are pressed, change to PlayerFollow mode
        // to do: event handler for input
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentCamMode = CamMode.FreeRoam;
        }

        switch (currentCamMode)
        {
            case CamMode.PlayerFollow:
                // as soon as returning to this mode, smooth snap back to the target
                CheckCurrentUnit();

                // fade out Player Actions HUD
                //ActionsHUD.FadeOutAnim();
                ActionsHUD.SetActive(true);

                // Define a target position above and behind (LATER TO DO: use offset)
                Vector3 targetPosition = target.TransformPoint(offset);

                // Smoothly move the camera towards the target position
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

                break;
            case CamMode.FreeRoam:
                transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed, Space.World);

                // fade out Player Actions HUD
                //ActionsHUD.FadeOutAnim();
                ActionsHUD.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Checks the current unit being controlled and applies as current target for follow-cam
    /// </summary>
    private void CheckCurrentUnit()
    {
        target = unitController.controlledUnit.gameObject.transform;
    }
}
