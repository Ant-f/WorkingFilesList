// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class SortOptionsService : ISortOptionsService
    {
        /// <summary>
        /// Evaluates which <see cref="SortDescription"/> instances are needed to
        /// represent the sorting criteria of an <see cref="IUserPreferences"/>
        /// instance
        /// </summary>
        /// <param name="userPreferences">
        /// <see cref="IUserPreferences"/> instance that includes and represents
        /// the desired sorting criteria
        /// </param>
        /// <returns>
        /// <see cref="SortDescription"/> instances that represents the sorting
        /// criteria in <see cref="userPreferences"/>
        /// </returns>
        public SortDescription[] EvaluateAppliedSortDescriptions(
            IUserPreferences userPreferences)
        {
            var sortOptions = new[]
            {
                // Project sort option needs to be before document sort option
                // so that files are grouped by containing project

                userPreferences.SelectedProjectSortOption,
                userPreferences.SelectedDocumentSortOption
            };

            var sortDescriptions = sortOptions
                .Where(s => s != null && s.HasSortDescription)
                .Select(s => s.GetSortDescription())
                .ToArray();

            return sortDescriptions;
        }

        public void AssignDisplayOrder(
            IList<Type> displayOrder,
            IList<ISortOption> sortOptions)
        {
            foreach (var sortOption in sortOptions)
            {
                var type = sortOption.GetType();
                var index = displayOrder.IndexOf(type);
                sortOption.DisplayIndex = index;
            }
        }
    }
}
