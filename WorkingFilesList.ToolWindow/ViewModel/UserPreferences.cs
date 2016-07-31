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

using System.Collections.Generic;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class UserPreferences : PropertyChangedNotifier, IUserPreferences
    {
        private readonly IStoredSettingsRepository _storedSettingsRepository;

        private int _pathSegmentCount;
        private ISortOption _selectedDocumentSortOption;
        private ISortOption _selectedProjectSortOption;

        /// <summary>
        /// The number of path segments to display, a path segment being either
        /// a single file or directory name that makes up the full name of a file
        /// </summary>
        public int PathSegmentCount
        {
            get
            {
                return _pathSegmentCount;
            }

            set
            {
                if (_pathSegmentCount != value)
                {
                    _pathSegmentCount = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetPathSegmentCount(
                        _pathSegmentCount);
                }
            }
        }

        public ISortOption SelectedDocumentSortOption
        {
            get
            {
                return _selectedDocumentSortOption;
            }

            set
            {
                if (_selectedDocumentSortOption != value)
                {
                    _selectedDocumentSortOption = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetSelectedDocumentSortOptionName(
                        _selectedDocumentSortOption?.DisplayName);
                }
            }
        }

        public ISortOption SelectedProjectSortOption
        {
            get
            {
                return _selectedProjectSortOption;
            }

            set
            {
                if (_selectedProjectSortOption != value)
                {
                    _selectedProjectSortOption = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetSelectedProjectSortOptionName(
                        _selectedProjectSortOption?.DisplayName);
                }
            }
        }

        public UserPreferences(
            IStoredSettingsRepository storedSettingsRepository,
            IList<ISortOption> sortOptions)
        {
            _storedSettingsRepository = storedSettingsRepository;

            _pathSegmentCount = _storedSettingsRepository.GetPathSegmentCount();

            var documentSortOptionName = _storedSettingsRepository
                .GetSelectedDocumentSortOptionName();

            _selectedDocumentSortOption = sortOptions
                .Single(s => s.DisplayName == documentSortOptionName);

            var projectSortOptionName = _storedSettingsRepository
                .GetSelectedProjectSortOptionName();

            _selectedProjectSortOption = sortOptions
                .Single(s => s.DisplayName == projectSortOptionName);
        }
    }
}
