using System.Collections.Generic;
using UnityEngine;

namespace Arcade
{
    public class ModelTriggerSetup : MonoBehaviour
    {
        public Dictionary<Event, List<TriggerWrapper>> triggerEvents = new Dictionary<Event, List<TriggerWrapper>>();
        ModelSetup modelSetup;
        public bool savedSelected; // Public because we need it when we replace the model mesh.

        // Global Variables:
        // ArcadeManager.arcadeCameras
        // ArcadeManager.ArcadeState
        // ArcadeStateManager.selectedModel
        // ArcadeStateManager.selectedModelSetup

        void Start()
        {
            modelSetup = gameObject.transform.parent.gameObject.GetComponent<ModelSetup>();
            if (modelSetup == null)
            {
                print("error - no modelsetup found for " + gameObject.name);
            }
        }

        public void Setup(Dictionary<Event, List<TriggerWrapper>> triggerEvents)
        {
            this.triggerEvents = triggerEvents;
        }

        void Update()
        {
            if (savedSelected != modelSetup.isSelected)
            {
                if (modelSetup.isSelected == true)
                {
                    savedSelected = true;
                    NewEvent(Event.ModelSelected, null);
                }
                else
                {
                    savedSelected = false;
                    NewEvent(Event.ModelDeSelected, null);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (ArcadeManager.arcadeState == ArcadeStates.Running)
            {
                NewEvent(Event.ModelCollisionEnter, other.collider.gameObject);
            }
            Debug.Log(other.collider.name);
        }

        private void OnCollisionExit(Collision other)
        {
            if (ArcadeManager.arcadeState == ArcadeStates.Running)
            {
                NewEvent(Event.ModelCollisionExit, other.collider.gameObject);
            }
            Debug.Log(other.collider.name);
        }

        public void NewEvent(Event triggerEvent, GameObject triggerSource)
        {
            //print("New event: " + triggerEvent.ToString());
            if (!triggerEvents.ContainsKey(triggerEvent))
            {
                return;
            }

            List<TriggerWrapper> triggerWrappers = triggerEvents[triggerEvent];
            if (triggerWrappers.Count < 1)
            {
                return;
            }

            ActionSetup(triggerWrappers, triggerSource);
        }

        private void ActionSetup(List<TriggerWrapper> triggerWrappers, GameObject triggerSource)
        {
            foreach (TriggerWrapper triggerWrapper in triggerWrappers)
            {
                if (System.Enum.TryParse(triggerWrapper.trigger.triggerEvent, true, out Event triggerEvent))
                {
                    if (triggerEvent == Event.ModelCollisionEnter || triggerEvent == Event.ModelCollisionExit)
                    {
                        bool ok = false;
                        foreach (GameObject sourceObject in triggerWrapper.triggerSourceGameObjects)
                        {
                            if (sourceObject == triggerSource)
                            { ok = true; }
                        }
                        if (ok == false)
                        { continue; }
                    }
                }
                foreach (GameObject targetObject in triggerWrapper.triggerTargetGameObjects)
                {
                    // Check if there is a target for this action
                    if (targetObject == null)
                    { continue; }
                    GameObject targetObjectParent = targetObject.transform.parent.gameObject; // Every model has a dummy parent!
                    if (targetObjectParent == null)
                    { continue; }

                    if (System.Enum.TryParse(triggerWrapper.trigger.triggerAction, true, out Action triggerAction))
                    {
                        switch (triggerAction)
                        {
                            case Action.PlayAudio:
                                ActionPlayAudio(triggerWrapper.trigger.audioProperties, targetObject);
                                break;
                            case Action.StopAudio:
                                ActionStopAudio(triggerWrapper.trigger.audioProperties, targetObject);
                                break;
                            case Action.PauseAudio:
                                ActionPauseAudio(triggerWrapper.trigger.audioProperties, targetObject);
                                break;
                            case Action.PlayAnimation:
                                ActionPlayAnimation(triggerWrapper.trigger.animationProperties, targetObject);
                                break;
                            case Action.PauseAnimation:
                                ActionPauseAnimation(triggerWrapper.trigger.animationProperties, targetObject);
                                break;
                            case Action.StopAnimation:
                                ActionStopAnimation(triggerWrapper.trigger.animationProperties, targetObject);
                                break;
                            case Action.SetTransform:
                                ActionSetTransform(triggerWrapper.trigger.triggerTansformProperties, targetObject, targetObjectParent);
                                break;
                            case Action.SetActiveEnabled:
                                ActionSetActiveEnabled(targetObjectParent);
                                break;
                            case Action.SetActiveDisabled:
                                ActionSetActiveDisabled(targetObjectParent);
                                break;
                            case Action.GetArtworkFromSelectedModel:
                                ActionGetArtworkFromSelectedModel(targetObjectParent);
                                break;
                            default:
                                print("error");
                                break;
                        }
                    }
                }
            }
        }

        private void ActionPlayAudio(List<AudioProperties> audioPropertiesList, GameObject targetObject)
        {
            if (audioPropertiesList.Count < 1)
            {
                return;
            }

            AudioProperties audioProperties = audioPropertiesList[0];
            if (audioProperties.name.Trim() != "")
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                ModelAudioSetup modelAudioSetup = targetObject.GetComponent<ModelAudioSetup>();
                if (modelAudioSetup == null)
                {
                    modelAudioSetup = targetObject.AddComponent<ModelAudioSetup>();
                }
                modelAudioSetup.Setup(audioProperties);
            }
        }

        private void ActionPauseAudio(List<AudioProperties> audioPropertiesList, GameObject targetObject)
        {
            if (audioPropertiesList.Count < 1)
            {
                return;
            }

            AudioProperties audioProperties = audioPropertiesList[0];
            if (audioProperties.name.Trim() != "")
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                ModelAudioSetup modelAudioSetup = targetObject.GetComponent<ModelAudioSetup>();
                if (modelAudioSetup != null)
                {
                    modelAudioSetup.JukeboxEnabled = false;
                }
                AudioSource audioSource = targetObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Pause();
                }
            }
        }

