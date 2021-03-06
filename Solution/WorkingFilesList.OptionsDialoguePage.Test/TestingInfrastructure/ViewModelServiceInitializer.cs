﻿// Working Files List
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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.OptionsDialoguePage.Test.TestingInfrastructure
{
    internal class ViewModelServiceInitializer
    {
        public ViewModelService InitializeViewModelService(
            IAboutPanelService aboutPanelService = null,
            IDialoguePageCommands dialoguePageCommands = null,
            IDocumentMetadataManager documentMetadataManager = null,
            IOptionsLists optionsLists = null,
            IOptionsPageControlFactory optionsPageControlFactory = null,
            ISolutionEventsService solutionEventsService = null,
            IToolWindowCommands toolWindowCommands = null,
            IUserPreferences userPreferences = null,
            IUserPreferencesModelFactory userPreferencesModelFactory = null,
            IUserPreferencesModelRepository userPreferencesModelRepository = null)
        {
            var repository = new MockRepository(MockBehavior.Loose)
            {
                DefaultValue = DefaultValue.Mock
            };

            var service = new ViewModelService(
                aboutPanelService ?? Mock.Of<IAboutPanelService>(),
                dialoguePageCommands ?? Mock.Of<IDialoguePageCommands>(),
                documentMetadataManager ?? Mock.Of<IDocumentMetadataManager>(),
                optionsLists ?? repository.OneOf<IOptionsLists>(),
                optionsPageControlFactory ?? repository.OneOf<IOptionsPageControlFactory>(),
                solutionEventsService ?? Mock.Of<ISolutionEventsService>(),
                toolWindowCommands ?? Mock.Of<IToolWindowCommands>(),
                userPreferences ?? Mock.Of<IUserPreferences>(),
                userPreferencesModelFactory ?? Mock.Of<IUserPreferencesModelFactory>(),
                userPreferencesModelRepository ?? Mock.Of<IUserPreferencesModelRepository>());

            return service;
        }
    }
}
