// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service.Locator
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
        public static IUserPreferences UserPreferences { get; private set; }

        public ViewModelService()
        {
            // Parameterless constructor needed for instance creation from XAML
        }

        public ViewModelService(
            IAboutPanelService aboutPanelService,
            ICommands commands,
            IDocumentMetadataManager documentMetadataManager,
            IOptionsLists optionsLists,
            IUserPreferences userPreferences)
        {
            AboutPanelService = aboutPanelService;
            Commands = commands;
            DocumentMetadataManager = documentMetadataManager;
            OptionsLists = optionsLists;
            UserPreferences = userPreferences;
        }
    }
}
