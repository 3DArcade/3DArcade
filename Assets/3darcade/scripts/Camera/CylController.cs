using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Arcade
{
    public class CylController : MonoBehaviour
    {
        private List<GameObject> _objects = new List<GameObject>();
        private List<ModelProperties> _games = new List<ModelProperties>();

        public Camera thisCamera;
        public ArcadeType arcadeType = ArcadeType.CylArcade;
        public CylArcadeProperties cylArcadeProperties;

        [HideInInspector]
        public float timer = 0;

        // Jump to a new game position properties
        private bool _jump;
        private readonly bool _jumpForwards = true;
        private int _jumps;
        private float _jumpsCount = 0;

        private int _tempTargetModelGamePosition;
        private float _cameraRotationDeltaDamping = 2f;
        //private bool cameraOrientationVertical;
        public bool setupFinished;
        GameObject _dummyCameraTransform;

        // Global Variables

        void Update()
        {
            if (!setupFinished)
            {
                return;
            }

            if (arcadeType == ArcadeType.CylArcade && !(ArcadeManager.arcadeState == ArcadeStates.Running && ArcadeManager.activeArcadeType == ArcadeType.CylArcade))
            {
                return;
            }

            if (arcadeType == ArcadeType.CylMenu && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu && ArcadeManager.activeMenuType == ArcadeType.CylMenu))
            {
                return;
            }

            thisCamera.transform.rotation = Quaternion.Lerp(thisCamera.transform.rotation, _dummyCameraTransform.transform.rotation, timer);
            thisCamera.transform.position = Vector3.Lerp(thisCamera.transform.position, _dummyCameraTransform.transform.position, timer);

            //print("cyl type " + arcadeType + " timer " + timer + " delta " + Time.deltaTime + " damp " + cameraRotationDeltaDamping);
            timer += (Time.deltaTime * (_cameraRotationDeltaDamping));
            if (timer <= 1)
            {
                return;
            }
            if (_jump)
            {
                if (_jumpForwards)
                {
                    _tempTargetModelGamePosition += 1;
                    if (_tempTargetModelGamePosition > _games.Count - 1)
                    {
                        _tempTargetModelGamePosition = 0;
                    }
                    Forwards(_tempTargetModelGamePosition);
                }
                else
                {
                    _tempTargetModelGamePosition -= 1;
                    if (_tempTargetModelGamePosition < 0)
                    {
                        _tempTargetModelGamePosition = _games.Count - 1;
                    }
                    Backwards(_tempTargetModelGamePosition);
                }
                _jumps += 1;
                if (_jumps < _jumpsCount)
                {
                    return;
                }

                _jump = false;
            }
            _jumpsCount = 0;

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
                _cameraRotationDeltaDamping = 2 + (cylArcadeProperties.cameraRotationdamping * (straffe / 1f));
                cylArcadeProperties.gamePosition += 1;
                if (cylArcadeProperties.gamePosition > _games.Count - 1)
                {
                    cylArcadeProperties.gamePosition = 0;
                }
                Forwards(cylArcadeProperties.gamePosition);
            }
            if (straffe < -0.0017)
            {
                _cameraRotationDeltaDamping = 2 + (cylArcadeProperties.cameraRotationdamping * (straffe / -1f));
                cylArcadeProperties.gamePosition -= 1;
                if (cylArcadeProperties.gamePosition < 0)
                {
                    cylArcadeProperties.gamePosition = _games.Count - 1;
                }
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
            if (targetGamePosition > _games.Count - 1)
            {
                targetGamePosition -= _games.Count;
            }
            if (_objects.Count == cylArcadeProperties.sprockets)
            {
                if (Application.isPlaying)
                {
                    Destroy(_objects[0]);
                }
                else
                {
                    DestroyImmediate(_objects[0]);
                }
            }
            GameObject model = AddModel(targetGamePosition);
            if (_objects.Count == cylArcadeProperties.sprockets)
            {
                _objects.RemoveAt(0);
            }
            _objects.Add(model);
            UpdateModelTransform(_objects.Count - 2, model, true);
            if (_objects.Count == cylArcadeProperties.sprockets)
            {
                UpdateCamera();
            }
        }

        void Backwards(int targetGamePosition)
        {
            targetGamePosition -= cylArcadeProperties.selectedSprocket;
            if (targetGamePosition < 0)
            {
                targetGamePosition += _games.Count;
            }
            if (Application.isPlaying)
            {
                Destroy(_objects[_objects.Count - 1]);
            }
            else
            {
                DestroyImmediate(_objects[_objects.Count - 1]);
            }
            GameObject model = AddModel(targetGamePosition);
            _objects.RemoveAt(_objects.Count - 1);
            _objects.Insert(0, model);
            UpdateModelTransform(1, model, false);
            UpdateCamera();
        }

        //private void GoToGamePosition(int goToGamePosition)
        //{
        //    //print("goto " + goToGamePosition);
        //    jumpsCount = cylArcadeProperties.sprockets;
        //    jumpForwards = true;
        //    int cw; //
        //    if (cylArcadeProperties.gamePosition - goToGamePosition > 0)
        //    {
        //        cw = cylArcadeProperties.gamePosition - goToGamePosition;
        //    }
        //    else
        //    {
        //        cw = games.Count - goToGamePosition + cylArcadeProperties.gamePosition;
        //    }
        //    int ccw;
        //    if (goToGamePosition - cylArcadeProperties.gamePosition > 0)
        //    {
        //        ccw = goToGamePosition - cylArcadeProperties.gamePosition;
        //    }
        //    else
        //    {
        //        ccw = games.Count - cylArcadeProperties.gamePosition + goToGamePosition;
        //    }
        //    int tjump;
        //    if (cw < ccw)
        //    {
        //        tjump = cw;
        //        jumpForwards = false;
        //        jumpsCount = cw;

        //        if (tjump <= cylArcadeProperties.sprockets)
        //        {
        //            tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
        //            cylArcadeProperties.gamePosition = goToGamePosition;
        //        }
        //        else
        //        {
        //            jumpsCount = cylArcadeProperties.sprockets;
        //            tempTargetModelGamePosition = goToGamePosition + cylArcadeProperties.sprockets;
        //            cylArcadeProperties.gamePosition = goToGamePosition;
        //            if (tempTargetModelGamePosition > games.Count - 1)
        //            {
        //                tempTargetModelGamePosition -= games.Count;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        tjump = ccw;
        //        jumpForwards = true; // forwards in list is ccw
        //        jumpsCount = ccw;
        //        if (tjump <= cylArcadeProperties.sprockets)
        //        {
        //            tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
        //            cylArcadeProperties.gamePosition = goToGamePosition;
        //        }
        //        else
        //        {
        //            jumpsCount = cylArcadeProperties.sprockets;
        //            tempTargetModelGamePosition = goToGamePosition - cylArcadeProperties.sprockets;
        //            cylArcadeProperties.gamePosition = goToGamePosition;
        //            if (tempTargetModelGamePosition < 0)
        //            {
        //                tempTargetModelGamePosition = games.Count + tempTargetModelGamePosition;
        //            }
        //        }
        //    }
        //    jump = true;
        //    jumps = 0;
        //    cameraRotationDeltaDamping = 2 + cylArcadeProperties.cameraRotationdamping;
        //}

        void UpdateModelTransform(int index, GameObject model, bool forwards)
        {
            model.transform.position = _objects[index].transform.position;
            model.transform.rotation = _objects[index].transform.rotation;
            GameObject tOrig = _objects[index];
            float tOrigWidth = GetModelWidth(tOrig);
            float modelWidth = GetModelWidth(model);
            float width = ((tOrigWidth / 2) + (modelWidth / 2) + cylArcadeProperties.modelSpacing) / cylArcadeProperties.radius;
            model.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, (width * (forwards == true ? 1 : -1)) * 57.29577951f);
        }

        public void UpdateCamera()
        {
            if (!setupFinished)
            {
                return;
            }

            Vector3 direction = _objects[cylArcadeProperties.selectedSprocket].transform.position - Vector3.zero;
            direction.y = 0;
            _dummyCameraTransform.transform.position = _objects[cylArcadeProperties.selectedSprocket].transform.position;
            _dummyCameraTransform.transform.Translate(direction * -0.99f, Space.World);
            _dummyCameraTransform.transform.LookAt(Vector3.zero);
            _dummyCameraTransform.transform.Rotate(cylArcadeProperties.cameraLocalEularAngleX, cylArcadeProperties.cameraLocalEularAngleY, cylArcadeProperties.cameraLocalEularAngleZ, Space.Self);
            if (cylArcadeProperties.cameraTranslationDirectionAxisY)
            {
                direction = Vector3.up;
            }
            _dummyCameraTransform.transform.Translate(direction * cylArcadeProperties.cameraTranslation * (cylArcadeProperties.cameraTranslationReverse == true ? -1 : 1), Space.World);
            _dummyCameraTransform.transform.Translate(cylArcadeProperties.cameraLocalTranslation, Space.Self);
            timer = 0;
        }

        void UpdateCameraDistance()
        {
            if (!setupFinished)
            {
                return;
            }

            Vector3 direction = _objects[cylArcadeProperties.selectedSprocket].transform.position - Vector3.zero;
            direction.y = 0;
            thisCamera.transform.position = _objects[cylArcadeProperties.selectedSprocket].transform.position;
            thisCamera.transform.Translate(direction * -0.99f, Space.World);
            if (cylArcadeProperties.cameraTranslationDirectionAxisY)
            {
                direction = Vector3.up;
            }
            thisCamera.transform.Translate(direction * cylArcadeProperties.cameraTranslation * (cylArcadeProperties.cameraTranslationReverse == true ? -1 : 1), Space.World);
            thisCamera.transform.Translate(cylArcadeProperties.cameraLocalTranslation, Space.Self);
            timer = 1;
        }

        GameObject AddModel(int position, bool addTrigger = true)
        {
            GameObject model = Arcade.ArcadeManager.loadSaveArcadeConfiguration.AddModelToArcade(Arcade.ModelType.Game, _games[position], arcadeType, addTrigger);
            Rigidbody rigid = model.transform.GetChild(0).GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.isKinematic = cylArcadeProperties.modelIsKinemtatic;
            }
            return model;
        }

        float GetModelWidth(GameObject obj)
        {
            Transform t = obj.transform;
            BoxCollider col = t.GetChild(0).GetComponent<BoxCollider>();
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
                foreach (Renderer r in rr)
                {
                    b.Encapsulate(r.bounds);
                }
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
            if (arcadeConfiguration.cylArcadeProperties.Count < 1)
            {
                return false;
            }
            _games = arcadeConfiguration.gameModelList;
            _objects = new List<GameObject>();
            cylArcadeProperties = arcadeConfiguration.cylArcadeProperties[0];

            if (_games.Count < cylArcadeProperties.sprockets)
            {
                if (cylArcadeProperties.selectedSprocket == 0)
                {

                }
                else if (cylArcadeProperties.selectedSprocket == cylArcadeProperties.sprockets - 1)
                {
                    cylArcadeProperties.selectedSprocket = _games.Count - 1;
                }
                else
                {
                    cylArcadeProperties.selectedSprocket = (((_games.Count % 2 == 0) ? _games.Count : (_games.Count - 1)) / 2) - 1;
                }
                cylArcadeProperties.sprockets = _games.Count - 1;
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
                _dummyCameraTransform = new GameObject
                {
                    name = "dummyCameraTransform"
                };
                _dummyCameraTransform.transform.parent = gameObject.transform;
            }
            else
            {
                _dummyCameraTransform = t.gameObject;
            }

            _cameraRotationDeltaDamping = cylArcadeProperties.cameraRotationdamping; // Reset damping

            _tempTargetModelGamePosition = cylArcadeProperties.gamePosition;
            cylArcadeProperties.gamePosition -= cylArcadeProperties.sprockets;
            if (cylArcadeProperties.gamePosition < 0)
            {
                cylArcadeProperties.gamePosition += _games.Count;
            }
            // Setup first model
            GameObject model = AddModel(cylArcadeProperties.gamePosition);
            _objects.Add(model);
            model.transform.position = new Vector3(0, 0, 0);
            model.transform.rotation = Quaternion.identity;
            model.transform.Translate(new Vector3(0, 0, cylArcadeProperties.radius), Space.World);
            // Local rotation of the sprocket
            model.transform.Rotate(cylArcadeProperties.sprocketLocalEularAngleX, cylArcadeProperties.sprocketLocalEularAngleY, cylArcadeProperties.sprocketLocalEularAngleZ);
            for (int i = 0; i < cylArcadeProperties.sprockets; i++)
            {
                cylArcadeProperties.gamePosition += 1;
                if (cylArcadeProperties.gamePosition > _games.Count - 1)
                {
                    cylArcadeProperties.gamePosition = 0;
                }
                Forwards(cylArcadeProperties.gamePosition);
            }
            cylArcadeProperties.gamePosition = _tempTargetModelGamePosition;
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
                obj = _objects[cylArcadeProperties.selectedSprocket];
            }
            catch (Exception)
            {

            }
            return obj != null ? _objects[cylArcadeProperties.selectedSprocket].transform.GetChild(0).gameObject : null;
        }
    }
}
