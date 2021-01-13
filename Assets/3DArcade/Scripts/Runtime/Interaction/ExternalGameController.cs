/* MIT License

 * Copyright (c) 2020 Skurdt
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. */

namespace Arcade
{
    public sealed class ExternalGameController
    {
        public event System.Action<OSUtils.ProcessStartedData, EmulatorConfiguration, string> OnAppStarted;
        public event System.Action<OSUtils.ProcessExitedData, EmulatorConfiguration, string> OnAppExited;

        private readonly OSUtils.ProcessLauncher _processLauncher;

        public ExternalGameController() => _processLauncher = new OSUtils.ProcessLauncher();

        public bool StartGame(EmulatorConfiguration emulator, string game, bool persistent = false)
        {
            if (string.IsNullOrEmpty(game))
            {
                UnityEngine.Debug.LogError("[ExternalGameController.StartGame] game is null or empty.");
                return false;
            }

            if (emulator == null)
            {
                UnityEngine.Debug.LogError("[ExternalGameController.StartGame] emulator is null.");
                return false;
            }

            string extension = string.Empty;
            if (emulator.SupportedExtensions != null)
            {
                foreach (string supportedExtension in emulator.SupportedExtensions)
                {
                    foreach (string gameDirectory in emulator.GamesDirectories)
                    {
                        if (FileSystem.FileExists($"{gameDirectory}/{game}.{supportedExtension}"))
                        {
                            extension = supportedExtension;
                            break;
                        }
                    }
                }
            }

            OSUtils.ProcessCommand command = new OSUtils.ProcessCommand
            {
                Name             = emulator.DescriptiveName,
                Id               = emulator.Id,
                Path             = System.IO.Path.Combine(emulator.Directory, emulator.Executable),
                WorkingDirectory = emulator.WorkingDirectory,
                Extension        = extension,
                CommandLine      = emulator.Arguments,
                CommandId        = game
            };

            _processLauncher.StartProcess(command,
                                          persistent,
                                          (OSUtils.ProcessStartedData processStartedData) => OnAppStarted?.Invoke(processStartedData, emulator, game),
                                          (OSUtils.ProcessExitedData processExitedData)   => OnAppExited?.Invoke(processExitedData, emulator, game));

            return true;
        }

        public void StopCurrent() => _processLauncher.StopCurrentProcess();

        public void StopAll() => _processLauncher.StopAllProcesses();
    }
}
