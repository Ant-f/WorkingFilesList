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
using System.IO;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;

namespace WorkingFilesList.ToolWindow.Factory
{
    /// <summary>
    /// Contains methods to create new instances of <see cref="DocumentMetadata"/>
    /// passing both the full name provided, and an automatically evaluated version
    /// matching the casing as written on the file system as constructor parameters
    /// </summary>
    public class DocumentMetadataFactory : IDocumentMetadataFactory
    {
        private readonly IDisplayNameHighlightEvaluator _displayNameHighlightEvaluator;
        private readonly IDocumentIconService _documentIconService;
        private readonly IFilePathService _filePathService;
        private readonly IPathCasingRestorer _pathCasingRestorer;
        private readonly IProjectBrushService _projectBrushService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;

        public DocumentMetadataFactory(
            IDisplayNameHighlightEvaluator displayNameHighlightEvaluator,
            IDocumentIconService documentIconService,
            IFilePathService filePathService,
            IPathCasingRestorer pathCasingRestorer,
            IProjectBrushService projectBrushService,
            ITimeProvider timeProvider,
            IUserPreferences userPreferences)
        {
            _displayNameHighlightEvaluator = displayNameHighlightEvaluator;
            _documentIconService = documentIconService;
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

            var extension = Path.GetExtension(info.FullName);
            var icon = _documentIconService.GetIcon(extension);

            var displayNameHighlight = _displayNameHighlightEvaluator
                .GetHighlight(displayName);

            var displayNamePostHighlight = _displayNameHighlightEvaluator
                .GetPostHighlight(displayName);

            var displayNamePreHighlight = _displayNameHighlightEvaluator
                .GetPreHighlight(displayName);

            var metadata = new DocumentMetadata(info, correctedCasing, icon)
            {
                ActivatedAt = activatedAt,
                DisplayNameHighlight = displayNameHighlight,
                DisplayNamePostHighlight = displayNamePostHighlight,
                DisplayNamePreHighlight = displayNamePreHighlight,
                ProjectBrush = projectBrush
            };

            return metadata;
        }
    }
}
