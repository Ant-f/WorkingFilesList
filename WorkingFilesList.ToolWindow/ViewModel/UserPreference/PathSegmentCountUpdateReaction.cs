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

using System.ComponentModel;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference
{
    public class PathSegmentCountUpdateReaction : IUpdateReaction
    {
        private readonly IFilePathService _filePathService;

        public PathSegmentCountUpdateReaction(IFilePathService filePathService)
        {
            _filePathService = filePathService;
        }

        public void UpdateCollection(
            ICollectionView view,
            IUserPreferences userPreferences)
        {
            foreach (var metadata in view.Cast<DocumentMetadata>())
            {
                metadata.DisplayName = _filePathService.ReducePath(
                    metadata.CorrectedFullName,
                    userPreferences.PathSegmentCount);
            }

            // Collection may need to be sorted: changing the number of path
            // segments could make the displayed item order no longer agree with
            // the selected sorting option, e.g. if an alphabetical sort is
            // applied.

            view.Refresh();
        }
    }
}
