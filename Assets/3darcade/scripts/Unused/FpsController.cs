using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Arcade;

public class FpsController : MonoBehaviour {

    public float speed = 10.0f;
    public ArcadeType arcadeType = ArcadeType.FpsArcade;
    private float translation;
    private float straffe;

    // No longer used

    void Update()
    {
        if (arcadeType == ArcadeType.FpsArcade && ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu) { return; }
        if (arcadeType == ArcadeType.FpsMenu && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu)) { return; }
        // Update while in running or movacabs state, cursor is locked and the activeArcadeType is not FpsArcade
        if (!(ArcadeManager.arcadeState == ArcadeStates.Running || ArcadeManager.arcadeState == ArcadeStates.MoveCabs) && Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }
        if (!(ArcadeManager.activeArcadeType == arcadeType)) { return; }
        print("fps " + ArcadeManager.activeArcadeType);
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        translation = translation + (CrossPlatformInputManager.GetAxis("Vertical") * speed * Time.deltaTime);
        straffe = straffe + (CrossPlatformInputManager.GetAxis("Horizontal") * speed * Time.deltaTime);
        transform.Translate(straffe, 0, translation);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
