// GENERATED AUTOMATICALLY FROM 'Assets/Libretro/LibretroControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SK.Libretro
{
    public class @LibretroControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @LibretroControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""LibretroControls"",
    ""maps"": [
        {
            ""name"": ""All"",
            ""id"": ""639a1fe9-28e1-41ea-a98f-20b729bf505d"",
            ""actions"": [
                {
                    ""name"": ""JoypadDirections"",
                    ""type"": ""Value"",
                    ""id"": ""3d136013-1adf-4a01-ad86-7e5f4e85dd81"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadStartButton"",
                    ""type"": ""Value"",
                    ""id"": ""9c24f261-5878-4731-84f6-c78c63030707"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadSelectButton"",
                    ""type"": ""Value"",
                    ""id"": ""f36bdbe8-984d-4ee1-828f-09864532efd2"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadAButton"",
                    ""type"": ""Value"",
                    ""id"": ""e53ba5ed-7472-4b78-84c0-1fb5090f8c02"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadBButton"",
                    ""type"": ""Value"",
                    ""id"": ""43910c93-05e7-444e-a9a2-e9672dc24851"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadXButton"",
                    ""type"": ""Value"",
                    ""id"": ""e4753e14-08e3-4a66-8300-14400a4ac68f"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadYButton"",
                    ""type"": ""Value"",
                    ""id"": ""b06fcc7f-56a6-474b-8210-8542c0a84670"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadLButton"",
                    ""type"": ""Value"",
                    ""id"": ""7b0cf759-77c7-4989-9771-c92f0d252cbe"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadRButton"",
                    ""type"": ""Value"",
                    ""id"": ""818c3450-ed6f-43c4-b028-bd2e462d9531"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadL2Button"",
                    ""type"": ""Value"",
                    ""id"": ""056ab63b-501f-43c2-b9ef-6f18ba200cbc"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadR2Button"",
                    ""type"": ""Value"",
                    ""id"": ""53f451b1-0b6c-4fb3-aa44-e9adb960fde3"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadL3Button"",
                    ""type"": ""Value"",
                    ""id"": ""e24819fc-988c-4580-b3f3-5cf81d5bc5f6"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""JoypadR3Button"",
                    ""type"": ""Value"",
                    ""id"": ""c370147f-9b2e-4df5-94d7-ee55bbf96f12"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePositionDelta"",
                    ""type"": ""Value"",
                    ""id"": ""d1bb5935-abc8-408f-8be9-e7c13106d47f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseWheelDelta"",
                    ""type"": ""Value"",
                    ""id"": ""d4393c8f-ae26-43d5-a210-aada0d924e9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseLeftButton"",
                    ""type"": ""Value"",
                    ""id"": ""fb5fe1ab-4f43-4283-aa33-979f65f561f8"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseRightButton"",
                    ""type"": ""Value"",
                    ""id"": ""0f8e30f0-b439-4f01-b0d1-a44602b472a9"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMiddleButton"",
                    ""type"": ""Value"",
                    ""id"": ""d083c22f-6003-497b-b429-a6768fdd427c"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseForwardButton"",
                    ""type"": ""Value"",
                    ""id"": ""717e1b43-c43a-483a-b741-74bcebf873c7"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseBackButton"",
                    ""type"": ""Value"",
                    ""id"": ""2dd72a4a-66e3-4798-a075-6bb2dc2960bc"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2c219cb1-24e1-4536-b0b7-a043e6996c4e"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadStartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05314db2-fc53-4007-908e-4cefbcaded21"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadStartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9038026f-a11d-4b46-8642-914319aecbc9"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadSelectButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a8b16ce-e843-4273-8290-27d71cd99c61"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadSelectButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76e05051-e233-44a1-9eb1-124f5865a557"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadAButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3ccd30f-77fc-4578-a8df-c0913f9a0453"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadAButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96354b97-ed6e-4294-ad85-4cf0aa8b7f0f"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61173740-6cfb-488d-a57d-dcddd1a8a2e5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e8cf6e5-d265-48c0-b522-6ef65b9841ab"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadR3Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27e6739e-e46c-40a1-b371-c209c58baaed"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadR3Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d80c12b-1749-473c-9c2c-c60fa0590331"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadL3Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eebd950f-62b4-42d1-bc9c-ecf76e78bb42"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadL3Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d818302f-e70c-439a-93f3-29729ffc2709"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadR2Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bff5f60b-7033-4c2b-b8cb-7e960accda4c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadR2Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5bc3b7e-58b2-4333-a557-4bcaff47c53e"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadL2Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d902525-a63f-45ba-b8b1-ec4e35f59b2f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadL2Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77e966b4-1ecd-432d-a7a8-0d1c9de4abfb"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadRButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82af4109-f72d-433e-82b4-7031442c1b89"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadRButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86528e80-6a72-441b-9a7e-0f59f081e5d4"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadLButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""457ca745-3ab0-479b-b09a-174382947b59"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadLButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7173dc3-17d0-49f2-917b-a1713f08362d"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadYButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f2fe01b-53ad-4bc4-95d6-f91356467e79"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadYButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b04b3254-45f7-42ff-91e7-3ee94dd070f1"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadXButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d1cea9a-b937-4dab-af57-6f7703f0f3e5"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadXButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""a86c4a94-0270-4007-a825-dfb4278da3b0"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d066947d-869b-4ef5-b12b-d8171c6afe14"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""daf19ca1-8e59-4594-82a0-bd296f51d482"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b518a295-89a9-4b8a-8179-5561175b5bf8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a6bdebb2-50ff-429c-9cef-81bbe52b8e8a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""99f154ac-4827-4a36-9bb4-8fb6b50dcd67"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3107eda7-625c-4653-a7a5-faa061d03dd4"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""JoypadDirections"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b057da26-acc6-4521-a361-fc48dedc8a4a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MousePositionDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c01ed5f5-ff06-4b6b-b483-0f7f6d5306f3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseLeftButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8db7c5c-9c9f-4f4a-bd9d-7f3db65545a9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseRightButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6eebfaf6-8f0c-4277-8c84-1e64f3173785"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseMiddleButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3701c51-43c9-420a-bb40-1eab9a0a0d8b"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseForwardButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e66ff22-9651-4278-a6cf-39a8c28c7d34"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseBackButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fdf7fa4-a6c6-4349-80bf-0b243f538620"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseWheelDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // All
            m_All = asset.FindActionMap("All", throwIfNotFound: true);
            m_All_JoypadDirections = m_All.FindAction("JoypadDirections", throwIfNotFound: true);
            m_All_JoypadStartButton = m_All.FindAction("JoypadStartButton", throwIfNotFound: true);
            m_All_JoypadSelectButton = m_All.FindAction("JoypadSelectButton", throwIfNotFound: true);
            m_All_JoypadAButton = m_All.FindAction("JoypadAButton", throwIfNotFound: true);
            m_All_JoypadBButton = m_All.FindAction("JoypadBButton", throwIfNotFound: true);
            m_All_JoypadXButton = m_All.FindAction("JoypadXButton", throwIfNotFound: true);
            m_All_JoypadYButton = m_All.FindAction("JoypadYButton", throwIfNotFound: true);
            m_All_JoypadLButton = m_All.FindAction("JoypadLButton", throwIfNotFound: true);
            m_All_JoypadRButton = m_All.FindAction("JoypadRButton", throwIfNotFound: true);
            m_All_JoypadL2Button = m_All.FindAction("JoypadL2Button", throwIfNotFound: true);
            m_All_JoypadR2Button = m_All.FindAction("JoypadR2Button", throwIfNotFound: true);
            m_All_JoypadL3Button = m_All.FindAction("JoypadL3Button", throwIfNotFound: true);
            m_All_JoypadR3Button = m_All.FindAction("JoypadR3Button", throwIfNotFound: true);
            m_All_MousePositionDelta = m_All.FindAction("MousePositionDelta", throwIfNotFound: true);
            m_All_MouseWheelDelta = m_All.FindAction("MouseWheelDelta", throwIfNotFound: true);
            m_All_MouseLeftButton = m_All.FindAction("MouseLeftButton", throwIfNotFound: true);
            m_All_MouseRightButton = m_All.FindAction("MouseRightButton", throwIfNotFound: true);
            m_All_MouseMiddleButton = m_All.FindAction("MouseMiddleButton", throwIfNotFound: true);
            m_All_MouseForwardButton = m_All.FindAction("MouseForwardButton", throwIfNotFound: true);
            m_All_MouseBackButton = m_All.FindAction("MouseBackButton", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // All
        private readonly InputActionMap m_All;
        private IAllActions m_AllActionsCallbackInterface;
        private readonly InputAction m_All_JoypadDirections;
        private readonly InputAction m_All_JoypadStartButton;
        private readonly InputAction m_All_JoypadSelectButton;
        private readonly InputAction m_All_JoypadAButton;
        private readonly InputAction m_All_JoypadBButton;
        private readonly InputAction m_All_JoypadXButton;
        private readonly InputAction m_All_JoypadYButton;
        private readonly InputAction m_All_JoypadLButton;
        private readonly InputAction m_All_JoypadRButton;
        private readonly InputAction m_All_JoypadL2Button;
        private readonly InputAction m_All_JoypadR2Button;
        private readonly InputAction m_All_JoypadL3Button;
        private readonly InputAction m_All_JoypadR3Button;
        private readonly InputAction m_All_MousePositionDelta;
        private readonly InputAction m_All_MouseWheelDelta;
        private readonly InputAction m_All_MouseLeftButton;
        private readonly InputAction m_All_MouseRightButton;
        private readonly InputAction m_All_MouseMiddleButton;
        private readonly InputAction m_All_MouseForwardButton;
        private readonly InputAction m_All_MouseBackButton;
        public struct AllActions
        {
            private @LibretroControls m_Wrapper;
            public AllActions(@LibretroControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @JoypadDirections => m_Wrapper.m_All_JoypadDirections;
            public InputAction @JoypadStartButton => m_Wrapper.m_All_JoypadStartButton;
            public InputAction @JoypadSelectButton => m_Wrapper.m_All_JoypadSelectButton;
            public InputAction @JoypadAButton => m_Wrapper.m_All_JoypadAButton;
            public InputAction @JoypadBButton => m_Wrapper.m_All_JoypadBButton;
            public InputAction @JoypadXButton => m_Wrapper.m_All_JoypadXButton;
            public InputAction @JoypadYButton => m_Wrapper.m_All_JoypadYButton;
            public InputAction @JoypadLButton => m_Wrapper.m_All_JoypadLButton;
            public InputAction @JoypadRButton => m_Wrapper.m_All_JoypadRButton;
            public InputAction @JoypadL2Button => m_Wrapper.m_All_JoypadL2Button;
            public InputAction @JoypadR2Button => m_Wrapper.m_All_JoypadR2Button;
            public InputAction @JoypadL3Button => m_Wrapper.m_All_JoypadL3Button;
            public InputAction @JoypadR3Button => m_Wrapper.m_All_JoypadR3Button;
            public InputAction @MousePositionDelta => m_Wrapper.m_All_MousePositionDelta;
            public InputAction @MouseWheelDelta => m_Wrapper.m_All_MouseWheelDelta;
            public InputAction @MouseLeftButton => m_Wrapper.m_All_MouseLeftButton;
            public InputAction @MouseRightButton => m_Wrapper.m_All_MouseRightButton;
            public InputAction @MouseMiddleButton => m_Wrapper.m_All_MouseMiddleButton;
            public InputAction @MouseForwardButton => m_Wrapper.m_All_MouseForwardButton;
            public InputAction @MouseBackButton => m_Wrapper.m_All_MouseBackButton;
            public InputActionMap Get() { return m_Wrapper.m_All; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(AllActions set) { return set.Get(); }
            public void SetCallbacks(IAllActions instance)
            {
                if (m_Wrapper.m_AllActionsCallbackInterface != null)
                {
                    @JoypadDirections.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadDirections;
                    @JoypadDirections.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadDirections;
                    @JoypadDirections.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadDirections;
                    @JoypadStartButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadStartButton;
                    @JoypadStartButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadStartButton;
                    @JoypadStartButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadStartButton;
                    @JoypadSelectButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadSelectButton;
                    @JoypadSelectButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadSelectButton;
                    @JoypadSelectButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadSelectButton;
                    @JoypadAButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadAButton;
                    @JoypadAButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadAButton;
                    @JoypadAButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadAButton;
                    @JoypadBButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadBButton;
                    @JoypadBButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadBButton;
                    @JoypadBButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadBButton;
                    @JoypadXButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadXButton;
                    @JoypadXButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadXButton;
                    @JoypadXButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadXButton;
                    @JoypadYButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadYButton;
                    @JoypadYButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadYButton;
                    @JoypadYButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadYButton;
                    @JoypadLButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadLButton;
                    @JoypadLButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadLButton;
                    @JoypadLButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadLButton;
                    @JoypadRButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadRButton;
                    @JoypadRButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadRButton;
                    @JoypadRButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadRButton;
                    @JoypadL2Button.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL2Button;
                    @JoypadL2Button.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL2Button;
                    @JoypadL2Button.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL2Button;
                    @JoypadR2Button.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR2Button;
                    @JoypadR2Button.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR2Button;
                    @JoypadR2Button.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR2Button;
                    @JoypadL3Button.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL3Button;
                    @JoypadL3Button.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL3Button;
                    @JoypadL3Button.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadL3Button;
                    @JoypadR3Button.started -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR3Button;
                    @JoypadR3Button.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR3Button;
                    @JoypadR3Button.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnJoypadR3Button;
                    @MousePositionDelta.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMousePositionDelta;
                    @MousePositionDelta.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMousePositionDelta;
                    @MousePositionDelta.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMousePositionDelta;
                    @MouseWheelDelta.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseWheelDelta;
                    @MouseWheelDelta.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseWheelDelta;
                    @MouseWheelDelta.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseWheelDelta;
                    @MouseLeftButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseLeftButton;
                    @MouseLeftButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseLeftButton;
                    @MouseLeftButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseLeftButton;
                    @MouseRightButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseRightButton;
                    @MouseRightButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseRightButton;
                    @MouseRightButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseRightButton;
                    @MouseMiddleButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseMiddleButton;
                    @MouseMiddleButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseMiddleButton;
                    @MouseMiddleButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseMiddleButton;
                    @MouseForwardButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseForwardButton;
                    @MouseForwardButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseForwardButton;
                    @MouseForwardButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseForwardButton;
                    @MouseBackButton.started -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseBackButton;
                    @MouseBackButton.performed -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseBackButton;
                    @MouseBackButton.canceled -= m_Wrapper.m_AllActionsCallbackInterface.OnMouseBackButton;
                }
                m_Wrapper.m_AllActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @JoypadDirections.started += instance.OnJoypadDirections;
                    @JoypadDirections.performed += instance.OnJoypadDirections;
                    @JoypadDirections.canceled += instance.OnJoypadDirections;
                    @JoypadStartButton.started += instance.OnJoypadStartButton;
                    @JoypadStartButton.performed += instance.OnJoypadStartButton;
                    @JoypadStartButton.canceled += instance.OnJoypadStartButton;
                    @JoypadSelectButton.started += instance.OnJoypadSelectButton;
                    @JoypadSelectButton.performed += instance.OnJoypadSelectButton;
                    @JoypadSelectButton.canceled += instance.OnJoypadSelectButton;
                    @JoypadAButton.started += instance.OnJoypadAButton;
                    @JoypadAButton.performed += instance.OnJoypadAButton;
                    @JoypadAButton.canceled += instance.OnJoypadAButton;
                    @JoypadBButton.started += instance.OnJoypadBButton;
                    @JoypadBButton.performed += instance.OnJoypadBButton;
                    @JoypadBButton.canceled += instance.OnJoypadBButton;
                    @JoypadXButton.started += instance.OnJoypadXButton;
                    @JoypadXButton.performed += instance.OnJoypadXButton;
                    @JoypadXButton.canceled += instance.OnJoypadXButton;
                    @JoypadYButton.started += instance.OnJoypadYButton;
                    @JoypadYButton.performed += instance.OnJoypadYButton;
                    @JoypadYButton.canceled += instance.OnJoypadYButton;
                    @JoypadLButton.started += instance.OnJoypadLButton;
                    @JoypadLButton.performed += instance.OnJoypadLButton;
                    @JoypadLButton.canceled += instance.OnJoypadLButton;
                    @JoypadRButton.started += instance.OnJoypadRButton;
                    @JoypadRButton.performed += instance.OnJoypadRButton;
                    @JoypadRButton.canceled += instance.OnJoypadRButton;
                    @JoypadL2Button.started += instance.OnJoypadL2Button;
                    @JoypadL2Button.performed += instance.OnJoypadL2Button;
                    @JoypadL2Button.canceled += instance.OnJoypadL2Button;
                    @JoypadR2Button.started += instance.OnJoypadR2Button;
                    @JoypadR2Button.performed += instance.OnJoypadR2Button;
                    @JoypadR2Button.canceled += instance.OnJoypadR2Button;
                    @JoypadL3Button.started += instance.OnJoypadL3Button;
                    @JoypadL3Button.performed += instance.OnJoypadL3Button;
                    @JoypadL3Button.canceled += instance.OnJoypadL3Button;
                    @JoypadR3Button.started += instance.OnJoypadR3Button;
                    @JoypadR3Button.performed += instance.OnJoypadR3Button;
                    @JoypadR3Button.canceled += instance.OnJoypadR3Button;
                    @MousePositionDelta.started += instance.OnMousePositionDelta;
                    @MousePositionDelta.performed += instance.OnMousePositionDelta;
                    @MousePositionDelta.canceled += instance.OnMousePositionDelta;
                    @MouseWheelDelta.started += instance.OnMouseWheelDelta;
                    @MouseWheelDelta.performed += instance.OnMouseWheelDelta;
                    @MouseWheelDelta.canceled += instance.OnMouseWheelDelta;
                    @MouseLeftButton.started += instance.OnMouseLeftButton;
                    @MouseLeftButton.performed += instance.OnMouseLeftButton;
                    @MouseLeftButton.canceled += instance.OnMouseLeftButton;
                    @MouseRightButton.started += instance.OnMouseRightButton;
                    @MouseRightButton.performed += instance.OnMouseRightButton;
                    @MouseRightButton.canceled += instance.OnMouseRightButton;
                    @MouseMiddleButton.started += instance.OnMouseMiddleButton;
                    @MouseMiddleButton.performed += instance.OnMouseMiddleButton;
                    @MouseMiddleButton.canceled += instance.OnMouseMiddleButton;
                    @MouseForwardButton.started += instance.OnMouseForwardButton;
                    @MouseForwardButton.performed += instance.OnMouseForwardButton;
                    @MouseForwardButton.canceled += instance.OnMouseForwardButton;
                    @MouseBackButton.started += instance.OnMouseBackButton;
                    @MouseBackButton.performed += instance.OnMouseBackButton;
                    @MouseBackButton.canceled += instance.OnMouseBackButton;
                }
            }
        }
        public AllActions @All => new AllActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        public interface IAllActions
        {
            void OnJoypadDirections(InputAction.CallbackContext context);
            void OnJoypadStartButton(InputAction.CallbackContext context);
            void OnJoypadSelectButton(InputAction.CallbackContext context);
            void OnJoypadAButton(InputAction.CallbackContext context);
            void OnJoypadBButton(InputAction.CallbackContext context);
            void OnJoypadXButton(InputAction.CallbackContext context);
            void OnJoypadYButton(InputAction.CallbackContext context);
            void OnJoypadLButton(InputAction.CallbackContext context);
            void OnJoypadRButton(InputAction.CallbackContext context);
            void OnJoypadL2Button(InputAction.CallbackContext context);
            void OnJoypadR2Button(InputAction.CallbackContext context);
            void OnJoypadL3Button(InputAction.CallbackContext context);
            void OnJoypadR3Button(InputAction.CallbackContext context);
            void OnMousePositionDelta(InputAction.CallbackContext context);
            void OnMouseWheelDelta(InputAction.CallbackContext context);
            void OnMouseLeftButton(InputAction.CallbackContext context);
            void OnMouseRightButton(InputAction.CallbackContext context);
            void OnMouseMiddleButton(InputAction.CallbackContext context);
            void OnMouseForwardButton(InputAction.CallbackContext context);
            void OnMouseBackButton(InputAction.CallbackContext context);
        }
    }
}
