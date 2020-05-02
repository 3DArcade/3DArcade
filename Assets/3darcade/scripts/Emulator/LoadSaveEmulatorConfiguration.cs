using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Arcade
{
    public class LoadSaveEmulatorConfiguration
    {
        // This saves Emulators Configuration from the loaded gameobjects, not from ArcadeManager's static property. Only useful if we support making changes in the editor!
        public void SaveEmulators()
        {
            List<EmulatorProperties> emulatorsList = GetListOfEmulatorProperties();
            if (emulatorsList == null)
            { return; }
            List<EmulatorConfiguration> tempEmulatorsConfigurationList = new List<EmulatorConfiguration>();
            foreach (EmulatorProperties emulatorProperties in emulatorsList)
            {
                EmulatorConfiguration cfg = GetEmulatorConfiguration(emulatorProperties.id);
                if (cfg == null)
                {
                    cfg = new EmulatorConfiguration();
                }
                cfg.emulator = emulatorProperties;
                tempEmulatorsConfigurationList.Add(cfg);
            }
            ArcadeManager.emulatorsConfigurationList = tempEmulatorsConfigurationList;
            SaveEmulatorsConfigurationList();

            List<EmulatorProperties> GetListOfEmulatorProperties()
            {
                GameObject objParent = GameObject.Find("Emulators"); // TODO: Ugly fix this!
                List<EmulatorProperties> list = new List<EmulatorProperties>();
                if (objParent == null)
                {
                    Debug.Log("no items found");
                    return null;
                }
                EmulatorSetup[] obj = objParent.GetComponents<EmulatorSetup>();
                foreach (EmulatorSetup item in obj)
                {
                    EmulatorProperties emulator = item.GetEmulatorSetup();
                    list.Add(emulator);
                }
                return list;
            }
        }

        public void SaveEmulatorConfiguration(EmulatorConfiguration emulatorConfiguration)
        {
            string filePath = ArcadeManager.applicationPath + ArcadeManager.emulatorsConfigurationPath;
            string fileName = emulatorConfiguration.emulator.id + ".json";
            Debug.Log("Save Emulator Configuration " + filePath + fileName);
            FileManager.SaveJSONData(emulatorConfiguration, filePath, fileName);
        }

        public void SaveEmulatorsConfigurationList()
        {
            string[] files = FileManager.FilesFromDirectory(ArcadeManager.applicationPath + ArcadeManager.emulatorsConfigurationPath, "*.json");
            foreach (string file in files)
            {
                File.Delete(file);
            }

            foreach (EmulatorConfiguration emulatorConfiguration in ArcadeManager.emulatorsConfigurationList)
            {
                SaveEmulatorConfiguration(emulatorConfiguration);
            }
        }

        // Load Emulator configurations into gameobject nodes. TODO: Only in editor mode?
        public void LoadEmulators()
        {
            GameObject emulators = GameObject.Find("Emulators"); // TODO: Ugly fix this!
            if (emulators == null)
            {
                Debug.Log("No Emulators node found, create one...");
            }

            if (LoadEmulatorsConfigurationList())
            {
                SetListOfEmulatorProperties(ArcadeManager.emulatorsConfigurationList);
            }

            void SetListOfEmulatorProperties(List<EmulatorConfiguration> list)
            {
                EmulatorSetup[] obj = emulators.GetComponents<EmulatorSetup>();
                // Destroy the current list of emulators in the editor...
                foreach (EmulatorSetup item in obj)
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(item);
                    }
                    else
                    {
                        Object.DestroyImmediate(item);
                    }
                }
                // Set the new list of emulators
                foreach (EmulatorConfiguration item in list)
                {
                    EmulatorSetup component = emulators.AddComponent<EmulatorSetup>();
                    component.SetEmulatorSetup(item.emulator);
                }
            }
        }

        public bool LoadEmulatorsConfigurationList()
        {
            string filePath = ArcadeManager.applicationPath + ArcadeManager.emulatorsConfigurationPath;
            ArcadeManager.emulatorsConfigurationList = new List<EmulatorConfiguration>();
            string[] files = FileManager.FilesFromDirectory(filePath, "*.json", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                EmulatorConfiguration cfg = FileManager.LoadJSONData<EmulatorConfiguration>(file);
                ArcadeManager.emulatorsConfigurationList.Add(cfg);
            }

            return files.Length > 0;
        }

        public EmulatorConfiguration GetEmulatorConfiguration(string emulatorID)
        {
            if (ArcadeManager.emulatorsConfigurationList.Count < 1)
            {
                _ = LoadEmulatorsConfigurationList();
            }
            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == emulatorID).ToList();
            if (emulatorConfiguration.Count > 0)
            {
                return emulatorConfiguration[0];
            }
            return null;
        }

        public void DeleteEmulatorConfiguration(EmulatorConfiguration emulatorConfiguration)
        {
            string filePath = ArcadeManager.applicationPath + ArcadeManager.emulatorsConfigurationPath;
            Debug.Log("Delete! " + emulatorConfiguration.emulator.id);
            string fileName = emulatorConfiguration.emulator.id + ".json";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".json.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/mame/";
            fileName = emulatorConfiguration.emulator.id + ".xml";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".xml.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/hyperspin/";
            fileName = emulatorConfiguration.emulator.id + ".xml";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".xml.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            filePath = ArcadeManager.applicationPath + "/3darcade~/Configuration/MasterGamelists/atf/";
            fileName = emulatorConfiguration.emulator.id + ".atf";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".atf.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".ini.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
            fileName = emulatorConfiguration.emulator.id + ".dat.meta";
            _ = FileManager.DeleteFile(filePath, fileName);
        }
    }
}
