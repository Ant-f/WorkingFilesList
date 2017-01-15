// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using EnvDTE80;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using Microsoft.VisualStudio.OLE.Interop;

namespace WorkingFilesList.ToolWindow.Service
{
    public class SettingsStoreService : ISettingsStoreService
    {
        private readonly DTE2 _dte2;

        public SettingsStoreService(DTE2 dte2)
        {
            _dte2 = dte2;
        }

        public SettingsStoreContainer GetWritableSettingsStore()
        {
            var serviceProvider = new ServiceProvider((IServiceProvider) _dte2);
            var settingsManager = new ShellSettingsManager(serviceProvider);

            var settingsStore = settingsManager.GetWritableSettingsStore(
                SettingsScope.UserSettings);

            var container = new SettingsStoreContainer(serviceProvider, settingsStore);
            return container;
        }
    }
}
