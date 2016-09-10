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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service.EventRelay;
using static WorkingFilesList.ToolWindow.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class WindowEventsServiceTests
    {
        [Test]
        public void ActivatedDocumentIsActivated()
        {
            // Arrange

            const string documentName = "DocumentName";

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            documentMock.Setup(d => d.FullName).Returns(documentName);

            var gotFocus = new Mock<Window>();
            gotFocus.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            gotFocus.Setup(w => w.Document).Returns(documentMock.Object);

            // Act

            service.WindowActivated(gotFocus.Object, null);

            // Assert

            metadataManagerMock.Verify(m => m.Activate(documentName));
        }

        [Test]
        public void CreatingDocumentWindowWithEmptyMetadataCollectionSynchronizesDocuments()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            metadataManagerMock.Setup(m => m.ActiveDocumentMetadata.IsEmpty).Returns(true);

            var service = new WindowEventsService(metadataManagerMock.Object);

            var documents = Mock.Of<Documents>();

            var dte2Mock = new Mock<DTE>();
            dte2Mock.Setup(d => d.Documents).Returns(documents);

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            created.Setup(w => w.Document).Returns(documentMock.Object);
            created.Setup(w => w.DTE).Returns(dte2Mock.Object);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            metadataManagerMock.Verify(m => m.Synchronize(documents, false));
        }

        [Test]
        public void CreatingDocumentWindowWithEmptyMetadataCollectionActivatesActivatedDocument()
        {
            // Arrange

            const string documentName = "DocumentName";

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            metadataManagerMock.Setup(m => m.ActiveDocumentMetadata.IsEmpty).Returns(true);

            var service = new WindowEventsService(metadataManagerMock.Object);

            var dte2Mock = new Mock<DTE>();
            dte2Mock.Setup(d => d.Documents).Returns(Mock.Of<Documents>());

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            documentMock.Setup(d => d.FullName).Returns(documentName);

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            created.Setup(w => w.Document).Returns(documentMock.Object);
            created.Setup(w => w.DTE).Returns(dte2Mock.Object);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            metadataManagerMock.Verify(m => m.Activate(documentName));
        }

        [Test]
        public void ActivateOccursAfterSynchronizationWhenWindowIsCreated()
        {
            // Arrange

            var synchronized = false;
            var synchronizedWhenUpdatingActivatedTime = false;

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            metadataManagerMock.Setup(m => m.ActiveDocumentMetadata.IsEmpty).Returns(true);

            metadataManagerMock
                .Setup(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    false))
                .Callback(() => synchronized = true);

            // Check that Activate is called after Synchronize: the value of
            // synchronizedWhenUpdatingActivatedTime will be true only if
            // Activate is called after Synchronize
            metadataManagerMock.Setup(m => m.Activate(It.IsAny<string>()))
                .Callback(() => synchronizedWhenUpdatingActivatedTime = synchronized);

            var service = new WindowEventsService(metadataManagerMock.Object);

            var dte2Mock = new Mock<DTE>();
            dte2Mock.Setup(d => d.Documents).Returns(Mock.Of<Documents>());

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            documentMock.Setup(d => d.FullName).Returns("DocumentName");

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            created.Setup(w => w.Document).Returns(documentMock.Object);
            created.Setup(w => w.DTE).Returns(dte2Mock.Object);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            metadataManagerMock
                .Verify(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    false));

            metadataManagerMock.Verify(m => m.Activate(It.IsAny<string>()));

            Assert.IsTrue(synchronizedWhenUpdatingActivatedTime);
        }

        [Test]
        public void CreatingDocumentWindowWithNonEmptyMetadataCollectionAddsDocument()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName",
                ProjectDisplayName = "ProjectDisplayName",
                ProjectFullName = "ProjectFullName"
            };

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            metadataManagerMock.Setup(m => m.ActiveDocumentMetadata.IsEmpty).Returns(false);

            var service = new WindowEventsService(metadataManagerMock.Object);
            var document = CreateDocumentWithInfo(info);

            var created = Mock.Of<Window>(w =>
                w.Type == vsWindowType.vsWindowTypeDocument &&
                w.Document == document);

            // Act

            service.WindowCreated(created);

            // Assert

            metadataManagerMock.Verify(m => m.Add(info));
        }

        [Test]
        public void ClosingDocumentWindowSynchronizesDocumentMetadata()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var closing = Mock.Of<Window>(w =>
                w.Type == vsWindowType.vsWindowTypeDocument &&
                w.DTE == Mock.Of<DTE>(d =>
                    d.Documents == Mock.Of<Documents>()));

            // Act

            service.WindowClosing(closing);

            // Assert

            metadataManagerMock
                .Verify(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    It.IsAny<bool>()));
        }

        [Test]
        public void ClosingDocumentWindowSetsUsageOrder()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var created = Mock.Of<Window>(w =>
                w.Type == vsWindowType.vsWindowTypeDocument &&
                w.DTE == Mock.Of<DTE>(d =>
                    d.Documents == Mock.Of<Documents>()));

            // Act

            service.WindowClosing(created);

            // Assert

            metadataManagerMock
                .Verify(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    true));
        }

        // Test cases should not include vsWindowType.vsWindowTypeDocument
        [TestCase(vsWindowType.vsWindowTypeAutos)]
        [TestCase(vsWindowType.vsWindowTypeBrowser)]
        [TestCase(vsWindowType.vsWindowTypeCallStack)]
        [TestCase(vsWindowType.vsWindowTypeCodeWindow)]
        [TestCase(vsWindowType.vsWindowTypeColorPalette)]
        [TestCase(vsWindowType.vsWindowTypeDesigner)]
        [TestCase(vsWindowType.vsWindowTypeDocumentOutline)]
        [TestCase(vsWindowType.vsWindowTypeFind)]
        [TestCase(vsWindowType.vsWindowTypeFindReplace)]
        [TestCase(vsWindowType.vsWindowTypeImmediate)]
        [TestCase(vsWindowType.vsWindowTypeLinkedWindowFrame)]
        [TestCase(vsWindowType.vsWindowTypeLocals)]
        [TestCase(vsWindowType.vsWindowTypeMainWindow)]
        [TestCase(vsWindowType.vsWindowTypeOutput)]
        [TestCase(vsWindowType.vsWindowTypePreview)]
        [TestCase(vsWindowType.vsWindowTypeProperties)]
        [TestCase(vsWindowType.vsWindowTypeRunningDocuments)]
        [TestCase(vsWindowType.vsWindowTypeSolutionExplorer)]
        [TestCase(vsWindowType.vsWindowTypeTaskList)]
        [TestCase(vsWindowType.vsWindowTypeThreads)]
        [TestCase(vsWindowType.vsWindowTypeToolbox)]
        [TestCase(vsWindowType.vsWindowTypeToolWindow)]
        [TestCase(vsWindowType.vsWindowTypeWatch)]
        public void NonDocumentWindowActivationDoesNotActivateDocumentMetadata(
            vsWindowType windowType)
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var gotFocus = new Mock<Window>();
            gotFocus.Setup(w => w.Type).Returns(windowType);

            // Act

            service.WindowActivated(gotFocus.Object, null);

            // Assert

            gotFocus.Verify(w => w.Document, Times.Never);

            metadataManagerMock
                .Verify(m => m.Activate(null),
                Times.Never);

            metadataManagerMock
                .Verify(m => m.Activate(It.IsAny<string>()),
                Times.Never);
        }

        // Test cases should not include vsWindowType.vsWindowTypeDocument
        [TestCase(vsWindowType.vsWindowTypeAutos)]
        [TestCase(vsWindowType.vsWindowTypeBrowser)]
        [TestCase(vsWindowType.vsWindowTypeCallStack)]
        [TestCase(vsWindowType.vsWindowTypeCodeWindow)]
        [TestCase(vsWindowType.vsWindowTypeColorPalette)]
        [TestCase(vsWindowType.vsWindowTypeDesigner)]
        [TestCase(vsWindowType.vsWindowTypeDocumentOutline)]
        [TestCase(vsWindowType.vsWindowTypeFind)]
        [TestCase(vsWindowType.vsWindowTypeFindReplace)]
        [TestCase(vsWindowType.vsWindowTypeImmediate)]
        [TestCase(vsWindowType.vsWindowTypeLinkedWindowFrame)]
        [TestCase(vsWindowType.vsWindowTypeLocals)]
        [TestCase(vsWindowType.vsWindowTypeMainWindow)]
        [TestCase(vsWindowType.vsWindowTypeOutput)]
        [TestCase(vsWindowType.vsWindowTypePreview)]
        [TestCase(vsWindowType.vsWindowTypeProperties)]
        [TestCase(vsWindowType.vsWindowTypeRunningDocuments)]
        [TestCase(vsWindowType.vsWindowTypeSolutionExplorer)]
        [TestCase(vsWindowType.vsWindowTypeTaskList)]
        [TestCase(vsWindowType.vsWindowTypeThreads)]
        [TestCase(vsWindowType.vsWindowTypeToolbox)]
        [TestCase(vsWindowType.vsWindowTypeToolWindow)]
        [TestCase(vsWindowType.vsWindowTypeWatch)]
        public void NonDocumentWindowCreatedDoesNotAddDocument(
            vsWindowType windowType)
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(windowType);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            created.Verify(w => w.Document, Times.Never);

            metadataManagerMock
                .Verify(m => m.Add(It.IsAny<DocumentMetadataInfo>()),
                Times.Never);
        }

        // Test cases should not include vsWindowType.vsWindowTypeDocument
        [TestCase(vsWindowType.vsWindowTypeAutos)]
        [TestCase(vsWindowType.vsWindowTypeBrowser)]
        [TestCase(vsWindowType.vsWindowTypeCallStack)]
        [TestCase(vsWindowType.vsWindowTypeCodeWindow)]
        [TestCase(vsWindowType.vsWindowTypeColorPalette)]
        [TestCase(vsWindowType.vsWindowTypeDesigner)]
        [TestCase(vsWindowType.vsWindowTypeDocumentOutline)]
        [TestCase(vsWindowType.vsWindowTypeFind)]
        [TestCase(vsWindowType.vsWindowTypeFindReplace)]
        [TestCase(vsWindowType.vsWindowTypeImmediate)]
        [TestCase(vsWindowType.vsWindowTypeLinkedWindowFrame)]
        [TestCase(vsWindowType.vsWindowTypeLocals)]
        [TestCase(vsWindowType.vsWindowTypeMainWindow)]
        [TestCase(vsWindowType.vsWindowTypeOutput)]
        [TestCase(vsWindowType.vsWindowTypePreview)]
        [TestCase(vsWindowType.vsWindowTypeProperties)]
        [TestCase(vsWindowType.vsWindowTypeRunningDocuments)]
        [TestCase(vsWindowType.vsWindowTypeSolutionExplorer)]
        [TestCase(vsWindowType.vsWindowTypeTaskList)]
        [TestCase(vsWindowType.vsWindowTypeThreads)]
        [TestCase(vsWindowType.vsWindowTypeToolbox)]
        [TestCase(vsWindowType.vsWindowTypeToolWindow)]
        [TestCase(vsWindowType.vsWindowTypeWatch)]
        public void ClosingNonDocumentWindowDoesNotSynchronizesDocumentMetadata(
            vsWindowType windowType)
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new WindowEventsService(metadataManagerMock.Object);

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(windowType);

            // Act

            service.WindowClosing(created.Object);

            // Assert

            created.Verify(w => w.DTE, Times.Never);

            metadataManagerMock.Verify(m => m.Synchronize(
                    null,
                    It.IsAny<bool>()),
                Times.Never);

            metadataManagerMock.Verify(m => m.Synchronize(
                It.IsAny<Documents>(),
                It.IsAny<bool>()),
                Times.Never);
        }
    }
}
