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

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class UserPreferences : UserPreferencesModel
    {
        private readonly IStoredSettingsRepository _storedSettingsRepository;

        private readonly bool _initializing = false;

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

        protected override void OnSelectedDocumentSortOptionUpdate()
        {
            if (_initializing)
            {
                return;
            }

            var typeString = SelectedDocumentSortOption?.ToString();
            _storedSettingsRepository.SetSelectedDocumentSortType(typeString);
        }

        protected override void OnSelectedProjectSortOptionUpdate()
        {
            if (_initializing)
            {
                return;
            }

            var typeString = SelectedProjectSortOption?.ToString();
            _storedSettingsRepository.SetSelectedProjectSortType(typeString);
        }

        public UserPreferences(
            IStoredSettingsRepository storedSettingsRepository,
            IList<ISortOption> sortOptions)
        {
            _initializing = true;
            _storedSettingsRepository = storedSettingsRepository;

            AssignProjectColours = _storedSettingsRepository.GetAssignProjectColours();
            GroupByProject = _storedSettingsRepository.GetGroupByProject();
            HighlightFileName = storedSettingsRepository.GetHighlightFileName();
            PathSegmentCount = _storedSettingsRepository.GetPathSegmentCount();
            ShowFileTypeIcons = _storedSettingsRepository.GetShowFileTypeIcons();
            ShowRecentUsage = _storedSettingsRepository.GetShowRecentUsage();

            var documentSortOptionType = _storedSettingsRepository
                .GetSelectedDocumentSortType();

            SelectedDocumentSortOption = sortOptions
                .SingleOrDefault(s => s.ToString() == documentSortOptionType);

            var projectSortOptionName = _storedSettingsRepository
                .GetSelectedProjectSortType();

            SelectedProjectSortOption = sortOptions
                .SingleOrDefault(s => s.ToString() == projectSortOptionName);

            _initializing = false;
        }
    }
}
