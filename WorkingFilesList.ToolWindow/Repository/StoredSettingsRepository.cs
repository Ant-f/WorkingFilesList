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

using WorkingFilesList.ToolWindow.Interface;

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

        public void Reset()
        {
            Properties.Settings.Default.Reset();
        }
    }
}