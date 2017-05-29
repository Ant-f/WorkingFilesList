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

using System;
using EnvDTE;
using Microsoft.VisualStudio;
using Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class ProjectItemsEventsServiceTests
    {
        private static string CreateKindString(Guid kind)
        {
            var kindString = $"{{{kind}}}";
            return kindString;
        }

        [Test]
        public void RemovingItemSynchronizesDocumentMetadata()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);
            var documents = Mock.Of<Documents>();

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.DTE == Mock.Of<DTE>(dte =>
                    dte.Documents == documents));

            // Act

            service.ItemRemoved(projectItem);

            // Assert

            metadataManagerMock.Verify(m => m.Synchronize(documents, true));
        }

        [Test]
        public void RenamingDocumentCallsDocumentMetadataManager()
        {
            // Arrange

            var metadataManager = Mock.Of<IDocumentMetadataManager>(m =>
                m.UpdateFullName(
                    It.IsAny<string>(),
                    It.IsAny<string>()) == true);

            var service = new ProjectItemsEventsService(metadataManager);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Document == Mock.Of<Document>(d =>
                    d.FullName == "NewName") &&
                p.Kind == CreateKindString(VSConstants.GUID_ItemType_PhysicalFile));

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            Mock.Get(metadataManager).Verify(m => m.UpdateFullName(
                It.IsAny<string>(),
                It.IsAny<string>()));
        }

        [Test]
        public void RenamingProjectItemWithUnparsableKindDoesNotThrowException()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Kind == "UnparsableKind");

            // Act, Assert

            Assert.DoesNotThrow(() =>
                service.ItemRenamed(projectItem, "OldName"));

            metadataManagerMock.Verify(m => m.UpdateFullName(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void RenamingNullDocumentProjectItemDoesNotCallDocumentMetadataManager()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManagerMock.Object);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Kind == CreateKindString(VSConstants.GUID_ItemType_PhysicalFile));

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            metadataManagerMock.Verify(m => m.UpdateFullName(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void MetadataIsSynchronizedIfRenamingDocumentFails(bool renameSuccess)
        {
            // Arrange

            var metadataManager = Mock.Of<IDocumentMetadataManager>(m =>
                m.UpdateFullName(
                    It.IsAny<string>(),
                    It.IsAny<string>()) == renameSuccess);

            var service = new ProjectItemsEventsService(metadataManager);
            var documents = Mock.Of<Documents>();

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Document == Mock.Of<Document>(d =>
                    d.FullName == "NewName") &&
                p.DTE == Mock.Of<DTE>(d => d.Documents == documents) &&
                p.Kind == CreateKindString(VSConstants.GUID_ItemType_PhysicalFile));

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            var times = renameSuccess
                ? Times.Never()
                : Times.Once();

            Mock.Get(metadataManager).Verify(m => m.Synchronize(documents, true), times);
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

            var metadataManager = Mock.Of<IDocumentMetadataManager>(m =>
                m.UpdateFullName(
                    It.IsAny<string>(),
                    It.IsAny<string>()) == true);

            var service = new ProjectItemsEventsService(metadataManager);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.Document == Mock.Of<Document>(d =>
                    d.FullName == newFullName) &&
                p.Kind == CreateKindString(VSConstants.GUID_ItemType_PhysicalFile));

            // Act

            service.ItemRenamed(projectItem, oldFileName);

            // Assert

            Mock.Get(metadataManager).Verify(m => m.UpdateFullName(
                newFullName,
                expectedOldName));
        }

        [Test]
        public void SynchronizeIsCalledWhenProjectItemTypeIsPhysicalFolder()
        {
            // Arrange

            var documents = Mock.Of<Documents>();
            var metadataManager = Mock.Of<IDocumentMetadataManager>();
            var service = new ProjectItemsEventsService(metadataManager);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.DTE == Mock.Of<DTE>(dte =>
                    dte.Documents == documents) &&
                p.Kind == CreateKindString(VSConstants.GUID_ItemType_PhysicalFolder));

            // Act

            service.ItemRenamed(projectItem, "OldName");

            // Assert

            Mock.Get(metadataManager).Verify(m => m.Synchronize(
                documents,
                true));
        }
    }
}
