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

using System.Collections.Generic;
using System.Linq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class UserPreferences : UserPreferencesModel, IUserPreferences
    {
        private readonly bool _initializing = false;
        private readonly IList<ISortOption> _sortOptions;
        private readonly IStoredSettingsRepository _storedSettingsRepository;

        private ISortOption _documentSortOption;
        private ISortOption _projectSortOption;

        public ISortOption DocumentSortOption
        {
            get
            {
                return _documentSortOption;
            }

            private set
            {
                if (_documentSortOption != value)
                {
                    _documentSortOption = value;
                    OnPropertyChanged();
                }
            }
        }

        public ISortOption ProjectSortOption
        {
            get
            {
                return _projectSortOption;
            }

            private set
            {
                if (_projectSortOption != value)
                {
                    _projectSortOption = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnAssignProjectColoursUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetAssignProjectColours(
                AssignProjectColours);
        }

        protected override void OnGroupByProjectUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetGroupByProject(GroupByProject);
        }

        protected override void OnHighlightFileNameUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetHighlightFileName(HighlightFileName);
        }

        protected override void OnShowRecentUsageUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetShowRecentUsage(ShowRecentUsage);
        }

        protected override void OnShowFileTypeIconsUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetShowFileTypeIcons(ShowFileTypeIcons);
        }

        protected override void OnPathSegmentCountUpdate()
        {
            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetPathSegmentCount(PathSegmentCount);
        }

        protected override void OnDocumentSortOptionNameUpdate()
        {
            DocumentSortOption = _sortOptions.FirstOrDefault(s =>
                s.DisplayName == DocumentSortOptionName);

            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetDocumentSortOptionName(DocumentSortOptionName);
        }

        protected override void OnProjectSortOptionNameUpdate()
        {
            ProjectSortOption = _sortOptions.FirstOrDefault(s =>
                s.DisplayName == ProjectSortOptionName);

            if (_initializing)
            {
                return;
            }

            _storedSettingsRepository.SetProjectSortOptionName(ProjectSortOptionName);
        }

        public UserPreferences(
            IStoredSettingsRepository storedSettingsRepository,
            IList<ISortOption> sortOptions,
            IUserPreferencesModelRepository userPreferencesModelRepository)
        {
            _initializing = true;
            _storedSettingsRepository = storedSettingsRepository;
            _sortOptions = sortOptions;
            userPreferencesModelRepository.LoadInto(this);
            _initializing = false;
        }
    }
}
