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

using EnvDTE;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class DocumentMetadataManager : IDocumentMetadataManager
    {
        private readonly ICollectionViewGenerator _collectionViewGenerator;
        private readonly IDocumentMetadataEqualityService _metadataEqualityService;
        private readonly IDocumentMetadataFactory _documentMetadataFactory;
        private readonly INormalizedUsageOrderService _normalizedUsageOrderService;
        private readonly IProjectItemService _projectItemService;
        private readonly ITimeProvider _timeProvider;
        private readonly IUserPreferences _userPreferences;
        private readonly ObservableCollection<DocumentMetadata> _activeDocumentMetadata;

        public ICollectionView ActiveDocumentMetadata { get; }
        public ICollectionView PinnedDocumentMetadata { get; }

        public DocumentMetadataManager(
            ICollectionViewGenerator collectionViewGenerator,
            IDocumentMetadataEqualityService metadataEqualityService,
            IDocumentMetadataFactory documentMetadataFactory,
            INormalizedUsageOrderService normalizedUsageOrderService,
            IProjectItemService projectItemService,
            ITimeProvider timeProvider,
            IUpdateReactionManager updateReactionManager,
            IUserPreferences userPreferences)
        {
            _activeDocumentMetadata = new ObservableCollection<DocumentMetadata>();
            _collectionViewGenerator = collectionViewGenerator;

            ActiveDocumentMetadata = InitializeActiveDocumentMetadata();
            PinnedDocumentMetadata = InitializePinnedDocumentMetadata();

            _metadataEqualityService = metadataEqualityService;
            _documentMetadataFactory = documentMetadataFactory;
            _normalizedUsageOrderService = normalizedUsageOrderService;
            _projectItemService = projectItemService;
            _timeProvider = timeProvider;
            _userPreferences = userPreferences;

            updateReactionManager.Initialize(ActiveDocumentMetadata);
        }

        private ICollectionView InitializeActiveDocumentMetadata()
        {
            var view = _collectionViewGenerator.CreateView(
                _activeDocumentMetadata);

            view.Filter += obj =>
            {
                var metadata = obj as DocumentMetadata;

                var include =
                    metadata != null &&
                    metadata.HasWindow;

                return include;
            };

            return view;
        }

        private ICollectionView InitializePinnedDocumentMetadata()
        {
            var view = _collectionViewGenerator.CreateView(
                _activeDocumentMetadata);

            var pinOrderSortDescription = new SortDescription(
                nameof(DocumentMetadata.PinOrder),
                ListSortDirection.Ascending);

            view.SortDescriptions.Add(pinOrderSortDescription);

            view.Filter += obj =>
            {
                var metadata = obj as DocumentMetadata;

                var include =
                    metadata != null &&
                    metadata.IsPinned;

                return include;
            };

            return view;
        }

        /// <summary>
        /// Adds the provided name to <see cref="ActiveDocumentMetadata"/> if
        /// not already present. Sets <see cref="DocumentMetadata.HasWindow"/>
        /// to true
        /// </summary>
        /// <param name="info">
        /// Information about the document's full name and containing project
        /// </param>
        public void Add(DocumentMetadataInfo info)
        {
            var metadata = _activeDocumentMetadata.FirstOrDefault(m =>
                _metadataEqualityService.Compare(info, m));

            if (metadata == null)
            {
                metadata = _documentMetadataFactory.Create(info);
                _activeDocumentMetadata.Add(metadata);
            }

            metadata.HasWindow = true;
            ActiveDocumentMetadata.Refresh();
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
            var activatedDocument = _activeDocumentMetadata.FirstOrDefault(m =>
                m.IsActive)?.FullName;

            if (fullName == activatedDocument)
            {
                return;
            }

            var activated = false;

            foreach (var metadata in _activeDocumentMetadata)
            {
                metadata.IsActive = _metadataEqualityService.Compare(
                    metadata.FullName,
                    fullName);

                if (metadata.IsActive)
                {
                    var utcNow = _timeProvider.UtcNow;
                    metadata.ActivatedAt = utcNow;
                    activated = true;
                }
            }

            if (activated)
            {
                _normalizedUsageOrderService.SetUsageOrder(
                    _activeDocumentMetadata,
                    _userPreferences);

                ActiveDocumentMetadata.Refresh();
                PinnedDocumentMetadata.Refresh();
            }
        }

        /// <summary>
        /// Remove all referenced <see cref="DocumentMetadata"/> instances
        /// </summary>
        public void Clear()
        {
            _activeDocumentMetadata.Clear();
        }

        /// <summary>
        /// Replaces the specified document metadata item with another that
        /// reflects the updated full name. Does not alter the value of
        /// <see cref="DocumentMetadata.ActivatedAt"/>
        /// </summary>
        /// <param name="newName">
        /// The new full path and name of the document file
        /// </param>
        /// <param name="oldName">
        /// Full path and name of the document file that was renamed
        /// </param>
        /// <returns>true if a metadata item was updated, false otherwise</returns>
        public bool UpdateFullName(string newName, string oldName)
        {
            var match = false;

            for (int i = 0; i < _activeDocumentMetadata.Count; i++)
            {
                var existingMetadata = _activeDocumentMetadata[i];

                match = _metadataEqualityService.Compare(
                    existingMetadata.FullName,
                    oldName);

                if (match)
                {
                    var info = new DocumentMetadataInfo
                    {
                        FullName = newName,

                        ProjectDisplayName = _activeDocumentMetadata[i]
                            .ProjectNames.DisplayName,

                        ProjectFullName = _activeDocumentMetadata[i]
                            .ProjectNames.FullName
                    };

                    var newMetadata = _documentMetadataFactory.Create(
                        info,
                        _activeDocumentMetadata[i].ActivatedAt);

                    newMetadata.HasWindow = existingMetadata.HasWindow;
                    newMetadata.PinOrder = existingMetadata.PinOrder;
                    _activeDocumentMetadata[i] = newMetadata;

                    _normalizedUsageOrderService.SetUsageOrder(
                        _activeDocumentMetadata,
                        _userPreferences);

                    break;
                }
            }

            return match;
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
        /// <param name="setUsageOrder">
        /// true to update <see cref="DocumentMetadata.UsageOrder"/> for every
        /// <see cref="DocumentMetadata"/> in <see cref="ActiveDocumentMetadata"/>
        /// after Synchronization, false otherwise
        /// </param>
        public void Synchronize(Documents documents, bool setUsageOrder)
        {
            // DocumentMetadataInfo for each Document in 'documents'
            var documentsInfoSet = new HashSet<DocumentMetadataInfo>();

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

                    var info = new DocumentMetadataInfo
                    {
                        FullName = document.FullName,
                        ProjectDisplayName = document.ProjectItem.ContainingProject.Name,
                        ProjectFullName = document.ProjectItem.ContainingProject.FullName
                    };

                    documentsInfoSet.Add(info);
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
                var foundInDocumentInfoSet = documentsInfoSet.Any(info =>
                    _metadataEqualityService.Compare(
                        info, _activeDocumentMetadata[i]));

                if (!foundInDocumentInfoSet)
                {
                    var removeMetadata = true;

                    if (_activeDocumentMetadata[i].IsPinned)
                    {
                        var exists = _projectItemService.FindProjectItem(
                            _activeDocumentMetadata[i].FullName) != null;

                        if (exists)
                        {
                            _activeDocumentMetadata[i].HasWindow = false;
                            removeMetadata = false;
                        }
                    }

                    if (removeMetadata)
                    {
                        _activeDocumentMetadata.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (setUsageOrder)
            {
                _normalizedUsageOrderService.SetUsageOrder(
                    _activeDocumentMetadata,
                    _userPreferences);
            }

            ActiveDocumentMetadata.Refresh();
        }

        /// <summary>
        /// Move an item in <see cref="PinnedDocumentMetadata"/>, reordering
        /// the collection
        /// </summary>
        /// <param name="itemToMove">The item to move</param>
        /// <param name="targetLocation">Item that currently occupies the index
        /// where <paramref name="itemToMove"/> should be moved to</param>
        public void MovePinnedItem(
            DocumentMetadata itemToMove,
            DocumentMetadata targetLocation)
        {
            if (itemToMove.PinOrder == targetLocation.PinOrder)
            {
                return;
            }

            var offset = itemToMove.PinOrder > targetLocation.PinOrder ? -1 : 1;
            itemToMove.PinOrder = targetLocation.PinOrder + offset;
            RefreshPinnedDocumentMetadataAndAssignPinOrder();
        }

        /// <summary>
        /// Inverts the value of <see cref="DocumentMetadata.IsPinned"/> for
        /// <paramref name="metadata"/>, and adds/removes it to/from
        /// <see cref="PinnedDocumentMetadata"/> accordingly
        /// </summary>
        /// <param name="metadata">
        /// <see cref="DocumentMetadata"/> to update
        /// <see cref="DocumentMetadata.IsPinned"/> for
        /// </param>
        public void TogglePinnedStatus(DocumentMetadata metadata)
        {
            if (metadata.IsPinned)
            {
                metadata.PinOrder = DocumentMetadata.UnpinnedOrderValue;
            }
            else
            {
                var pinnedItems = _activeDocumentMetadata.Count(m => m.IsPinned);

                // Set to double, so that an intermediate value is available
                // when re-ordering items
                metadata.PinOrder = pinnedItems * 2;
            }

            RefreshPinnedDocumentMetadataAndAssignPinOrder();
        }

        private void RefreshPinnedDocumentMetadataAndAssignPinOrder()
        {
            PinnedDocumentMetadata.Refresh();

            // Assign PinOrder for each DocumentMetadata, based on the order
            // presented in the refreshed view

            var index = 0;

            foreach (var metadata in PinnedDocumentMetadata.Cast<DocumentMetadata>())
            {
                metadata.PinOrder = index * 2;
                index++;
            }
        }
    }
}