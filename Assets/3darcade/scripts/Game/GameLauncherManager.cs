using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Arcade
{
    public class GameLauncherManager
    {
        // Uses Statics from ArcadeManager for arcadeState, arcadeConfiguration, menuConfiguration and emulatorsConfigurationList, applicationPath.

        private static ModelSetup selectedModelSetup;
        private static ModelSetup gameModelSetup;
        private static readonly List<ModelLibretroGameSetup> modelLibretroGameSetupsList = new List<ModelLibretroGameSetup>();

        public static void LoadGame(ModelSetup game, ModelSetup selected)
        {
            TriggerManager.SendEvent(Event.GameStarted);

            selectedModelSetup = selected; // The model not its dummy node
            gameModelSetup = game; // Different from selected when game was choosen in CylMenu or FpsMenu.

            if (gameModelSetup == null)
            {
                UnityEngine.Debug.Log("Launcher: model setup not found");
                return;
            }
            else
            {
                UnityEngine.Debug.Log("Launcher: launching " + gameModelSetup.id);
            }
            if (selectedModelSetup == null)
            {
                UnityEngine.Debug.Log("Launcher: selectedmodel setup not found");
                return;
            }
            else
            {
                UnityEngine.Debug.Log("Launcher: launchingselected " + selectedModelSetup.id);
            }
            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == gameModelSetup.emulator).ToList();
            if (emulatorConfiguration.Count < 1)
            {
                UnityEngine.Debug.Log("Launcher: no emulator configuration found");
                return;
            }
            selectedModelSetup.isPlaying = true;
            GameLauncherMethod launcherMethod = GameLauncherMethod.None;
            launcherMethod = gameModelSetup.gameLauncherMethod;
            if (launcherMethod == GameLauncherMethod.None)
            {
                _ = Enum.TryParse(emulatorConfiguration[0].emulator.gameLauncherMethod, true, out launcherMethod);
            }
            if (launcherMethod == GameLauncherMethod.None)
            {
                _ = Enum.TryParse(selectedModelSetup == gameModelSetup ? ArcadeManager.arcadeConfiguration.gameLauncherMethod : ArcadeManager.menuConfiguration.gameLauncherMethod, true, out launcherMethod);
            }
            switch (launcherMethod)
            {
                case GameLauncherMethod.Internal:
                    LoadInternalGame(emulatorConfiguration[0].emulator);
                    break;
                case GameLauncherMethod.External:
                    LoadExternalGame(emulatorConfiguration[0].emulator);
                    break;
                case GameLauncherMethod.URL:
                    LoadURLGame(emulatorConfiguration[0].emulator);
                    break;
                default:
                    UnityEngine.Debug.Log("Launcher: no launcher method found");
                    break;
            }

            void LoadExternalGame(EmulatorProperties emulator)
            {
                // Application.OpenURL("mameios://");
                // TODO: Damn this works, but ugly! You know better!
                string path = ArcadeManager.applicationPath;
                string executable = emulator.executable.Trim();
                string extension = emulator.extension != null ? emulator.extension.Trim() : "";
                // TODO: Implement commandline arguments
                //string arguments = emulator.arguments.Trim();
                string options = emulator.options.TrimStart();
                string emulatorPath = FileManager.CorrectFilePath(emulator.emulatorPath);
                if (emulatorPath != "")
                {
                    emulatorPath = path + emulatorPath;
                }
                string gamePath = FileManager.CorrectFilePath(emulator.gamePath);
                if (gamePath != "")
                {
                    gamePath = path + gamePath;
                }
                string workingDir = FileManager.CorrectFilePath(emulator.workingDir);
                if (workingDir != "")
                {
                    workingDir = path + workingDir;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = emulatorPath + executable,
                    Arguments = options + gamePath + gameModelSetup.id.Trim() + extension // space char after -File
                };
                if (workingDir != "")
                { startInfo.WorkingDirectory = workingDir; }
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                bool started = false;
                Process process = new Process
                {
                    StartInfo = startInfo
                };
                started = process.Start();
                try
                {
                    int procId = process.Id;
                }
                catch (InvalidOperationException)
                {
                    started = false;
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("Error: " + ex.Message + "  " + ex.InnerException);
                    started = false;
                }
                if (started)
                {
                    UnityEngine.Debug.Log("game started");
                    ArcadeManager.arcadeState = ArcadeStates.Game;
                }
            }

            void LoadInternalGame(EmulatorProperties emulator)
            {
                UnityEngine.Debug.Log("Launcher: start internal launcher " + selectedModelSetup.transform.gameObject.name);

                GameObject screen = selectedModelSetup.transform.GetChild(0).GetChild(1).gameObject;
                UnityEngine.Debug.Log("selected obj " + selectedModelSetup.name);
                ModelVideoSetup video = screen.GetComponent<ModelVideoSetup>();
                video.ReleasePlayer(false);
                UnityEngine.Debug.Log("selected video " + video);

                RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
                if (arcadeRigidbodyFirstPersonController != null)
                {
                    arcadeRigidbodyFirstPersonController.pause = true;
                }

                // TODO: Damn this works, but ugly!
                string path = ArcadeManager.applicationPath;
                string executable = emulator.libretroCore.Trim();
                string extension = emulator.extension != null ? emulator.extension.Trim() : "";
                string arguments = emulator.arguments.Trim();
                string options = emulator.options.TrimStart();
                string emulatorPath = FileManager.CorrectFilePath(emulator.emulatorPath);
                if (emulatorPath != "")
                {
                    emulatorPath = Path.Combine(path + emulatorPath);
                }
                string gamePath = FileManager.CorrectFilePath(emulator.gamePath);
                if (gamePath != "")
                {
                    gamePath = Path.Combine(path + gamePath);
                }
                string workingDir = FileManager.CorrectFilePath(emulator.workingDir);
                if (workingDir != "")
                {
                    workingDir = Path.Combine(path + workingDir);
                }

                //UnityEngine.Debug.Log("is path" + gamePath );
                //UnityEngine.Debug.Log("is game" + gameModelSetup.id);
                ModelLibretroGameSetup libretroGame;
                libretroGame = selectedModelSetup.gameObject.GetComponent<ModelLibretroGameSetup>();
                if (libretroGame == null)
                {
                    libretroGame = selectedModelSetup.gameObject.AddComponent<ModelLibretroGameSetup>();
                    modelLibretroGameSetupsList.Add(libretroGame);
                    libretroGame.StartGame(executable, gamePath, gameModelSetup.id);
                }
                else
                {
                    libretroGame.ResumeGame();
                }
                // Increase 1 to support more then one game at once
                if (modelLibretroGameSetupsList.Count > 1)
                {
                    libretroGame = modelLibretroGameSetupsList[0];
                    if (libretroGame != null)
                    {
                        libretroGame.StopGame();
                        modelLibretroGameSetupsList.RemoveAt(0);
                    }
                }
                ArcadeManager.arcadeState = ArcadeStates.Game;
            }

            void LoadURLGame(EmulatorProperties emulator)
            {
                Application.OpenURL(emulator.executable.Trim() + emulator.options.Trim() + gameModelSetup.id.Trim() + emulator.arguments.Trim());
                ArcadeManager.arcadeState = ArcadeStates.Game;
            }
        }

        public static void UnLoadGame()
        {
            TriggerManager.SendEvent(Event.GameEnded);

            if (selectedModelSetup == null)
            {
                UnityEngine.Debug.Log("Launcher:  model setup not found");
                if (selectedModelSetup == null)
                {
                    UnityEngine.Debug.Log("Launcher: selected model setup not found");
                }
                return;
            }
            List<EmulatorConfiguration> emulatorConfiguration = ArcadeManager.emulatorsConfigurationList.Where(x => x.emulator.id == gameModelSetup.emulator).ToList();
            if (emulatorConfiguration.Count < 1)
            {
                UnityEngine.Debug.Log("Launcher: no emulator configuration found");
                return;
            }
            selectedModelSetup.isPlaying = false;
            GameLauncherMethod launcherMethod = GameLauncherMethod.None;
            launcherMethod = gameModelSetup.gameLauncherMethod;
            if (launcherMethod == GameLauncherMethod.None)
            {
                _ = Enum.TryParse(emulatorConfiguration[0].emulator.gameLauncherMethod, true, out launcherMethod);
            }
            if (launcherMethod == GameLauncherMethod.None)
            {
                _ = Enum.TryParse(selectedModelSetup == gameModelSetup ? ArcadeManager.arcadeConfiguration.gameLauncherMethod : ArcadeManager.menuConfiguration.gameLauncherMethod, true, out launcherMethod);
            }
            switch (launcherMethod)
            {
                case GameLauncherMethod.Internal:
                    UnLoadInternalGame();
                    break;
                case GameLauncherMethod.External:
                    UnLoadExternalGame();
                    break;
                case GameLauncherMethod.URL:
                    UnLoadExternalGame();
                    break;
                default:
                    UnityEngine.Debug.Log("Launcher: no launcher method found");
                    break;
            }
            void UnLoadExternalGame()
            {
                // Stop Game
                ArcadeManager.arcadeState = ArcadeStates.Running;
            }

            void UnLoadInternalGame()
            {
                // Stop Game
                ModelLibretroGameSetup libretroGame = selectedModelSetup.gameObject.GetComponent<ModelLibretroGameSetup>();
                if (libretroGame != null)
                {
                    libretroGame.PauseGame(false, true, true);
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ArcadeManager.arcadeState = ArcadeStates.Running;
                if (ArcadeManager.activeMenuType != ArcadeType.None || (ArcadeManager.activeMenuType == ArcadeType.None && ArcadeManager.activeArcadeType == ArcadeType.FpsArcade))
                {
                    UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController arcadeRigidbodyFirstPersonController = ArcadeManager.arcadeControls[ArcadeType.FpsArcade].GetComponent<RigidbodyFirstPersonController>();
                    if (arcadeRigidbodyFirstPersonController != null)
                    {
                        arcadeRigidbodyFirstPersonController.pause = false;
                    }
                }
            }
        }
    }
}