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
using WorkingFilesList.Interface;
using WorkingFilesList.Service;

namespace WorkingFilesList.Test.Service
{
    [TestFixture]
    public class WindowEventsServiceTests
    {
        [Test]
        public void DocumentFromActivatedDocumentWindowIsUpsertedInMetadataService()
        {
            // Arrange

            const string documentName = "DocumentName";

            var metadataServiceMock = new Mock<IDocumentMetadataService>();
            var service = new WindowEventsService(metadataServiceMock.Object);

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            documentMock.Setup(d => d.FullName).Returns(documentName);

            var gotFocus = new Mock<Window>();
            gotFocus.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            gotFocus.Setup(w => w.Document).Returns(documentMock.Object);

            // Act

            service.WindowActivated(gotFocus.Object, null);

            // Assert

            metadataServiceMock.Verify(m => m.Upsert(documentName));
        }

        [Test]
        public void DocumentFromCreatedDocumentWindowIsUpsertedInMetadataService()
        {
            // Arrange

            const string documentName = "DocumentName";

            var metadataServiceMock = new Mock<IDocumentMetadataService>();
            var service = new WindowEventsService(metadataServiceMock.Object);

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.ActiveWindow).Returns(Mock.Of<Window>());
            documentMock.Setup(d => d.FullName).Returns(documentName);

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(vsWindowType.vsWindowTypeDocument);
            created.Setup(w => w.Document).Returns(documentMock.Object);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            metadataServiceMock.Verify(m => m.Upsert(documentName));
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
        public void NonDocumentWindowActivatedDoesNotUpsertDocument(vsWindowType windowType)
        {
            // Arrange

            var metadataServiceMock = new Mock<IDocumentMetadataService>();
            var service = new WindowEventsService(metadataServiceMock.Object);

            var gotFocus = new Mock<Window>();
            gotFocus.Setup(w => w.Type).Returns(windowType);

            // Act

            service.WindowActivated(gotFocus.Object, null);

            // Assert

            gotFocus.Verify(w => w.Document, Times.Never);
            metadataServiceMock.Verify(m => m.Upsert(null), Times.Never);
            metadataServiceMock.Verify(m => m.Upsert(It.IsAny<string>()), Times.Never);
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
        public void NonDocumentWindowCreatedDoesNotUpsertDocument(vsWindowType windowType)
        {
            // Arrange

            var metadataServiceMock = new Mock<IDocumentMetadataService>();
            var service = new WindowEventsService(metadataServiceMock.Object);

            var created = new Mock<Window>();
            created.Setup(w => w.Type).Returns(windowType);

            // Act

            service.WindowCreated(created.Object);

            // Assert

            created.Verify(w => w.Document, Times.Never);
            metadataServiceMock.Verify(m => m.Upsert(null), Times.Never);
            metadataServiceMock.Verify(m => m.Upsert(It.IsAny<string>()), Times.Never);
        }
    }
}
