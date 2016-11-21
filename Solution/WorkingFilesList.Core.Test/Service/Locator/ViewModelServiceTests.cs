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
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.Core.Test.Service.Locator
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)] // Uses static properties in ViewModelService
    public class ViewModelServiceTests
    {
        [Test]
        public void AllComponentsAreAssigned()
        {
            // Arrange

            var aboutPanelService = Mock.Of<IAboutPanelService>();
            var dialoguePageCommands = Mock.Of<IDialoguePageCommands>();
            var documentMetadataManager = Mock.Of<IDocumentMetadataManager>();
            var optionsLists = Mock.Of<IOptionsLists>();
            var optionsPageControlFactory = Mock.Of<IOptionsPageControlFactory>();
            var solutionEventsService = Mock.Of<ISolutionEventsService>();
            var toolWindowCommands = Mock.Of<IToolWindowCommands>();
            var userPreferences = Mock.Of<IUserPreferences>();
            var userPreferencesModelFactory = Mock.Of<IUserPreferencesModelFactory>();
            var userPreferencesModelRepository = Mock.Of<IUserPreferencesModelRepository>();

            // Act

            var service = new ViewModelService(
                aboutPanelService,
                dialoguePageCommands,
                documentMetadataManager,
                optionsLists,
                optionsPageControlFactory,
                solutionEventsService,
                toolWindowCommands,
                userPreferences,
                userPreferencesModelFactory,
                userPreferencesModelRepository);

            // Assert

            Assert.That(ViewModelService.AboutPanelService,
                Is.EqualTo(aboutPanelService));

            Assert.That(ViewModelService.DialoguePageCommands,
                Is.EqualTo(dialoguePageCommands));

            Assert.That(ViewModelService.DocumentMetadataManager,
                Is.EqualTo(documentMetadataManager));

            Assert.That(ViewModelService.OptionsLists,
                Is.EqualTo(optionsLists));

            Assert.That(ViewModelService.OptionsPageControlFactory,
                Is.EqualTo(optionsPageControlFactory));

            Assert.That(ViewModelService.SolutionEventsService,
                Is.EqualTo(solutionEventsService));

            Assert.That(ViewModelService.ToolWindowCommands,
                Is.EqualTo(toolWindowCommands));

            Assert.That(ViewModelService.UserPreferences,
                Is.EqualTo(userPreferences));

            Assert.That(ViewModelService.UserPreferencesModelFactory,
                Is.EqualTo(userPreferencesModelFactory));

            Assert.That(ViewModelService.UserPreferencesModelRepository,
                Is.EqualTo(userPreferencesModelRepository));
        }
    }
}
