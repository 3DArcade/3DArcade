using UnityEngine;

namespace Arcade
{
    public class MouseCamLook : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 5.0f;
        [SerializeField] private float smoothing   = 2.0f;
        // The Arcade Type this instance is used with, TODO: auto find this?
        [SerializeField] private ArcadeType arcadeType = default;
        // The character is the capsule
        [SerializeField] private GameObject arcadeControl = default;

        // Get the incremental value of mouse moving
        [HideInInspector]
        public Vector2 mouseLook;
        // Smooth the mouse moving
        private Vector2 smoothV;
        private Vector2 mouseDelta;

#if MOBILE_INPUT
        private int fingerSaved = -1;
#endif
        // Global Variables:
        // ArcadeManager.arcadeControls
        // ArcadeManager.arcadeState
        // ArcadeManager.activeArcadeType
        // ArcadeManager.activeMenuType

        //void Start()
        //{
        //    //arcadeControl = this.transform.parent.gameObject;
        //}

        void Update()
        {
            if ((arcadeControl == ArcadeManager.arcadeControls[ArcadeType.FpsArcade] || arcadeControl == ArcadeManager.arcadeControls[ArcadeType.CylArcade]) && ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu)
            {
                return;
            }

            if ((arcadeControl == ArcadeManager.arcadeControls[ArcadeType.FpsMenu] || arcadeControl == ArcadeManager.arcadeControls[ArcadeType.CylMenu]) && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                return;
            }

            if (!(ArcadeManager.arcadeState == ArcadeStates.Running || ArcadeManager.arcadeState == ArcadeStates.Game || ArcadeManager.arcadeState == ArcadeStates.MoveCabs || ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                return;
            }
            // If fps is active return
            if (ArcadeManager.activeArcadeType != arcadeType && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                return;
            }

            if (ArcadeManager.activeMenuType != arcadeType && (ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                return;
            }

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                return;
            }

#if MOBILE_INPUT
        // Check if there is a touch
        int touchesCount = Input.touchCount;
        //Debug.Log("tocuCount:" + touchesCount);
        bool updateMouse = true;
        Touch updateTouch;
        for (int i = 0; i < touchesCount; i++)
        {

            Touch touch = Input.GetTouch(i);
            updateTouch = touch;
            //  Debug.Log("touchnr: " + i + "on ui: " + EventSystem.current.IsPointerOverGameObject(touch.fingerId) + " with id: " + touch.fingerId);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
            {
                continue;
            }
             //Debug.Log("touchnr: " + i + "on ui: " + EventSystem.current.IsPointerOverGameObject(touch.fingerId) + " with id: " + touch.fingerId);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                updateMouse = false;
                continue;

                //Debug.Log("Ignore touches on ui, ie virtual joystick " + touch.fingerId);
                fingerSaved = touch.fingerId;
                // return;
                //Debug.Log("touchnr: " + i + "on ui: " + EventSystem.current.IsPointerOverGameObject(touch.fingerId) + " with id: " + touch.fingerId);
            }
            else
            {
                updateMouse = true;
            }
            //if (touch.position.x < Screen.width/2)
            //{
            //    return;
            //}
            //Debug.Log("finger: " + touch.fingerId + " count: " + touchesCount);
            //return;
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                float rotationZ = delta.x * 5.0f * Time.deltaTime;
                //rotationZ = invertX ? rotationZ : rotationZ * -1;
                float rotationX = delta.y * 5.0f * Time.deltaTime;
                //rotationX = invertY ? rotationX : rotationX * -1;

                transform.localEulerAngles += new Vector3(-1 * rotationX, 0, 0);
                character.transform.localEulerAngles += new Vector3(0, rotationZ, 0);
            }

            //mouseDelta = new Vector2((float)(touch.deltaPosition.x * 0.04), (float)(touch.deltaPosition.y * 0.04));
            //Debug.Log("finger: " + touch.fingerId + " count: " + touchesCount + " delta: " + mouseDelta);
            //mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
        if (!updateMouse)
        {
            return;
        }
        return;



#else
            mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
#endif
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            // The interpolated float result between the two float values
            //print("mouse hier x " + mouseDelta.x + " y " + mouseDelta.y);
            if ((ArcadeManager.activeArcadeType == ArcadeType.CylArcade && ArcadeManager.arcadeState != ArcadeStates.ArcadeMenu) || (arcadeType == ArcadeType.CylMenu && ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                if (mouseDelta.y == 0 && mouseDelta.x == 0)
                {
                    return;
                }

                CylController c = arcadeControl.GetComponent<CylController>();
                if (c.cylArcadeProperties.cameraLocalEularAngleRotation)
                {
                    smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x * (c.cylArcadeProperties.mouseLookReverse ? -1f : 1f), 1f / smoothing);
                    smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y * (c.cylArcadeProperties.mouseLookReverse ? -1f : 1f), 1f / smoothing);
                    // Incrementally add to the camera look
                    mouseLook.x = c.cylArcadeProperties.cameraLocalEularAngleX;
                    mouseLook.y = c.cylArcadeProperties.cameraLocalEularAngleX;
                    mouseLook += smoothV;
                    if (mouseLook.y > c.cylArcadeProperties.cameraLocalMaxEularAngleRotation)
                    { mouseLook.y = c.cylArcadeProperties.cameraLocalMaxEularAngleRotation; }
                    if (mouseLook.y < c.cylArcadeProperties.cameraLocalMinEularAngleRotation)
                    { mouseLook.y = c.cylArcadeProperties.cameraLocalMinEularAngleRotation; }
                    if (mouseLook.x > c.cylArcadeProperties.cameraLocalMaxEularAngleRotation)
                    { mouseLook.x = c.cylArcadeProperties.cameraLocalMaxEularAngleRotation; }
                    if (mouseLook.x < c.cylArcadeProperties.cameraLocalMinEularAngleRotation)
                    { mouseLook.x = c.cylArcadeProperties.cameraLocalMinEularAngleRotation; }

                    c.cylArcadeProperties.cameraLocalEularAngleX = (c.cylArcadeProperties.mouseLookAxisY ? mouseLook.y : mouseLook.x);
                    c.UpdateCamera();
                    c.timer = 1;
                }
            }
            else
            {
                smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
                smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
                // Incrementally add to the camera look
                mouseLook += smoothV;
                if (mouseLook.y < -70)
                { mouseLook.y = -70f; }
                if (mouseLook.y > 70)
                { mouseLook.y = 70f; }
                transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                arcadeControl.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, arcadeControl.transform.up);
            }
        }
    }
}
