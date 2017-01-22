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

using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.OptionsDialoguePage.Interface;
using WorkingFilesList.OptionsDialoguePage.ViewModel;
using WorkingFilesList.OptionsDialoguePage.ViewModel.Command;

namespace WorkingFilesList.OptionsDialoguePage.Test.ViewModel
{
    [TestFixture]
    public class DialoguePageCommandsTests
    {
        [Test]
        public void AllCommandsAreAssigned()
        {
            // Arrange

            var navigate = new Navigate(Mock.Of<IProcessStarter>());

            var resetSettings = new ResetSettings(
                Mock.Of<IStoredSettingsRepository>(),
                Mock.Of<IUserPreferencesModelRepository>());

            var commandList = new List<ICommand>
            {
                navigate,
                resetSettings
            };

            // Act

            var commands = new DialoguePageCommands(commandList);

            // Assert

            Assert.That(commands.Navigate, Is.EqualTo(navigate));
            Assert.That(commands.ResetSettings, Is.EqualTo(resetSettings));
        }
    }
}
