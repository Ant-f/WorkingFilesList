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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.OptionsDialoguePage.ViewModel.Command;

namespace WorkingFilesList.OptionsDialoguePage.Test.ViewModel.Command
{
    public class ResetSettingsTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new ResetSettings(
                Mock.Of<IStoredSettingsRepository>(),
                Mock.Of<IUserPreferencesModelRepository>());

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteCallsRepositoryResetAndReloadsModel()
        {
            // Arrange

            var resetCalled = false;
            var loadIntoCalledAfterReset = false;
            var model = Mock.Of<IUserPreferencesModel>();

            var settingsRepositoryMock = new Mock<IStoredSettingsRepository>();

            settingsRepositoryMock
                .Setup(s => s.Reset())
                .Callback(() => { resetCalled = true; });

            var preferencesRepositoryMock = new Mock<IUserPreferencesModelRepository>();

            // IStoredSettingsRepository.Reset should be called before the model
            // is reloaded using IUserPreferencesModelRepository.LoadInto
            // This is tested by assigning the value of resetCalled into
            // loadIntoCalledAfterReset

            preferencesRepositoryMock
                .Setup(p => p.LoadInto(model))
                .Callback<IUserPreferencesModel>(u =>
                    loadIntoCalledAfterReset = resetCalled);

            var command = new ResetSettings(
                settingsRepositoryMock.Object,
                preferencesRepositoryMock.Object);

            // Act

            command.Execute(model);

            // Assert

            settingsRepositoryMock.Verify(s => s.Reset());
            preferencesRepositoryMock.Verify(p => p.LoadInto(model));

            Assert.IsTrue(loadIntoCalledAfterReset);
        }

        [Test]
        public void RepositoryResetAndModelReloadIsNotPerformedIfParameterIsNotUserPreferencesModel()
        {
            // Arrange

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();
            var preferencesRepository = Mock.Of<IUserPreferencesModelRepository>();

            var command = new ResetSettings(
                settingsRepository,
                preferencesRepository);

            // Act

            command.Execute(new object());

            // Assert

            Mock.Get(settingsRepository)
                .Verify(s => s.Reset(),
                    Times.Never);

            Mock.Get(preferencesRepository)
                .Verify(p => p.LoadInto(It.IsAny<IUserPreferencesModel>()),
                    Times.Never);
        }
    }
}
