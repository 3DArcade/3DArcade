// GENERATED AUTOMATICALLY FROM 'Assets/3darcade/InputSystemSettings/InputSettingsActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Arcade
{
    public class @InputSettingsActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputSettingsActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSettingsActions"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""dcef0966-dff4-49e7-a970-23d5259d1a82"",
            ""actions"": [
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""d8177f08-24f3-4440-af5b-6cbb708553c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleCursor"",
                    ""type"": ""Button"",
                    ""id"": ""111e58fd-0332-48ec-a8c5-6a8f4028d3af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""71a3424e-444f-46f7-a84f-dff06c7d8d0c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92403178-6904-4c3e-bd0a-553260b9b699"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18a59c22-2410-49a6-b201-848edb236c4e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82d02491-296f-44c7-b295-9203ba7e8bb3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ee846d6-4a8e-491a-af01-c96c7c9e00c1"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FpsArcade"",
            ""id"": ""22122fb4-ab81-4a29-9a20-1efdb89252ad"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""9e3d2a7f-4e36-413f-a0db-967f82c11f2f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""c4a091c0-183c-4683-bd92-688cb76a48d0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraHeight"",
                    ""type"": ""Value"",
                    ""id"": ""a90ee00d-75ff-4601-85ad-c4ce6914610c"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""8de635a7-b295-43d9-b05f-89daa55015c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Value"",
                    ""id"": ""5786bc8b-1bdb-410d-8fce-1efa022cc1d5"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f8f41772-c9a4-4700-9873-67ca7da3cdf0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMoveCab"",
                    ""type"": ""Button"",
                    ""id"": ""c3ac06ce-bcb9-4007-b358-0e03157f5c46"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WSAD"",
                    ""id"": ""acf59875-e049-48c7-a28a-744b5b337ca9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""65f115c7-f132-43a4-88df-b6d93bc2789f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""18c43523-0b1c-4653-b74f-319a6d4f87db"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""22e04564-028a-42c3-9c72-9ea9ca8195d5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3ea590fd-28cd-40a2-ade7-a4d716c8e467"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""b1bedf90-557c-42a3-815a-b4f2deaab53f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b817b16a-6c66-4ba9-af1b-6951709c5ab6"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""77e8eee0-b398-4c10-a586-737f27d84f6e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""51ea1253-0afa-4851-af30-fea86a493324"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""76ad32ba-a9c5-4991-be62-43a851579e78"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""eca59f9b-0916-4aae-ba8b-e1728489a66a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""979699a5-0170-472e-b44c-55f7930528bd"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.08,y=0.08)"",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""RFAE"",
                    ""id"": ""5bc352fa-37c6-428f-b639-dce6f5a7151f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0ed4de18-42e8-4f43-a942-a5347f5bcca5"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9f363d4e-ac8b-4e7d-8ca8-1dc4f28f39c0"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7d12984a-74ab-4dc8-b361-a1636b370eca"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fb2726d9-80d1-4706-b072-d261c0d8b76a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""167049f4-9b46-4dba-9f98-621ae2c93be9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8be74ff5-e143-46a3-8557-c470a5dbc5f2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6075b4c4-c227-4352-817a-fb79a1d2fd17"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b8f287e-136d-4a76-a1bd-b1d930366911"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d6d2fec-43dd-4b65-ad81-5e0d762f8932"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23902d4e-e621-4abd-a958-022bff195241"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8a668ba-0a38-4d76-b692-b8fe5c53c28f"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""TG"",
                    ""id"": ""79870c4d-fadb-4cdb-a403-73c0ac0769c9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""f307a84c-c441-4477-9b8b-b899be2ad6b2"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""dd3eb91e-9fbc-4c81-ac07-0e1bbfd9e71e"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""f3b06e97-b2db-43a2-96ef-5a12f1382aa7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""a15d2d42-7acb-4f96-9a64-f0fdcdcfcf5c"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""4592d88f-b596-43a1-9b33-0bad3cd34d04"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a992bda7-e0a9-4b5f-9258-4f48b27434e3"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20db50ba-9db8-4968-b4fd-b8259ce69367"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bccf0dca-76f0-4960-9e54-0dbf8103b620"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FpsMoveCab"",
            ""id"": ""826403ab-f082-4a58-8a51-3a726b972945"",
            ""actions"": [
                {
                    ""name"": ""MoveModel"",
                    ""type"": ""Value"",
                    ""id"": ""3c404557-4b9e-4d5c-b5ef-b2e19649b1d5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RotateModel"",
                    ""type"": ""Value"",
                    ""id"": ""5bd2bcf2-81e7-487d-a7bb-9efd6a463306"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GrabReleaseModel"",
                    ""type"": ""Button"",
                    ""id"": ""da98b889-a257-4260-bdb7-c2d25d037c14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddModel"",
                    ""type"": ""Button"",
                    ""id"": ""3b4cdfb5-a154-4ec7-af71-6918230a09a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""IKJL"",
                    ""id"": ""e2dbf37a-fe69-4f24-907b-785db15190c5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveModel"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b0ee920e-807b-4a67-a268-5ae3d1cd35cb"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MoveModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6aab8201-b777-408d-a0cc-1bf61d49b99d"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MoveModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d22e5217-f071-4ce6-9b1c-6f543611f61e"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MoveModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcc18843-1103-4a63-b59e-375348100c0b"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MoveModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a32c1c88-d8b1-4753-b208-b49d17aac77b"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""UO"",
                    ""id"": ""405e8f8e-765a-4380-be52-32c34ebee78c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateModel"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5b6b77dd-c8b5-4336-88aa-3e986721dfde"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""RotateModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""267ba5e7-001a-4917-a6c9-7ad831e33a53"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""RotateModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Shoulders"",
                    ""id"": ""341a6c34-3c4a-45be-a2fe-b950191f5f2b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateModel"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c8fd3857-6235-4e6a-9c95-c5420b95dc87"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RotateModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""645fdfac-6ae9-4d08-b2b1-2c2c6b2be365"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RotateModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e4abb522-babd-4918-9659-68a20604bf3f"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AddModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0990e09d-e138-4b42-8abe-72a9e4b5a367"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""GrabReleaseModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6741b1b1-5301-4dc5-81f1-d07abd193117"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""GrabReleaseModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""803e54e5-a269-486a-aa6a-8789717c70f5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GrabReleaseModel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CylArcade"",
            ""id"": ""cee92ad0-24ac-4ae8-b5bb-9fd31a285831"",
            ""actions"": [
                {
                    ""name"": ""NavigationUpDown"",
                    ""type"": ""Value"",
                    ""id"": ""e1f0c0ad-badd-44d1-a958-cb657e58637d"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NavigationLeftRight"",
                    ""type"": ""Value"",
                    ""id"": ""ab61ee91-1a7d-471b-bc23-8f1c4a0e8260"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""ec7b6c22-2306-4be1-90de-d60a48c1f2bf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraHeight"",
                    ""type"": ""Value"",
                    ""id"": ""b794dfff-5179-436f-b532-d0afa18c37a8"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""5c66f33d-85c8-49d5-87ec-8b63f8ed1eee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WS"",
                    ""id"": ""05099181-0021-4413-8078-44e1c7ef9f1f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f3ade314-c401-40d7-9775-6eeb0cfd6c5a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8d1c0ab6-fc80-4c8a-a037-8855f834cce3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""8da28b44-fd4e-4869-9af8-293cf1218fee"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""725e64b4-65e6-4ef3-94ac-9c4d6f7582e1"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4e166e49-4761-415c-a091-6581f34fced1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""8f73a6aa-397c-4b77-9b67-8c08db46c72a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""089ee213-684e-4e55-9a13-2fd3a3ba25e7"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""20c36024-7d77-43dc-b64a-b6613a82f62a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NavigationUpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1868423f-1af1-4ae5-90e4-4c855df6bc04"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.08,y=0.08)"",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""RFAE"",
                    ""id"": ""928bd405-c860-4f27-aa41-02db7f658248"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c1a9e278-11cf-4338-9ed6-2f12f23ced36"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""65998c15-72ed-4441-879b-34098d1bc31e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ad275b4f-d8d4-42f1-b3eb-3baee334cd01"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dd0dc889-8276-4d2e-abc5-a33121c73045"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2a412500-fde2-47b3-bf9c-73f3027249c5"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""TG"",
                    ""id"": ""32782cf7-c276-4a78-90a2-f479766e39f4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""0c3ced21-82b0-4fa9-8cd6-e40b12b063f0"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""c6474c29-88d2-491f-b759-7a4f0b1de030"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""412ac7a8-42e3-4fc9-a11b-4fa7a0d6378c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""b9a243ce-4ec4-4c94-bec7-4d1c78fc4ad5"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""f5a9fba8-1b74-429c-bd3c-d3420808b02c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CameraHeight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a4b979a9-91a6-4aed-bc66-9bab7490a5b5"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d8d3697-a117-4fb6-af5e-b2b848b5551b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dc06803-7870-4a8d-bfef-33a7c084dd8f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""b76e3822-8474-4c2b-a7e0-4e0790b92080"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6a011495-7033-4d48-b7cb-48916ad4dc14"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d86cc60b-79a5-46f8-985e-2d8f3a95ed4b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""eccc8847-1486-49fb-b442-e1b0d66555b1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""053853c8-3df8-4c1c-95f3-e72ce8cc5d4a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f6a65dc4-4309-47b9-88f4-595edfbaa098"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""28430306-508e-46e6-8109-95035133f7b6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7f9762f5-77c1-4f27-93ac-2efb169650b7"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f817b234-a513-4848-9537-cf005ff89bf3"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NavigationLeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
            // Global
            m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
            m_Global_Quit = m_Global.FindAction("Quit", throwIfNotFound: true);
            m_Global_ToggleCursor = m_Global.FindAction("ToggleCursor", throwIfNotFound: true);
            // FpsArcade
            m_FpsArcade = asset.FindActionMap("FpsArcade", throwIfNotFound: true);
            m_FpsArcade_Movement = m_FpsArcade.FindAction("Movement", throwIfNotFound: true);
            m_FpsArcade_Look = m_FpsArcade.FindAction("Look", throwIfNotFound: true);
            m_FpsArcade_CameraHeight = m_FpsArcade.FindAction("CameraHeight", throwIfNotFound: true);
            m_FpsArcade_Interact = m_FpsArcade.FindAction("Interact", throwIfNotFound: true);
            m_FpsArcade_Sprint = m_FpsArcade.FindAction("Sprint", throwIfNotFound: true);
            m_FpsArcade_Jump = m_FpsArcade.FindAction("Jump", throwIfNotFound: true);
            m_FpsArcade_ToggleMoveCab = m_FpsArcade.FindAction("ToggleMoveCab", throwIfNotFound: true);
            // FpsMoveCab
            m_FpsMoveCab = asset.FindActionMap("FpsMoveCab", throwIfNotFound: true);
            m_FpsMoveCab_MoveModel = m_FpsMoveCab.FindAction("MoveModel", throwIfNotFound: true);
            m_FpsMoveCab_RotateModel = m_FpsMoveCab.FindAction("RotateModel", throwIfNotFound: true);
            m_FpsMoveCab_GrabReleaseModel = m_FpsMoveCab.FindAction("GrabReleaseModel", throwIfNotFound: true);
            m_FpsMoveCab_AddModel = m_FpsMoveCab.FindAction("AddModel", throwIfNotFound: true);
            // CylArcade
            m_CylArcade = asset.FindActionMap("CylArcade", throwIfNotFound: true);
            m_CylArcade_NavigationUpDown = m_CylArcade.FindAction("NavigationUpDown", throwIfNotFound: true);
            m_CylArcade_NavigationLeftRight = m_CylArcade.FindAction("NavigationLeftRight", throwIfNotFound: true);
            m_CylArcade_Look = m_CylArcade.FindAction("Look", throwIfNotFound: true);
            m_CylArcade_CameraHeight = m_CylArcade.FindAction("CameraHeight", throwIfNotFound: true);
            m_CylArcade_Interact = m_CylArcade.FindAction("Interact", throwIfNotFound: true);
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

        // Global
        private readonly InputActionMap m_Global;
        private IGlobalActions m_GlobalActionsCallbackInterface;
        private readonly InputAction m_Global_Quit;
        private readonly InputAction m_Global_ToggleCursor;
        public struct GlobalActions
        {
            private @InputSettingsActions m_Wrapper;
            public GlobalActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Quit => m_Wrapper.m_Global_Quit;
            public InputAction @ToggleCursor => m_Wrapper.m_Global_ToggleCursor;
            public InputActionMap Get() { return m_Wrapper.m_Global; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
            public void SetCallbacks(IGlobalActions instance)
            {
                if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
                {
                    @Quit.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                    @Quit.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                    @Quit.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnQuit;
                    @ToggleCursor.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleCursor;
                    @ToggleCursor.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleCursor;
                    @ToggleCursor.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleCursor;
                }
                m_Wrapper.m_GlobalActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Quit.started += instance.OnQuit;
                    @Quit.performed += instance.OnQuit;
                    @Quit.canceled += instance.OnQuit;
                    @ToggleCursor.started += instance.OnToggleCursor;
                    @ToggleCursor.performed += instance.OnToggleCursor;
                    @ToggleCursor.canceled += instance.OnToggleCursor;
                }
            }
        }
        public GlobalActions @Global => new GlobalActions(this);

        // FpsArcade
        private readonly InputActionMap m_FpsArcade;
        private IFpsArcadeActions m_FpsArcadeActionsCallbackInterface;
        private readonly InputAction m_FpsArcade_Movement;
        private readonly InputAction m_FpsArcade_Look;
        private readonly InputAction m_FpsArcade_CameraHeight;
        private readonly InputAction m_FpsArcade_Interact;
        private readonly InputAction m_FpsArcade_Sprint;
        private readonly InputAction m_FpsArcade_Jump;
        private readonly InputAction m_FpsArcade_ToggleMoveCab;
        public struct FpsArcadeActions
        {
            private @InputSettingsActions m_Wrapper;
            public FpsArcadeActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_FpsArcade_Movement;
            public InputAction @Look => m_Wrapper.m_FpsArcade_Look;
            public InputAction @CameraHeight => m_Wrapper.m_FpsArcade_CameraHeight;
            public InputAction @Interact => m_Wrapper.m_FpsArcade_Interact;
            public InputAction @Sprint => m_Wrapper.m_FpsArcade_Sprint;
            public InputAction @Jump => m_Wrapper.m_FpsArcade_Jump;
            public InputAction @ToggleMoveCab => m_Wrapper.m_FpsArcade_ToggleMoveCab;
            public InputActionMap Get() { return m_Wrapper.m_FpsArcade; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FpsArcadeActions set) { return set.Get(); }
            public void SetCallbacks(IFpsArcadeActions instance)
            {
                if (m_Wrapper.m_FpsArcadeActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnMovement;
                    @Look.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnLook;
                    @CameraHeight.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnCameraHeight;
                    @CameraHeight.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnCameraHeight;
                    @CameraHeight.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnCameraHeight;
                    @Interact.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnInteract;
                    @Sprint.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnSprint;
                    @Sprint.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnSprint;
                    @Sprint.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnSprint;
                    @Jump.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnJump;
                    @ToggleMoveCab.started -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnToggleMoveCab;
                    @ToggleMoveCab.performed -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnToggleMoveCab;
                    @ToggleMoveCab.canceled -= m_Wrapper.m_FpsArcadeActionsCallbackInterface.OnToggleMoveCab;
                }
                m_Wrapper.m_FpsArcadeActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @CameraHeight.started += instance.OnCameraHeight;
                    @CameraHeight.performed += instance.OnCameraHeight;
                    @CameraHeight.canceled += instance.OnCameraHeight;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @Sprint.started += instance.OnSprint;
                    @Sprint.performed += instance.OnSprint;
                    @Sprint.canceled += instance.OnSprint;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @ToggleMoveCab.started += instance.OnToggleMoveCab;
                    @ToggleMoveCab.performed += instance.OnToggleMoveCab;
                    @ToggleMoveCab.canceled += instance.OnToggleMoveCab;
                }
            }
        }
        public FpsArcadeActions @FpsArcade => new FpsArcadeActions(this);

        // FpsMoveCab
        private readonly InputActionMap m_FpsMoveCab;
        private IFpsMoveCabActions m_FpsMoveCabActionsCallbackInterface;
        private readonly InputAction m_FpsMoveCab_MoveModel;
        private readonly InputAction m_FpsMoveCab_RotateModel;
        private readonly InputAction m_FpsMoveCab_GrabReleaseModel;
        private readonly InputAction m_FpsMoveCab_AddModel;
        public struct FpsMoveCabActions
        {
            private @InputSettingsActions m_Wrapper;
            public FpsMoveCabActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveModel => m_Wrapper.m_FpsMoveCab_MoveModel;
            public InputAction @RotateModel => m_Wrapper.m_FpsMoveCab_RotateModel;
            public InputAction @GrabReleaseModel => m_Wrapper.m_FpsMoveCab_GrabReleaseModel;
            public InputAction @AddModel => m_Wrapper.m_FpsMoveCab_AddModel;
            public InputActionMap Get() { return m_Wrapper.m_FpsMoveCab; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FpsMoveCabActions set) { return set.Get(); }
            public void SetCallbacks(IFpsMoveCabActions instance)
            {
                if (m_Wrapper.m_FpsMoveCabActionsCallbackInterface != null)
                {
                    @MoveModel.started -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnMoveModel;
                    @MoveModel.performed -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnMoveModel;
                    @MoveModel.canceled -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnMoveModel;
                    @RotateModel.started -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnRotateModel;
                    @RotateModel.performed -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnRotateModel;
                    @RotateModel.canceled -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnRotateModel;
                    @GrabReleaseModel.started -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @GrabReleaseModel.performed -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @GrabReleaseModel.canceled -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @AddModel.started -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnAddModel;
                    @AddModel.performed -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnAddModel;
                    @AddModel.canceled -= m_Wrapper.m_FpsMoveCabActionsCallbackInterface.OnAddModel;
                }
                m_Wrapper.m_FpsMoveCabActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveModel.started += instance.OnMoveModel;
                    @MoveModel.performed += instance.OnMoveModel;
                    @MoveModel.canceled += instance.OnMoveModel;
                    @RotateModel.started += instance.OnRotateModel;
                    @RotateModel.performed += instance.OnRotateModel;
                    @RotateModel.canceled += instance.OnRotateModel;
                    @GrabReleaseModel.started += instance.OnGrabReleaseModel;
                    @GrabReleaseModel.performed += instance.OnGrabReleaseModel;
                    @GrabReleaseModel.canceled += instance.OnGrabReleaseModel;
                    @AddModel.started += instance.OnAddModel;
                    @AddModel.performed += instance.OnAddModel;
                    @AddModel.canceled += instance.OnAddModel;
                }
            }
        }
        public FpsMoveCabActions @FpsMoveCab => new FpsMoveCabActions(this);

        // CylArcade
        private readonly InputActionMap m_CylArcade;
        private ICylArcadeActions m_CylArcadeActionsCallbackInterface;
        private readonly InputAction m_CylArcade_NavigationUpDown;
        private readonly InputAction m_CylArcade_NavigationLeftRight;
        private readonly InputAction m_CylArcade_Look;
        private readonly InputAction m_CylArcade_CameraHeight;
        private readonly InputAction m_CylArcade_Interact;
        public struct CylArcadeActions
        {
            private @InputSettingsActions m_Wrapper;
            public CylArcadeActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @NavigationUpDown => m_Wrapper.m_CylArcade_NavigationUpDown;
            public InputAction @NavigationLeftRight => m_Wrapper.m_CylArcade_NavigationLeftRight;
            public InputAction @Look => m_Wrapper.m_CylArcade_Look;
            public InputAction @CameraHeight => m_Wrapper.m_CylArcade_CameraHeight;
            public InputAction @Interact => m_Wrapper.m_CylArcade_Interact;
            public InputActionMap Get() { return m_Wrapper.m_CylArcade; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CylArcadeActions set) { return set.Get(); }
            public void SetCallbacks(ICylArcadeActions instance)
            {
                if (m_Wrapper.m_CylArcadeActionsCallbackInterface != null)
                {
                    @NavigationUpDown.started -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationUpDown;
                    @NavigationUpDown.performed -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationUpDown;
                    @NavigationUpDown.canceled -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationUpDown;
                    @NavigationLeftRight.started -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationLeftRight;
                    @NavigationLeftRight.performed -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationLeftRight;
                    @NavigationLeftRight.canceled -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnNavigationLeftRight;
                    @Look.started -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnLook;
                    @CameraHeight.started -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnCameraHeight;
                    @CameraHeight.performed -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnCameraHeight;
                    @CameraHeight.canceled -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnCameraHeight;
                    @Interact.started -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_CylArcadeActionsCallbackInterface.OnInteract;
                }
                m_Wrapper.m_CylArcadeActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @NavigationUpDown.started += instance.OnNavigationUpDown;
                    @NavigationUpDown.performed += instance.OnNavigationUpDown;
                    @NavigationUpDown.canceled += instance.OnNavigationUpDown;
                    @NavigationLeftRight.started += instance.OnNavigationLeftRight;
                    @NavigationLeftRight.performed += instance.OnNavigationLeftRight;
                    @NavigationLeftRight.canceled += instance.OnNavigationLeftRight;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @CameraHeight.started += instance.OnCameraHeight;
                    @CameraHeight.performed += instance.OnCameraHeight;
                    @CameraHeight.canceled += instance.OnCameraHeight;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                }
            }
        }
        public CylArcadeActions @CylArcade => new CylArcadeActions(this);
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
        public interface IGlobalActions
        {
            void OnQuit(InputAction.CallbackContext context);
            void OnToggleCursor(InputAction.CallbackContext context);
        }
        public interface IFpsArcadeActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnCameraHeight(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnToggleMoveCab(InputAction.CallbackContext context);
        }
        public interface IFpsMoveCabActions
        {
            void OnMoveModel(InputAction.CallbackContext context);
            void OnRotateModel(InputAction.CallbackContext context);
            void OnGrabReleaseModel(InputAction.CallbackContext context);
            void OnAddModel(InputAction.CallbackContext context);
        }
        public interface ICylArcadeActions
        {
            void OnNavigationUpDown(InputAction.CallbackContext context);
            void OnNavigationLeftRight(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnCameraHeight(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
        }
    }
}