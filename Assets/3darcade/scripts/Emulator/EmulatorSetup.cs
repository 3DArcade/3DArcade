using UnityEngine;
using UnityEditor;

namespace Arcade
{
    public class EmulatorSetup : MonoBehaviour
    {
        public string descriptiveName;
        public string id;
        public string about;
        [Arcade("Exe"), ContextMenuItem("Select Executable", nameof(SelectExecutable)), ContextMenuItem("Select Executable & Fill Default Paths", nameof(SelectExecutableAndFillDefaultPaths))]
        public string executable;
        public string extension;
        [Arcade("Exe"), ContextMenuItem("Select Libretro Core", nameof(SelectlibretroCore)), ContextMenuItem("Select Libretro Core & Fill Default Paths", nameof(SelectlibretroCoreAndFillDefaultPaths))]
        public string libretroCore;
        public string arguments;
        public string options;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(EmulatorPathGetFolderPath))]
        public string emulatorPath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(GamePathGetFolderPath))]
        public string gamePath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(WorkingDirGetFolderPath))]
        public string workingDir;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(MarqueePathGetFolderPath))]
        public string marqueePath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(ScreenPathGetFolderPath))]
        public string screenPath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(ScreenVideoPathGetFolderPath))]
        public string screenVideoPath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(GenericPathGetFolderPath))]
        public string genericPath;
        [ContextMenuItem("Select Folder Path", nameof(TitlePathGetFolderPath))]
        [Arcade("Folder")]
        public string titlePath;
        [Arcade("Folder"), ContextMenuItem("Select Folder Path", nameof(InfoPathGetFolderPath))]
        public string infoPath;
        public GameLauncherMethod gameLauncherMethod = GameLauncherMethod.None;
        public bool outputCommandLine;

        public void SetEmulatorSetup(EmulatorProperties emulatorProperties)
        {
            descriptiveName   = emulatorProperties.descriptiveName;
            id                = emulatorProperties.id;
            about             = emulatorProperties.about;
            _ = System.Enum.TryParse(emulatorProperties.gameLauncherMethod, true, out gameLauncherMethod);
            executable        = emulatorProperties.executable;
            extension         = emulatorProperties.extension;
            libretroCore      = emulatorProperties.libretroCore;
            arguments         = emulatorProperties.arguments;
            options           = emulatorProperties.options;
            emulatorPath      = emulatorProperties.emulatorPath;
            gamePath          = emulatorProperties.gamePath;
            workingDir        = emulatorProperties.workingDir;
            marqueePath       = emulatorProperties.marqueePath;
            screenPath        = emulatorProperties.screenPath;
            screenVideoPath   = emulatorProperties.screenVideoPath;
            genericPath       = emulatorProperties.genericPath;
            titlePath         = emulatorProperties.titlePath;
            infoPath          = emulatorProperties.infoPath;
            outputCommandLine = emulatorProperties.outputCommandLine;
        }

        public EmulatorProperties GetEmulatorSetup()
        {
            return new EmulatorProperties
            {
                descriptiveName    = descriptiveName,
                id                 = id,
                about              = about,
                gameLauncherMethod = gameLauncherMethod.ToString(),
                executable         = executable,
                extension          = extension,
                libretroCore       = libretroCore,
                arguments          = arguments,
                options            = options,
                emulatorPath       = emulatorPath,
                gamePath           = gamePath,
                workingDir         = workingDir,
                outputCommandLine  = outputCommandLine,
                marqueePath        = marqueePath,
                screenPath         = screenPath,
                screenVideoPath    = screenVideoPath,
                genericPath        = genericPath,
                titlePath          = titlePath,
                infoPath           = infoPath
            };
        }

        #region UnityMenus
        public delegate void ShowSelectMasterGamelistWindowDelegate(EmulatorSetup emulatorSetup);
        public static ShowSelectMasterGamelistWindowDelegate ShowSelectMasterGamelistWindow;

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
            string exe = FileManager.DialogGetFilePart("Select Emulator Executable", null, FileManager.FilePart.Name_Extension);
            if (exe != null)
            {
                executable = exe;
            }
        }

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

        private void EmulatorPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                emulatorPath = folder;
            }
        }

        private void GamePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                gamePath = folder;
            }
        }

        private void WorkingDirGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                workingDir = folder;
            }
        }

        private void MarqueePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                marqueePath = folder;
            }
        }

        private void ScreenPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                screenPath = folder;
            }
        }

        private void ScreenVideoPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                screenVideoPath = folder;
            }
        }

        private void GenericPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                genericPath = folder;
            }
        }

        private void TitlePathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                titlePath = folder;
            }
        }

        private void InfoPathGetFolderPath()
        {
            string folder = FileManager.DialogGetFolderPath(true);
            if (folder != null)
            {
                infoPath = folder;
            }
        }
        #endregion

#if UNITY_EDITOR
        [MenuItem("CONTEXT/EmulatorSetup/Save Emulator Configuration")]
        private static void SaveEmulatorConfigurationMenuOption(MenuCommand menuCommand)
        {
            EmulatorSetup emulatorSetup = menuCommand.context as EmulatorSetup;
            EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(emulatorSetup.id);
            if (emulatorConfiguration == null)
            {
                emulatorConfiguration = new EmulatorConfiguration();
            }
            emulatorConfiguration.emulator = emulatorSetup.GetEmulatorSetup();
            ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
            //TODO: Change so that we update ArcadeManager.EmulatorsConfigurationList for save cfg instead of reloading all
            _ = ArcadeManager.loadSaveEmulatorConfiguration.LoadEmulatorsConfigurationList();
        }

        [MenuItem("CONTEXT/EmulatorSetup/Select MasterGamelist")]
        private static void SelectMasterGamelistMenuOption(MenuCommand menuCommand)
        {
            EmulatorSetup emulatorSetup = menuCommand.context as EmulatorSetup;
            GameObject obj = emulatorSetup.transform.gameObject;
            ShowSelectMasterGamelistWindow?.Invoke(emulatorSetup);
        }

        [MenuItem("CONTEXT/EmulatorSetup/Update MasterGamelist")]
        private static void UpdateMasterGamelistMenuOption(MenuCommand menuCommand)
        {
            EmulatorSetup emulatorSetup = menuCommand.context as EmulatorSetup;
            EmulatorConfiguration emulatorConfiguration = ArcadeManager.loadSaveEmulatorConfiguration.GetEmulatorConfiguration(emulatorSetup.id);
            if (emulatorConfiguration != null)
            {
                emulatorConfiguration = UpdateMasterGamelists.UpdateMasterGamelistFromEmulatorConfiguration(emulatorConfiguration);
                ArcadeManager.loadSaveEmulatorConfiguration.SaveEmulatorConfiguration(emulatorConfiguration);
            }
        }
#endif
    }
}
