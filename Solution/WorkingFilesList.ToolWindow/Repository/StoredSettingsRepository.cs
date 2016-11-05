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

namespace WorkingFilesList.ToolWindow.Repository
{
    public class StoredSettingsRepository : IStoredSettingsRepository
    {
        public int GetPathSegmentCount()
        {
            return Properties.Settings.Default.PathSegmentCount;
        }

        public void SetPathSegmentCount(int count)
        {
            Properties.Settings.Default.PathSegmentCount = count;
            Properties.Settings.Default.Save();
        }

        public string GetSelectedDocumentSortType()
        {
            return Properties.Settings.Default.SelectedDocumentSortType;
        }

        public void SetSelectedDocumentSortType(string name)
        {
            Properties.Settings.Default.SelectedDocumentSortType = name;
            Properties.Settings.Default.Save();
        }

        public string GetSelectedProjectSortType()
        {
            return Properties.Settings.Default.SelectedProjectSortType;
        }

        public void SetSelectedProjectSortType(string name)
        {
            Properties.Settings.Default.SelectedProjectSortType = name;
            Properties.Settings.Default.Save();
        }

        public bool GetGroupByProject()
        {
            return Properties.Settings.Default.GroupByProject;
        }

        public void SetGroupByProject(bool value)
        {
            Properties.Settings.Default.GroupByProject = value;
            Properties.Settings.Default.Save();
        }

        public bool GetShowRecentUsage()
        {
            return Properties.Settings.Default.ShowRecentUsage;
        }

        public void SetShowRecentUsage(bool value)
        {
            Properties.Settings.Default.ShowRecentUsage = value;
            Properties.Settings.Default.Save();
        }

        public bool GetAssignProjectColours()
        {
            return Properties.Settings.Default.AssignProjectColours;
        }

        public void SetAssignProjectColours(bool value)
        {
            Properties.Settings.Default.AssignProjectColours = value;
            Properties.Settings.Default.Save();
        }

        public bool GetShowFileTypeIcons()
        {
            return Properties.Settings.Default.ShowFileTypeIcons;
        }

        public void SetShowFileTypeIcons(bool value)
        {
            Properties.Settings.Default.ShowFileTypeIcons = value;
            Properties.Settings.Default.Save();
        }

        public void Reset()
        {
            Properties.Settings.Default.Reset();
        }
    }
}