// WorkingFilesList
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using Ninject;
using WorkingFilesList.Interface;
using WorkingFilesList.Ioc;

namespace WorkingFilesList.Service.Locator
{
    /// <summary>
    /// Service locator for view model related objects, for use when references
    /// are required within XAML
    /// </summary>
    public class ViewModelService
    {
        private static ICommands _commands;
        private static IDocumentMetadataManager _documentMetadataManager;
        private static IOptionsLists _optionsLists;
        private static IUserPreferences _userPreferences;

        public static ICommands Commands =>
            _commands ??
            (_commands = NinjectContainer.Kernel.Get<ICommands>());

        public static IDocumentMetadataManager DocumentMetadataManager =>
            _documentMetadataManager ??
            (_documentMetadataManager = NinjectContainer.Kernel.Get<IDocumentMetadataManager>());

        public static IOptionsLists OptionsLists =>
            _optionsLists ??
            (_optionsLists = NinjectContainer.Kernel.Get<IOptionsLists>());

        public static IUserPreferences UserPreferences =>
            _userPreferences ??
            (_userPreferences = NinjectContainer.Kernel.Get<IUserPreferences>());
    }
}
