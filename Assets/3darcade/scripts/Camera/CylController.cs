using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Arcade
{
    public class CylController : MonoBehaviour
    {
        List<GameObject> objects = new List<GameObject>();
        List<ModelProperties> games = new List<ModelProperties>();

        public Camera thisCamera;
        public ArcadeType arcadeType = ArcadeType.CylArcade;
        public CylArcadeProperties cylArcadeProperties;

        [HideInInspector]
        public float timer = 0;

        // Jump to a new game position properties
        private bool jump;
        private bool jumpForwards = true;
        private int jumps;
        private float jumpsCount = 0;

        private int tempTargetModelGamePosition;
        private float cameraRotationDeltaDamping = 2f;
        private bool cameraOrientationVertical;
        public bool setupFinished;
        GameObject dummyCameraTransform;

        // Global Variables

        void Update()
        {
            if (!setupFinished) { return; }
            if (arcadeType == ArcadeType.CylArcade && !(ArcadeManager.arcadeState == ArcadeStates.Running && ArcadeManager.activeArcadeType == ArcadeType.CylArcade)) { return; }
            if (arcadeType == ArcadeType.CylMenu && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu && ArcadeManager.activeMenuType == ArcadeType.CylMenu)) { return; }
            thisCamera.transform.rotation = Quaternion.Lerp(thisCamera.transform.rotation, dummyCameraTransform.transform.rotation, timer);
            thisCamera.transform.position = Vector3.Lerp(thisCamera.transform.position, dummyCameraTransform.transform.position, timer);

            //print("cyl type " + arcadeType + " timer " + timer + " delta " + Time.deltaTime + " damp " + cameraRotationDeltaDamping);
            timer += (Time.deltaTime * (cameraRotationDeltaDamping));
            if (timer <= 1)
            {
                return;
            }
            if (jump)
            {
                if (jumpForwards)
                {
                    tempTargetModelGamePosition += 1;
                    if (tempTargetModelGamePosition > games.Count - 1) { tempTargetModelGamePosition = 0; }
                    Forwards(tempTargetModelGamePosition);
                }
                else
                {
                    tempTargetModelGamePosition = tempTargetModelGamePosition - 1;
                    if (tempTargetModelGamePosition < 0) { tempTargetModelGamePosition = games.Count - 1; }
                    Backwards(tempTargetModelGamePosition);
                }
                jumps += 1;
                if (jumps < jumpsCount) { return; }
                jump = false;
            }
            jumpsCount = 0;

            cylArcadeProperties.cameraTranslation += CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * cylArcadeProperties.cameraTranslationdamping;
            if (cylArcadeProperties.cameraTranslation > cylArcadeProperties.cameraMaxTranslation)
            {
                cylArcadeProperties.cameraTranslation = cylArcadeProperties.cameraMaxTranslation;
            }
            if (cylArcadeProperties.cameraTranslation < cylArcadeProperties.cameraMinTranslation)
            {
                cylArcadeProperties.cameraTranslation = cylArcadeProperties.cameraMinTranslation;
            }
            UpdateCameraDistance();


            float straffe = (CrossPlatformInputManager.GetAxis("Horizontal"));
            if (straffe > 0.0017)
            {
                cameraRotationDeltaDamping = 2 + (cylArcadeProperties.cameraRotationdamping * (straffe / 1f));
                cylArcadeProperties.gamePosition += 1;
                if (cylArcadeProperties.gamePosition > games.Count - 1) { cylArcadeProperties.gamePosition = 0; }
                Forwards(cylArcadeProperties.gamePosition);
            }
            if (straffe < -0.0017)
            {
                cameraRotationDeltaDamping = 2 + (cylArcadeProperties.cameraRotationdamping * (straffe / -1f));
                cylArcadeProperties.gamePosition -= 1;
                if (cylArcadeProperties.gamePosition < 0) { cylArcadeProperties.gamePosition = games.Count - 1; }
                Backwards(cylArcadeProperties.gamePosition);
            }

            //if (Input.GetKey("space"))
            //{
            //    var random = new System.Random();
            //    GoToGamePosition(random.Next(0, games.Count - 1));

            //}
        }

        void Forwards(int targetGamePosition)
        {
            targetGamePosition += cylArcadeProperties.sprockets - cylArcadeProperties.selectedSprocket - 1;
            if (targetGamePosition > games.Count - 1)
            {
                targetGamePosition -= games.Count;
            }
            if (objects.Count == cylArcadeProperties.sprockets)
            {
                if (Application.isPlaying)
                {
                    Destroy(objects[0]);
                }
                else
                {
                    DestroyImmediate(objects[0]);
                }
            }
            GameObject model = AddModel(targetGamePosition);
            if (objects.Count == cylArcadeProperties.sprockets) { objects.RemoveAt(0); }
            objects.Add(model);
            UpdateModelTransform(objects.Count - 2, model, true);
            if (objects.Count == cylArcadeProperties.sprockets) { UpdateCamera(); }
        }
        void Backwards(int targetGamePosition)
        {
            targetGamePosition -= cylArcadeProperties.selectedSprocket;
            if (targetGamePosition < 0)
            {
                targetGamePosition += games.Count;
            }
            if (Application.isPlaying)
            {
                Destroy(objects[objects.Count - 1]);
            }
            else
            {
                DestroyImmediate(objects[objects.Count - 1]);
            }
            GameObject model = AddModel(targetGamePosition); ;
            objects.RemoveAt(objects.Count - 1);
            objects.Insert(0, model);
            UpdateModelTransform(1, model, false);
            UpdateCamera();
        }

        void GoToGamePosition(int goToGamePosition)
        {
            //print("goto " + goToGamePosition);
            jumpsCount = cylArcadeProperties.sprockets;
            jumpForwards = true;
            int cw; //
            if (cylArcadeProperties.gamePosition - goToGamePosition > 0)
            {
                cw = cylArcadeProperties.gamePosition - goToGamePosition;
            }
            else
            {
                cw = games.Count - goToGamePosition + cylArcadeProperties.gamePosition;
            }
            int ccw;
            if (goToGamePosition - cylArcadeProperties.gamePosition > 0)
            {
                ccw = goToGamePosition - cylArcadeProperties.gamePosition;
            }
            else
            {
                ccw = games.Count - cylArcadeProperties.gamePosition + goToGamePosition;
            }
            int tjump;
            if (cw < ccw)
            {
                tjump = cw;
                jumpForwards = false;
                jumpsCount = cw;

                if (tjump <= cylArcadeProperties.sprockets)
                {
                    tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
                    cylArcadeProperties.gamePosition = goToGamePosition;
                }
                else
                {
                    jumpsCount = cylArcadeProperties.sprockets;
                    tempTargetModelGamePosition = goToGamePosition + cylArcadeProperties.sprockets;
                    cylArcadeProperties.gamePosition = goToGamePosition;
                    if (tempTargetModelGamePosition > games.Count - 1)
                    {
                        tempTargetModelGamePosition = tempTargetModelGamePosition - games.Count;
                    }
                }
            }
            else
            {
                tjump = ccw;
                jumpForwards = true; // forwards in list is ccw
                jumpsCount = ccw;
                if (tjump <= cylArcadeProperties.sprockets)
                {
                    tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
                    cylArcadeProperties.gamePosition = goToGamePosition;
                }
                else
                {
                    jumpsCount = cylArcadeProperties.sprockets;
                    tempTargetModelGamePosition = goToGamePosition - cylArcadeProperties.sprockets;
                    cylArcadeProperties.gamePosition = goToGamePosition;
                    if (tempTargetModelGamePosition < 0)
                    {
                        tempTargetModelGamePosition = games.Count + tempTargetModelGamePosition;
                    }
                }
            }
            jump = true;
            jumps = 0;
            cameraRotationDeltaDamping = 2 + cylArcadeProperties.cameraRotationdamping;
        }

        void UpdateModelTransform(int index, GameObject model, bool forwards)
        {
            model.transform.position = objects[index].transform.position;
            model.transform.rotation = objects[index].transform.rotation;
            var tOrig = objects[index];
            var tOrigWidth = GetModelWidth(tOrig);
            var modelWidth = GetModelWidth(model);
            float width = ((tOrigWidth / 2) + (modelWidth / 2) + cylArcadeProperties.modelSpacing) / cylArcadeProperties.radius;
            model.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, (width * (forwards == true ? 1 : -1)) * 57.29577951f);
        }

        public void UpdateCamera()
        {
            if (!setupFinished) { return; } 
            var direction = objects[cylArcadeProperties.selectedSprocket].transform.position - Vector3.zero;
            direction.y = 0;
            dummyCameraTransform.transform.position = objects[cylArcadeProperties.selectedSprocket].transform.position;
            dummyCameraTransform.transform.Translate(direction * -0.99f, Space.World);
            dummyCameraTransform.transform.LookAt(Vector3.zero);
            dummyCameraTransform.transform.Rotate(cylArcadeProperties.cameraLocalEularAngleX, cylArcadeProperties.cameraLocalEularAngleY, cylArcadeProperties.cameraLocalEularAngleZ, Space.Self);
            if (cylArcadeProperties.cameraTranslationDirectionAxisY) { direction = Vector3.up; }
            dummyCameraTransform.transform.Translate(direction * cylArcadeProperties.cameraTranslation * (cylArcadeProperties.cameraTranslationReverse == true ? -1 : 1), Space.World);
            dummyCameraTransform.transform.Translate(cylArcadeProperties.cameraLocalTranslation, Space.Self);
            timer = 0;
        }

        void UpdateCameraDistance()
        {
            if (!setupFinished) { return; }
            var direction = objects[cylArcadeProperties.selectedSprocket].transform.position - Vector3.zero;
            direction.y = 0;
            thisCamera.transform.position = objects[cylArcadeProperties.selectedSprocket].transform.position;
            thisCamera.transform.Translate(direction * -0.99f, Space.World);
            if (cylArcadeProperties.cameraTranslationDirectionAxisY) { direction = Vector3.up; }
            thisCamera.transform.Translate(direction * cylArcadeProperties.cameraTranslation * (cylArcadeProperties.cameraTranslationReverse == true ? -1 : 1), Space.World);
            thisCamera.transform.Translate(cylArcadeProperties.cameraLocalTranslation, Space.Self);
            timer = 1;
        }

        GameObject AddModel(int position, bool addTrigger = true)
        {
            GameObject model = Arcade.ArcadeManager.loadSaveArcadeConfiguration.AddModelToArcade(Arcade.ModelType.Game, games[position], arcadeType, addTrigger);
            var rigid = model.transform.GetChild(0).GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.isKinematic = cylArcadeProperties.modelIsKinemtatic;
            }
            return model;
        }

        float GetModelWidth(GameObject obj)
        {
            Transform t = obj.transform;
            var col = t.GetChild(0).GetComponent<BoxCollider>();
            Vector3 size;
            if (col != null)
            {
                size = col.size;
            }
            else
            {
                Vector3 savedPosition = t.position;
                Vector3 savedLocalPosition = t.localPosition;
                Quaternion savedRotation = t.rotation;
                Quaternion savedLocalRotation = t.localRotation;
                t.position = Vector3.zero;
                t.localPosition = Vector3.zero;
                t.rotation = Quaternion.identity;
                t.localRotation = Quaternion.identity;
                Renderer[] rr = t.GetChild(0).GetComponentsInChildren<Renderer>();
                Bounds b = rr[0].bounds;
                foreach (Renderer r in rr) { b.Encapsulate(r.bounds); }
                t.position = savedPosition;
                t.localPosition = savedLocalPosition;
                t.rotation = savedRotation;
                t.localRotation = savedLocalRotation;
                //print("boundsREND x " + b.size.x + " y " + b.size.y + " z " + b.size.z);
                size = b.size;
            }
            return cylArcadeProperties.modelWidthAxisY ? size.y : size.x;
        }

        public bool SetupCylArcade(ArcadeConfiguration arcadeConfiguration)
        {
            if (arcadeConfiguration.cylArcadeProperties.Count < 1) { return false; }
            games = arcadeConfiguration.gameModelList;
            objects = new List<GameObject>();
            cylArcadeProperties = arcadeConfiguration.cylArcadeProperties[0];

            if (games.Count < cylArcadeProperties.sprockets)
            {
                if (cylArcadeProperties.selectedSprocket == 0)
                {

                }
                else if (cylArcadeProperties.selectedSprocket == cylArcadeProperties.sprockets - 1)
                {
                    cylArcadeProperties.selectedSprocket = games.Count - 1;
                }
                else
                {
                    cylArcadeProperties.selectedSprocket = (((games.Count % 2 == 0) ? games.Count : (games.Count - 1)) / 2)-1;
                }
                cylArcadeProperties.sprockets = games.Count - 1;
            }

            thisCamera.ResetAspect();
            if (arcadeType == ArcadeType.CylMenu && cylArcadeProperties.cameraAspectRatio != 0)
            {
                //print("aspect " + cylArcadeProperties.cameraAspectRatio);
                thisCamera.aspect = cylArcadeProperties.cameraAspectRatio;
            }

            //print("gamesc " + games.Count + " sp " + cylArcadeProperties.sprockets + " ssp " + cylArcadeProperties.selectedSprocket + " gp " + cylArcadeProperties.gamePosition);
           
            // Dummy transform to smoothly interpolate our camera towards.
            Transform t = transform.Find("dummyCameraTransform");
            if (t == null)
            {
                dummyCameraTransform = new GameObject();
                dummyCameraTransform.name = "dummyCameraTransform";
                dummyCameraTransform.transform.parent = gameObject.transform;
            }
            else
            {
                dummyCameraTransform = t.gameObject;
            }

            cameraRotationDeltaDamping = cylArcadeProperties.cameraRotationdamping; // Reset damping

            tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
            cylArcadeProperties.gamePosition -= cylArcadeProperties.sprockets;
            if (cylArcadeProperties.gamePosition < 0)
            {
                cylArcadeProperties.gamePosition += games.Count;
            }
            // Setup first model
            GameObject model = AddModel(cylArcadeProperties.gamePosition);
            objects.Add(model);
            model.transform.position = new Vector3(0, 0, 0);
            model.transform.rotation = Quaternion.identity;
            model.transform.Translate(new Vector3(0, 0, cylArcadeProperties.radius), Space.World);
            // Local rotation of the sprocket
            model.transform.Rotate(cylArcadeProperties.sprocketLocalEularAngleX, cylArcadeProperties.sprocketLocalEularAngleY, cylArcadeProperties.sprocketLocalEularAngleZ);
            for (int i = 0; i < cylArcadeProperties.sprockets; i++)
            {
                cylArcadeProperties.gamePosition += 1;
                if (cylArcadeProperties.gamePosition > games.Count - 1) { cylArcadeProperties.gamePosition = 0; }
                Forwards(cylArcadeProperties.gamePosition);
            }
            cylArcadeProperties.gamePosition = tempTargetModelGamePosition;
            // Don't animate the camera, so set timer to 1;
            timer = 1;
            setupFinished = true;
            UpdateCamera();
            return true;
        }

        public GameObject GetSelectedGame()
        {
            GameObject obj = null;
            try
            {
                obj = objects[cylArcadeProperties.selectedSprocket];
            }
            catch (Exception e)
            {

            }
            return obj != null ? objects[cylArcadeProperties.selectedSprocket].transform.GetChild(0).gameObject : null;
        }
    }
}