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
using System.Windows.Controls;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    /// <summary>
    /// Contains lists that UI controls can bind their
    /// <see cref="ItemsControl.ItemsSource"/> property to
    /// </summary>
    public class OptionsLists : IOptionsLists
    {
        public IList<ISortOption> DocumentSortOptions { get; }

        public OptionsLists(IEnumerable<ISortOption> sortOptions)
        {
            DocumentSortOptions = sortOptions
                .Where(s => s.ApplicableTypes.HasFlag(ProjectItemType.Document))
                .ToList();
        }
    }
}
