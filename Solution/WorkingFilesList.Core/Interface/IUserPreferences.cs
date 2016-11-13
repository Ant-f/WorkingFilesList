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

using System.ComponentModel;
using WorkingFilesList.Core.Model;

namespace WorkingFilesList.Core.Interface
{
    public interface IUserPreferences : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should be assigned a colour associated with its
        /// <see cref="DocumentMetadataInfo.ProjectFullName"/>
        /// </summary>
        bool AssignProjectColours { get; set; }

        /// <summary>
        /// Indicates whether a <see cref="GroupDescription"/> should be added
        /// to views of <see cref="DocumentMetadata"/> collections to group
        /// projects together
        /// </summary>
        bool GroupByProject { get; set; }

        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should emphasize the part representing the file name
        /// </summary>
        bool HighlightFileName { get; set; }

        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should show an icon representing its file type, based on its
        /// file extension
        /// </summary>
        bool ShowFileTypeIcons { get; set; }

        /// <summary>
        /// Indicates whether each entry on the <see cref="DocumentMetadata"/>
        /// list should show the order of its historical usage reletive to the
        /// other entries on that list
        /// </summary>
        bool ShowRecentUsage { get; set; }

        /// <summary>
        /// The number of path segments to display, a path segment being either
        /// a single file or directory name that makes up the full name of a file
        /// </summary>
        int PathSegmentCount { get; set; }

        /// <summary>
        /// The selected sorting option that should be applied to entries on the
        /// <see cref="DocumentMetadata"/> list, with respect to each entry's
        /// displayed file path. This sort should be secondary to
        /// <see cref="SelectedProjectSortOption"/>
        /// </summary>
        ISortOption SelectedDocumentSortOption { get; set; }

        /// <summary>
        /// The selected sorting option that should be applied to entries on the
        /// <see cref="DocumentMetadata"/> list, with respect to each entry's
        /// parent project name. This sorting should be applied before
        /// <see cref="SelectedProjectSortOption"/>
        /// </summary>
        ISortOption SelectedProjectSortOption { get; set; }
    }
}
