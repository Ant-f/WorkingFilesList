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
