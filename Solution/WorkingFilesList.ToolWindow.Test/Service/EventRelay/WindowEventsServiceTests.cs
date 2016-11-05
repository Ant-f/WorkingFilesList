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
using Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
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

            var created = Mock.Of<Window>(w =>
                w.Type == vsWindowType.vsWindowTypeDocument &&

                w.Document == Mock.Of<Document>(d =>
                    d.ActiveWindow == Mock.Of<Window>() &&
                    d.FullName == documentName) &&

                w.DTE == Mock.Of<DTE>(dte =>
                    dte.Documents == Mock.Of<Documents>()));

            // Act

            service.WindowCreated(created);

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

            var created = Mock.Of<Window>(w =>
                w.Type == vsWindowType.vsWindowTypeDocument &&

                w.Document == Mock.Of<Document>(d =>
                    d.ActiveWindow == Mock.Of<Window>() &&
                    d.FullName == "DocumentName") &&

                w.DTE == Mock.Of<DTE>(dte =>
                    dte.Documents == Mock.Of<Documents>()));

            // Act

            service.WindowCreated(created);

            // Assert

            metadataManagerMock
                .Verify(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    false));

            metadataManagerMock.Verify(m => m.Activate(It.IsAny<string>()));

            Assert.IsTrue(synchronizedWhenUpdatingActivatedTime);
        }

        [Test]
        public void CreatingDocumentWindowWithNonEmptyMetadataCollectionAddsAndActivatesDocument()
        {
            // Arrange

            const string documentFullName = "DocumentFullName";

            var info = new DocumentMetadataInfo
            {
                FullName = documentFullName,
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
            metadataManagerMock.Verify(m => m.Activate(documentFullName));
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
