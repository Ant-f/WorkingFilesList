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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WorkingFilesList.Core.Interface;

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
