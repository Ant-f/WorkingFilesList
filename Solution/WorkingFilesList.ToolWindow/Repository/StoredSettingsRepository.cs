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
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Repository
{
    public class StoredSettingsRepository : IStoredSettingsRepository
    {
        private const int DefaultPathSegmentCount = 1;
        private const string DefaultDocumentSortOptionName = "A-Z";
        private const string DefaultProjectSortOptionName = "None";
        private const bool DefaultGroupByProject = false;
        private const bool DefaultShowRecentUsage = false;
        private const bool DefaultAssignProjectColours = false;
        private const bool DefaultShowFileTypeIcons = true;
        private const bool DefaultHighlightFileName = true;

        private readonly ISettingsStoreService _settingsStoreService;
        private readonly string _settingsCollectionName;

        public StoredSettingsRepository(
            ISettingsStoreService settingsStoreService,
            string settingsCollectionNameRoot)
        {
            _settingsStoreService = settingsStoreService;
            _settingsCollectionName = $"{settingsCollectionNameRoot}\\Settings";

            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.CreateCollection(_settingsCollectionName);
            }
        }

        public int GetPathSegmentCount()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var count = service.SettingsStore.GetInt32(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.PathSegmentCount),
                    DefaultPathSegmentCount);

                return count;
            }
        }

        public void SetPathSegmentCount(int count)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetInt32(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.PathSegmentCount),
                    count);
            }
        }

        public string GetDocumentSortOptionName()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var name = service.SettingsStore.GetString(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.DocumentSortOptionName),
                    DefaultDocumentSortOptionName);

                return name;
            }
        }

        public void SetDocumentSortOptionName(string name)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetString(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.DocumentSortOptionName),
                    name);
            }
        }

        public string GetProjectSortOptionName()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var name = service.SettingsStore.GetString(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ProjectSortOptionName),
                    DefaultProjectSortOptionName);

                return name;
            }
        }

        public void SetProjectSortOptionName(string name)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetString(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ProjectSortOptionName),
                    name);
            }
        }

        public bool GetGroupByProject()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var groupByProject = service.SettingsStore.GetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.GroupByProject),
                    DefaultGroupByProject);

                return groupByProject;
            }
        }

        public void SetGroupByProject(bool value)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.GroupByProject),
                    value);
            }
        }

        public bool GetHighlightFileName()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var highlight = service.SettingsStore.GetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.HighlightFileName),
                    DefaultHighlightFileName);

                return highlight;
            }
        }

        public void SetHighlightFileName(bool value)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.HighlightFileName),
                    value);
            }
        }

        public bool GetShowRecentUsage()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var showRecentUsage = service.SettingsStore.GetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ShowRecentUsage),
                    DefaultShowRecentUsage);

                return showRecentUsage;
            }
        }

        public void SetShowRecentUsage(bool value)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ShowRecentUsage),
                    value);
            }
        }

        public bool GetAssignProjectColours()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var assignProjectColours = service.SettingsStore.GetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.AssignProjectColours),
                    DefaultAssignProjectColours);

                return assignProjectColours;
            }
        }

        public void SetAssignProjectColours(bool value)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.AssignProjectColours),
                    value);
            }
        }

        public bool GetShowFileTypeIcons()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                var showFileTypeIcons = service.SettingsStore.GetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ShowFileTypeIcons),
                    DefaultShowFileTypeIcons);

                return showFileTypeIcons;
            }
        }

        public void SetShowFileTypeIcons(bool value)
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.SetBoolean(
                    _settingsCollectionName,
                    nameof(IUserPreferencesModel.ShowFileTypeIcons),
                    value);
            }
        }

        public void Reset()
        {
            using (var service = _settingsStoreService.GetWritableSettingsStore())
            {
                service.SettingsStore.DeleteCollection(_settingsCollectionName);
            }
        }
    }
}