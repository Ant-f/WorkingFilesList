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
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Model.SortOption
{
    /// <summary>
    /// Represents an option for criteria on sorting the displayed document
    /// metadata
    /// </summary>
    public abstract class SortOptionBase : ISortOption
    {
        /// <summary>
        /// Indicates whether calling <see cref="GetSortDescription"/> should
        /// return a valid <see cref="SortDescription"/>
        /// </summary>
        public bool HasSortDescription { get; }

        /// <summary>
        /// Indicates desired order of appearance when displayed in a sorted list
        /// </summary>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// Indicates which project items types derived classes are applicable
        /// to
        /// </summary>
        public ProjectItemType ApplicableType { get; }

        /// <summary>
        /// Name that this set of sorting criteria will be displayed as
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Name of the property used as sorting criteria
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Direction that sorting should occur
        /// </summary>
        public ListSortDirection SortDirection { get; }

        protected SortOptionBase(string displayName, ProjectItemType applicableTypes)
        {
            ApplicableType = applicableTypes;
            DisplayName = displayName;
            HasSortDescription = false;
        }

        protected SortOptionBase(
            string displayName,
            string propertyName,
            ListSortDirection sortDirection,
            ProjectItemType applicableTypes)
        {
            ApplicableType = applicableTypes;
            DisplayName = displayName;
            PropertyName = propertyName;
            SortDirection = sortDirection;
            HasSortDescription = true;
        }

        /// <summary>
        /// Creates a <see cref="SortDescription"/> that can be added to the
        /// sort descriptions of an <see cref="ICollectionView"/>
        /// </summary>
        /// <returns>
        /// A <see cref="SortDescription"/> that describes the sorting criteria
        /// represented by this <see cref="ISortOption"/> instance
        /// </returns>
        public SortDescription GetSortDescription()
        {
            if (HasSortDescription)
            {
                var sortDescription = new SortDescription(
                    PropertyName,
                    SortDirection);

                return sortDescription;
            }
            else
            {
                throw new NotSupportedException(
                    "This type does not represent usable sorting criteria");
            }
        }
    }
}
