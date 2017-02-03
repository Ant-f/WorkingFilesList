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

using Moq;
using NUnit.Framework;
using System;
using Microsoft.VisualStudio.Settings;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Repository;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Repository
{
    [TestFixture]
    public class StoredSettingsRepositoryTests
    {
        private const string CollectionNameRoot = "CollectionName";
        private const string CollectionName = CollectionNameRoot + @"\Settings";

        private static StoredSettingsRepository CreateStoredSettingsRepository(
            out InMemorySettingsStore settingsStore,
            out ISettingsStoreService settingsStoreService)
        {
            var store = new InMemorySettingsStore();
            var serviceMock = new Mock<ISettingsStoreService>();

            serviceMock
                .Setup(s => s.GetSettingsStore(It.IsAny<bool>()))
                .Returns<bool>(isReadOnly =>
                {
                    store.IsReadOnly = isReadOnly;
                    return new SettingsStoreContainer(Mock.Of<IDisposable>(), store);
                });

            settingsStoreService = serviceMock.Object;
            settingsStore = store;

            var repository = new StoredSettingsRepository(
                settingsStoreService,
                CollectionNameRoot);

            return repository;
        }

        [Test]
        public void ResetDeletesExistingSettingsCollection()
        {
            // Arrange

            var settingsStore = Mock.Of<WritableSettingsStore>(s =>
                s.CollectionExists(It.IsAny<string>()) == true);

            var service = Mock.Of<ISettingsStoreService>(s =>
                s.GetSettingsStore(false) == new SettingsStoreContainer(
                    Mock.Of<IDisposable>(),
                    settingsStore));

            var repository = new StoredSettingsRepository(service, CollectionNameRoot);

            // Act

            repository.Reset();

            // Assert

            Mock.Get(settingsStore).Verify(s => s.DeleteCollection(
                It.Is<string>(name => name == CollectionName)));
        }

        [Test]
        public void ResetDoesNotAttemptToDeleteNonExistentSettingsCollection()
        {
            // Arrange

            var settingsStore = Mock.Of<WritableSettingsStore>(s =>
                s.CollectionExists(It.IsAny<string>()) == false);

            var service = Mock.Of<ISettingsStoreService>(s =>
                s.GetSettingsStore(false) == new SettingsStoreContainer(
                    Mock.Of<IDisposable>(),
                    settingsStore));

            var repository = new StoredSettingsRepository(service, CollectionNameRoot);

            // Act

            repository.Reset();

            // Assert

            Mock.Get(settingsStore).Verify(s => s.DeleteCollection(It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void GetPathSegmentCountReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetPathSegmentCount();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void PathSegmentCountCanBeStoredAndRead()
        {
            // Arrange

            const int pathSegmentCount = 7;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetPathSegmentCount(pathSegmentCount);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(pathSegmentCount));
        }

        [Test]
        public void GetDocumentSortOptionNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetDocumentSortOptionName();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.That(value, Is.EqualTo("A-Z"));
        }

        [Test]
        public void SelectedDocumentSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetDocumentSortOptionName(name);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetDocumentSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void GetProjectSortOptionNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetProjectSortOptionName();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.That(value, Is.EqualTo("None"));
        }

        [Test]
        public void SelectedProjectSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetProjectSortOptionName(name);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetProjectSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void GetGroupByProjectReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetGroupByProject();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.IsTrue(value);
        }

        [Test]
        public void GroupByProjectCanBeStoredAndRead()
        {
            // Arrange

            const bool groupByProject = true;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetGroupByProject(groupByProject);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetGroupByProject();
            Assert.That(storedValue, Is.EqualTo(groupByProject));
        }

        [Test]
        public void GetHighlightFileNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetHighlightFileName();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.IsTrue(value);
        }

        [Test]
        public void HighlightFileNameCanBeStoredAndRead()
        {
            // Arrange

            const bool highlightFileName = false;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetHighlightFileName(highlightFileName);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetHighlightFileName();
            Assert.That(storedValue, Is.EqualTo(highlightFileName));
        }

        [Test]
        public void GetShowRecentUsageReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetShowRecentUsage();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.IsTrue(value);
        }

        [Test]
        public void ShowRecentUsageCanBeStoredAndRead()
        {
            // Arrange

            const bool showRecentUsage = true;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetShowRecentUsage(showRecentUsage);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetShowRecentUsage();
            Assert.That(storedValue, Is.EqualTo(showRecentUsage));
        }

        [Test]
        public void GetAssignProjectColoursReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetAssignProjectColours();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.IsTrue(value);
        }

        [Test]
        public void AssignProjectColoursCanBeStoredAndRead()
        {
            // Arrange

            const bool assignProjectColours = true;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetAssignProjectColours(assignProjectColours);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetAssignProjectColours();
            Assert.That(storedValue, Is.EqualTo(assignProjectColours));
        }

        [Test]
        public void GetShowFileTypeIconsReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            var value = repository.GetShowFileTypeIcons();

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(true));

            Assert.IsTrue(value);
        }

        [Test]
        public void ShowFileTypeIconsCanBeStoredAndRead()
        {
            // Arrange

            const bool showFileTypeIcons = false;

            InMemorySettingsStore settingsStore;
            ISettingsStoreService settingsStoreService;

            var repository = CreateStoredSettingsRepository(
                out settingsStore,
                out settingsStoreService);

            // Act

            repository.SetShowFileTypeIcons(showFileTypeIcons);

            // Assert

            Mock.Get(settingsStoreService).Verify(s =>
                s.GetSettingsStore(false));

            Assert.Contains(CollectionName, settingsStore.SettingsStore.Keys);

            var storedValue = repository.GetShowFileTypeIcons();
            Assert.That(storedValue, Is.EqualTo(showFileTypeIcons));
        }
    }
}
