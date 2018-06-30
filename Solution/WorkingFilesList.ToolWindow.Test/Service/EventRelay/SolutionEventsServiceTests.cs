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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;
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
                Mock.Of<IPinnedItemStorageService>(),
                projectBrushServiceMock.Object,
                Mock.Of<IUserPreferences>());

            // Act

            service.AfterClosing();

            // Assert

            projectBrushServiceMock.Verify(p =>
                p.ClearBrushIdCollection());
        }

        [Test]
        public void ClosingSolutionClearsMetadataCollection()
        {
            // Arrange

            var documentMetadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                documentMetadataManager,
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

            // Act

            service.AfterClosing();

            // Assert

            Mock.Get(documentMetadataManager).Verify(d =>
                d.Clear());
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
                Mock.Of<IPinnedItemStorageService>(),
                projectBrushServiceMock.Object,
                Mock.Of<IUserPreferences>());

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
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

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
                Mock.Of<IPinnedItemStorageService>(),
                projectBrushServiceMock.Object,
                Mock.Of<IUserPreferences>());

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
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

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
        public void SavedPinnedMetadataInfoIsRestoredWhenSolutionIsOpened()
        {
            // Arrange

            const string solutionFullName = "SolutionFullName";

            var info1 = new DocumentMetadataInfo
            {
                FullName = "FullName1"
            };

            var info2 = new DocumentMetadataInfo
            {
                FullName = "FullName2"
            };

            var pinnedItemService = Mock.Of<IPinnedItemStorageService>(s =>
                s.Read(solutionFullName) == new[] {info1, info2});

            var metadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(d =>
                    d.Solution.FullName == solutionFullName),
                metadataManager,
                pinnedItemService,
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

            // Act

            service.Opened();

            // Assert

            Mock.Get(metadataManager).Verify(s =>
                s.AddPinned(info1));

            Mock.Get(metadataManager).Verify(s =>
                s.AddPinned(info2));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SavedPinnedMetadataInfoIsNotRestoredWhenSolutionNameIsEmpty(
            string solutionName)
        {
            // Arrange

            var pinnedItemService = Mock.Of<IPinnedItemStorageService>();
            var metadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(d =>
                    d.Solution.FullName == solutionName),
                metadataManager,
                pinnedItemService,
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

            // Act

            service.Opened();

            // Assert

            Mock.Get(pinnedItemService).Verify(s =>
                s.Read(It.IsAny<string>()),
                Times.Never);

            Mock.Get(metadataManager).Verify(s =>
                s.AddPinned(It.IsAny<DocumentMetadataInfo>()),
                Times.Never);
        }

        [Test]
        public void SolutionNameChangedIsRaisedWithEmptyStringWhenSolutionIsClosed()
        {
            // Arrange

            string eventArgsNewName = null;

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

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

        [Test]
        public async Task AddingUnityProjectSynchronisesMetadata()
        {
            // Arrange

            var documents = Mock.Of<Documents>();
            var metadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                metadataManager,
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>(u =>
                    u.UnityRefreshDelay == 100));

            var fullName = GetTestDataPath("UnityProjectFile");

            var project = Mock.Of<Project>(p =>
                p.FullName == fullName &&
                p.DTE == Mock.Of<DTE>(d =>
                    d.Documents == documents));

            // Act

            await service.ProjectAdded(project);

            // Assert

            Mock.Get(metadataManager).Verify(m =>
                m.Synchronize(documents, true));
        }

        [Test]
        public async Task AddingNonUnityProjectDoesNotSynchroniseMetadata()
        {
            // Arrange

            var metadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                metadataManager,
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>(u =>
                    u.UnityRefreshDelay == 100));

            var fullName = GetTestDataPath("NonUnityProjectFile");

            var project = Mock.Of<Project>(p =>
                p.FullName == fullName &&
                p.DTE == Mock.Of<DTE>());

            // Act

            await service.ProjectAdded(project);

            // Assert

            Mock.Get(metadataManager).Verify(m =>
                m.Synchronize(
                    It.IsAny<Documents>(),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        public async Task UserPreferencesUnityRefreshDelayIsUsed()
        {
            // Arrange

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>(u =>
                    u.UnityRefreshDelay == -2));

            var fullName = GetTestDataPath("UnityProjectFile");

            var project = Mock.Of<Project>(p =>
                p.FullName == fullName &&
                p.DTE == Mock.Of<DTE>());

            ArgumentOutOfRangeException exception = null;

            // Act

            try
            {
                await service.ProjectAdded(project);
            }
            catch (ArgumentOutOfRangeException e)
            {
                exception = e;
            }

            // Assert

            Assert.AreEqual(
                "The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer.\r\nParameter name: millisecondsDelay",
                exception.Message);
        }

        [Test]
        public async Task SettingUnityRefreshDelayToZeroDoesNotSynchroniseMetadata()
        {
            // Arrange

            var metadataManager = Mock.Of<IDocumentMetadataManager>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(),
                metadataManager,
                Mock.Of<IPinnedItemStorageService>(),
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>(u =>
                    u.UnityRefreshDelay == 0));

            var fullName = GetTestDataPath("UnityProjectFile");

            var project = Mock.Of<Project>(p =>
                p.FullName == fullName &&
                p.DTE == Mock.Of<DTE>());

            // Act

            await service.ProjectAdded(project);

            // Assert

            Mock.Get(metadataManager).Verify(m =>
                m.Synchronize(
                    It.IsAny<Documents>(),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        public void MetadataIsWrittenBeforeClosingSolution()
        {
            // Arrange

            const string fullName = "FullName";

            var metadata = new DocumentMetadata[0];
            var metadataView = new ListCollectionView(metadata);
            var storageService = Mock.Of<IPinnedItemStorageService>();

            var service = new SolutionEventsService(
                Mock.Of<DTE2>(d =>
                    d.Solution.FullName == fullName),
                Mock.Of<IDocumentMetadataManager>(m =>
                    m.PinnedDocumentMetadata == metadataView),
                storageService,
                Mock.Of<IProjectBrushService>(),
                Mock.Of<IUserPreferences>());

            // Act

            service.BeforeClosing();

            // Assert

            Mock.Get(storageService).Verify(s =>
                s.Write(metadata, fullName));
        }

        private static string GetTestDataPath(string fileName)
        {
            var runningDir = AppDomain.CurrentDomain.BaseDirectory;

            var relativePath = Path.Combine(
                "TestingData",
                $"{fileName}.txt");

            var fullName = Path.Combine(runningDir, relativePath);
            return fullName;
        }
    }
}
