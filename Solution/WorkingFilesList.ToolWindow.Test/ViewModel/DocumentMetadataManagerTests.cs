// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 - 2019 Anthony Fung and The Working Files List Project contributors

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
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.Core.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.Interface;
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
                ProjectFullName = string.Empty
            };

            var document = CreateDocumentWithInfo(info, nullActiveWindow);
            return document;
        }

        private static IList<Document> CreateDocumentList(params string[] fullNames)
        {
            var documents = fullNames.Select(f => CreateDocument(f)).ToList();
            return documents;
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
        public void AddSetsHasWindowTrueWhenAppendingDocumentMetadataToList()
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
            Assert.IsTrue(collection[0].HasWindow);
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
        public void AddSetsHasWindowTrueWhenInvokedWithExistingDocumentMetadata()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();
            metadata.HasWindow = false;

            // Act
            
            manager.Add(info);

            // Assert

            Assert.That(collection.Single(), Is.EqualTo(metadata));
            Assert.IsTrue(metadata.HasWindow);
        }

        [Test]
        public void AddRefreshesActiveDocumentMetadata()
        {
            // Arrange

            var generator = new Mock<ICollectionViewGenerator>();

            generator
                .Setup(g => g.CreateView(It.IsAny<IList>()))
                .Returns<IList>(l => new Mock<ICollectionView>
                {
                    DefaultValue = DefaultValue.Mock
                }.Object);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generator.Object,
                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Add(new DocumentMetadataInfo());

            // Assert

            Mock.Get(manager.ActiveDocumentMetadata).Verify(a => a.Refresh());

            Mock.Get(manager.PinnedDocumentMetadata).Verify(p =>
                p.Refresh(),
                Times.Never);
        }

        [Test]
        public void AddPinnedSetsIsPinnedTrue()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.AddPinned(new DocumentMetadataInfo());

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.AreEqual(1, collection.Count);
            Assert.AreNotEqual(DocumentMetadata.UnpinnedOrderValue, collection[0].PinOrder);
            Assert.IsTrue(collection[0].IsPinned);
        }

        [Test]
        public void AddPinnedAppendsDocumentMetadataToListIfFullPathDoesNotExist()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.AddPinned(info);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(info.FullName, collection[0].FullName);
        }

        [Test]
        public void AddPinnedSetsHasWindowFalseWhenAppendingDocumentMetadataToList()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.AddPinned(info);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.AreEqual(1, collection.Count);
            Assert.IsFalse(collection[0].HasWindow);
        }

        [Test]
        public void AddPinnedDoesNotAppendDocumentMetadataToListIfFullPathAlreadyExist()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.AddPinned(info);
            manager.AddPinned(info);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.AreEqual(1, collection.Count);
            Assert.That(collection[0].FullName, Is.EqualTo(info.FullName));
        }

        [Test]
        public void AddPinnedSetsHasWindowTrueWhenInvokedWithExistingDocumentMetadata()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();
            metadata.HasWindow = false;

            // Act

            manager.AddPinned(info);

            // Assert

            Assert.AreEqual(metadata, collection.Single());
            Assert.IsTrue(metadata.HasWindow);
        }

        [Test]
        public void AddPinnedRefreshesDocumentMetadataViewsWhenAppendingMetadata()
        {
            // Arrange

            var generator = new Mock<ICollectionViewGenerator>();

            generator
                .Setup(g => g.CreateView(It.IsAny<IList>()))
                .Returns<IList>(l => new Mock<ICollectionView>
                {
                    DefaultValue = DefaultValue.Mock
                }.Object);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generator.Object,
                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.AddPinned(new DocumentMetadataInfo());

            // Assert

            Mock.Get(manager.ActiveDocumentMetadata).Verify(a => a.Refresh());
            Mock.Get(manager.PinnedDocumentMetadata).Verify(p => p.Refresh());
        }

        [Test]
        public void AddPinnedRefreshesOnlyPinnedDocumentMetadataWhenPinningExistingMetadata()
        {
            // Arrange

            var generator = new Mock<ICollectionViewGenerator>();

            generator
                .Setup(g => g.CreateView(It.IsAny<IList>()))
                .Returns<IList>(l => new Mock<ICollectionView>
                {
                    DefaultValue = DefaultValue.Mock
                }.Object);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generator.Object,
                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            // Act

            manager.Add(info);

            Mock.Get(manager.ActiveDocumentMetadata).Invocations.Clear();
            Mock.Get(manager.PinnedDocumentMetadata).Invocations.Clear();

            manager.AddPinned(info);

            // Assert

            Mock.Get(manager.ActiveDocumentMetadata).Verify(a =>
                a.Refresh(),
                Times.Never);

            Mock.Get(manager.PinnedDocumentMetadata).Verify(p => p.Refresh());
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

            manager.Synchronize(documents, false);

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

            manager.Synchronize(documents, false);

            var updatedDocumentMockList = new List<Document>
            {
                CreateDocument(documentToRetain)
            };

            // Synchronizing with the updated list should remove one item

            var updatedDocuments = CreateDocuments(updatedDocumentMockList);

            // Act

            manager.Synchronize(updatedDocuments, false);

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
        public void SynchronizeSetsHasWindowFalseForPinnedItemsCorrespondingToExistingRemovedDocuments()
        {
            // Arrange

            const string remove = "Remove";
            const string retain = "Retain";

            var documentMockList = CreateDocumentList(remove, retain);
            var documents = CreateDocuments(documentMockList);

            var builder = new DocumentMetadataManagerBuilder();

            builder.ProjectItemServiceMock
                .Setup(p => p.FindProjectItem(It.IsAny<string>()))
                .Returns(Mock.Of<ProjectItem>());

            var manager = builder.CreateDocumentMetadataManager();

            // Synchronize to set two items in the document metadata service
            // metadata list

            manager.Synchronize(documents, false);

            // Synchronizing with the updated list should remove one item
            var updatedDocumentList = CreateDocumentList(retain);
            var updatedDocuments = CreateDocuments(updatedDocumentList);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var removed = collection.Single(m => m.FullName == remove);
            manager.TogglePinnedStatus(removed);

            var retained = collection.Single(m => m.FullName == retain);

            // Act

            manager.Synchronize(updatedDocuments, false);

            // Assert

            Assert.IsTrue(collection.Contains(removed));
            Assert.IsTrue(collection.Contains(retained));

            Assert.IsFalse(removed.HasWindow);
            Assert.IsTrue(retained.HasWindow);
        }

        [Test]
        public void SynchronizeRemovesPinnedItemsCorrespondingToNonExistentRemovedDocuments()
        {
            // Arrange

            const string remove = "Remove";
            const string retain = "Retain";

            var documentMockList = CreateDocumentList(remove, retain);

            var documents = Mock.Of<Documents>(d =>
                d.GetEnumerator() == documentMockList.GetEnumerator() &&
                d.DTE == Mock.Of<DTE>(dte =>
                    dte.Solution == Mock.Of<Solution>()));

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Synchronize to set two items in the document metadata service
            // metadata list

            manager.Synchronize(documents, false);

            // Synchronizing with the updated list should remove one item
            var updatedDocumentMockList = CreateDocumentList(retain);

            var updatedDocuments = Mock.Of<Documents>(d =>
                d.GetEnumerator() == updatedDocumentMockList.GetEnumerator() &&
                d.DTE == Mock.Of<DTE>(dte =>
                    dte.Solution == Mock.Of<Solution>()));

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var removed = collection.Single(m => m.FullName == remove);
            manager.TogglePinnedStatus(removed);

            var retained = collection.Single(m => m.FullName == retain);

            // Act

            manager.Synchronize(updatedDocuments, false);

            // Assert

            Assert.IsFalse(collection.Contains(removed));
            Assert.IsTrue(collection.Contains(retained));

            Assert.IsTrue(retained.HasWindow);
        }

        [Test]
        public void SynchronizeComparesAllDocumentMetadataInfoPropertiesWhenSynchronizing()
        {
            // Arrange

            const string document1Name = "Document1Name";
            const string document2Name = "Document2Name";
            const string originalProjectDisplayName = "OriginalProjectDisplayName";
            const string originalProjectFullName = "OriginalProjectFullName";
            const string updatedProjectDisplayName = "UpdatedProjectDisplayName";
            const string updatedProjectFullName = "UpdatedProjectFullName";

            var info1 = new DocumentMetadataInfo
            {
                FullName = document1Name,
                ProjectDisplayName = originalProjectDisplayName,
                ProjectFullName = originalProjectFullName
            };

            var documentMockList = new List<Document>
            {
                CreateDocumentWithInfo(info1),
                CreateDocumentWithInfo(new DocumentMetadataInfo
                {
                    FullName = document2Name,
                    ProjectDisplayName = originalProjectDisplayName,
                    ProjectFullName = originalProjectFullName
                })
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            // Synchronize to set two items in the document metadata service
            // metadata list

            manager.Synchronize(documents, false);

            var updatedDocumentMockList = new List<Document>
            {
                CreateDocumentWithInfo(info1),
                CreateDocumentWithInfo(new DocumentMetadataInfo
                {
                    FullName = document2Name,
                    ProjectDisplayName = updatedProjectDisplayName,
                    ProjectFullName = updatedProjectFullName
                })
            };

            // Synchronizing with the updated list should update project
            // properties of second document metadata

            var updatedDocuments = CreateDocuments(updatedDocumentMockList);

            // Act

            manager.Synchronize(updatedDocuments, false);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(2));

            var document1 = collection.SingleOrDefault(m =>
                m.FullName == document1Name &&
                m.ProjectNames.DisplayName == originalProjectDisplayName &&
                m.ProjectNames.FullName == originalProjectFullName);

            var document2OriginalProject = collection.SingleOrDefault(m =>
                m.FullName == document2Name &&
                m.ProjectNames.DisplayName == originalProjectDisplayName &&
                m.ProjectNames.FullName == originalProjectFullName);

            var document2UpdatedProject = collection.SingleOrDefault(m =>
                m.FullName == document2Name &&
                m.ProjectNames.DisplayName == updatedProjectDisplayName &&
                m.ProjectNames.FullName == updatedProjectFullName);

            Assert.That(document1, Is.Not.Null);
            Assert.That(document2UpdatedProject, Is.Not.Null);
            Assert.That(document2OriginalProject, Is.Null);
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

            manager.Synchronize(documents, false);

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

            manager.Synchronize(documents, false);

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

            manager.Synchronize(documents, false);

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

            manager.Synchronize(documents, false);

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

            manager.Synchronize(documents, false);

            // Act

            manager.UpdateFullName(document1NewName, document1OldName);

            // Assert

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document1OldName),
                Is.Null);

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document1NewName),
                Is.Not.Null);

            Assert.That(
                collection.SingleOrDefault(m => m.FullName == document2Name),
                Is.Not.Null);

            var document2 = collection.Single(m => m.FullName == document2Name);
            Assert.That(document2.FullName, Is.EqualTo(document2Name));
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

            Assert.DoesNotThrow(() => manager.Synchronize(documentsMock.Object, false));
        }

        [Test]
        public void AddUsesDocumentMetadataFactory()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName",
                ProjectDisplayName = "ProjectDisplayName",
                ProjectFullName = "ProjectFullName"
            };

            var factoryMock = new Mock<IDocumentMetadataFactory>();

            factoryMock
                .Setup(f => f.Create(info))
                .Returns(new DocumentMetadata(info, string.Empty, null));

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
                ProjectFullName = "ProjectFullName"
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
                    "CorrectedFullName",
                    null));

            var builder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factoryMock.Object
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            manager.Synchronize(documents, false);

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

            const string displayName = "DisplayName";
            const string propertyName = "PropertyName";
            const ListSortDirection sortDirection = ListSortDirection.Descending;

            var builder = new DocumentMetadataManagerBuilder();

            builder.UserPreferencesBuilder.SortOptions = new List<ISortOption>
            {
                new TestingSortOption(
                    displayName,
                    propertyName,
                    sortDirection,
                    ProjectItemType.Document)
            };

            var manager = builder.CreateDocumentMetadataManager();

            // Act

            builder.UserPreferences.DocumentSortOptionName = displayName;

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

            const string defaultSortOption = "DefaultSortOption";
            const string propertyName = "PropertyName";
            const string sortOption1 = "SortOption1";
            const string sortOption2 = "SortOption2";
            const ListSortDirection direction = ListSortDirection.Descending;
            const ProjectItemType applicableType = ProjectItemType.Document;

            var builder = new DocumentMetadataManagerBuilder();

            builder.UserPreferencesBuilder.SortOptions = new[]
            {
                Mock.Of<ISortOption>(s =>
                    s.DisplayName == defaultSortOption &&
                    s.HasSortDescription == false),

                new TestingSortOption(
                    sortOption1,
                    propertyName,
                    direction,
                    applicableType),

                new TestingSortOption(
                    sortOption2,
                    propertyName,
                    direction,
                    applicableType)
            };

            var manager = builder.CreateDocumentMetadataManager();
            builder.UserPreferences.ProjectSortOptionName = defaultSortOption;

            // Act

            builder.UserPreferences.DocumentSortOptionName = sortOption1;
            builder.UserPreferences.DocumentSortOptionName = sortOption2;

            // Assert

            var sortOptionCount = manager.ActiveDocumentMetadata.SortDescriptions.Count;
            Assert.That(sortOptionCount, Is.EqualTo(1));
        }

        [Test]
        public void ActivateRefreshesDocumentMetadataViews()
        {
            // Arrange

            const string documentName = "DocumentName";

            var documentMockList = new List<Document>
            {
                CreateDocument(documentName)
            };

            var documents = CreateDocuments(documentMockList);
            var collectionViewMocks = new List<Mock<ICollectionView>>();
            var generatorMock = new Mock<ICollectionViewGenerator>();

            generatorMock
                .Setup(c => c.CreateView(It.IsAny<IList>()))
                .Returns<IList>(data =>
                {
                    var viewMock = new Mock<ICollectionView>
                    {
                        DefaultValue = DefaultValue.Mock
                    };

                    collectionViewMocks.Add(viewMock);
                    return viewMock.Object;
                });

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>();
            var mapping = new TestingUpdateReactionMapping(mappingTable);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generatorMock.Object,
                UpdateReactionMapping = mapping
            };

            var manager = builder.CreateDocumentMetadataManager();
            manager.Synchronize(documents, false);

            // Act

            manager.Activate(documentName);

            // Assert

            // ActiveDocumentMetadata, PinnedDocumentMetadata
            Assert.That(collectionViewMocks.Count, Is.EqualTo(2));

            collectionViewMocks[0].Verify(c => c.Refresh());
            collectionViewMocks[1].Verify(c => c.Refresh());
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

            manager.Synchronize(documents, false);

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
            manager.Synchronize(documents, false);

            // Act

            manager.Activate(documentName);

            // Assert

            Assert.That(metadataCollection, Is.Not.Null);

            builder.NormalizedUsageOrderServiceMock
                .Verify(n => n.SetUsageOrder(
                    (IList<DocumentMetadata>) metadataCollection,
                    It.IsAny<IUserPreferences>()));
        }

        [Test]
        public void ActivateAssignsUsageOrderAfterRemovingMetadataForSameItem()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName"
            };

            var generatorMock = new Mock<ICollectionViewGenerator>
            {
                DefaultValue = DefaultValue.Mock
            };

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>();
            var mapping = new TestingUpdateReactionMapping(mappingTable);

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = generatorMock.Object,
                UpdateReactionMapping = mapping
            };

            // add item and activate for the first time
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);
            manager.Activate(info.FullName);

            // remove item
            var emptyDocuments = CreateDocuments(new List<Document>());
            manager.Synchronize(emptyDocuments, false);

            // add item again
            manager.Add(info);
            builder.NormalizedUsageOrderServiceMock.Invocations.Clear();

            // Act

            manager.Activate(info.FullName);

            // Assert

            builder.NormalizedUsageOrderServiceMock
                .Verify(n => n.SetUsageOrder(
                    It.IsAny<IList<DocumentMetadata>>(),
                    It.IsAny<IUserPreferences>()));
        }

        [Test]
        public void UpdateFullNameMatchesOldNameWithFullName()
        {
            // Arrange

            const string oldName = "OldName";
            const string newName = "NewName";

            var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();
            var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(true);

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
            manager.Synchronize(documents, false);

            // Act

            manager.UpdateFullName(newName, oldName);

            // Assert

            var collection =
                (IList<DocumentMetadata>) manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].FullName, Is.EqualTo(newName));
        }

        [Test]
        public void UpdateFullNameAssignsProjectDisplayName()
        {
            // Arrange

            const string oldName = "OldName";
            const string projectDisplayName = "ProjectDisplayName";

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder();
            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var info = new DocumentMetadataInfo
            {
                FullName = oldName,
                ProjectDisplayName = projectDisplayName
            };

            var oldMetadata = new DocumentMetadata(info, string.Empty, null);
            collection.Add(oldMetadata);

            // Act

            manager.UpdateFullName("NewName", oldName);

            // Assert

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].ProjectNames.DisplayName, Is.EqualTo(projectDisplayName));
        }

        [Test]
        public void UpdateFullNameAssignsProjectFullName()
        {
            // Arrange

            const string oldName = "OldName";
            const string projectFullName = "ProjectFullName";

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder();
            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var info = new DocumentMetadataInfo
            {
                FullName = oldName,
                ProjectFullName = projectFullName
            };

            var oldMetadata = new DocumentMetadata(info, string.Empty, null);
            collection.Add(oldMetadata);

            // Act

            manager.UpdateFullName("NewName", oldName);

            // Assert

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].ProjectNames.FullName, Is.EqualTo(projectFullName));
        }

        [Test]
        public void UpdateFullNameAssignsPinOrder()
        {
            // Arrange

            const int pinOrder = 6;
            const string oldName = "OldName";

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder();
            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var info = new DocumentMetadataInfo
            {
                FullName = oldName
            };

            var oldMetadata = new DocumentMetadata(info, string.Empty, null)
            {
                PinOrder = pinOrder
            };

            collection.Add(oldMetadata);

            // Act

            manager.UpdateFullName("NewName", oldName);

            // Assert

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].PinOrder, Is.EqualTo(pinOrder));
            Assert.IsTrue(collection[0].IsPinned);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UpdateFullNameAssignsHasWindow(bool hasWindow)
        {
            // Arrange

            const string oldName = "OldName";

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder();
            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var info = new DocumentMetadataInfo
            {
                FullName = oldName
            };

            var oldMetadata = new DocumentMetadata(info, string.Empty, null)
            {
                HasWindow = hasWindow
            };

            collection.Add(oldMetadata);

            // Act

            manager.UpdateFullName("NewName", oldName);

            // Assert

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.AreEqual(hasWindow, collection[0].HasWindow);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SynchronizeUsesNormalizedUsageOrderService(bool setUsageOrder)
        {
            // Arrange

            var setUsageOrderInvoked = false;

            var builder = new DocumentMetadataManagerBuilder();

            builder.NormalizedUsageOrderServiceMock
                .Setup(n => n.SetUsageOrder(
                    It.IsAny<IList<DocumentMetadata>>(),
                    It.IsAny<IUserPreferences>()))
                .Callback(() => setUsageOrderInvoked = true);

            builder.UpdateReactionMapping = new TestingUpdateReactionMapping(
                new Dictionary<string, IEnumerable<IUpdateReaction>>());

            var manager = builder.CreateDocumentMetadataManager();

            var documentsMock = new Mock<Documents>
            {
                DefaultValue = DefaultValue.Mock
            };

            // Act

            manager.Synchronize(documentsMock.Object, setUsageOrder);

            // Assert

            Assert.That(setUsageOrderInvoked, Is.EqualTo(setUsageOrder));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SynchronizeRefreshesActiveDocumentMetadata(bool setUsageOrder)
        {
            // Arrange

            var viewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = Mock.Of<ICollectionViewGenerator>(c =>
                    c.CreateView(It.IsAny<IList>()) == viewMock.Object),

                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            var documentsMock = new Mock<Documents>
            {
                DefaultValue = DefaultValue.Mock
            };

            // Act

            manager.Synchronize(documentsMock.Object, setUsageOrder);

            // Assert

            Mock.Get(manager.ActiveDocumentMetadata).Verify(a => a.Refresh());
        }

        [Test]
        public void UpdateFullNameSetsUsageOrder()
        {
            // Arrange

            const string oldName = "OldName";

            var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();
            var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(true);

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
            manager.Synchronize(documents, false);

            // Act

            manager.UpdateFullName("NewName", oldName);

            // Assert

            metadataManagerBuilder.NormalizedUsageOrderServiceMock
                .Verify(n => n.SetUsageOrder(
                    It.IsAny<IList<DocumentMetadata>>(),
                    It.IsAny<IUserPreferences>()));
        }

        [Test]
        public void UpdateFullNameReturnsFalseIfMetadataNotUpdated()
        {
            // Arrange

            var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();
            var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(true);

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factory
            };

            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();
            var documentMockList = CreateDocumentList();
            var documents = CreateDocuments(documentMockList);
            manager.Synchronize(documents, false);

            // Act

            var updated = manager.UpdateFullName("NewName", "NonExistentName");

            // Assert

            Assert.IsFalse(updated);
        }

        [Test]
        public void UpdateFullNameReturnsTrueIfMetadataUpdated()
        {
            // Arrange

            const string oldName = "OldName";

            var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();
            var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(true);

            var metadataManagerBuilder = new DocumentMetadataManagerBuilder
            {
                DocumentMetadataFactory = factory
            };

            var manager = metadataManagerBuilder.CreateDocumentMetadataManager();
            var documentMockList = CreateDocumentList(oldName);
            var documents = CreateDocuments(documentMockList);
            manager.Synchronize(documents, false);

            // Act

            var updated = manager.UpdateFullName("NewName", oldName);

            // Assert

            Assert.IsTrue(updated);
        }

        [Test]
        public void AlreadyActiveDocumentIsNotReactivated()
        {
            // Arrange

            const string document1Name = "Document1";
            var utcNow = DateTime.UtcNow;

            var documentMockList = new List<Document>
            {
                CreateDocument(document1Name)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() =>
                {
                    // Simulate time passing
                    utcNow = utcNow + TimeSpan.FromSeconds(1);
                    return utcNow;
                });

            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents, false);
            manager.Activate(document1Name);
            var firstActivationTime = utcNow;

            // Act

            manager.Activate(document1Name);

            // Assert

            builder.NormalizedUsageOrderServiceMock.Verify(n => n
                .SetUsageOrder(
                    It.IsAny<IList<DocumentMetadata>>(),
                    It.IsAny<IUserPreferences>()),
                Times.Once);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0].ActivatedAt, Is.EqualTo(firstActivationTime));
        }

        [Test]
        public void PinnedDocumentMetadataContainsMetadataWithPinOrderGreaterThanMinusTwo()
        {
            // Arrange

            var documentMockList = new List<Document>
            {
                CreateDocument("document1"),
                CreateDocument("document2"),
                CreateDocument("document3")
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document1Metadata = activeMetadataCollection[0];
            document1Metadata.PinOrder = 0;

            var document2Metadata = activeMetadataCollection[1];
            document2Metadata.PinOrder = 2;

            var document3Metadata = activeMetadataCollection[2];

            // Act

            manager.PinnedDocumentMetadata.Refresh();

            // Assert

            Assert.IsTrue(manager.PinnedDocumentMetadata.Contains(document1Metadata));
            Assert.IsTrue(manager.PinnedDocumentMetadata.Contains(document2Metadata));
            Assert.IsFalse(manager.PinnedDocumentMetadata.Contains(document3Metadata));
        }

        [Test]
        public void TogglePinnedStatusSetsPinOrderToDoublePinnedItemCountWhenPinningItem()
        {
            // Arrange

            var documentMockList = new List<Document>
            {
                CreateDocument("document1"),
                CreateDocument("document2")
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document1Metadata = activeMetadataCollection[0];
            var document2Metadata = activeMetadataCollection[1];

            // Act

            manager.TogglePinnedStatus(document2Metadata);
            manager.TogglePinnedStatus(document1Metadata);

            // Assert

            Assert.That(document1Metadata.PinOrder, Is.EqualTo(2));
            Assert.That(document2Metadata.PinOrder, Is.EqualTo(0));
        }

        [Test]
        public void TogglePinnedStatusSetsPinOrderToMinusTwoWhenUnpinningItem()
        {
            // Arrange

            var documentMockList = new List<Document>
            {
                CreateDocument("document1")
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var document1Metadata = activeMetadataCollection[0];
            document1Metadata.PinOrder = 8;

            // Act

            manager.TogglePinnedStatus(document1Metadata);

            // Assert

            Assert.That(document1Metadata.PinOrder, Is.EqualTo(-2));
        }

        [Test]
        public void TogglePinnedStatusRefreshesPinnedDocumentMetadataWhenPinningItem()
        {
            // Arrange

            var viewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = Mock.Of<ICollectionViewGenerator>(c =>
                    c.CreateView(It.IsAny<IList>()) == viewMock.Object),

                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            var documentMetadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Act

            manager.TogglePinnedStatus(documentMetadata);

            // Assert

            viewMock.Verify(v => v.Refresh());
            Assert.That(documentMetadata.PinOrder, Is.EqualTo(0));
        }

        [Test]
        public void TogglePinnedStatusRefreshesPinnedDocumentMetadataWhenUnpinningItem()
        {
            // Arrange

            var viewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = Mock.Of<ICollectionViewGenerator>(c =>
                    c.CreateView(It.IsAny<IList>()) == viewMock.Object),

                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            var documentMetadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null)
            {
                PinOrder = 8
            };

            // Act

            manager.TogglePinnedStatus(documentMetadata);

            // Assert

            viewMock.Verify(v => v.Refresh());
            Assert.That(documentMetadata.PinOrder, Is.EqualTo(-2));
        }

        [Test]
        public void TogglePinnedStatusUpdatesItemOrderWhenUnpinningItem()
        {
            // Arrange

            var documentMockList = CreateDocumentList("d1", "d2");
            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata1 = activeMetadataCollection[0];
            metadata1.PinOrder = 0;

            var metadata2 = activeMetadataCollection[1];
            metadata2.PinOrder = 2;

            // Act

            manager.TogglePinnedStatus(metadata1);

            // Assert

            Assert.That(metadata2.PinOrder, Is.EqualTo(0));
        }

        [Test]
        public void MovePinnedItemUpdatesItemOrderWhenMovingItemsUpwards()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            var documentMockList = CreateDocumentList("d1", "d2", "d3", "d4");
            var documents = CreateDocuments(documentMockList);

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata1 = activeMetadataCollection[0];
            var metadata2 = activeMetadataCollection[1];
            var metadata3 = activeMetadataCollection[2];
            var metadata4 = activeMetadataCollection[3];

            foreach (var metadata in activeMetadataCollection)
            {
                manager.TogglePinnedStatus(metadata);
            }

            // Act

            manager.MovePinnedItem(metadata3, metadata1);

            // Assert

            var expectedOrder = new[]
            {
                metadata3, metadata1, metadata2, metadata4
            };

            const string separator = ", ";

            var expectedOrderNames = string.Join(
                separator,
                expectedOrder.Select(m => m.FullName));

            var actualOrderNames = string.Join(
                separator,
                manager.PinnedDocumentMetadata.Cast<DocumentMetadata>().Select(m =>
                    m.FullName));

            Assert.That(actualOrderNames, Is.EqualTo(expectedOrderNames));

            Assert.That(metadata1.PinOrder, Is.EqualTo(2));
            Assert.That(metadata2.PinOrder, Is.EqualTo(4));
            Assert.That(metadata3.PinOrder, Is.EqualTo(0));
            Assert.That(metadata4.PinOrder, Is.EqualTo(6));
        }

        [Test]
        public void MovePinnedItemUpdatesItemOrderWhenMovingItemsDownwards()
        {
            // Arrange

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            var documentMockList = CreateDocumentList("d1", "d2", "d3", "d4");
            var documents = CreateDocuments(documentMockList);

            manager.Synchronize(documents, false);

            var activeMetadataCollection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata1 = activeMetadataCollection[0];
            var metadata2 = activeMetadataCollection[1];
            var metadata3 = activeMetadataCollection[2];
            var metadata4 = activeMetadataCollection[3];

            foreach (var metadata in activeMetadataCollection)
            {
                manager.TogglePinnedStatus(metadata);
            }

            // Act

            manager.MovePinnedItem(metadata1, metadata3);

            // Assert

            var expectedOrder = new[]
            {
                metadata2, metadata3, metadata1, metadata4
            };

            const string separator = ", ";

            var expectedOrderNames = string.Join(
                separator,
                expectedOrder.Select(m => m.FullName));

            var actualOrderNames = string.Join(
                separator,
                manager.PinnedDocumentMetadata.Cast<DocumentMetadata>().Select(m =>
                    m.FullName));

            Assert.That(actualOrderNames, Is.EqualTo(expectedOrderNames));

            Assert.That(metadata1.PinOrder, Is.EqualTo(4));
            Assert.That(metadata2.PinOrder, Is.EqualTo(0));
            Assert.That(metadata3.PinOrder, Is.EqualTo(2));
            Assert.That(metadata4.PinOrder, Is.EqualTo(6));
        }

        [Test]
        public void MovePinnedItemDoesNotRefreshViewWhenSourceAndTargetSharePinOrder()
        {
            // Arrange

            const int pinOrder = 4;

            var viewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            var builder = new DocumentMetadataManagerBuilder
            {
                CollectionViewGenerator = Mock.Of<ICollectionViewGenerator>(c =>
                    c.CreateView(It.IsAny<IList>()) == viewMock.Object),

                UpdateReactionManager = Mock.Of<IUpdateReactionManager>()
            };

            var manager = builder.CreateDocumentMetadataManager();

            var metadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null)
            {
                PinOrder = pinOrder
            };

            // Act

            manager.MovePinnedItem(metadata, metadata);

            // Assert

            viewMock.Verify(v => v.Refresh(), Times.Never);
            Assert.That(metadata.PinOrder, Is.EqualTo(pinOrder));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PinnedDocumentMetadataIncludesMetadataWithAllHasWindowValues(
            bool hasWindow)
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();
            manager.TogglePinnedStatus(metadata);

            // Act

            metadata.HasWindow = hasWindow;
            manager.PinnedDocumentMetadata.Refresh();

            // Assert

            var pinnedMetadata = manager.PinnedDocumentMetadata
                .Cast<DocumentMetadata>().Single();

            Assert.AreEqual(metadata, pinnedMetadata);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PinnedDocumentMetadataIncludesOnlyMetadataWhereIsPinnedIsTrue(
            bool isPinned)
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();

            // Act

            if (isPinned)
            {
                manager.TogglePinnedStatus(metadata);
            }

            manager.PinnedDocumentMetadata.Refresh();

            // Assert

            var exists = manager.PinnedDocumentMetadata
                .Cast<DocumentMetadata>().Contains(metadata);

            Assert.AreEqual(isPinned, exists);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ActiveDocumentMetadataIncludesMetadataWithAllIsPinnedValues(
            bool isPinned)
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();

            // Act

            if (isPinned)
            {
                manager.TogglePinnedStatus(metadata);
            }

            manager.ActiveDocumentMetadata.Refresh();

            // Assert

            var activeMetadata = manager.ActiveDocumentMetadata
                .Cast<DocumentMetadata>().Single();

            Assert.AreEqual(metadata, activeMetadata);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ActiveDocumentMetadataIncludesOnlyMetadataWhereHasWindowIsTrue(
            bool hasWindow)
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Add(info);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            var metadata = collection.Single();

            // Act

            metadata.HasWindow = hasWindow;
            manager.ActiveDocumentMetadata.Refresh();

            // Assert

            var exists = manager.ActiveDocumentMetadata
                .Cast<DocumentMetadata>().Contains(metadata);

            Assert.AreEqual(hasWindow, exists);
        }

        [Test]
        public void ClearRemovesAllMetadata()
        {
            // Arrange

            var documentList = CreateDocumentList("d1", "d2");
            var documents = CreateDocuments(documentList);
            var builder = new DocumentMetadataManagerBuilder();
            var manager = builder.CreateDocumentMetadataManager();
            manager.Synchronize(documents, false);

            var collection =
                (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection;

            manager.TogglePinnedStatus(collection[1]);

            manager.ActiveDocumentMetadata.Refresh();
            manager.PinnedDocumentMetadata.Refresh();

            // Act

            manager.Clear();

            // Assert

            Assert.IsEmpty(manager.ActiveDocumentMetadata);
            Assert.IsEmpty(manager.PinnedDocumentMetadata);
            Assert.IsEmpty(collection);
        }

        public class FilterStringTests
        {
            private const string AaaDocument1 = "AAADocument1";
            private const string BbbDocument2 = "BBBDocument2";
            private const string AabDocument3 = "AABDocument3";

            private static IDictionary<string, DocumentMetadata> PrepareDocumentsForFilterTest(
                out IDocumentMetadataManager manager,
                params string[] docNames)
            {
                var documentMockList = docNames.Select(d => CreateDocumentWithInfo(
                    new DocumentMetadataInfo
                    {
                        FullName = d,
                        ProjectDisplayName = d
                    })).ToList();

                var documents = CreateDocuments(documentMockList);

                var metadataFactoryBuilder = new DocumentMetadataFactoryBuilder();

                metadataFactoryBuilder.FilePathServiceMock
                    .Setup(f => f.ReducePath(
                        It.IsAny<string>(),
                        It.IsAny<int>()))
                    .Returns<string, int>((str, i) => str);

                metadataFactoryBuilder.DisplayNameHighlightEvaluatorMock
                    .Setup(d => d.GetHighlight(
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                    .Returns<string, bool>((str, b) => str);

                var factory = metadataFactoryBuilder.CreateDocumentMetadataFactory(true);

                var builder = new DocumentMetadataManagerBuilder
                {
                    DocumentMetadataFactory = factory
                };

                builder.ProjectItemServiceMock
                    .Setup(p => p.FindProjectItem(It.IsAny<string>()))
                    .Returns(Mock.Of<ProjectItem>());

                manager = builder.CreateDocumentMetadataManager();
                manager.Synchronize(documents, false);

                var metadata = new Dictionary<string, DocumentMetadata>();
                foreach (var d in (IList<DocumentMetadata>)manager.ActiveDocumentMetadata.SourceCollection)
                {
                    metadata.Add(d.FullName, d);
                }

                return metadata;
            }

            private static DocumentMetadata GetMetadata(
                IDocumentMetadataManager manager,
                string documentName)
            {
                return ((IList<DocumentMetadata>)manager
                    .ActiveDocumentMetadata.SourceCollection)
                    .Single(d => d.FullName == documentName);
            }

            [TestCase("BDoc", new[] { BbbDocument2, AabDocument3 })]
            [TestCase("bdoc", new[] { BbbDocument2, AabDocument3 })]
            [TestCase("XXX", new string[0])]
            [TestCase(null, new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            [TestCase("", new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            [TestCase(" ", new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            public void FilterMatchesExpectedDocumentsWhenOnlyActive(
                string filterString,
                string[] expectedMatches)
            {
                // Arrange

                var documents = PrepareDocumentsForFilterTest(
                    out var manager,
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3);

                // Act

                manager.FilterString = filterString;

                // Assert

                var toTest = new[]
                {
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3
                };

                foreach (var document in toTest)
                {
                    var expectedMatch = expectedMatches.Contains(document);

                    Assert.AreEqual(
                        expectedMatch,
                        manager.ActiveDocumentMetadata.Contains(documents[document]));

                    Assert.IsFalse(manager.PinnedDocumentMetadata.Contains(documents[document]));
                }
            }

            [TestCase("BDoc", new[] { BbbDocument2, AabDocument3 })]
            [TestCase("bdoc", new[] { BbbDocument2, AabDocument3 })]
            [TestCase("XXX", new string[0])]
            [TestCase(null, new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            [TestCase("", new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            [TestCase(" ", new[] { AaaDocument1, BbbDocument2, AabDocument3 })]
            public void FilterMatchesExpectedDocumentsWhenOnlyPinned(
                string filterString,
                string[] expectedMatches)
            {
                // Arrange

                var documents = PrepareDocumentsForFilterTest(
                    out var manager,
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3);

                manager.TogglePinnedStatus(documents[AaaDocument1]);
                manager.TogglePinnedStatus(documents[BbbDocument2]);
                manager.TogglePinnedStatus(documents[AabDocument3]);

                var emptyDocuments = CreateDocuments(new List<Document>());
                manager.Synchronize(emptyDocuments, false);

                // Act

                manager.FilterString = filterString;

                // Assert

                var toTest = new[]
                {
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3
                };

                foreach (var document in toTest)
                {
                    var expectedMatch = expectedMatches.Contains(document);

                    Assert.IsFalse(manager.ActiveDocumentMetadata.Contains(documents[document]));

                    Assert.AreEqual(
                        expectedMatch,
                        manager.PinnedDocumentMetadata.Contains(documents[document]));
                }
            }

            [TestCase("BDoc", new[] {BbbDocument2, AabDocument3})]
            [TestCase("bdoc", new[] {BbbDocument2, AabDocument3})]
            [TestCase("XXX", new string[0])]
            [TestCase(null, new[] {AaaDocument1, BbbDocument2, AabDocument3})]
            [TestCase("", new[] {AaaDocument1, BbbDocument2, AabDocument3})]
            [TestCase(" ", new[] {AaaDocument1, BbbDocument2, AabDocument3})]
            public void FilterMatchesExpectedDocumentsWhenActiveAndPinned(
                string filterString,
                string[] expectedMatches)
            {
                // Arrange

                var documents = PrepareDocumentsForFilterTest(
                    out var manager,
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3);

                manager.TogglePinnedStatus(documents[AaaDocument1]);
                manager.TogglePinnedStatus(documents[BbbDocument2]);
                manager.TogglePinnedStatus(documents[AabDocument3]);

                // Act

                manager.FilterString = filterString;

                // Assert

                var toTest = new[]
                {
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3
                };

                foreach (var document in toTest)
                {
                    var expectedMatch = expectedMatches.Contains(document);

                    Assert.AreEqual(
                        expectedMatch,
                        manager.ActiveDocumentMetadata.Contains(documents[document]));

                    Assert.AreEqual(
                        expectedMatch,
                        manager.PinnedDocumentMetadata.Contains(documents[document]));
                }
            }

            [TestCase("XXBDocument4", true)]
            [TestCase("XXXDocument4", false)]
            public void FilterMatchesExpectedDocumentsWhenActiveAndPinnedAfterAddingDocument(
                string newDocumentName,
                bool expectedMatch)
            {
                // Arrange

                var documents = PrepareDocumentsForFilterTest(
                    out var manager,
                    AaaDocument1,
                    BbbDocument2,
                    AabDocument3);

                manager.TogglePinnedStatus(documents[AaaDocument1]);
                manager.TogglePinnedStatus(documents[BbbDocument2]);
                manager.TogglePinnedStatus(documents[AabDocument3]);

                // Act

                manager.FilterString = "BDoc";

                var newDoc = new DocumentMetadataInfo
                {
                    FullName = newDocumentName,
                    ProjectDisplayName = newDocumentName
                };

                manager.Add(newDoc);
                var newDocument = GetMetadata(manager, newDocumentName);
                manager.TogglePinnedStatus(newDocument);

                // Assert

                Assert.IsFalse(manager.ActiveDocumentMetadata.Contains(documents[AaaDocument1]));
                Assert.IsFalse(manager.PinnedDocumentMetadata.Contains(documents[AaaDocument1]));

                Assert.IsTrue(manager.ActiveDocumentMetadata.Contains(documents[BbbDocument2]));
                Assert.IsTrue(manager.PinnedDocumentMetadata.Contains(documents[BbbDocument2]));

                Assert.IsTrue(manager.ActiveDocumentMetadata.Contains(documents[AabDocument3]));
                Assert.IsTrue(manager.PinnedDocumentMetadata.Contains(documents[AabDocument3]));

                Assert.AreEqual(expectedMatch, manager.ActiveDocumentMetadata.Contains(newDocument));
                Assert.AreEqual(expectedMatch, manager.PinnedDocumentMetadata.Contains(newDocument));
            }
        }
    }
}
