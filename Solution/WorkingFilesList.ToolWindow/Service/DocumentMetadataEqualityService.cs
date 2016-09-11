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

using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

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
                info.FullName == metadata.FullName &&
                info.ProjectDisplayName == metadata.ProjectNames.DisplayName &&
                info.ProjectFullName == metadata.ProjectNames.FullName;

            return isEqual;
        }
    }
}
