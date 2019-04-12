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
    public Canvas ActionsHUD;               // Canvas for the player's actions HUD in the world space should be a child of the camera
    private CanvasGroup ActionsHUDGroup;
    private float fadeSpeed = 1.5f;

    void Start()
    {
        ActionsHUDGroup = ActionsHUD.GetComponent<CanvasGroup>();
    }

    void Update() {

        // Check for automatic changes of camera type
            // If FREE ROAM CAM CONTROLS (L1+LS/Arrow Keys) are pressed, change to freeRoam mode
            // TO DO: event handler for input
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
                CheckCurrentUnit(); // MAKE SURE THIS IS ALWAYS CALLED WHEN CHANGING TO PLAYER FOLLOW 

                // fade out Player Actions HUD
                if (ActionsHUDGroup.alpha < 1f)
                    ActionsHUDGroup.alpha += Time.deltaTime * fadeSpeed;

                // allow targetting with mouse only within one hex of the current unit
                // get raycast of mouse
          
                // TO DO: Fade out all enemy stat HUD when entering this mode, if they were up


                // Define a target position above and behind
                //Vector3 targetPosition = target.TransformPoint(offset);

                // Smoothly move the camera towards the target position
                // transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothTime);

                // Rotate camera with player    //  NO
                //transform.LookAt(target);

                break;
            case CamMode.FreeRoam:
                transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed, Space.World);

                // fade out Player Actions HUD
                if (ActionsHUDGroup.alpha > 0f)
                    ActionsHUDGroup.alpha -= Time.deltaTime * fadeSpeed;

                // TO DO: By default, show all the enemy stat info in this mode, fade in

                // TO DO: disable all functionality of action menu when in this mode
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