        private void ActionStopAudio(List<AudioProperties> audioPropertiesList, GameObject targetObject)
        {
            if (audioPropertiesList.Count < 1)
            {
                return;
            }

            AudioProperties audioProperties = audioPropertiesList[0];
            if (audioProperties.name.Trim() != "")
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                ModelAudioSetup modelAudioSetup = targetObject.GetComponent<ModelAudioSetup>();
                if (modelAudioSetup != null)
                {
                    modelAudioSetup.JukeboxEnabled = false;
                    modelAudioSetup.audioProperties = null;
                }
                AudioSource audioSource = targetObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }

        private void ActionPlayAnimation(List<AnimationProperties> animationProperties, GameObject targetObject)
        {
            foreach (AnimationProperties animationItem in animationProperties)
            {
                Animation animationComponent = targetObject.GetComponent<Animation>();
                if (animationComponent == null)
                {
                    animationComponent = targetObject.GetComponentInChildren<Animation>();
                }
                bool ok = false;
                if (animationComponent != null)
                {
                    string animationString = animationItem.name;
                    foreach (AnimationState clip in animationComponent)
                    {
                        if (clip.name == animationString)
                        { ok = true; }
                    }
                    if (ok == false)
                    {
                        return;
                    }

                    animationComponent.enabled = true;
                    //print("startanimation " + animationItem.name + " on " + targetObject.name);
                    animationComponent[animationItem.name].layer = animationItem.layer;
                    _ = animationComponent.Play(animationItem.name);
                    animationComponent[animationItem.name].speed = animationItem.speed;
                    animationComponent[animationItem.name].wrapMode = animationItem.loop ? WrapMode.Loop : WrapMode.Once;
                }
            }
        }

        private void ActionPauseAnimation(List<AnimationProperties> animationProperties, GameObject targetObject)
        {
            foreach (AnimationProperties animationItem in animationProperties)
            {
                Animation animationComponent = targetObject.GetComponent<Animation>();
                bool ok = false;
                if (animationComponent != null)
                {
                    string animationString = animationItem.name;
                    foreach (AnimationState clip in animationComponent)
                    {
                        if (clip.name == animationString)
                        { ok = true; }
                    }
                    if (ok == false)
                    {
                        return;
                    }

                    animationComponent[animationItem.name].speed = 0;
                }
            }
        }

