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

namespace Arcade_r
{
    public sealed class ExternalAppController
    {
        public event System.Action<OSUtils.ProcessStartedData, LauncherConfiguration, ContentConfiguration> OnAppStarted;
        public event System.Action<OSUtils.ProcessExitedData, LauncherConfiguration, ContentConfiguration> OnAppExited;

        private readonly OSUtils.ProcessLauncher _processLauncher;

        public ExternalAppController()
        {
            _processLauncher = new OSUtils.ProcessLauncher();
        }

        public bool StartContent(LauncherConfiguration launcher, ContentConfiguration content, bool persistent = false)
        {
            if (content == null)
            {
                UnityEngine.Debug.LogError("[ExternalGameLauncher.StartContent] content is null.");
                return false;
            }

            if (launcher == null)
            {
                UnityEngine.Debug.LogError("[ExternalGameLauncher.StartGame] launcher is null.");
                return false;
            }

            string extension = string.Empty;
            if (launcher.SupportedExtensions != null)
            {
                foreach (string supportedExtension in launcher.SupportedExtensions)
                {
                    if (FileSystem.FileExists($"{launcher.ContentDirectory}/{content.Id}.{supportedExtension}"))
                    {
                        extension = supportedExtension;
                        break;
                    }
                }
            }

            OSUtils.ProcessCommand command = new OSUtils.ProcessCommand
            {
                Name                = launcher.DescriptiveName,
                Id                  = launcher.Id,
                Path                = System.IO.Path.Combine(launcher.Directory, launcher.Executable),
                WorkingDirectory    = launcher.WorkingDirectory,
                Extension           = extension,
                CommandLine         = launcher.Arguments,
                CommandId           = content.Id
            };

            _processLauncher.StartProcess(command, persistent,
                (OSUtils.ProcessStartedData processStartedData) =>
                {
                    OnAppStarted?.Invoke(processStartedData, launcher, content);
                },
                (OSUtils.ProcessExitedData processExitedData) =>
                {
                    OnAppExited?.Invoke(processExitedData, launcher, content);
                });

            return true;
        }

        public void StopCurrent()
        {
            _processLauncher.StopCurrentProcess();
        }

        public void StopAll()
        {
            _processLauncher.StopAllProcesses();
        }
    }
}
