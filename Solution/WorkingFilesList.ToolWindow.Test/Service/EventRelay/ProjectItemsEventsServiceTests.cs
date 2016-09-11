// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using EnvDTE;
using Moq;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class ProjectItemsEventsServiceTests
    {
        [Test]
        public void RenamingDocumentCallsDocumentMetadataManager()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Document == Mock.Of<Document>(d =>
                    d.FullName == "NewName"));

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            metadataManagerMock.Verify(m => m.UpdateFullName(
                It.IsAny<string>(),
                It.IsAny<string>()));
        }

        [Test]
        public void RenamingNullDocumentProjectItemDoesNotCallDocumentMetadataManager()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);

            var projectItem = Mock.Of<ProjectItem>();

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            metadataManagerMock.Verify(m => m.UpdateFullName(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void OldNameIsConverterToFullName()
        {
            // Arrange

            const string oldFileName = "OldFileName.txt";
            const string newFileName = "NewFileName.txt";
            const string path = @"C:\Directory\SubDirectory\";

            const string newFullName = path + newFileName;
            const string expectedOldName = path + oldFileName;

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Document == Mock.Of<Document>(d =>
                    d.FullName == newFullName));

            // Act

            service.ItemRenamed(projectItem, oldFileName);

            // Assert

            metadataManagerMock.Verify(m => m.UpdateFullName(
                newFullName,
                expectedOldName));
        }
    }
}
