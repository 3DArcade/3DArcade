﻿using UnityEngine;
using UnityEditor;

namespace Arcade
{
    public class EmulatorSetup : MonoBehaviour
    {
        public delegate void ShowSelectMasterGamelistWindowDelegate(EmulatorSetup emulatorSetup);
        public static ShowSelectMasterGamelistWindowDelegate ShowSelectMasterGamelistWindow;

        public string descriptiveName;
        public string id;
        public string about;
        public GameLauncherMethod gameLauncherMethod = GameLauncherMethod.None;
        [ContextMenuItem("Select Executable + Fill Default Paths", "SelectExecutableAndFillDefaultPaths")]
        [ContextMenuItem("Select Executable", "SelectExecutable")]
        [ArcadeAttribute("Exe")]
        public string executable;
        private void SelectExecutableAndFillDefaultPaths()
        {
            string exe = FileManager.DialogGetFilePart("Select Emulator Executable", null, FileManager.FilePart.Name_Extension);
            if (exe != null)
            {
                executable = exe;
            }
        }
        private void SelectExecutable()
        {
            var exe = FileManager.DialogGetFilePart("Select Emulator Executable", null, FileManager.FilePart.Name_Extension);
            if (exe != null)
            {
                executable = exe;
            }
        }
        public string extension;
        [ContextMenuItem("Select libretroCore + Fill Default Paths", "SelectlibretroCoreAndFillDefaultPaths")]
        [ContextMenuItem("Select libretroCore", "SelectlibretroCore")]
        [ArcadeAttribute("Exe")]
        public string libretroCore;
        private void SelectlibretroCoreAndFillDefaultPaths()
        {
            string exe = FileManager.DialogGetFilePart("Select LibretroCore Executable", null, FileManager.FilePart.Name_Extension);
            if (exe != null)
            {
                libretroCore = exe;
            }
        }
        private void SelectlibretroCore()
        {
            string exe = FileManager.DialogGetFilePart("Select LibretroCore Executable", null, FileManager.FilePart.Name_Extension);
            if (exe != null)
            {
                libretroCore = exe;
            }
        }
        public string arguments;
        public string options;
        [ContextMenuItem("Select Folder Path", "EmulatorPathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string emulatorPath;
        private void EmulatorPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                emulatorPath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "GamePathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string gamePath;
        private void GamePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                gamePath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "WorkingDirGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string workingDir;
        private void WorkingDirGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                workingDir = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "MarqueePathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string marqueePath;
        private void MarqueePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                marqueePath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "ScreenPathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string screenPath;
        private void ScreenPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                screenPath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "ScreenVideoPathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string screenVideoPath;
        private void ScreenVideoPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                screenVideoPath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "GenericPathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string genericPath;
        private void GenericPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                genericPath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "TitlePathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string titlePath;
        private void TitlePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                titlePath = folder;
            }
        }
        [ContextMenuItem("Select Folder Path", "InfoPathGetFolderPath")]
        [ArcadeAttribute("Folder")]
        public string infoPath;
        private void InfoPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                infoPath = folder;
            }
        }
        public bool outputCommandLine;

#if UNITY_EDITOR
        [MenuItem("CONTEXT/EmulatorSetup/Save Emulator Configuration")]
        private static void SaveEmulatorConfigurationMenuOption(MenuCommand menuCommand)
        {
            var emulatorSetup = menuCommand.context as EmulatorSetup;
            EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(emulatorSetup.id);
            if (emulatorConfiguration == null)
            {
                emulatorConfiguration = new EmulatorConfiguration();
            }
            emulatorConfiguration.emulator = emulatorSetup.GetEmulatorSetup();
            ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
            //TODO: Change so that we update ArcadeManager.EmulatorsConfigurationList for save cfg instead of reloading all
            ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList();
        }
        [MenuItem("CONTEXT/EmulatorSetup/Select MasterGamelist")]
        private static void SelectMasterGamelistMenuOption(MenuCommand menuCommand)
        {
            var emulatorSetup = menuCommand.context as EmulatorSetup;
            var obj = emulatorSetup.transform.gameObject;
            if (ShowSelectMasterGamelistWindow != null) ShowSelectMasterGamelistWindow(emulatorSetup);
        }
        [MenuItem("CONTEXT/EmulatorSetup/Update MasterGamelist")]
        private static void UpdateMasterGamelistMenuOption(MenuCommand menuCommand)
        {
            var emulatorSetup = menuCommand.context as EmulatorSetup;
            EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(emulatorSetup.id);
            if (emulatorConfiguration != null)
            {
                emulatorConfiguration = UpdateMasterGamelists.UpdateMasterGamelistFromEmulatorConfiguration(emulatorConfiguration);
                ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
            }
        }
#endif

        public void SetEmulatorSetup(EmulatorProperties emulatorProperties)
        {
            descriptiveName = emulatorProperties.descriptiveName;
            id = emulatorProperties.id;
            about = emulatorProperties.about;
            System.Enum.TryParse(emulatorProperties.gameLauncherMethod, true, out gameLauncherMethod);
            executable = emulatorProperties.executable;
            extension = emulatorProperties.extension;
            libretroCore = emulatorProperties.libretroCore;
            arguments = emulatorProperties.arguments;
            options = emulatorProperties.options;
            emulatorPath = emulatorProperties.emulatorPath;
            gamePath = emulatorProperties.gamePath;
            workingDir = emulatorProperties.workingDir;
            marqueePath = emulatorProperties.marqueePath;
            screenPath = emulatorProperties.screenPath;
            screenVideoPath = emulatorProperties.screenVideoPath;
            genericPath = emulatorProperties.genericPath;
            titlePath = emulatorProperties.titlePath;
            infoPath = emulatorProperties.infoPath;
            outputCommandLine = emulatorProperties.outputCommandLine;
        }

        public EmulatorProperties GetEmulatorSetup()
        {
            var emulatorProperties = new EmulatorProperties();
            emulatorProperties.descriptiveName = descriptiveName;
            emulatorProperties.id = id;
            emulatorProperties.about = about;
            emulatorProperties.gameLauncherMethod = gameLauncherMethod.ToString();
            emulatorProperties.executable = executable;
            emulatorProperties.extension = extension;
            emulatorProperties.libretroCore = libretroCore;
            emulatorProperties.arguments = arguments;
            emulatorProperties.options = options;
            emulatorProperties.emulatorPath = emulatorPath;
            emulatorProperties.gamePath = gamePath;
            emulatorProperties.workingDir = workingDir;
            emulatorProperties.outputCommandLine = outputCommandLine;
            emulatorProperties.marqueePath = marqueePath;
            emulatorProperties.screenPath = screenPath;
            emulatorProperties.screenVideoPath = screenVideoPath;
            emulatorProperties.genericPath = genericPath;
            emulatorProperties.titlePath = titlePath;
            emulatorProperties.infoPath = infoPath;
            return emulatorProperties;
        }

        // Start is called before the first frame update
        //void Start()
        //{

        //}
    }
}