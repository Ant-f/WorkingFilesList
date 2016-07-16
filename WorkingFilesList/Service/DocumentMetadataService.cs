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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Data;
using WorkingFilesList.Interface;
using WorkingFilesList.Model;

namespace WorkingFilesList.Service
{
    public class DocumentMetadataService : IDocumentMetadataService
    {
        private readonly char[] _pathSeparators =
        {
            Path.AltDirectorySeparatorChar,
            Path.DirectorySeparatorChar
        };

        private readonly IDocumentMetadataFactory _documentMetadataFactory;
        private readonly ISortOptionsService _sortOptionsService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        public ICollectionView ActiveDocumentMetadata { get; }

        public DocumentMetadataService(
            ICollectionViewGenerator collectionViewGenerator,
            IDocumentMetadataFactory documentMetadataFactory,
            ISortOptionsService sortOptionsService,
            ITimeProvider timeProvider,
            IUserPreferences userPreferences)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();

            ActiveDocumentMetadata = collectionViewGenerator.CreateView(
                _activeDocumentMetadata);

            _documentMetadataFactory = documentMetadataFactory;
            _sortOptionsService = sortOptionsService;
            _timeProvider = timeProvider;
            _userPreferences = userPreferences;

            _userPreferences.PropertyChanged += UserPreferencesPropertyChanged;
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
                var metadata = _documentMetadataFactory.Create(fullName);
                metadata.DisplayName = EvaluateDisplayName(metadata);
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
                    var newMetadata = _documentMetadataFactory.Create(
                        newName,
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
                    Add(document.FullName);
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

        private void UserPreferencesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IUserPreferences.SelectedSortOption):
                {
                    ActiveDocumentMetadata.SortDescriptions.Clear();

                    var sortDescriptions =
                        _sortOptionsService.EvaluateAppliedSortDescriptions(_userPreferences);

                    foreach (var sortDescription in sortDescriptions)
                    {
                        ActiveDocumentMetadata.SortDescriptions.Add(sortDescription);
                    }

                    break;
                }

                case nameof(IUserPreferences.PathSegmentCount):
                {
                    foreach (var metadata in _activeDocumentMetadata)
                    {
                        metadata.DisplayName = EvaluateDisplayName(metadata);
                    }
                    break;
                }
            }
        }

        private string EvaluateDisplayName(DocumentMetadata metadata)
        {
            var maxIndex = metadata.CorrectedFullName.Length - 1;

            // Start search at the last position of the string
            var startIndex = maxIndex;

            for (int i = 0;
                i < _userPreferences.PathSegmentCount && startIndex > 0;
                i++)
            {
                var index = metadata.CorrectedFullName.LastIndexOfAny(
                    _pathSeparators,
                    startIndex);

                // Decrement index: subsequent calls to LastIndexOfAny will
                // return the same index otherwise

                startIndex = index - 1;
            }

            int substringStartIndex;
            if (startIndex < 0 || startIndex == maxIndex)
            {
                substringStartIndex = 0;
            }
            else
            {
                // startIndex points to a character before a directory separator.
                // Increment startIndex by two:
                // - one to point at a directory separator, which we don't want
                // - one to point at the first character after the directory separator

                substringStartIndex = startIndex + 2;
            }

            var displayName = metadata.CorrectedFullName.Substring(substringStartIndex);
            return displayName;
        }
    }
}