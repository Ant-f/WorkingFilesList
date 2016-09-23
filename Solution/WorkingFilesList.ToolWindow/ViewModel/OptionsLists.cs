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
        public IList<ISortOption> ProjectSortOptions { get; }

        public OptionsLists(
            IList<ISortOption> sortOptions,
            IDisplayOrderContainer sortOptionsDisplayOrder,
            ISortOptionsService sortOptionsService)
        {
            sortOptionsService.AssignDisplayOrder(
                sortOptionsDisplayOrder.DisplayOrder,
                sortOptions);

            DocumentSortOptions = sortOptions
                .Where(s => s.ApplicableType == ProjectItemType.Document)
                .OrderBy(s => s.DisplayIndex)
                .ToList();

            ProjectSortOptions = sortOptions
                .Where(s => s.ApplicableType == ProjectItemType.Project)
                .OrderBy(s => s.DisplayIndex)
                .ToList();
        }
    }
}
