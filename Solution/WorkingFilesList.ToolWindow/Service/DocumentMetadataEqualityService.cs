﻿// Working Files List
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
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class DocumentMetadataEqualityService : IDocumentMetadataEqualityService
    {
        /// <summary>
        /// Compares the properties of a <see cref="DocumentMetadataInfo"/>
        /// with the corresponding properties of a <see cref="DocumentMetadata"/>
        /// to determine whether they refer to the same document
        /// </summary>
        /// <param name="info">
        /// <see cref="DocumentMetadataInfo"/> instance to compare against
        /// <paramref name="metadata"/>
        /// </param>
        /// <param name="metadata">
        /// <see cref="DocumentMetadata"/> instance to compare against
        /// <paramref name="info"/>
        /// </param>
        /// <returns>true if <paramref name="info"/> and <paramref name="metadata"/>
        /// refer to the same document, false otherwise
        /// </returns>
        public bool Compare(DocumentMetadataInfo info, DocumentMetadata metadata)
        {
            var isEqual =
                Compare(info.FullName, metadata.FullName) &&
                Compare(info.ProjectDisplayName, metadata.ProjectNames.DisplayName) &&
                Compare(info.ProjectFullName, metadata.ProjectNames.FullName);

            return isEqual;
        }

        /// <summary>
        /// Perform comparison of file names
        /// </summary>
        /// <param name="name1">Name to compare against <paramref name="name2"/></param>
        /// <param name="name2">Name to compare against <paramref name="name1"/></param>
        /// <returns>true if both parameters are considered equal, false otherwise</returns>
        public bool Compare(string name1, string name2)
        {
            var isEqual = string.Compare(
                name1,
                name2,
                StringComparison.OrdinalIgnoreCase) == 0;

            return isEqual;
        }
    }
}
