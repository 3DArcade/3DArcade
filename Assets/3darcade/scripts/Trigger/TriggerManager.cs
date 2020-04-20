using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Arcade
{
    public class TriggerManager : MonoBehaviour
    {
        public static Dictionary<Event, List<ModelTriggerSetup>> triggersActive = new Dictionary<Event, List<ModelTriggerSetup>>();
        public static List<ModelSetup> modelSetups = new List<ModelSetup>();

        // Global Variables:
        // ArcadeManager.arcadeControls, Select which player controller to use (arcade or menu).

        public static void UnLoadTriggers()
        {
            triggersActive = new Dictionary<Event, List<ModelTriggerSetup>>();
            modelSetups = new List<ModelSetup>();
        }

        public static void Setup(ArcadeType arcadeType)
        {
            string type = arcadeType == ArcadeType.FpsArcade || arcadeType == ArcadeType.CylArcade || arcadeType == ArcadeType.None ? "Arcade/" : "Menu/";
            List<ModelSetup> tempModelSetups = new List<ModelSetup>();


            GameObject obj = GameObject.Find(type + "ArcadeModels");
            if (obj != null)
            {
                AddFromParent(obj);
            }
            obj = GameObject.Find(type + "GameModels");
            if (obj != null)
            {
                AddFromParent(obj);
            }
            obj = GameObject.Find(type + "PropModels");
            if (obj != null)
            {
                AddFromParent(obj);
            }

            foreach (ModelSetup modelSetup in tempModelSetups)
            {
                Add(modelSetup, arcadeType);
            }
            modelSetups.AddRange(tempModelSetups);
            modelSetups.RemoveAll(x => x == null);

            void AddFromParent(GameObject objParent)
            {
                var transform = objParent.transform;
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    ModelSetup modelSetup = objParent.transform.GetChild(i).gameObject.GetComponent<ModelSetup>();
                    if (modelSetup != null) { tempModelSetups.Add(modelSetup); }
                }
            }
        }

        

        public static bool Add(ModelSetup modelSetup, ArcadeType arcadeType)
        {
            Dictionary<Event, List<TriggerWrapper>> triggerEvents = new Dictionary<Event, List<TriggerWrapper>>();
            ModelTriggerSetup modelTriggerSetup = null;
            GameObject obj = modelSetup.gameObject.transform.GetChild(0).gameObject; // Get the model from its dummy parent
            if (obj == null) { return false; }

            if (modelSetup.triggers.Count > 0)
            {
                modelTriggerSetup = obj.GetComponent<ModelTriggerSetup>();

                if (modelTriggerSetup == null)
                {
                    modelTriggerSetup = obj.AddComponent<ModelTriggerSetup>();
                }
                else
                {
                    // We already setup the triggers for this model!
                    return true;
                }

                foreach (Trigger trigger in modelSetup.triggers)
                {
                    TriggerWrapper triggerWrapper = new TriggerWrapper();
                    triggerWrapper.triggerSourceGameObjects = new List<GameObject>();
                    triggerWrapper.triggerTargetGameObjects = new List<GameObject>();
                    triggerWrapper.trigger = trigger;
                    foreach (string source in trigger.triggerSource)
                    {
                        if (source.Trim().ToLower() == "self")
                        {
                            triggerWrapper.triggerSourceGameObjects.Add(obj);
                        }
                        else if (source.Trim().ToLower() == "camera")
                        {
                            triggerWrapper.triggerSourceGameObjects.Add(ArcadeManager.arcadeControls[arcadeType]);
                        }
                        else
                        {
                            List<ModelSetup> modelSetupsWithTriggerSources = modelSetups.Where(x => x.triggerIDs.Contains(source)).ToList();
                            if (modelSetupsWithTriggerSources.Count > 0)
                            {
                                foreach (ModelSetup modelSetupWithTriggerSource in modelSetupsWithTriggerSources)
                                {
                                    if (modelSetupWithTriggerSource == null) { continue; }
                                    triggerWrapper.triggerSourceGameObjects.Add(modelSetupWithTriggerSource.gameObject.transform.GetChild(0).gameObject);
                                }
                            }
                            else
                            {
                                triggerWrapper.triggerSourceGameObjects.Add(obj);
                            }

                        }
                    }
                    if (triggerWrapper.triggerSourceGameObjects.Count < 1) { triggerWrapper.triggerSourceGameObjects.Add(ArcadeManager.arcadeControls[arcadeType]); }
                    foreach (string target in trigger.triggerTarget)
                    {
                        if (target.Trim().ToLower() == "self")
                        {
                            triggerWrapper.triggerTargetGameObjects.Add(obj);
                        }
                        else if (target.Trim().ToLower() == "camera")
                        {
                            triggerWrapper.triggerTargetGameObjects.Add(ArcadeManager.arcadeControls[arcadeType]);
                        }
                        else
                        {
                            List<ModelSetup> modelSetupsWithTriggerTargets = modelSetups.Where(x => x.triggerIDs.Contains(target)).ToList();
                            if (modelSetupsWithTriggerTargets.Count > 0)
                            {
                                foreach (ModelSetup modelSetupWithTriggerTarget in modelSetupsWithTriggerTargets)
                                {
                                    if (modelSetupWithTriggerTarget == null) { continue; }
                                    triggerWrapper.triggerTargetGameObjects.Add(modelSetupWithTriggerTarget.gameObject.transform.GetChild(0).gameObject);
                                }
                            }
                            else
                            {
                                triggerWrapper.triggerTargetGameObjects.Add(obj);
                            }

                        }
                    }
                    if (triggerWrapper.triggerTargetGameObjects.Count < 1) { triggerWrapper.triggerTargetGameObjects.Add(obj); }
                    Event triggerEvent;
                    if (System.Enum.TryParse(trigger.triggerEvent, true, out triggerEvent))
                    {

                        if (!triggerEvents.ContainsKey(triggerEvent)) { triggerEvents[triggerEvent] = new List<TriggerWrapper>(); }
                        if (!triggersActive.ContainsKey(triggerEvent)) { triggersActive[triggerEvent] = new List<ModelTriggerSetup>(); }
                        triggerEvents[triggerEvent].Add(triggerWrapper);
                        if (modelTriggerSetup != null && !triggersActive[triggerEvent].Contains(modelTriggerSetup))
                        {
                            triggersActive[triggerEvent].Add(modelTriggerSetup);
                        }
                    }
                }
                modelTriggerSetup.Setup(triggerEvents);
            }
            return true;
        }

        public static void SendEvent(Event triggerEvent)
        {
            if (triggersActive.ContainsKey(triggerEvent))
            {
                foreach (ModelTriggerSetup modelTriggerSetup in triggersActive[triggerEvent])
                {
                    modelTriggerSetup.NewEvent(triggerEvent, null);
                }
            }
        }

        private void Update()
        {
            
        }
    }
}