        private void ActionStopAnimation(List<AnimationProperties> animationProperties, GameObject targetObject)
        {
            foreach (AnimationProperties animationItem in animationProperties)
            {
                Animation animationComponent = targetObject.GetComponent<Animation>();
                bool ok = false;
                if (animationComponent != null)
                {
                    string animationString = animationItem.name;
                    foreach (AnimationState clip in animationComponent)
                    {
                        if (clip.name == animationString)
                        { ok = true; }
                    }
                    if (ok == false)
                    {
                        return;
                    }

                    animationComponent.Stop(animationString);
                }
            }
        }

        private void ActionSetTransform(List<TriggerTransformProperties> triggerTransformProperties, GameObject targetObject, GameObject targetObjectParent)
        {
            foreach (TriggerTransformProperties triggerTransformItem in triggerTransformProperties)
            {
                if (triggerTransformItem.setParent)
                {


                    // TODO: Implement setting parent to a triggerID.
                    string parent = triggerTransformItem.parent.Trim().ToLower();
                    switch (parent)
                    {
                        case "":
                            //
                            break;
                        case "arcadecamera":
                            targetObjectParent.transform.SetParent(ArcadeManager.arcadeCameras[ArcadeType.FpsArcade].transform);
                            targetObjectParent.layer = LayerMask.NameToLayer("ArcadeCamera");
                            targetObjectParent.RunOnChildrenRecursive(child => child.layer = LayerMask.NameToLayer("ArcadeCamera"));
                            break;
                        case "menucamera":
                            targetObjectParent.transform.SetParent(ArcadeManager.arcadeCameras[ArcadeType.CylMenu].transform);
                            targetObjectParent.layer = LayerMask.NameToLayer("MenuCamera");
                            targetObjectParent.RunOnChildrenRecursive(child => child.layer = LayerMask.NameToLayer("MenuCamera"));
                            break;
                        case "selectedmodel":
                            if (ArcadeStateManager.selectedModel != null)
                            {
                                targetObjectParent.transform.SetParent(ArcadeStateManager.selectedModel.transform);
                                if (ArcadeStateManager.selectedModel.layer == LayerMask.NameToLayer("Menu"))
                                {
                                    targetObjectParent.layer = LayerMask.NameToLayer("MenuCamera");
                                    targetObjectParent.RunOnChildrenRecursive(child => child.layer = LayerMask.NameToLayer("MenuCamera"));
                                }
                                else
                                {
                                    targetObjectParent.layer = LayerMask.NameToLayer("ArcadeCamera");
                                    targetObjectParent.RunOnChildrenRecursive(child => child.layer = LayerMask.NameToLayer("ArcadeCamera"));
                                }
                            }
                            break;
                        default:
                            print("error, not set to parent " + parent);
                            break;
                    }
                }
                if (triggerTransformItem.setIsKinematic)
                {
                    Rigidbody rigidbody = targetObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = triggerTransformItem.isKinematic;
                    }
                }
                if (triggerTransformItem.setPosition)
                {
                    targetObjectParent.transform.localPosition = triggerTransformItem.position;
                }
                if (triggerTransformItem.setRotation)
                {
                    targetObjectParent.transform.localRotation = triggerTransformItem.rotation;
                }
                if (triggerTransformItem.setScale)
                {
                    targetObjectParent.transform.localScale = triggerTransformItem.scale;
                }
                if (triggerTransformItem.setIsActive)
                {
                    targetObjectParent.SetActive(triggerTransformItem.isActive);
                }
            }
        }

        private void ActionSetActiveEnabled(GameObject targetObjectParent)
        {
            targetObjectParent.SetActive(true);
        }

        private void ActionSetActiveDisabled(GameObject targetObjectParent)
        {
            targetObjectParent.SetActive(false);
        }

        private void ActionGetArtworkFromSelectedModel(GameObject targetObjectParent)
        {
            if (ArcadeStateManager.selectedModelSetup == null || !(Application.isPlaying) || !(targetObjectParent.activeSelf))
            {
                return;
            }

            ModelProperties modelProperties = ArcadeStateManager.selectedModelSetup.GetModelProperties();
            ModelSharedProperties modelSharedProperties = ArcadeStateManager.selectedModelSetup.modelSharedProperties;
            ModelSetup objModelSetup = targetObjectParent.GetComponent<ModelSetup>();
            if (objModelSetup != null)
            {
                targetObjectParent.tag = "gamemodel";
                objModelSetup.Setup(modelProperties, modelSharedProperties);

            }
        }
    }
}
