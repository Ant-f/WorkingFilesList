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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WorkingFilesList.Model;
using WorkingFilesList.Test.TestingInfrastructure;

namespace WorkingFilesList.Test.Service
{
    [TestFixture]
    public class DocumentMetadataServiceTests
    {
        private static Document CreateDocument(
            string fullName,
            bool nullActiveWindow = false)
        {
            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.FullName).Returns(fullName);

            if (!nullActiveWindow)
            {
                documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            }

            return documentMock.Object;
        }

        private static Documents CreateDocuments(List<Document> documentsToReturn)
        {
            var documentsMock = new Mock<Documents>();

            documentsMock.Setup(d => d.GetEnumerator())
                .Returns(documentsToReturn.GetEnumerator());

            return documentsMock.Object;
        }

        [Test]
        public void AddAppendsDocumentMetadataToListIfFullPathDoesNotExist()
        {
            // Arrange

            const string documentName = "DocumentName";

            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Act

            service.Add(documentName);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(1));
            Assert.That(
                ((DocumentMetadata) service.ActiveDocumentMetadata.GetItemAt(0)).FullName,
                Is.EqualTo(documentName));
        }

        [Test]
        public void AddDoesNotAppendDocumentMetadataToListIfFullPathAlreadyExist()
        {
            // Arrange

            const string documentName = "DocumentName";

            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Act

            service.Add(documentName);
            service.Add(documentName);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(1));
            Assert.That(
                ((DocumentMetadata) service.ActiveDocumentMetadata.GetItemAt(0)).FullName,
                Is.EqualTo(documentName));
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
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Act

            service.Synchronize(documents);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(2));

            var collection =
                (IList<DocumentMetadata>) service.ActiveDocumentMetadata.SourceCollection;

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
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Synchronize to set two items in the document metadata service
            // metadata list

            service.Synchronize(documents);

            var updatedDocumentMockList = new List<Document>
            {
                CreateDocument(documentToRetain)
            };

            // Synchronizing with the updated list should remove one item

            var updatedDocuments = CreateDocuments(updatedDocumentMockList);

            // Act

            service.Synchronize(updatedDocuments);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(1));

            var collection =
                (IList<DocumentMetadata>)service.ActiveDocumentMetadata.SourceCollection;

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
            
            var builder = new DocumentMetadataServiceBuilder();
            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(activatedAt);

            var service = builder.CreateDocumentMetadataService();
            var documents = CreateDocuments(documentMockList);

            // Act

            service.Synchronize(documents);

            // Assert

            var collection =
                (IList<DocumentMetadata>)service.ActiveDocumentMetadata.SourceCollection;

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

            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();
            var documents = CreateDocuments(documentMockList);

            // Act

            service.Synchronize(documents);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.IsEmpty);
        }

        [Test]
        public void UpdateActivatedTimeUpdatesOnlyMetadataWithMatchingPath()
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
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            service.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)service.ActiveDocumentMetadata.SourceCollection;

            var document1 = collection.Single(m => m.FullName == document1Name);
            var document2 = collection.Single(m => m.FullName == document2Name);

            var document1InitialActivationTime = document1.ActivatedAt;
            var document2InitialActivationTime = document2.ActivatedAt;

            // Act

            service.UpdateActivatedTime(document1Name);

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
        public void UpdateActivatedTimeDoesNotThrowExceptionIfFullNameDoesNotExist()
        {
            // Arrange
            
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            // Assert

            Assert.DoesNotThrow(() => service.UpdateActivatedTime("Document"));
        }

        [Test]
        public void UpdateFullNameDoesNotAlterActivatedAtTime()
        {
            // Arrange

            const string document1OldName = "Document1OldName";
            const string document1NewName = "Document1NewName";

            var documentMockList = new List<Document>
            {
                CreateDocument(document1OldName)
            };

            var documents = CreateDocuments(documentMockList);
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            service.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)service.ActiveDocumentMetadata.SourceCollection;

            var document1 = collection.Single(m => m.FullName == document1OldName);
            var document1InitialActivationTime = document1.ActivatedAt;

            // Act

            service.UpdateFullName(document1NewName, document1OldName);

            // Assert

            // Time provider called once: during setup
            builder.TimeProviderMock.Verify(t => t.UtcNow, Times.Exactly(1));

            Assert.That(
                document1.ActivatedAt,
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
            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            builder.TimeProviderMock.Setup(t => t.UtcNow)
                .Returns(() => DateTime.UtcNow);

            service.Synchronize(documents);

            var collection =
                (IList<DocumentMetadata>)service.ActiveDocumentMetadata.SourceCollection;

            var document1 = collection.Single(m => m.FullName == document1OldName);
            var document2 = collection.Single(m => m.FullName == document2Name);

            // Act

            service.UpdateFullName(document1NewName, document1OldName);

            // Assert

            Assert.That(
                document1.FullName,
                Is.EqualTo(document1NewName));

            Assert.That(
                document2.FullName,
                Is.EqualTo(document2Name));
        }

        [Test]
        public void UpdateFullNameDoesNotThrowExceptionIfOldNameDoesNotExist()
        {
            // Arrange

            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Assert

            Assert.DoesNotThrow(() => service.UpdateFullName(
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

            var builder = new DocumentMetadataServiceBuilder();
            var service = builder.CreateDocumentMetadataService();

            // Assert

            Assert.DoesNotThrow(() => service.Synchronize(documentsMock.Object));
        }
    }
}
