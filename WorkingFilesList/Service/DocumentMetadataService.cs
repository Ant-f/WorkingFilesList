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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Data;
using WorkingFilesList.Interface;
using WorkingFilesList.Model;

namespace WorkingFilesList.Service
{
    public class DocumentMetadataService : IDocumentMetadataService
    {
        private readonly IPathCasingRestorer _pathCasingRestorer;
        private readonly ITimeProvider _timeProvider;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        public ICollectionView ActiveDocumentMetadata { get; }

        public DocumentMetadataService(
            IPathCasingRestorer pathCasingRestorer,
            ITimeProvider timeProvider)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();
            ActiveDocumentMetadata = new ListCollectionView(_activeDocumentMetadata);

            _pathCasingRestorer = pathCasingRestorer;
            _timeProvider = timeProvider;
        }

        /// <summary>
        /// Adds the provided name to <see cref="ActiveDocumentMetadata"/> if
        /// not already present. Does nothing otherwise.
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        public void Add(string fullName)
        {
            var metadataExists = _activeDocumentMetadata
                .Any(m => m.FullName == fullName);

            if (!metadataExists)
            {
                var correctedCasing = _pathCasingRestorer.RestoreCasing(fullName);

                var metadata = new DocumentMetadata
                {
                    FullName = correctedCasing
                };

                _activeDocumentMetadata.Add(metadata);
            }
        }

        /// <summary>
        /// Sets <see cref="DocumentMetadata.ActivatedAt"/> of the active
        /// document metadata item matching the provided file name to the
        /// current time in UTC
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        public void UpdateActivatedTime(string fullName)
        {
            var metadata = _activeDocumentMetadata.SingleOrDefault(m =>
                string.CompareOrdinal(m.FullName, fullName) == 0);

            if (metadata != null)
            {
                var utcNow = _timeProvider.UtcNow;
                metadata.ActivatedAt = utcNow;
            }
        }

        /// <summary>
        /// Updates <see cref="DocumentMetadata.FullName"/> of the specified
        /// active doucment metadata item. Does not alter the value of
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
            var metadata = _activeDocumentMetadata.SingleOrDefault(m =>
                string.CompareOrdinal(
                    m.FullName,
                    oldName) == 0);

            if (metadata != null)
            {
                metadata.FullName = newName;
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

                    var correctedCasing = _pathCasingRestorer
                        .RestoreCasing(document.FullName);

                    documentNameSet.Add(correctedCasing);

                    var existingMetadata = _activeDocumentMetadata.SingleOrDefault(m =>
                        string.Compare(
                            m.FullName,
                            correctedCasing,
                            StringComparison.OrdinalIgnoreCase) == 0);

                    if (existingMetadata == null)
                    {
                        var newMetadata = new DocumentMetadata
                        {
                            ActivatedAt = _timeProvider.UtcNow,
                            FullName = correctedCasing
                        };

                        _activeDocumentMetadata.Add(newMetadata);
                    }
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