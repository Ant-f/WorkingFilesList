// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 - 2020 Anthony Fung

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
    public class UserPreferencesModel : PropertyChangedNotifier, IUserPreferencesModel
    {
        private bool _assignProjectColours;
        private bool _groupByProject;
        private bool _highlightFileName;
        private bool _showConfigurationBar = true;
        private bool _showSearchBar = true;
        private bool _showFileTypeIcons;
        private bool _showRecentUsage;
        private int _pathSegmentCount;
        private int _unityRefreshDelay;
        private string _documentSortOptionName;
        private string _projectSortOptionName;

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

        public bool ShowConfigurationBar
        {
            get
            {
                return _showConfigurationBar;
            }

            set
            {
                if (_showConfigurationBar != value)
                {
                    _showConfigurationBar = value;
                    OnPropertyChanged();
                    OnShowConfigurationBarUpdate();
                }
            }
        }

        public bool ShowSearchBar
        {
            get
            {
                return _showSearchBar;
            }

            set
            {
                if (_showSearchBar != value)
                {
                    _showSearchBar = value;
                    OnPropertyChanged();
                    OnShowSearchBarUpdate();
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

        public int UnityRefreshDelay
        {
            get
            {
                return _unityRefreshDelay;
            }

            set
            {
                if (_unityRefreshDelay != value)
                {
                    _unityRefreshDelay = value;
                    OnPropertyChanged();
                    OnUnityRefreshDelayUpdate();
                }
            }
        }

        public string DocumentSortOptionName
        {
            get
            {
                return _documentSortOptionName;
            }

            set
            {
                if (_documentSortOptionName != value)
                {
                    _documentSortOptionName = value;
                    OnPropertyChanged();
                    OnDocumentSortOptionNameUpdate();
                }
            }
        }

        public string ProjectSortOptionName
        {
            get
            {
                return _projectSortOptionName;
            }

            set
            {
                if (_projectSortOptionName != value)
                {
                    _projectSortOptionName = value;
                    OnPropertyChanged();
                    OnProjectSortOptionNameUpdate();
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

        protected virtual void OnShowConfigurationBarUpdate()
        {
        }

        protected virtual void OnShowSearchBarUpdate()
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

        protected virtual void OnUnityRefreshDelayUpdate()
        {
        }

        protected virtual void OnDocumentSortOptionNameUpdate()
        {
        }

        protected virtual void OnProjectSortOptionNameUpdate()
        {
        }
    }
}
