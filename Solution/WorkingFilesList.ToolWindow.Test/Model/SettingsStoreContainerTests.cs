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
using NUnit.Framework;
using System;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Test.Model
{
    [TestFixture]
    public class SettingsStoreContainerTests
    {
        [Test]
        public void DisposeDisposesServiceProvider()
        {
            // Arrange

            var serviceProvider = Mock.Of<IDisposable>();
            var writableSettingsStore = Mock.Of<WritableSettingsStore>();

            // Act

            using (new SettingsStoreContainer(serviceProvider, writableSettingsStore))
            {
                // Intentionally empty
            }

            // Assert

            Mock.Get(serviceProvider).Verify(s => s.Dispose());
        }
    }
}
