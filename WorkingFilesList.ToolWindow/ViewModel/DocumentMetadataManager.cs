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

using EnvDTE;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class DocumentMetadataManager : IDocumentMetadataManager
    {
        private readonly IDocumentMetadataFactory _documentMetadataFactory;
        private readonly IFilePathService _filePathService;
        private readonly INormalizedUseOrderService _normalizedUseOrderService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        public ICollectionView ActiveDocumentMetadata { get; }

        public DocumentMetadataManager(
            ICollectionViewGenerator collectionViewGenerator,
            IDocumentMetadataFactory documentMetadataFactory,
            IFilePathService filePathService,
            INormalizedUseOrderService normalizedUseOrderService,
            ITimeProvider timeProvider,
            IUpdateReactionManager updateReactionManager,
            IUserPreferences userPreferences)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();

            ActiveDocumentMetadata = collectionViewGenerator.CreateView(
                _activeDocumentMetadata);

            _documentMetadataFactory = documentMetadataFactory;
            _filePathService = filePathService;
            _normalizedUseOrderService = normalizedUseOrderService;
            _timeProvider = timeProvider;
            _userPreferences = userPreferences;

            updateReactionManager.Initialize(ActiveDocumentMetadata);
        }

        /// <summary>
        /// Adds the provided name to <see cref="ActiveDocumentMetadata"/> if
        /// not already present. Does nothing otherwise.
        /// </summary>
        /// <param name="info">
        /// Information about the document's full name and containing project
        /// </param>
        public void Add(DocumentMetadataInfo info)
        {
            var metadataExists = _activeDocumentMetadata
                .Any(m => m.FullName == info.FullName);

            if (!metadataExists)
            {
                var metadata = _documentMetadataFactory.Create(info);

                metadata.DisplayName = _filePathService.ReducePath(
                    metadata.CorrectedFullName,
                    _userPreferences.PathSegmentCount);

                _activeDocumentMetadata.Add(metadata);
            }
        }

        /// <summary>
        /// Sets <see cref="DocumentMetadata.IsActive"/> to true and
        /// <see cref="DocumentMetadata.ActivatedAt"/> to the current time in UTC
        /// for the specified file. Sets <see cref="DocumentMetadata.IsActive"/>
        /// to false for all other files
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        public void Activate(string fullName)
        {
            var activated = false;

            foreach (var metadata in _activeDocumentMetadata)
            {
                metadata.IsActive = string.CompareOrdinal(metadata.FullName, fullName) == 0;

                if (metadata.IsActive)
                {
                    var utcNow = _timeProvider.UtcNow;
                    metadata.ActivatedAt = utcNow;
                    activated = true;
                }
            }

            if (activated)
            {
                _normalizedUseOrderService.SetUseOrder(
                    _activeDocumentMetadata,
                    _userPreferences);

                ActiveDocumentMetadata.Refresh();
            }
        }

        /// <summary>
        /// Replaces the specified doucment metadata item with another that
        /// reflects the updated full name. Does not alter the value of
        /// <see cref="DocumentMetadata.ActivatedAt"/>
        /// </summary>
        /// <param name="newName">
        /// The new full path and name of the document file
        /// </param>
        /// <param name="oldName">
        /// Full path and name of the document file that was renamed
        /// </param>
        public void UpdateFullName(string newName, string oldName)
        {
            for (int i = 0; i < _activeDocumentMetadata.Count; i++)
            {
                var existingMetadata = _activeDocumentMetadata[i];
                var match = string.CompareOrdinal(existingMetadata.FullName, oldName) == 0;
                if (match)
                {
                    var info = new DocumentMetadataInfo
                    {
                        FullName = newName,

                        ProjectDisplayName = _activeDocumentMetadata[i]
                            .ProjectNames.DisplayName,

                        ProjectUniqueName = _activeDocumentMetadata[i]
                            .ProjectNames.UniqueName
                    };

                    var newMetadata = _documentMetadataFactory.Create(
                        info,
                        _activeDocumentMetadata[i].ActivatedAt);

                    _activeDocumentMetadata[i] = newMetadata;

                    break;
                }
            }
        }

        /// <summary>
        /// Updates <see cref="ActiveDocumentMetadata"/> to reflect the documents
        /// in the method argument. Does not update the value of
        /// <see cref="DocumentMetadata.ActivatedAt"/> for existing metadata; sets
        /// as the current time in UTC for new metadata.
        /// </summary>
        /// <param name="documents">
        /// <see cref="Documents"/> that <see cref="ActiveDocumentMetadata"/>
        /// should reflect
        /// </param>
        public void Synchronize(Documents documents)
        {
            var documentNameSet = new HashSet<string>();

            // Add documents unique to method parameter collection

            try
            {
                foreach (var obj in documents)
                {
                    var document = (Document)obj;

                    if (document.ActiveWindow == null)
                    {
                        continue;
                    }

                    documentNameSet.Add(document.FullName);

                    var info = new DocumentMetadataInfo
                    {
                        FullName = document.FullName,
                        ProjectDisplayName = document.ProjectItem.ContainingProject.Name,
                        ProjectUniqueName = document.ProjectItem.ContainingProject.UniqueName
                    };

                    Add(info);
                }
            }
            catch (COMException)
            {
                // COMException is thrown during enumeration of 'documents'
                // when a project is closed in Visual Studio. Do nothing: this
                // will result in the active documents metadata collection
                // being emptied, which is appropriate
            }

            // Remove documents not in method parameter collection

            for (int i = 0; i < _activeDocumentMetadata.Count; i++)
            {
                var removeMetadata = !documentNameSet
                    .Contains(_activeDocumentMetadata[i].FullName);

                if (removeMetadata)
                {
                    _activeDocumentMetadata.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}