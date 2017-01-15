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

using Microsoft.VisualStudio.Settings;
using Moq;
using System;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.Test.TestingInfrastructure
{
    /// <summary>
    /// Use when testing resolution of IoC bindings for interfaces.
    /// <see cref="SettingsStoreService"/> contains logic that will throw
    /// exceptions when using mocks.
    /// </summary>
    internal class SettingsStoreServiceStub : ISettingsStoreService
    {
        public SettingsStoreContainer GetWritableSettingsStore()
        {
            var container = new SettingsStoreContainer(
                Mock.Of<IDisposable>(),
                Mock.Of<WritableSettingsStore>());

            return container;
        }
    }
}
