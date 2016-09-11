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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Factory
{
    /// <summary>
    /// Contains methods to create new instances of <see cref="DocumentMetadata"/>
    /// passing both the full name provided, and an automatically evaluated version
    /// matching the casing as written on the file system as constructor parameters
    /// </summary>
    public class DocumentMetadataFactory : IDocumentMetadataFactory
    {
        private readonly IFilePathService _filePathService;
        private readonly IPathCasingRestorer _pathCasingRestorer;
        private readonly IProjectBrushService _projectBrushService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;

        public DocumentMetadataFactory(
            IFilePathService filePathService,
            IPathCasingRestorer pathCasingRestorer,
            IProjectBrushService projectBrushService,
            ITimeProvider timeProvider,
            IUserPreferences userPreferences)
        {
            _filePathService = filePathService;
            _pathCasingRestorer = pathCasingRestorer;
            _projectBrushService = projectBrushService;
            _timeProvider = timeProvider;
            _userPreferences = userPreferences;
        }

        /// <summary>
        /// Creates a new <see cref="DocumentMetadata"/>, setting
        /// <see cref="DocumentMetadata.ActivatedAt"/> at the current time in UTC
        /// </summary>
        /// <param name="info">
        /// Information about the document's full name and containing project
        /// </param>
        /// <returns>A new <see cref="DocumentMetadata"/> instance</returns>
        public DocumentMetadata Create(DocumentMetadataInfo info)
        {
            var utcNow = _timeProvider.UtcNow;
            var metadata = Create(info, utcNow);
            return metadata;
        }

        /// <summary>
        /// Creates a new <see cref="DocumentMetadata"/>, setting
        /// <see cref="DocumentMetadata.ActivatedAt"/> at the specified time
        /// </summary>
        /// <param name="info">
        /// Information about the document's full name and containing project
        /// </param>
        /// <param name="activatedAt">
        /// Value to set <see cref="DocumentMetadata.ActivatedAt"/> as
        /// </param>
        /// <returns>A new <see cref="DocumentMetadata"/> instance</returns>
        public DocumentMetadata Create(DocumentMetadataInfo info, DateTime activatedAt)
        {
            var correctedCasing = _pathCasingRestorer.RestoreCasing(info.FullName);

            var displayName = _filePathService.ReducePath(
                correctedCasing,
                _userPreferences.PathSegmentCount);

            var projectBrush = _projectBrushService.GetBrush(
                info.ProjectFullName,
                _userPreferences);

            var metadata = new DocumentMetadata(info, correctedCasing)
            {
                ActivatedAt = activatedAt,
                DisplayName = displayName,
                ProjectBrush = projectBrush
            };

            return metadata;
        }
    }
}
