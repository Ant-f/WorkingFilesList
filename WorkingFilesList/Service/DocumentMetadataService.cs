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
using System.Linq;
using System.Windows.Data;
using WorkingFilesList.Interface;
using WorkingFilesList.Model;

namespace WorkingFilesList.Service
{
    public class DocumentMetadataService : IDocumentMetadataService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        public ListCollectionView ActiveDocumentMetadata { get; }

        public DocumentMetadataService(ITimeProvider timeProvider)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();
            ActiveDocumentMetadata = new ListCollectionView(_activeDocumentMetadata);

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
                var metadata = new DocumentMetadata
                {
                    FullName = fullName
                };

                _activeDocumentMetadata.Add(metadata);
            }
        }

        /// <summary>
        /// Sets <see cref="DocumentMetadata.ActivatedAt"/> of the active
        /// document metadata list entry matching the provided file name to the
        /// current time in UTC
        /// </summary>
        /// <param name="fullName">Full path and name of document file</param>
        public void UpdateActivatedTime(string fullName)
        {
            throw new NotImplementedException();
        }

        public void Synchronize(Documents documents)
        {
            var documentNameSet = new HashSet<string>();

            // Add documents unique to method parameter collection

            foreach (var obj in documents)
            {
                var document = (Document) obj;

                if (document.ActiveWindow == null)
                {
                    continue;
                }

                documentNameSet.Add(document.FullName);

                var existingMetadata = _activeDocumentMetadata.SingleOrDefault(m =>
                    string.Compare(
                        m.FullName,
                        document.FullName,
                        StringComparison.OrdinalIgnoreCase) == 0);

                if (existingMetadata == null)
                {
                    var newMetadata = new DocumentMetadata
                    {
                        ActivatedAt = _timeProvider.UtcNow,
                        FullName = document.FullName
                    };

                    _activeDocumentMetadata.Add(newMetadata);
                }
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