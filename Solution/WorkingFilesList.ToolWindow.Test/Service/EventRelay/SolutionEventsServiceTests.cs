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
using EnvDTE80;
using Moq;
using NUnit.Framework;
using System;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class SolutionEventsServiceTests
    {
        [Test]
        public void ClosingSolutionClearsProjectBrushIdCollection()
        {
            // Arrange

            var projectBrushServiceMock = new Mock<IProjectBrushService>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                Mock.Of<IDocumentMetadataManager>(),
                projectBrushServiceMock.Object);

            // Act

            service.AfterClosing();

            // Assert

            projectBrushServiceMock.Verify(p =>
                p.ClearBrushIdCollection());
        }

        [Test]
        public void RenamingProjectUpdatesBrushIdWithNewAndOldProjectNames()
        {
            // Arrange

            const string oldName = "OldName";
            const string newName = "NewName";

            var projectBrushServiceMock = new Mock<IProjectBrushService>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                Mock.Of<IDocumentMetadataManager>(),
                projectBrushServiceMock.Object);

            var project = Mock.Of<Project>(p =>
                p.FullName == newName &&
                p.DTE == Mock.Of<DTE>(d =>
                    d.Documents == Mock.Of<Documents>()));

            // Act

            service.ProjectRenamed(project, oldName);

            // Assert

            projectBrushServiceMock.Verify(p =>
                p.UpdateBrushId(
                    oldName,
                    newName));
        }

        [Test]
        public void RenamingProjectSynchronizesDocumentMetadata()
        {
            // Arrange

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                metadataManagerMock.Object,
                Mock.Of<IProjectBrushService>());

            var renamedProject = Mock.Of<Project>(p =>
                p.DTE == Mock.Of<DTE>(d =>
                    d.Documents == Mock.Of<Documents>()));

            // Act

            service.ProjectRenamed(renamedProject, null);

            // Assert

            metadataManagerMock
                .Verify(m => m.Synchronize(
                    It.IsAny<Documents>(),
                    true));
        }

        [Test]
        public void RenamingProjectCallsUpdateBrushIdBeforeSynchronize()
        {
            // Arrange

            var updateBrushIdCalled = false;
            var updateBrushIdCalledBeforeSynchronize = false;

            var projectBrushServiceMock = new Mock<IProjectBrushService>();

            projectBrushServiceMock.Setup(p => p
                .UpdateBrushId(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => updateBrushIdCalled = true);

            var metadataManagerMock = new Mock<IDocumentMetadataManager>();

            metadataManagerMock.Setup(m => m
                .Synchronize(It.IsAny<Documents>(), It.IsAny<bool>()))
                .Callback(() => updateBrushIdCalledBeforeSynchronize = !updateBrushIdCalled);

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                metadataManagerMock.Object,
                projectBrushServiceMock.Object);

            var renamedProject = Mock.Of<Project>(p =>
                p.DTE == Mock.Of<DTE>(d =>
                    d.Documents == Mock.Of<Documents>()));

            // Act

            service.ProjectRenamed(renamedProject, null);

            // Assert

            Assert.IsFalse(updateBrushIdCalledBeforeSynchronize);
        }

        [Test]
        public void SolutionNameChangedIsRaisedWhenSolutionIsOpened()
        {
            // Arrange

            const string newName = "NewName";
            string eventArgsNewName = null;

            var dte2 = Mock.Of<DTE2>(d =>
                d.Solution.FullName == newName);

            var service = new SolutionEventsService(
                dte2,
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IProjectBrushService>());

            var handler = new EventHandler<SolutionNameChangedEventArgs>((s, e) =>
            {
                eventArgsNewName = e.NewName;
            });

            service.SolutionNameChanged += handler;

            // Act

            service.Opened();
            service.SolutionNameChanged -= handler;

            // Assert

            Assert.That(eventArgsNewName, Is.EqualTo(newName));
        }

        [Test]
        public void SolutionNameChangedIsRaisedWithEmptyStringWhenSolutionIsClosed()
        {
            // Arrange

            string eventArgsNewName = null;

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IProjectBrushService>());

            var handler = new EventHandler<SolutionNameChangedEventArgs>((s, e) =>
            {
                eventArgsNewName = e.NewName;
            });

            service.SolutionNameChanged += handler;

            // Act

            service.AfterClosing();
            service.SolutionNameChanged -= handler;

            // Assert

            Assert.That(eventArgsNewName, Is.EqualTo(string.Empty));
        }
    }
}
