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
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using static WorkingFilesList.ToolWindow.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.ToolWindow.Test.ViewModel
{
    [TestFixture]
    public class DocumentMetadataManagerTests
    {
        private static Document CreateDocument(
            string fullName,
            bool nullActiveWindow = false)
        {
            var info = new DocumentMetadataInfo
            {
                FullName = fullName,
                ProjectDisplayName = string.Empty,
                ProjectUniqueName = string.Empty
            };

            var document = CreateDocumentWithInfo(info, nullActiveWindow);
            return document;
        }

        [Test]
        public void AddAppendsDocumentMetadataToListIfFullPathDoesNotExist()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Add(info);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].FullName, Is.EqualTo(info.FullName));
        }

        [Test]
        public void AddDoesNotAppendDocumentMetadataToListIfFullPathAlreadyExist()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Add(info);
            manager.Add(info);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].FullName, Is.EqualTo(info.FullName));
        }

        [Test]
        public void SynchronizeAddsDocumentsMissingInTarget()
        {
            // Arrange

            const string document1Name = "Document1Name";
            const string document2Name = "Document2Name";

            var documentMockList = new List<Document>
            {
                CreateDocument(document1Name),
                CreateDocument(document2Name)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Synchronize(documents);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(2));

            var document1 = collection.SingleOrDefault(m => m.FullName == document1Name);
            var document2 = collection.SingleOrDefault(m => m.FullName == document2Name);

            Assert.That(document1, Is.Not.Null);
            Assert.That(document2, Is.Not.Null);
        }

        [Test]
        public void SynchronizeRemovesDocumentMissingInSource()
        {
            // Arrange

            const string documentToRemove = "DocumentToRemove";
            const string documentToRetain = "DocumentToRetain";

            var documentMockList = new List<Document>
            {
                CreateDocument(documentToRemove),
                CreateDocument(documentToRetain)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Synchronize to set two items in the document metadata service
            // metadata list

            manager.Synchronize(documents);

            var updatedDocumentMockList = new List<Document>
            {
                CreateDocument(documentToRetain)
            };

            // Synchronizing with the updated list should remove one item

            var updatedDocuments = CreateDocuments(updatedDocumentMockList);

            // Act

            manager.Synchronize(updatedDocuments);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));

            var remove = collection.SingleOrDefault(m => m.FullName == documentToRemove);
            var retain = collection.SingleOrDefault(m => m.FullName == documentToRetain);

            Assert.That(remove, Is.Null);
            Assert.That(retain, Is.Not.Null);
        }

        [Test]
        public void DocumentsAddedBySynchronizeSetActivatedAt()
        {
            // Arrange

            var activatedAt = DateTime.UtcNow;

            var documentMockList = new List<Document>
            {
                CreateDocument(string.Empty)
            };

            var builder = new DocumentMetadataManagerBuilder();
            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(activatedAt);

            var manager = builder.CreateDocumentMetadataManager();
            var documents = CreateDocuments(documentMockList);

            // Act

            manager.Synchronize(documents);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document = collection.Single();

            Assert.That(document.ActivatedAt, Is.EqualTo(activatedAt));
        }

        [Test]
        public void SynchronizeDoesNotAddDocumentIfActiveWindowIsNull()
        {
            // Arrange

            const string documentName = "DocumentName";

            var documentMockList = new List<Document>
            {
                CreateDocument(documentName, true)
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            var documents = CreateDocuments(documentMockList);

            // Act

            manager.Synchronize(documents);

            // Assert

            Assert.That(manager.ActiveDocumentMetadata.IsEmpty);
        }

        [Test]
        public void ActivateOnlySetsActivatedTimeForMetadataWithMatchingPath()
        {
            // Arrange

            const string document1Name = "Document1Name";
            const string document2Name = "Document2Name";
            var utcNow = DateTime.UtcNow;

            var documentMockList = new List<Document>
            {
                CreateDocument(document1Name),
                CreateDocument(document2Name)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() =>
                {
                    // Simulate time passing
                    utcNow = utcNow + TimeSpan.FromSeconds(1);
                    return utcNow;
                });

            manager.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document1 = collection.Single(m => m.FullName == document1Name);
            var document2 = collection.Single(m => m.FullName == document2Name);

            var document1InitialActivationTime = document1.ActivatedAt;
            var document2InitialActivationTime = document2.ActivatedAt;

            // Act

            manager.Activate(document1Name);

            // Assert

            // Three times: twice during setup, once on update
            builder.TimeProviderMock.Verify(t => t.UtcNow, Times.Exactly(3));

            Assert.That(
                document1.ActivatedAt,
                Is.GreaterThan(document1InitialActivationTime));

            Assert.That(
                document2.ActivatedAt,
                Is.EqualTo(document2InitialActivationTime));
        }

        [Test]
        public void ActivateDoesNotThrowExceptionIfFullNameDoesNotExist()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            // Assert

            Assert.DoesNotThrow(() => manager.Activate("Document"));
        }

        [Test]
        public void UpdateFullNameDoesNotAlterActivatedAtTime()
        {
            // Arrange

            const string documentOldName = "DocumentOldName";
            const string documentNewName = "DocumentNewName";
            var runTime = DateTime.UtcNow;

            var documentMockList = new List<Document>
            {
                CreateDocument(documentOldName)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() =>
                {
                    var simulatedTime = runTime + TimeSpan.FromSeconds(1);
                    runTime = simulatedTime;
                    return simulatedTime;
                });

            manager.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document = collection.Single(m => m.FullName == documentOldName);
            var document1InitialActivationTime = document.ActivatedAt;

            // Act

            manager.UpdateFullName(documentNewName, documentOldName);

            // Assert

            document = collection.Single(m => m.FullName == documentNewName);

            Assert.That(
                document.ActivatedAt,
                Is.EqualTo(document1InitialActivationTime));
        }

        [Test]
        public void UpdateFullNameUpdatesSpecifiedDocumentOnly()
        {
            // Arrange

            const string document1OldName = "Document1OldName";
            const string document1NewName = "Document1NewName";
            const string document2Name = "Document2Name";

            var documentMockList = new List<Document>
            {
                CreateDocument(document1OldName),
                CreateDocument(document2Name)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            manager.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document2 = collection.Single(m => m.FullName == document2Name);

            // Act

            manager.UpdateFullName(document1NewName, document1OldName);

            // Assert

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document1OldName),
                Is.Null);

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document1NewName),
                Is.Not.Null);

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document2Name),
                Is.Not.Null);

            Assert.That(
                document2.FullName,
                Is.EqualTo(document2Name));
        }

        [Test]
        public void UpdateFullNameDoesNotThrowExceptionIfOldNameDoesNotExist()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Assert

            Assert.DoesNotThrow(() => manager.UpdateFullName(
                "NewDocumentName",
                "OldDocumentName"));
        }

        [Test]
        public void SynchronizeHandlesComException()
        {
            // Arrange

            var documentsMock = new Mock<Documents>();

            // COMException is thrown in Synchronize when a project is closed
            // in Visual Studio

            documentsMock.Setup(d => d.GetEnumerator())
                .Callback(() => { throw new COMException(); });

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Assert

            Assert.DoesNotThrow(() => manager.Synchronize(documentsMock.Object));
        }

        [Test]
        public void AddUsesDocumentMetadataFactory()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName",
                ProjectDisplayName = "ProjectDisplayName",
                ProjectUniqueName = "ProjectUniqueName"
            };

            var factoryMock = new Mock<IDocumentMetadataFactory>();

            factoryMock
                .Setup(f => f.Create(info))
                .Returns(new DocumentMetadata(info, string.Empty));

            var builder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factoryMock.Object
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Add(info);

            // Assert

            factoryMock.Verify(p => p.Create(info));
        }

        [Test]
        public void SynchronizeUsesDocumentMetadataFactory()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName",
                ProjectDisplayName = "ProjectDisplayName",
                ProjectUniqueName = "ProjectUniqueName"
            };

            var documentMockList = new List<Document>
            {
                CreateDocumentWithInfo(info)
            };

            var documents = CreateDocuments(documentMockList);
            var factoryMock = new Mock<IDocumentMetadataFactory>();

            factoryMock
                .Setup(f => f.Create(It.IsAny<DocumentMetadataInfo>()))
                .Returns(new DocumentMetadata(
                    info,
                    "CorrectedFullName"));

            var builder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factoryMock.Object
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Synchronize(documents);

            // Assert

            factoryMock.Verify(p => p.Create(info));
        }

        [Test]
        public void UpdatingPathSegmentCountUpdatesDisplayName()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = @"C:\Folder\Document.txt"
            };
            
            var builder = new DocumentMetadataManagerBuilder();
            builder.UserPreferencesBuilder.StoredSettingsRepositoryMock
                .Setup(s => s.GetPathSegmentCount())
                .Returns(1);

            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            // Act

            builder.UserPreferences.PathSegmentCount = 3;

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].DisplayName, Is.EqualTo(info.FullName));
        }

        [Test]
        public void UpdatingSelectedSortAddsToDocumentMetadataSortDescriptions()
        {
            // Arrange

            const string propertyName = "PropertyName";
            const ListSortDirection sortDirection = ListSortDirection.Descending;

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            builder.UserPreferences.SelectedDocumentSortOption = new TestingSortOption(
                "Display Name",
                propertyName,
                sortDirection,
                ProjectItemType.Document);

            // Assert

            var addedSortOption = manager.ActiveDocumentMetadata.SortDescriptions
                .Single(s =>
                    s.PropertyName == propertyName &&
                    s.Direction == sortDirection);

            Assert.That(addedSortOption, Is.Not.Null);
        }

        [Test]
        public void UpdatingSelectedSortOptionTwiceOnlyAddsDescriptionOnce()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            builder.UserPreferences.SelectedProjectSortOption = Mock.Of<ISortOption>(s =>
                s.HasSortDescription == false);

            // Act

            builder.UserPreferences.SelectedDocumentSortOption = new TestingSortOption(
                "Display Name",
                "PropertyName",
                ListSortDirection.Descending,
                ProjectItemType.Document);

            builder.UserPreferences.SelectedDocumentSortOption = new TestingSortOption(
                "Display Name 2",
                "PropertyName",
                ListSortDirection.Descending,
                ProjectItemType.Document);

            // Assert

            var sortOptionCount = manager.ActiveDocumentMetadata.SortDescriptions.Count;
            Assert.That(sortOptionCount, Is.EqualTo(1));
        }

        [Test]
        public void ActivateRefreshesDocumentMetadataView()
        {
            // Arrange

            const string documentName = "DocumentName";

            var documentMockList = new List<Document>
            {
                CreateDocument(documentName)
            };

            var documents = CreateDocuments(documentMockList);

            var collectionViewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var generatorMock = new Mock<ICollectionViewGenerator>();

            generatorMock
                .Setup(c => c.CreateView(It.IsAny<IList>()))
                .Returns(collectionViewMock.Object);

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>();
            var mapping = new TestingUpdateReactionMapping(mappingTable);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generatorMock.Object,
                UpdateReactionMapping = mapping
            };

            var manager = builder.CreateDocumentMetadataManager();
            manager.Synchronize(documents);

            // Act

            manager.Activate(documentName);

            // Assert

            collectionViewMock.Verify(c => c.Refresh());
        }

        [Test]
        public void SortDescriptionsAreSetOnManagerInitialization()
        {
            // Arrange

            const string propertyName = "PropertyName";
            const ListSortDirection sortDirection = ListSortDirection.Descending;

            var builder = new DocumentMetadataManagerBuilder();

            var sortOptionsServiceMock = new Mock<ISortOptionsService>();

            sortOptionsServiceMock
                .Setup(s => s.EvaluateAppliedSortDescriptions(It.IsAny<IUserPreferences>()))
                .Returns(new[] { new SortDescription(propertyName, sortDirection) });

            builder.SortOptionsService = sortOptionsServiceMock.Object;

            // Act

            var manager = builder.CreateDocumentMetadataManager();

            // Assert

            var addedSortOption = manager.ActiveDocumentMetadata.SortDescriptions
                .Single(s =>
                    s.PropertyName == propertyName &&
                    s.Direction == sortDirection);

            Assert.That(addedSortOption, Is.Not.Null);
        }

        [Test]
        public void ActivateSetsSpecifiedDocumentExclusivelyActive()
        {
            // Arrange

            const string document1Name = "Document1";
            const string document2Name = "Document2";
            const string document3Name = "Document3";

            var documentMockList = new List<Document>
            {
                CreateDocument(document1Name),
                CreateDocument(document2Name),
                CreateDocument(document3Name)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document1 = collection.Single(m => m.FullName == document1Name);
            document1.IsActive = true;

            // Act

            manager.Activate(document3Name);

            // Assert

            Assert.IsFalse(document1.IsActive);

            var document2 = collection.Single(m => m.FullName == document2Name);
            Assert.IsFalse(document2.IsActive);

            var document3 = collection.Single(m => m.FullName == document3Name);
            Assert.IsTrue(document3.IsActive);
        }

        [Test]
        public void ActivateAssignsUsageOrder()
        {
            // Arrange

            const string documentName = "DocumentName";
            IList metadataCollection = null;

            var documentMockList = new List<Document>
            {
                CreateDocument(documentName)
            };

            var documents = CreateDocuments(documentMockList);

            var collectionViewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var generatorMock = new Mock<ICollectionViewGenerator>();

            generatorMock
                .Setup(c => c.CreateView(It.IsAny<IList>()))
                .Callback<IList>(mc => metadataCollection = mc)
                .Returns(collectionViewMock.Object);

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>();
            var mapping = new TestingUpdateReactionMapping(mappingTable);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generatorMock.Object,
                UpdateReactionMapping = mapping
            };

            var manager = builder.CreateDocumentMetadataManager();
            manager.Synchronize(documents);

            // Act

            manager.Activate(documentName);

            // Assert

            Assert.That(metadataCollection, Is.Not.Null);

            builder.NormalizedUsageOrderServiceMock
                .Verify(n => n.SetUsageOrder(
                    (IList<DocumentMetadata>) metadataCollection,
                    It.IsAny<IUserPreferences>()));
        }

        /// <summary>
        /// There are certain conditions where <see cref="Document"/> provides
        /// <see cref="Document.FullName"/> in lower case. When a project item
        /// is renamed, its full name is provided with the correct casing. In
        /// order for the names to match, it needs to be compared against
        /// <see cref="DocumentMetadata.CorrectedFullName"/>
        /// </summary>
        [Test]
        public void UpdateFullNameMatchesOldNameWithCorrectedFullName()
        {
            // Arrange

            const string oldName = "OldName";
            const string newName = "NewName";
            const string correctedOldName = "CorrectedOldName";

            var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();

            metadataFactoryBuilder.PathCasingRestorerMock
                .Setup(p => p.RestoreCasing(It.IsAny<string>()))
                .Returns(correctedOldName);

            var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(false);

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factory
            };

            metadataManagerBuilder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();

            var documentMockList = new List<Document>
            {
                CreateDocument(oldName)
            };

            var documents = CreateDocuments(documentMockList);
            manager.Synchronize(documents);

            // Act

            manager.UpdateFullName(newName, correctedOldName);

            // Assert

            var collection =
                (IList<DocumentMetadata>) manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].FullName, Is.EqualTo(newName));
        }
    }
}
