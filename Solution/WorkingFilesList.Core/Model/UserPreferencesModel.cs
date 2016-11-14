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

using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.Core.Model
{
    public class UserPreferencesModel : PropertyChangedNotifier, IUserPreferences
    {
        private bool _assignProjectColours;
        private bool _groupByProject;
        private bool _highlightFileName;
        private bool _showFileTypeIcons;
        private bool _showRecentUsage;
        private int _pathSegmentCount;
        private ISortOption _selectedDocumentSortOption;
        private ISortOption _selectedProjectSortOption;

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
                    OnAssignProjectColoursUpdate();
                }
            }
        }

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
                    OnGroupByProjectUpdate();
                }
            }
        }

        public bool HighlightFileName
        {
            get
            {
                return _highlightFileName;
            }

            set
            {
                if (_highlightFileName != value)
                {
                    _highlightFileName = value;
                    OnPropertyChanged();
                    OnHighlightFileNameUpdate();
                }
            }
        }

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
                    OnShowRecentUsageUpdate();
                }
            }
        }

        public bool ShowFileTypeIcons
        {
            get
            {
                return _showFileTypeIcons;
            }

            set
            {
                if (_showFileTypeIcons != value)
                {
                    _showFileTypeIcons = value;
                    OnPropertyChanged();
                    OnShowFileTypeIconsUpdate();
                }
            }
        }

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
                    OnPathSegmentCountUpdate();
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
                    OnSelectedDocumentSortOptionUpdate();
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
                    OnSelectedProjectSortOptionUpdate();
                }
            }
        }

        protected virtual void OnAssignProjectColoursUpdate()
        {
        }

        protected virtual void OnGroupByProjectUpdate()
        {
        }

        protected virtual void OnHighlightFileNameUpdate()
        {
        }

        protected virtual void OnShowRecentUsageUpdate()
        {
        }

        protected virtual void OnShowFileTypeIconsUpdate()
        {
        }

        protected virtual void OnPathSegmentCountUpdate()
        {
        }

        protected virtual void OnSelectedDocumentSortOptionUpdate()
        {
        }

        protected virtual void OnSelectedProjectSortOptionUpdate()
        {
        }
    }
}