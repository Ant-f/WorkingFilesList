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

using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.Core.Service.Locator
{
    /// <summary>
    /// Service locator for view model related objects, for use when references
    /// are required within XAML
    /// </summary>
    public class ViewModelService
    {
        public static IAboutPanelService AboutPanelService { get; private set; }
        public static ICommands Commands { get; private set; }
        public static IDocumentMetadataManager DocumentMetadataManager { get; private set; }
        public static IOptionsLists OptionsLists { get; private set; }
        public static IOptionsPageControlFactory OptionsPageControlFactory { get; private set; }
        public static ISolutionEventsService SolutionEventsService { get; private set; }
        public static IUserPreferences UserPreferences { get; private set; }
        public static IUserPreferencesModelFactory UserPreferencesModelFactory { get; private set; }

        public ViewModelService()
        {
            // Parameterless constructor needed for instance creation from XAML
        }

        public ViewModelService(
            IAboutPanelService aboutPanelService,
            ICommands commands,
            IDocumentMetadataManager documentMetadataManager,
            IOptionsLists optionsLists,
            IOptionsPageControlFactory optionsPageControlFactory,
            ISolutionEventsService solutionEventsService,
            IUserPreferences userPreferences,
            IUserPreferencesModelFactory userPreferencesModelFactory)
        {
            AboutPanelService = aboutPanelService;
            Commands = commands;
            DocumentMetadataManager = documentMetadataManager;
            OptionsLists = optionsLists;
            OptionsPageControlFactory = optionsPageControlFactory;
            SolutionEventsService = solutionEventsService;
            UserPreferences = userPreferences;
            UserPreferencesModelFactory = userPreferencesModelFactory;
        }
    }
}
