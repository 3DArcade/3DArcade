using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace Arcade
{
    // This enum is used by input (gamepad/keyboard) and ui buttons to set the correct buttonClicked.
    internal enum UIButtons
    {
        Select = 0,
        Cancel,
        Quit,
        Edit,
        Add,
        Delete,
        Settings,
        Information,
        MoveCabs,
        MoveCabsForward,
        MoveCabsBackward,
        MoveCabsLeft,
        MoveCabsRight,
        MoveCabsRotateRight,
        MoveCabsRotateLeft,
        MenuToggle,
        Update
    }

    public class ArcadeStateManager : MonoBehaviour
    {
        public int framesToSkip = 15;
        private float deltaTime;
        private ArcadeStates savedArcadeState;
        private bool arcadeStateHasChanged = false;
        private float rayLength;
        private bool isGrabbed;
        private Rigidbody grabbedObject;
        private readonly string[] inputButtons = Enum.GetNames(typeof(UIButtons));
        private UIButtons? buttonClicked;
        private GameObject selectZoneModel;
        private CylController arcadeCylController;
        private CylController menuCylController;
        private bool menuIsVisible = true;

        // Layers
        private LayerMask arcadeLayers;
        private LayerMask menuLayers;
        private LayerMask arcadeGameAndPropLayers;
        private LayerMask menuGameAndPropLayers;

        public static Camera activeCamera; // No outside refrences yet, make private?
        public static GameObject selectedModel = null; // Model itself not its dummy parent in playmode
        public static ModelSetup selectedModelSetup = null;
        public static GameObject savedArcadeModel = null;
        public static ModelSetup savedArcadeModelSetup = null;

        // UI Text
        public Text gameSelected;
        public Text fpsText;
        public Text gameDetails;
        public Text gameHistory;
        public Text dialogConfirm;
        public Text dialogAlert;
        public Text dialogProgress;

        // UI Menus
        public Canvas MoveCabs;
        public Canvas MoveCabsEdit;
        public Canvas Dialog;
        public Canvas Information;
        public Canvas Loading;
        public Canvas Running;
        public Canvas Settings;
        public Canvas GeneralConfigurationMenu;
        public Canvas ArcadesConfigurationMenu;
        public Canvas EmulatorsConfigurationMenu;
        public Canvas DialogAddEmulatorConfiguration;
        public Canvas DialogAlertConfiguration;
        public Canvas DialogProgressConfiguration;

        // Inputs Setup
        public Button mainMenuSettings;
        public Button generalConfigurationCancel;
        public Button arcadesConfigurationCancel;
        public Button emulatorsConfigurationCancel;
        public Button generalConfigurationSelect;
        public Button arcadesConfigurationSelect;
        public Button emulatorsConfigurationSelect;
        public Button arcadesConfigurationAdd;
        public Button arcadesConfigurationDelete;
        public Button emulatorsConfigurationAdd;
        public Button emulatorsConfigurationUpdate;
        public Button emulatorsConfigurationDelete;
        public Button runningSelect;
        public Button runningCancel;
        public Button runningMoveCabs;
        public Button runningInformation;
        public Button runningMainMenu;
        public Button moveCabsForward;
        public Button moveCabsBackward;
        public Button moveCabsLeft;
        public Button moveCabsRight;
        public Button moveCabsRotateRight;
        public Button moveCabsRotateLeft;
        public Button moveCabsEdit;
        public Button moveCabsAdd;
        public Button moveCabsDelete;
        public Button moveCabsEditSelect;
        public Button moveCabsEditCancel;
        public Button dialogSelect;
        public Button dialogCancel;
        public Button dialogAlertSelect;
        public Button dialogAddEmulatorConfigurationSelect;
        public Button dialogAddEmulatorConfigurationCancel;

        private void Start()
        {
            rayLength = 100.0f;
            isGrabbed = false;
            fpsText.text = "";

            // Setup Menus
            MoveCabs.gameObject.SetActive(false);
            MoveCabsEdit.gameObject.SetActive(false);
            Information.gameObject.SetActive(false);
            Dialog.gameObject.SetActive(false);
            Running.gameObject.SetActive(false);
            Settings.gameObject.SetActive(false);
            Loading.gameObject.SetActive(true);
            GeneralConfigurationMenu.gameObject.SetActive(false);
            ArcadesConfigurationMenu.gameObject.SetActive(false);
            EmulatorsConfigurationMenu.gameObject.SetActive(false);
            DialogAddEmulatorConfiguration.gameObject.SetActive(false);
            DialogAlertConfiguration.gameObject.SetActive(false);
            DialogProgressConfiguration.gameObject.SetActive(false);

            // Setup UI buttons
            mainMenuSettings.onClick.AddListener(() => ButtonClicked(UIButtons.Settings));
            generalConfigurationCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            arcadesConfigurationCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            emulatorsConfigurationCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            generalConfigurationSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            arcadesConfigurationSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            emulatorsConfigurationSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            arcadesConfigurationAdd.onClick.AddListener(() => ButtonClicked(UIButtons.Add));
            arcadesConfigurationDelete.onClick.AddListener(() => ButtonClicked(UIButtons.Delete));
            emulatorsConfigurationAdd.onClick.AddListener(() => ButtonClicked(UIButtons.Add));
            emulatorsConfigurationDelete.onClick.AddListener(() => ButtonClicked(UIButtons.Delete));
            emulatorsConfigurationUpdate.onClick.AddListener(() => ButtonClicked(UIButtons.Update));
            runningSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            runningInformation.onClick.AddListener(() => ButtonClicked(UIButtons.Information));
            runningMoveCabs.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabs));
            runningMainMenu.onClick.AddListener(() => ButtonClicked(UIButtons.Settings));
            runningCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Settings));
            moveCabsForward.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsForward));
            moveCabsBackward.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsBackward));
            moveCabsLeft.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsLeft));
            moveCabsRight.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsRight));
            moveCabsRotateRight.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsRotateRight));
            moveCabsRotateLeft.onClick.AddListener(() => ButtonClicked(UIButtons.MoveCabsRotateLeft));
            moveCabsEdit.onClick.AddListener(() => ButtonClicked(UIButtons.Edit));
            moveCabsAdd.onClick.AddListener(() => ButtonClicked(UIButtons.Add));
            moveCabsDelete.onClick.AddListener(() => ButtonClicked(UIButtons.Delete));
            moveCabsEditSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            dialogSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            moveCabsEditCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            dialogCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            dialogAddEmulatorConfigurationSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));
            dialogAddEmulatorConfigurationCancel.onClick.AddListener(() => ButtonClicked(UIButtons.Cancel));
            dialogAlertSelect.onClick.AddListener(() => ButtonClicked(UIButtons.Select));

            // Setup references
            arcadeLayers = LayerMask.GetMask("Arcade/ArcadeModels", "Arcade/GameModels", "Arcade/PropModels");
            menuLayers = LayerMask.GetMask("Menu/ArcadeModels", "Menu/GameModels", "Menu/PropModels");
            arcadeGameAndPropLayers = LayerMask.GetMask("Arcade/GameModels", "Arcade/PropModels");
            menuGameAndPropLayers = LayerMask.GetMask("Menu/GameModels", "Menu/PropModels");
            arcadeCylController = ArcadeManager.arcadeControls[ArcadeType.CylArcade].GetComponentInChildren<CylController>();
            menuCylController = ArcadeManager.arcadeControls[ArcadeType.CylMenu].GetComponentInChildren<CylController>();
            activeCamera = ArcadeManager.arcadeCameras[ArcadeType.FpsArcade];
        }

        private void ButtonClicked(UIButtons button) => buttonClicked = button;

        private void LateUpdate()
        {
            buttonClicked = null;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus && ArcadeManager.arcadeState == ArcadeStates.Game)
            {
                GameLauncherManager.UnLoadGame();
            }
        }

        private void Update()
        {
            // fps
            if (ArcadeManager.arcadeConfiguration.showFPS)
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;
                fpsText.text = Mathf.Ceil(fps).ToString();
            }

            // Check if input buttons/keys are pressed, then set the correct buttonClicked.
            for (int i = 0; i < inputButtons.Length; i++)
            {
                if (Input.GetButtonDown(inputButtons[i]))
                {
                    buttonClicked = (UIButtons)i;
                    break;
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Tab))
            {
                if (Cursor.lockState == CursorLockMode.Locked || Cursor.visible == false)
                {
                    print("unlock");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else if (Cursor.lockState == CursorLockMode.None || Cursor.lockState == CursorLockMode.Confined || Cursor.visible == true)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            //No need to update every frame.
            if (framesToSkip != 0 && Time.frameCount % framesToSkip != 0)
            {
                return;
            }

            if (GetSelectedModel())
            {
                TriggerManager.SendEvent(TriggerEvent.ModelSelectedChanged);
            }

            // Zoning
            if (ArcadeManager.allZones.ContainsKey(ArcadeType.FpsArcade) && !(ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                GetZone(ArcadeType.FpsArcade);
            }
            if (ArcadeManager.allZones.ContainsKey(ArcadeType.FpsMenu) && (ArcadeManager.arcadeState == ArcadeStates.ArcadeMenu))
            {
                GetZone(ArcadeType.FpsMenu);
            }

            if (ArcadeManager.arcadeState != savedArcadeState)
            {
                arcadeStateHasChanged = true;
                savedArcadeState = ArcadeManager.arcadeState;
                TriggerManager.SendEvent(TriggerEvent.ArcadeStateChanged);
            }
            else
            {
                arcadeStateHasChanged = false;
            }

            //print("Current State: " + ArcadeManager.arcadeState + " arcade " + ArcadeManager.activeArcadeType + " menu " + ArcadeManager.activeMenuType + " hist " + ArcadeManager.arcadeHistory.Count + " c " + Cursor.visible + " l " + Cursor.lockState.ToString());

            switch (ArcadeManager.arcadeState)
            {
                case ArcadeStates.Game:
                    if (buttonClicked == UIButtons.Quit)
                    {
                        // Stop Game
                        GameLauncherManager.UnLoadGame();
                    }
                    //if (buttonClicked == UIButtons.Select)
                    //{
                    //    buttonClicked = null;
                    //    if (selectedModel == null)
                    //    {
                    //        return;
                    //    }
                    //    if (selectedModelSetup.gameLauncherMethod == GameLauncherMethod.ArcadeConfiguration)
                    //    {   // Run new Arcade Configuration
                    //        StartCoroutine(StartNewArcade(selectedModelSetup.id, selectedModel));
                    //    }
                    //    else
                    //    {
                    //        // Run Game
                    //        GameLauncherManager.LoadGame(selectedModelSetup, selectedModelSetup);
                    //    }

                    //}
                    buttonClicked = null;
                    break;
                case ArcadeStates.Running:
                    // Update UI when arcadeState changes to Running
                    if (arcadeStateHasChanged)
                    {
                        Loading.gameObject.SetActive(false);
                        Settings.gameObject.SetActive(false);
                        runningCancel.gameObject.SetActive(false);
                        runningMainMenu.gameObject.SetActive(ArcadeManager.arcadeHistory.Count > 1);
                        mainMenuSettings.gameObject.SetActive(ArcadeManager.arcadeHistory.Count <= 1);
                        if (!menuIsVisible)
                        {
                            Running.gameObject.SetActive(false);
                        }
                        else
                        {
                            Running.gameObject.SetActive(true);
                            if (ArcadeManager.activeArcadeType == ArcadeType.CylArcade || ArcadeManager.activeArcadeType == ArcadeType.CylMenu)
                            {
                                runningMoveCabs.gameObject.SetActive(false);
                            }
                            else
                            {
                                runningMoveCabs.gameObject.SetActive(true);
                            }
                            if (ArcadeManager.arcadeConfiguration.showFPS)
                            {
                                fpsText.text = "";
                            }
                        }
                    }
                    if (buttonClicked == UIButtons.MenuToggle)
                    {
                        buttonClicked = null;
                        if (Running.gameObject.activeInHierarchy)
                        {
                            Running.gameObject.SetActive(false);
                            menuIsVisible = false;

                        }
                        else
                        {
                            Running.gameObject.SetActive(true);
                            if (ArcadeManager.activeArcadeType == ArcadeType.CylArcade || ArcadeManager.activeArcadeType == ArcadeType.CylMenu)
                            {
                                runningMoveCabs.gameObject.SetActive(false);
                            }
                            else
                            {
                                runningMoveCabs.gameObject.SetActive(true);
                            }
                            if (ArcadeManager.arcadeConfiguration.showFPS)
                            {
                                fpsText.text = "";
                            }
                            menuIsVisible = true;
                        }
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        buttonClicked = null;
                        if (selectedModel == null)
                        {
                            return;
                        }
                        ModelLibretroGameSetup modelLibretroGameSetup = selectedModel.transform.parent.GetComponent<ModelLibretroGameSetup>();
                        if (modelLibretroGameSetup != null)
                        {
                            modelLibretroGameSetup.ResumeGame();
                            RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
                            if (arcadeRigidbodyFirstPersonController != null)
                            {
                                arcadeRigidbodyFirstPersonController.pause = true;
                            }
                            ArcadeManager.arcadeState = ArcadeStates.Game;
                            return;
                        }
                        if (selectedModelSetup.gameLauncherMethod == GameLauncherMethod.ArcadeConfiguration)
                        {   // Run new Arcade Configuration
                            _ = StartCoroutine(StartNewArcade(selectedModelSetup.id));
                        }
                        else
                        {
                            // Run Game
                            GameLauncherManager.LoadGame(selectedModelSetup, selectedModelSetup);
                        }

                    }
                    else if (buttonClicked == UIButtons.Information)
                    {
                        buttonClicked = null;
                        if (ArcadeManager.arcadeState == ArcadeStates.MoveCabs)
                        {
                            return;
                        }
                        if (!Information.isActiveAndEnabled)
                        {
                            if (selectedModel == null)
                            {
                                return;
                            }
                            GetGameInformation(selectedModel);
                            Information.gameObject.SetActive(true);
                        }
                        else
                        {
                            Information.gameObject.SetActive(false);
                        }
                    }
                    else if (buttonClicked == UIButtons.Settings)
                    {
                        buttonClicked = null;
                        if (ArcadeManager.arcadeState == ArcadeStates.MoveCabs)
                        {
                            return;
                        }

                        // Toplevel menu then go to settings
                        if (ArcadeManager.arcadeHistory.Count < 2)
                        {
                            ArcadeManager.arcadeState = ArcadeStates.SettingsMenu;
                            Settings.gameObject.SetActive(true);
                            buttonClicked = null;
                            return;
                        }
                        // Else go back to previous Arcade Configuration

                        // Set UI
                        MoveCabs.gameObject.SetActive(false);
                        Information.gameObject.SetActive(false);
                        Running.gameObject.SetActive(false);
                        Loading.gameObject.SetActive(true);

                        if (ArcadeManager.activeArcadeType == ArcadeType.CylArcade)
                        {
                            arcadeCylController.setupFinished = false;
                        }
                        string newArcade = ArcadeManager.arcadeHistory[ArcadeManager.arcadeHistory.Count - 2];
                        ArcadeManager.arcadeHistory.RemoveRange(ArcadeManager.arcadeHistory.Count - 2, 2);
                        _ = StartCoroutine(StartNewArcade(newArcade));
                    }
                    else if (buttonClicked == UIButtons.MoveCabs)
                    {
                        buttonClicked = null;
                        if (ArcadeManager.arcadeState == ArcadeStates.Information)
                        {
                            return;
                        }
                        if (ArcadeManager.activeArcadeType != ArcadeType.FpsArcade)
                        {
                            return;
                        }
                        // Switch to MoveCabs State
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        MoveCabs.gameObject.SetActive(true);
                        Information.gameObject.SetActive(false);
                    }
                    else if (buttonClicked == UIButtons.Quit)
                    {
                        buttonClicked = null;
                        Application.Quit();
                    }
                    break;
                case ArcadeStates.ArcadeMenu:
                    if (arcadeStateHasChanged)
                    {
                        Loading.gameObject.SetActive(false);
                        runningMainMenu.gameObject.SetActive(false);
                        mainMenuSettings.gameObject.SetActive(false);
                        runningCancel.gameObject.SetActive(true);
                        runningMoveCabs.gameObject.SetActive(false);
                    }
                    if (buttonClicked == UIButtons.Quit || buttonClicked == UIButtons.Settings) // use settings for this!
                    {
                        buttonClicked = null;

                        // Unload Menu
                        ModelImageSetup modelImageSetup = savedArcadeModel.transform.GetChild(1).GetComponent<ModelImageSetup>();
                        if (modelImageSetup != null)
                        {
                            modelImageSetup.ResetArcadeTexture();
                        }
                        ModelVideoSetup modelVideoSetup = savedArcadeModel.transform.GetChild(1).GetComponent<ModelVideoSetup>();
                        if (modelVideoSetup != null)
                        {
                            modelVideoSetup.SetupVideoPlayer();
                        }
                        if (ArcadeManager.activeArcadeType == ArcadeType.FpsArcade)
                        {
                            Rigidbody arcadeCameraRigidBody = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<Rigidbody>();
                            arcadeCameraRigidBody.isKinematic = false;
                            CapsuleCollider arcadeCapsuleCollider = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<CapsuleCollider>();
                            arcadeCapsuleCollider.enabled = true;
                            RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
                            arcadeRigidbodyFirstPersonController.pause = false;
                        }
                        ArcadeManager.loadSaveArcadeConfiguration.ResetMenu();
                        selectedModel = null;
                        selectedModelSetup = null;
                        savedArcadeModel = null;
                        savedArcadeModelSetup = null;
                        ArcadeManager.activeMenuType = ArcadeType.None;
                        TriggerManager.SendEvent(TriggerEvent.MenuEnded);
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        buttonClicked = null;
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        buttonClicked = null;
                        if (selectedModel == null)
                        {
                            return;
                        }

                        // Unload Menu
                        ModelImageSetup modelImageSetup = savedArcadeModel.transform.GetChild(1).GetComponent<ModelImageSetup>();
                        if (modelImageSetup != null)
                        {
                            modelImageSetup.ResetArcadeTexture();
                        }
                        ModelVideoSetup modelVideoSetup = savedArcadeModel.transform.GetChild(1).GetComponent<ModelVideoSetup>();
                        if (modelVideoSetup != null)
                        {
                            modelVideoSetup.SetupVideoPlayer();
                        }
                        if (ArcadeManager.activeArcadeType == ArcadeType.FpsArcade)
                        {
                            Rigidbody arcadeCameraRigidBody = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<Rigidbody>();
                            arcadeCameraRigidBody.isKinematic = false;
                            CapsuleCollider arcadeCapsuleCollider = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<CapsuleCollider>();
                            arcadeCapsuleCollider.enabled = true;
                            RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
                            arcadeRigidbodyFirstPersonController.pause = false;
                        }
                        ArcadeManager.loadSaveArcadeConfiguration.ResetMenu();
                        //selectedModel = null;
                        //selectedModelSetup = null;
                        //savedArcadeModel = null;
                        //savedArcadeModelSetup = null;
                        ArcadeManager.activeMenuType = ArcadeType.None;
                        TriggerManager.SendEvent(TriggerEvent.MenuEnded);
                        ArcadeManager.arcadeState = ArcadeStates.Running;

                        if (selectedModelSetup.gameLauncherMethod == GameLauncherMethod.ArcadeConfiguration)
                        {   // Run new Arcade Configuration
                            _ = StartCoroutine(StartNewArcade(selectedModelSetup.id));
                        }
                        else
                        {
                            // Run Game
                            GameLauncherManager.LoadGame(selectedModelSetup, savedArcadeModelSetup);
                        }

                    }
                    break;
                //case ArcadeStates.Information:
                //    if (buttonClicked == UIButtons.Information)
                //    {
                //        ArcadeManager.arcadeState = ArcadeStates.Running;
                //        Information.gameObject.SetActive(false);
                //    }
                //    break;
                case ArcadeStates.LoadingAssets:
                    if (buttonClicked == UIButtons.Settings)
                    {
                        // print("Settings");
                    }
                    break;
                case ArcadeStates.MoveCabs:
                    if (ArcadeManager.activeMenuType != ArcadeType.None)
                    {
                        return;
                    }
                    if (buttonClicked == UIButtons.MoveCabs)
                    {
                        // If selected model is grabbed release it, then Switch to Running State.
                        if (isGrabbed)
                        {
                            ReleaseGrabSelectedObject();
                        }
                        ArcadeManager.loadSaveArcadeConfiguration.SaveArcade();
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        MoveCabs.gameObject.SetActive(false);
                    }
                    else if (buttonClicked == UIButtons.Edit)
                    {
                        if (selectedModel == null)
                        {
                            return;
                        }
                        ModelSetup gameModelSetup = selectedModel.transform.parent.GetComponent<ModelSetup>();
                        if (gameModelSetup == null)
                        {
                            return;
                        }
                        MoveCabs.gameObject.SetActive(false);
                        MoveCabsEdit.gameObject.SetActive(true);
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabsEdit;
                        MoveCabsEditGameProperties props = MoveCabsEdit.GetComponent<MoveCabsEditGameProperties>();
                        props.SetGameProperties(gameModelSetup.GetModelProperties());
                    }
                    else if (buttonClicked == UIButtons.Add)
                    {
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabsAdd;
                        MoveCabs.gameObject.SetActive(false);
                        MoveCabsEdit.gameObject.SetActive(true);
                    }
                    else if (buttonClicked == UIButtons.Delete)
                    {
                        if (selectedModel == null)
                        {
                            return;
                        }
                        ModelSetup gameModelSetup = selectedModel.transform.parent.GetComponent<ModelSetup>();
                        if (gameModelSetup == null)
                        {
                            return;
                        }
                        dialogConfirm.text = "Delete " + gameModelSetup.descriptiveName + " from this arcade?";
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabsDelete;
                        MoveCabs.gameObject.SetActive(false);
                        Dialog.gameObject.SetActive(true);
                    }
                    else if (buttonClicked == UIButtons.Select)
                    {
                        // If selected model is not grabbed, then grab it else release it.
                        if (!isGrabbed)
                        {
                            GrabSelectedObject();
                        }
                        else
                        {
                            ReleaseGrabSelectedObject();
                        }
                    }
                    else if (buttonClicked == UIButtons.Cancel)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        // If selected model is not grabbed and one of the transform inputs, then adjust the transform of the selected model.
                        if (!isGrabbed)
                        {
                            AdjustTransformSelectedModel();
                        }
                    }
                    break;
                case ArcadeStates.MoveCabsEdit:
                    if (buttonClicked == UIButtons.Select)
                    {
                        print("model add/edit is selected at it to the scene");

                        MoveCabs.gameObject.SetActive(true);
                        MoveCabsEdit.gameObject.SetActive(false);
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        if (selectedModel == null)
                        {
                            return;
                        }
                        MoveCabsEditGameProperties props = MoveCabsEdit.GetComponent<MoveCabsEditGameProperties>();
                        props.ReplaceModel(selectedModel);
                    }
                    else if (buttonClicked == UIButtons.Cancel)
                    {
                        print("model add/edit is not selected do nothing ");

                        MoveCabs.gameObject.SetActive(true);
                        MoveCabsEdit.gameObject.SetActive(false);
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                    }
                    break;
                case ArcadeStates.MoveCabsAdd:
                    if (buttonClicked == UIButtons.Select)
                    {
                        print("model add/edit is selected at it to the scene");
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        MoveCabs.gameObject.SetActive(true);
                        MoveCabsEdit.gameObject.SetActive(false);
                        MoveCabsEditGameProperties props = MoveCabsEdit.GetComponent<MoveCabsEditGameProperties>();
                        props.AddModel();
                    }
                    else if (buttonClicked == UIButtons.Cancel)
                    {
                        print("model add/edit is not selected do nothing ");
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        MoveCabs.gameObject.SetActive(true);
                        MoveCabsEdit.gameObject.SetActive(false);
                    }
                    break;
                case ArcadeStates.MoveCabsDelete:
                    if (buttonClicked == UIButtons.Select)
                    {
                        print("model delete is selected at it to the scene");

                        if (selectedModel == null)
                        {
                            return;
                        }

                        Destroy(selectedModel.transform.parent.gameObject);
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        MoveCabs.gameObject.SetActive(true);
                        Dialog.gameObject.SetActive(false);
                    }
                    else if (buttonClicked == UIButtons.Cancel)
                    {
                        print("model delete is not selected do nothing ");
                        ArcadeManager.arcadeState = ArcadeStates.MoveCabs;
                        MoveCabs.gameObject.SetActive(true);
                        Dialog.gameObject.SetActive(false);
                    }
                    break;
                case ArcadeStates.LoadingArcade:
                    buttonClicked = null;
                    print("load arcade now");
                    if (!Loading.isActiveAndEnabled)
                    {
                        Loading.gameObject.SetActive(true);
                        return;
                    }
                    if (Settings.isActiveAndEnabled)
                    {
                        Settings.gameObject.SetActive(false);
                    }
                    break;
                case ArcadeStates.SettingsMenu:
                    if (buttonClicked == UIButtons.Settings)
                    {
                        buttonClicked = null;
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        Settings.gameObject.SetActive(false);
                        GeneralConfigurationMenu.gameObject.SetActive(false);
                        ArcadesConfigurationMenu.gameObject.SetActive(false);
                        EmulatorsConfigurationMenu.gameObject.SetActive(false);
                    }
                    break;
                case ArcadeStates.GeneralConfigurationmenu:
                    if (!GeneralConfigurationMenu.isActiveAndEnabled)
                    {
                        Settings.gameObject.SetActive(false);
                        Running.gameObject.SetActive(false);
                        GeneralConfigurationGeneralProperties generalConfigurationGeneralProperties = GeneralConfigurationMenu.GetComponent<GeneralConfigurationGeneralProperties>();
                        if (generalConfigurationGeneralProperties != null)
                        {
                            generalConfigurationGeneralProperties.SetupList();
                        }
                        GeneralConfigurationMenu.gameObject.SetActive(true);
                    }
                    if (buttonClicked == UIButtons.Cancel)
                    {
                        GeneralConfigurationMenu.gameObject.SetActive(false);
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        GeneralConfigurationGeneralProperties generalConfigurationGeneralProperties = GeneralConfigurationMenu.GetComponent<GeneralConfigurationGeneralProperties>();
                        if (generalConfigurationGeneralProperties != null)
                        {
                            generalConfigurationGeneralProperties.SaveGeneralConfiguration();
                            Settings.gameObject.SetActive(false);
                            GeneralConfigurationMenu.gameObject.SetActive(false);
                        }
                    }
                    buttonClicked = null;
                    break;
                case ArcadeStates.ArcadesConfigurationMenu:
                    if (!ArcadesConfigurationMenu.isActiveAndEnabled)
                    {
                        Settings.gameObject.SetActive(false);
                        Running.gameObject.SetActive(false);
                        ArcadesConfigurationMenu.gameObject.SetActive(true);
                    }
                    if (buttonClicked == UIButtons.Cancel)
                    {
                        print("Cancel Arcade Configuration Menu");
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        //MainMenu.gameObject.SetActive(true);
                        _ = ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList();
                        //ArcadeManager.ShowMainMenu();
                        Settings.gameObject.SetActive(false);
                        ArcadesConfigurationMenu.gameObject.SetActive(false);
                        buttonClicked = null;
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        print("Save Arcade Configuration Menu");
                        ArcadesConfigurationArcadeProperties arcadesConfigurationArcadeProperties = ArcadesConfigurationMenu.GetComponent<ArcadesConfigurationArcadeProperties>();
                        if (arcadesConfigurationArcadeProperties != null)
                        {
                            arcadesConfigurationArcadeProperties.UpdateArcadeConfiguration();
                            ArcadeManager.loadSaveArcadeConfiguration.SaveArcadesConfigurationList();
                            _ = ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList();
                        }
                        ArcadeManager.arcadeState = ArcadeStates.Running;

                        //MainMenu.gameObject.SetActive(true);
                        //ArcadeManager.ShowMainMenu(); // Reloads main menu to refelct changes made here.
                        Settings.gameObject.SetActive(false);
                        ArcadesConfigurationMenu.gameObject.SetActive(false);
                        buttonClicked = null;
                    }
                    if (buttonClicked == UIButtons.Add)
                    {
                        ArcadesConfigurationArcadeProperties arcadesConfigurationArcadeProperties = ArcadesConfigurationMenu.GetComponent<ArcadesConfigurationArcadeProperties>();
                        if (arcadesConfigurationArcadeProperties != null)
                        {
                            arcadesConfigurationArcadeProperties.UpdateArcadeConfiguration();
                            ArcadeConfiguration arcadeConfiguration = ArcadeManager.arcadeConfiguration;
                            bool success = false;
                            int i = 1;
                            while (success == false)
                            {
                                string path = ArcadeManager.applicationPath + ArcadeManager.arcadesConfigurationPath;
                                if (FileManager.FileExists(path, arcadeConfiguration.id + "_" + i.ToString()) == null)
                                {
                                    arcadeConfiguration.id = arcadeConfiguration.id + "_" + i.ToString();
                                    arcadeConfiguration.descriptiveName = arcadeConfiguration.descriptiveName + " " + i.ToString();
                                    ArcadeManager.loadSaveArcadeConfiguration.SaveArcadeConfiguration(arcadeConfiguration);
                                    success = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            _ = ArcadeManager.loadSaveArcadeConfiguration.LoadArcadesConfigurationList();
                            ArcadeManager.arcadeConfiguration = arcadeConfiguration;
                            arcadesConfigurationArcadeProperties.Set();
                        }
                    }
                    if (buttonClicked == UIButtons.Delete)
                    {
                        ArcadesConfigurationArcadeProperties arcadesConfigurationArcadeProperties = ArcadesConfigurationMenu.GetComponent<ArcadesConfigurationArcadeProperties>();
                        GeneralConfiguration generalConfiguration = FileManager.LoadJSONData<GeneralConfiguration>(Path.Combine(ArcadeManager.applicationPath + "/3darcade~/Configuration/GeneralConfiguration.json"));
                        string startArcadeConfiguration = generalConfiguration != null ? generalConfiguration.mainMenuArcadeConfiguration : "None available";
                        if (arcadesConfigurationArcadeProperties != null && ArcadeManager.arcadesConfigurationList.Count() > 1 && arcadesConfigurationArcadeProperties.id.ToString().Trim() != startArcadeConfiguration)
                        {
                            arcadesConfigurationArcadeProperties.UpdateArcadeConfiguration();
                            ArcadeManager.loadSaveArcadeConfiguration.DeleteArcadeConfiguration(ArcadeManager.arcadeConfiguration);
                            arcadesConfigurationArcadeProperties.Set();
                        }
                        else if (arcadesConfigurationArcadeProperties != null)
                        {
                            if (ArcadeManager.arcadesConfigurationList.Count() > 1)
                            {
                                dialogAlert.text = "This Arcade Configuration is used as the startup Arcade Configuration. Change the startup Arcade Configuration first!";
                                DialogAlertConfiguration.gameObject.SetActive(true);
                                ArcadeManager.arcadeState = ArcadeStates.AlertArcadesConfigurationMenuError;
                            }
                            else
                            {
                                dialogAlert.text = "I cant't delete the last Arcade Configuration. We need at least one!";
                                DialogAlertConfiguration.gameObject.SetActive(true);
                                ArcadeManager.arcadeState = ArcadeStates.AlertArcadesConfigurationMenuError;
                            }
                        }
                    }
                    buttonClicked = null;
                    break;
                case ArcadeStates.AlertArcadesConfigurationMenuError:
                    DialogAlertConfiguration.gameObject.SetActive(false);
                    buttonClicked = null;
                    ArcadeManager.arcadeState = ArcadeStates.ArcadesConfigurationMenu;
                    break;
                case ArcadeStates.EmulatorsConfigurationMenu:
                    if (!EmulatorsConfigurationMenu.isActiveAndEnabled)
                    {
                        Settings.gameObject.SetActive(false);
                        Running.gameObject.SetActive(false);
                        EmulatorsConfigurationMenu.gameObject.SetActive(true);
                    }
                    if (buttonClicked == UIButtons.Cancel)
                    {
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        //MainMenu.gameObject.SetActive(true);
                        Settings.gameObject.SetActive(false);
                        EmulatorsConfigurationMenu.gameObject.SetActive(false);
                        buttonClicked = null;
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        EmulatorsConfigurationEmulatorProperties emulatorsConfigurationEmulatorProperties = EmulatorsConfigurationMenu.GetComponent<EmulatorsConfigurationEmulatorProperties>();
                        if (emulatorsConfigurationEmulatorProperties != null)
                        {
                            emulatorsConfigurationEmulatorProperties.UpdateEmulatorConfiguration();
                            emulatorsConfigurationEmulatorProperties.Save();
                        }
                        ArcadeManager.arcadeState = ArcadeStates.Running;
                        //MainMenu.gameObject.SetActive(true);
                        Settings.gameObject.SetActive(false);
                        EmulatorsConfigurationMenu.gameObject.SetActive(false);
                        buttonClicked = null;
                    }
                    if (buttonClicked == UIButtons.Add)
                    {
                        DialogAddEmulatorConfiguration.gameObject.SetActive(true);
                        ArcadeManager.arcadeState = ArcadeStates.DialogAddEmulatorConfiguration;
                    }
                    if (buttonClicked == UIButtons.Update)
                    {
                        Debug.Log("update");
                        dialogProgress.text = "Updating MasterGamelist, please wait...";
                        DialogProgressConfiguration.gameObject.SetActive(true);
                        ArcadeManager.arcadeState = ArcadeStates.DialogUpdateMasterGamelist;
                    }
                    if (buttonClicked == UIButtons.Delete)
                    {
                        EmulatorsConfigurationEmulatorProperties emulatorsConfigurationEmulatorProperties = EmulatorsConfigurationMenu.GetComponent<EmulatorsConfigurationEmulatorProperties>();
                        if (emulatorsConfigurationEmulatorProperties != null && ArcadeManager.emulatorsConfigurationList.Count() > 1)
                        {
                            emulatorsConfigurationEmulatorProperties.Delete();
                            emulatorsConfigurationEmulatorProperties.Set("");
                        }
                        else
                        {
                            dialogAlert.text = "I cant't delete the last Emulator Configuration. We need at least one!";
                            DialogAlertConfiguration.gameObject.SetActive(true);
                            ArcadeManager.arcadeState = ArcadeStates.AlertEmulatorsConfigurationMenuError;
                        }
                    }
                    buttonClicked = null;
                    break;
                case ArcadeStates.AlertEmulatorsConfigurationMenuError:
                    DialogAlertConfiguration.gameObject.SetActive(false);
                    buttonClicked = null;
                    ArcadeManager.arcadeState = ArcadeStates.EmulatorsConfigurationMenu;
                    break;
                case ArcadeStates.DialogAddEmulatorConfiguration:
                    if (buttonClicked == UIButtons.Cancel)
                    {
                        buttonClicked = null;
                        DialogAddEmulatorConfiguration.gameObject.SetActive(false);
                        ArcadeManager.arcadeState = ArcadeStates.EmulatorsConfigurationMenu;
                    }
                    if (buttonClicked == UIButtons.Select)
                    {
                        DialogAddEmulatorConfiguration dialogAddEmulatorConfiguration = DialogAddEmulatorConfiguration.gameObject.GetComponent<DialogAddEmulatorConfiguration>();
                        if (dialogAddEmulatorConfiguration != null)
                        {
                            Dictionary<string, string> props = dialogAddEmulatorConfiguration.GetEmulatorProperties();
                            //print(props["id"] + " " + props["masterGamelist"] + " " + props["descriptiveName"]);
                            string descriptiveName = props["descriptiveName"].Trim();
                            string id = props["id"].ToLower().Trim();
                            string masterGamelist = props["masterGamelist"].Trim();
                            string catVer = props["catVer"].Trim();
                            string error = "Error!";
                            bool ok = true;
                            if (descriptiveName == "" || id == "")
                            {
                                ok = false;
                                error += " Descriptive Name, Id can't be empty!";
                            }
                            if (FileManager.FileExists(ArcadeManager.applicationPath + ArcadeManager.emulatorsConfigurationPath, id + ".json") != null)
                            {
                                ok = false;
                                error = error + " Emulator Configuration with ID " + id + " already exists!";
                            }
                            if (ok && catVer != "")
                            {
                                if (FileManager.GetMasterGamelist(catVer, id) == null)
                                {
                                    //ok = false;
                                    error += " No valid CatVer selected!";
                                }
                            }
                            else if (ok)
                            {
                                if (FileManager.GetMasterGamelist(masterGamelist, id) == null)
                                {
                                    //ok = false;
                                    error += " No valid Master Gamelist selected!";
                                }
                            }
                            if (!ok)
                            {
                                dialogAlert.text = error;
                                DialogAlertConfiguration.gameObject.SetActive(true);
                                ArcadeManager.arcadeState = ArcadeStates.AlertAddEmulatorConfigurationError;
                            }
                            else
                            {
                                EmulatorConfiguration emulatorConfiguration = new EmulatorConfiguration
                                {
                                    emulator = new EmulatorProperties
                                    {
                                        descriptiveName = descriptiveName,
                                        id = id
                                    }
                                };
                                emulatorConfiguration = UpdateMasterGamelists.UpdateMasterGamelistFromEmulatorConfiguration(emulatorConfiguration);
                                ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
                                ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulators();
                                EmulatorsConfigurationEmulatorProperties emulatorsConfigurationArcadeProperties = EmulatorsConfigurationMenu.GetComponent<EmulatorsConfigurationEmulatorProperties>();
                                if (emulatorsConfigurationArcadeProperties != null)
                                {
                                    emulatorsConfigurationArcadeProperties.Set(descriptiveName);
                                }
                                DialogAddEmulatorConfiguration.gameObject.SetActive(false);
                                ArcadeManager.arcadeState = ArcadeStates.EmulatorsConfigurationMenu;
                            }
                        }
                        buttonClicked = null;
                    }
                    buttonClicked = null;
                    break;
                case ArcadeStates.AlertAddEmulatorConfigurationError:
                    if (buttonClicked == UIButtons.Select)
                    {
                        DialogAlertConfiguration.gameObject.SetActive(false);
                        buttonClicked = null;
                        ArcadeManager.arcadeState = ArcadeStates.DialogAddEmulatorConfiguration;
                    }
                    break;
                case ArcadeStates.DialogUpdateMasterGamelist:
                    buttonClicked = null;
                    EmulatorsConfigurationEmulatorProperties temulatorsConfigurationEmulatorProperties = EmulatorsConfigurationMenu.GetComponent<EmulatorsConfigurationEmulatorProperties>();
                    if (temulatorsConfigurationEmulatorProperties != null)
                    {

                        EmulatorConfiguration emulatorConfiguration =
                            UpdateMasterGamelists.UpdateMasterGamelistFromEmulatorConfiguration(temulatorsConfigurationEmulatorProperties.emulatorConfiguration);

                        temulatorsConfigurationEmulatorProperties.emulatorConfiguration.masterGamelist = emulatorConfiguration.masterGamelist;
                        temulatorsConfigurationEmulatorProperties.emulatorConfiguration.lastMasterGamelistUpdate = emulatorConfiguration.lastMasterGamelistUpdate;
                        temulatorsConfigurationEmulatorProperties.emulatorConfiguration.md5MasterGamelist = emulatorConfiguration.md5MasterGamelist;
                        temulatorsConfigurationEmulatorProperties.SetupList();
                    }
                    DialogProgressConfiguration.gameObject.SetActive(false);
                    ArcadeManager.arcadeState = ArcadeStates.EmulatorsConfigurationMenu;
                    break;
                default:
                    //    UnityEngine.Debug.Log("Arcade State Error " + ArcadeManager.arcadeState.ToString());
                    break;
            }
        }

        private bool GetSelectedModel()
        {
            GameObject obj = null;
            ModelSetup modelSetup = null;
            if (ArcadeManager.activeMenuType == ArcadeType.CylMenu)
            {
                obj = menuCylController.GetSelectedGame();
                if (obj != null)
                {
                    modelSetup = obj.transform.parent.GetComponent<ModelSetup>();
                    framesToSkip = 0; // We want updates as fast as possible in Cyl mode/
                }
            }
            else if (ArcadeManager.activeArcadeType == ArcadeType.CylArcade && ArcadeManager.activeMenuType == ArcadeType.None)
            {
                obj = arcadeCylController.GetSelectedGame();
                if (obj != null)
                {
                    modelSetup = obj.transform.parent.GetComponent<ModelSetup>();
                    framesToSkip = 0; // We want updates as fast as possible in Cyl mode/
                }
            }
            else if (Physics.Raycast(activeCamera.transform.position, activeCamera.transform.forward, out RaycastHit vision, rayLength, ArcadeManager.activeMenuType == ArcadeType.None ? arcadeGameAndPropLayers : menuGameAndPropLayers))
            {
                GameObject objectHit = vision.transform.gameObject;
                obj = objectHit;
                if (objectHit != null)
                {
                    modelSetup = objectHit.transform.parent.gameObject.GetComponent<ModelSetup>();
                }
            }
            else
            {
                framesToSkip = 15;
            }

            if (modelSetup != null)
            {
                if (modelSetup != selectedModelSetup)
                {
                    // New model selected
                    if (selectedModelSetup != null)
                    {
                        selectedModelSetup.isSelected = false;
                    }
                    selectedModel = obj;
                    selectedModelSetup = modelSetup;
                    selectedModelSetup.isSelected = true;
                    string descriptiveName = modelSetup.descriptiveName;
                    int index = descriptiveName.IndexOf("(", StringComparison.Ordinal);
                    if (index > 0)
                    {
                        descriptiveName = descriptiveName.Substring(0, index);
                    }
                    gameSelected.text = descriptiveName;
                    if (Information.isActiveAndEnabled == true)
                    {
                        GetGameInformation(selectedModel);
                    }
                    return true;
                }
                return false;
            }

            // Nothing Selected
            if (selectedModelSetup != null)
            {
                selectedModelSetup.isSelected = false;
            }
            selectedModel = null;
            selectedModelSetup = null;
            gameSelected.text = "";
            return false;
        }

        private void AdjustTransformSelectedModel()
        {
            if (selectedModel == null)
            {
                return;
            }

            if (buttonClicked == UIButtons.MoveCabsRotateRight)
            {
                selectedModel.transform.Rotate(new Vector3(0, 10, 0));
                if ((int)Mathf.Round(selectedModel.transform.eulerAngles.y) == 180)
                {
                    selectedModel.transform.Rotate(new Vector3(0, -180, 0));
                }
            }
            else if (buttonClicked == UIButtons.MoveCabsRotateLeft)
            {
                selectedModel.transform.Rotate(new Vector3(0, -10, 0));
                if ((int)Mathf.Round(selectedModel.transform.eulerAngles.y) == -180)
                {
                    selectedModel.transform.Rotate(new Vector3(0, 180, 0));
                }
            }
            else if (buttonClicked == UIButtons.MoveCabsForward)
            {
                selectedModel.transform.Translate(0, 0, -0.1f);
            }
            else if (buttonClicked == UIButtons.MoveCabsBackward)
            {
                selectedModel.transform.Translate(0, 0, 0.1f);
            }
            else if (buttonClicked == UIButtons.MoveCabsLeft)
            {
                selectedModel.transform.Translate(0.1f, 0, 0);
            }
            else if (buttonClicked == UIButtons.MoveCabsRight)
            {
                selectedModel.transform.Translate(-0.1f, 0, 0);
            }
        }

        private void GrabSelectedObject()
        {
            if (selectedModel == null)
            {
                return;
            }

            grabbedObject = selectedModel.GetComponent<Rigidbody>();
            if (grabbedObject == null)
            {
                return;
            }

            grabbedObject.isKinematic = true;
            grabbedObject.GetComponent<Collider>().enabled = false;
            grabbedObject.transform.parent.SetParent(activeCamera.transform);
            grabbedObject.transform.LookAt(activeCamera.transform.parent.transform);
            grabbedObject.transform.rotation = Quaternion.Euler(0, grabbedObject.transform.eulerAngles.y, grabbedObject.transform.eulerAngles.z);
            isGrabbed = true;
        }

        private void ReleaseGrabSelectedObject()
        {
            Transform trans = grabbedObject.transform;
            grabbedObject.transform.parent.parent = GameObject.Find("Arcade/GameModels").transform;
            grabbedObject.transform.position = trans.position;
            grabbedObject.transform.rotation = trans.rotation;
            grabbedObject.GetComponent<Collider>().enabled = true;
            grabbedObject.isKinematic = false;
            isGrabbed = false;
        }

        private void GetZone(ArcadeType arcadeType)
        {
            if (!Physics.Raycast(activeCamera.transform.parent.transform.position, -activeCamera.transform.parent.transform.up, out RaycastHit vision, rayLength, ArcadeManager.activeMenuType == ArcadeType.None ? arcadeLayers : menuLayers))
            {
                return;
            }

            GameObject objectHit = vision.transform.gameObject;
            if (objectHit == null)
            {
                return;
            }

            Transform objectHitParent = objectHit.transform.parent;
            if (objectHitParent != null && objectHitParent.gameObject == selectZoneModel)
            {
                return;
            }

            selectZoneModel = objectHitParent.gameObject;

            ModelSetup modelSetup = objectHitParent.GetComponent<ModelSetup>();
            if (modelSetup == null)
            {
                return;
            }

            foreach (KeyValuePair<int, List<GameObject>> entry in ArcadeManager.visibleZones[arcadeType])
            {
                // Zone 0 is always visible
                if (modelSetup.zone == 0 || (ArcadeManager.allZones[arcadeType].ContainsKey(modelSetup.zone) && ArcadeManager.allZones[arcadeType][modelSetup.zone].Contains(entry.Key)))
                {
                    foreach (GameObject obj in entry.Value)
                    {
                        obj.SetActive(true);
                    }
                }
                else if (entry.Key != 0)
                {
                    foreach (GameObject obj in entry.Value)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }

        private IEnumerator StartNewArcade(string arcadeConfigurationID)
        {
            ArcadeConfiguration arcadeConfiguration = ArcadeManager.loadSaveArcadeConfiguration.GetArcadeConfigurationByID(arcadeConfigurationID);
            if (arcadeConfiguration != null)
            {
                ArcadeManager.activeMenuType = ArcadeType.None;
                if (arcadeConfiguration.arcadeType == ArcadeType.FpsArcade.ToString() || arcadeConfiguration.arcadeType == ArcadeType.CylArcade.ToString())
                {
                    MoveCabs.gameObject.SetActive(false);
                    Settings.gameObject.SetActive(false);
                    ArcadeManager.arcadeState = ArcadeStates.LoadingArcade;
                    savedArcadeState = ArcadeStates.LoadingArcade;
                    Loading.gameObject.SetActive(true);
                }
                yield return null;
                _ = ArcadeManager.StartArcadeWith(arcadeConfigurationID);
            }
        }

        private void GetGameInformation(GameObject obj)
        {
            ModelSetup gameModelSetup = obj.transform.parent.GetComponent<ModelSetup>();
            if (gameModelSetup == null)
            {
                return;
            }

            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id.Equals(gameModelSetup.emulator)).ToList();
            if (emulatorConfiguration.Count < 1)
            {
                print("Launcher: no emulator configuration found");
                return;
            }

            string infoPath = $"{ArcadeManager.applicationPath}{FileManager.CorrectFilePath(emulatorConfiguration[0].emulator.infoPath)}";

            string infoText = File.ReadAllText(Path.Combine(infoPath, $"{gameModelSetup.id.Trim()}.txt"))
                           ?? File.ReadAllText(Path.Combine(infoPath, $"{gameModelSetup.idParent.Trim()}.txt"));

            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine($"Name: {gameModelSetup.descriptiveName}{Environment.NewLine}")
                  .AppendLine($"Manufacturer: {gameModelSetup.manufacturer}{Environment.NewLine}")
                  .AppendLine($"Year: {gameModelSetup.year}{Environment.NewLine}")
                  .AppendLine($"Genre: {gameModelSetup.genre}{Environment.NewLine}{Environment.NewLine}")
                  .AppendLine(infoText ?? string.Empty);
            gameHistory.text = sb.ToString();
        }
    }
}
