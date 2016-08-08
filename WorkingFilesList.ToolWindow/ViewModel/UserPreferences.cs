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
using System.ComponentModel;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class UserPreferences : PropertyChangedNotifier, IUserPreferences
    {
        private readonly IStoredSettingsRepository _storedSettingsRepository;

        private bool _assignProjectColours;
        private bool _groupByProject;
        private bool _showRecentUsage;
        private int _pathSegmentCount;
        private ISortOption _selectedDocumentSortOption;
        private ISortOption _selectedProjectSortOption;

        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should be assigned a colour associated with its
        /// <see cref="DocumentMetadataInfo.ProjectUniqueName"/>
        /// </summary>
        public bool AssignProjectColours
        {
            get
            {
                return _assignProjectColours;
            }

            set
            {
                if (_assignProjectColours != value)
                {
                    _assignProjectColours = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetAssignProjectColours(
                        _assignProjectColours);
                }
            }
        }

        /// <summary>
        /// Indicates whether a <see cref="GroupDescription"/> should be added
        /// to views of <see cref="DocumentMetadata"/> collections to group
        /// projects together
        /// </summary>
        public bool GroupByProject
        {
            get
            {
                return _groupByProject;
            }

            set
            {
                if (_groupByProject != value)
                {
                    _groupByProject = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetGroupByProject(
                        _groupByProject);
                }
            }
        }

        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should show the order of its historical usage reletive to the
        /// other entries on that list
        /// </summary>
        public bool ShowRecentUsage
        {
            get
            {
                return _showRecentUsage;
            }

            set
            {
                if (_showRecentUsage != value)
                {
                    _showRecentUsage = value;
                    OnPropertyChanged();

                    _storedSettingsRepository.SetShowRecentUsage(
                        _showRecentUsage);
                }
            }
        }

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

                    var typeString = _selectedDocumentSortOption?.ToString();
                    _storedSettingsRepository.SetSelectedDocumentSortType(typeString);
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

                    var typeString = _selectedProjectSortOption?.ToString();
                    _storedSettingsRepository.SetSelectedProjectSortType(typeString);
                }
            }
        }

        public UserPreferences(
            IStoredSettingsRepository storedSettingsRepository,
            IList<ISortOption> sortOptions)
        {
            _storedSettingsRepository = storedSettingsRepository;

            _assignProjectColours = _storedSettingsRepository.GetAssignProjectColours();
            _groupByProject = _storedSettingsRepository.GetGroupByProject();
            _pathSegmentCount = _storedSettingsRepository.GetPathSegmentCount();
            _showRecentUsage = _storedSettingsRepository.GetShowRecentUsage();

            var documentSortOptionType = _storedSettingsRepository
                .GetSelectedDocumentSortType();

            _selectedDocumentSortOption = sortOptions
                .SingleOrDefault(s => s.ToString() == documentSortOptionType);

            var projectSortOptionName = _storedSettingsRepository
                .GetSelectedProjectSortType();

            _selectedProjectSortOption = sortOptions
                .SingleOrDefault(s => s.ToString() == projectSortOptionName);
        }
    }
}
