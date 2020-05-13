// GENERATED AUTOMATICALLY FROM 'Assets/3darcade_r/InputSettingsActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Arcade_r
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
                },
                {
                    ""name"": ""TempModifierWorkaround"",
                    ""type"": ""Value"",
                    ""id"": ""f65850eb-e65d-4176-86f3-975111dcbae8"",
                    ""expectedControlType"": ""Digital"",
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
                    ""name"": ""SelectStart"",
                    ""id"": ""46ffc08c-0893-4e74-8ffd-3716e33c3a50"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""81321038-d1be-400f-8673-24c09f93c9fb"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""0dc26f33-0079-4289-9702-421873901f29"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
                    ""name"": ""SelectWest"",
                    ""id"": ""f59bdac8-5bee-4eaa-bc25-ecad57103947"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""2364bd83-7a0e-4ff0-b43d-05e38f8f2bdd"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""57be1adc-f7e6-49e9-90b5-c74a4885b358"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""509f5c27-c7e9-4e10-b690-9f5207bac653"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TempModifierWorkaround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FirstPerson"",
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
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""8de635a7-b295-43d9-b05f-89daa55015c8"",
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
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""SelectNorth"",
                    ""id"": ""ac70c759-5de6-48c7-8270-8b0b792ef980"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""c432e1c0-473b-47de-9451-1c073784a418"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""c7587667-a7f2-4037-af19-8cbf439f9d48"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleMoveCab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""FirstPersonMoveCab"",
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
            m_Global_TempModifierWorkaround = m_Global.FindAction("TempModifierWorkaround", throwIfNotFound: true);
            // FirstPerson
            m_FirstPerson = asset.FindActionMap("FirstPerson", throwIfNotFound: true);
            m_FirstPerson_Movement = m_FirstPerson.FindAction("Movement", throwIfNotFound: true);
            m_FirstPerson_Look = m_FirstPerson.FindAction("Look", throwIfNotFound: true);
            m_FirstPerson_Sprint = m_FirstPerson.FindAction("Sprint", throwIfNotFound: true);
            m_FirstPerson_Jump = m_FirstPerson.FindAction("Jump", throwIfNotFound: true);
            m_FirstPerson_Interact = m_FirstPerson.FindAction("Interact", throwIfNotFound: true);
            m_FirstPerson_ToggleMoveCab = m_FirstPerson.FindAction("ToggleMoveCab", throwIfNotFound: true);
            // FirstPersonMoveCab
            m_FirstPersonMoveCab = asset.FindActionMap("FirstPersonMoveCab", throwIfNotFound: true);
            m_FirstPersonMoveCab_MoveModel = m_FirstPersonMoveCab.FindAction("MoveModel", throwIfNotFound: true);
            m_FirstPersonMoveCab_RotateModel = m_FirstPersonMoveCab.FindAction("RotateModel", throwIfNotFound: true);
            m_FirstPersonMoveCab_GrabReleaseModel = m_FirstPersonMoveCab.FindAction("GrabReleaseModel", throwIfNotFound: true);
            m_FirstPersonMoveCab_AddModel = m_FirstPersonMoveCab.FindAction("AddModel", throwIfNotFound: true);
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
        private readonly InputAction m_Global_TempModifierWorkaround;
        public struct GlobalActions
        {
            private @InputSettingsActions m_Wrapper;
            public GlobalActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Quit => m_Wrapper.m_Global_Quit;
            public InputAction @ToggleCursor => m_Wrapper.m_Global_ToggleCursor;
            public InputAction @TempModifierWorkaround => m_Wrapper.m_Global_TempModifierWorkaround;
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
                    @TempModifierWorkaround.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnTempModifierWorkaround;
                    @TempModifierWorkaround.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnTempModifierWorkaround;
                    @TempModifierWorkaround.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnTempModifierWorkaround;
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
                    @TempModifierWorkaround.started += instance.OnTempModifierWorkaround;
                    @TempModifierWorkaround.performed += instance.OnTempModifierWorkaround;
                    @TempModifierWorkaround.canceled += instance.OnTempModifierWorkaround;
                }
            }
        }
        public GlobalActions @Global => new GlobalActions(this);

        // FirstPerson
        private readonly InputActionMap m_FirstPerson;
        private IFirstPersonActions m_FirstPersonActionsCallbackInterface;
        private readonly InputAction m_FirstPerson_Movement;
        private readonly InputAction m_FirstPerson_Look;
        private readonly InputAction m_FirstPerson_Sprint;
        private readonly InputAction m_FirstPerson_Jump;
        private readonly InputAction m_FirstPerson_Interact;
        private readonly InputAction m_FirstPerson_ToggleMoveCab;
        public struct FirstPersonActions
        {
            private @InputSettingsActions m_Wrapper;
            public FirstPersonActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_FirstPerson_Movement;
            public InputAction @Look => m_Wrapper.m_FirstPerson_Look;
            public InputAction @Sprint => m_Wrapper.m_FirstPerson_Sprint;
            public InputAction @Jump => m_Wrapper.m_FirstPerson_Jump;
            public InputAction @Interact => m_Wrapper.m_FirstPerson_Interact;
            public InputAction @ToggleMoveCab => m_Wrapper.m_FirstPerson_ToggleMoveCab;
            public InputActionMap Get() { return m_Wrapper.m_FirstPerson; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FirstPersonActions set) { return set.Get(); }
            public void SetCallbacks(IFirstPersonActions instance)
            {
                if (m_Wrapper.m_FirstPersonActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnMovement;
                    @Look.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnLook;
                    @Sprint.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnSprint;
                    @Sprint.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnSprint;
                    @Sprint.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnSprint;
                    @Jump.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnJump;
                    @Interact.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnInteract;
                    @ToggleMoveCab.started -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnToggleMoveCab;
                    @ToggleMoveCab.performed -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnToggleMoveCab;
                    @ToggleMoveCab.canceled -= m_Wrapper.m_FirstPersonActionsCallbackInterface.OnToggleMoveCab;
                }
                m_Wrapper.m_FirstPersonActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Sprint.started += instance.OnSprint;
                    @Sprint.performed += instance.OnSprint;
                    @Sprint.canceled += instance.OnSprint;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @ToggleMoveCab.started += instance.OnToggleMoveCab;
                    @ToggleMoveCab.performed += instance.OnToggleMoveCab;
                    @ToggleMoveCab.canceled += instance.OnToggleMoveCab;
                }
            }
        }
        public FirstPersonActions @FirstPerson => new FirstPersonActions(this);

        // FirstPersonMoveCab
        private readonly InputActionMap m_FirstPersonMoveCab;
        private IFirstPersonMoveCabActions m_FirstPersonMoveCabActionsCallbackInterface;
        private readonly InputAction m_FirstPersonMoveCab_MoveModel;
        private readonly InputAction m_FirstPersonMoveCab_RotateModel;
        private readonly InputAction m_FirstPersonMoveCab_GrabReleaseModel;
        private readonly InputAction m_FirstPersonMoveCab_AddModel;
        public struct FirstPersonMoveCabActions
        {
            private @InputSettingsActions m_Wrapper;
            public FirstPersonMoveCabActions(@InputSettingsActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveModel => m_Wrapper.m_FirstPersonMoveCab_MoveModel;
            public InputAction @RotateModel => m_Wrapper.m_FirstPersonMoveCab_RotateModel;
            public InputAction @GrabReleaseModel => m_Wrapper.m_FirstPersonMoveCab_GrabReleaseModel;
            public InputAction @AddModel => m_Wrapper.m_FirstPersonMoveCab_AddModel;
            public InputActionMap Get() { return m_Wrapper.m_FirstPersonMoveCab; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FirstPersonMoveCabActions set) { return set.Get(); }
            public void SetCallbacks(IFirstPersonMoveCabActions instance)
            {
                if (m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface != null)
                {
                    @MoveModel.started -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnMoveModel;
                    @MoveModel.performed -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnMoveModel;
                    @MoveModel.canceled -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnMoveModel;
                    @RotateModel.started -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnRotateModel;
                    @RotateModel.performed -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnRotateModel;
                    @RotateModel.canceled -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnRotateModel;
                    @GrabReleaseModel.started -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @GrabReleaseModel.performed -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @GrabReleaseModel.canceled -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnGrabReleaseModel;
                    @AddModel.started -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnAddModel;
                    @AddModel.performed -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnAddModel;
                    @AddModel.canceled -= m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface.OnAddModel;
                }
                m_Wrapper.m_FirstPersonMoveCabActionsCallbackInterface = instance;
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
        public FirstPersonMoveCabActions @FirstPersonMoveCab => new FirstPersonMoveCabActions(this);
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
            void OnTempModifierWorkaround(InputAction.CallbackContext context);
        }
        public interface IFirstPersonActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnToggleMoveCab(InputAction.CallbackContext context);
        }
        public interface IFirstPersonMoveCabActions
        {
            void OnMoveModel(InputAction.CallbackContext context);
            void OnRotateModel(InputAction.CallbackContext context);
            void OnGrabReleaseModel(InputAction.CallbackContext context);
            void OnAddModel(InputAction.CallbackContext context);
        }
    }
}
