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

        private static StoredSettingsRepository CreateStoredSettingsRepository()
        {
            var service = Mock.Of<ISettingsStoreService>(s =>
                s.GetWritableSettingsStore() == new SettingsStoreContainer(
                    Mock.Of<IDisposable>(),
                    new InMemorySettingsStore()));

            var repository = new StoredSettingsRepository(service, CollectionName);
            return repository;
        }

        [Test]
        public void SettingsCollectionIsCreatedWhenCreatingRepositoryInstance()
        {
            // Arrange

            var settingsStore = Mock.Of<WritableSettingsStore>();

            var service = Mock.Of<ISettingsStoreService>(s =>
                s.GetWritableSettingsStore() == new SettingsStoreContainer(
                    Mock.Of<IDisposable>(),
                    settingsStore));

            // Act

            var repository = new StoredSettingsRepository(service, CollectionNameRoot);

            // Assert

            Mock.Get(settingsStore).Verify(s => s.CreateCollection(
                It.Is<string>(name => name == CollectionName)));
        }

        [Test]
        public void ResetDeletesSettingsCollection()
        {
            // Arrange

            var settingsStore = Mock.Of<WritableSettingsStore>();

            var service = Mock.Of<ISettingsStoreService>(s =>
                s.GetWritableSettingsStore() == new SettingsStoreContainer(
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
        public void GetPathSegmentCountReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetPathSegmentCount();

            // Assert

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void PathSegmentCountCanBeStoredAndRead()
        {
            // Arrange

            const int pathSegmentCount = 7;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetPathSegmentCount(pathSegmentCount);

            // Assert

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(pathSegmentCount));
        }

        [Test]
        public void GetDocumentSortOptionNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetDocumentSortOptionName();

            // Assert

            Assert.That(value, Is.EqualTo("A-Z"));
        }

        [Test]
        public void SelectedDocumentSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetDocumentSortOptionName(name);

            // Assert

            var storedValue = repository.GetDocumentSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void GetProjectSortOptionNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetProjectSortOptionName();

            // Assert

            Assert.That(value, Is.EqualTo("None"));
        }

        [Test]
        public void SelectedProjectSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetProjectSortOptionName(name);

            // Assert

            var storedValue = repository.GetProjectSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void GetGroupByProjectReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetGroupByProject();

            // Assert

            Assert.IsFalse(value);
        }

        [Test]
        public void GroupByProjectCanBeStoredAndRead()
        {
            // Arrange

            const bool groupByProject = true;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetGroupByProject(groupByProject);

            // Assert

            var storedValue = repository.GetGroupByProject();
            Assert.That(storedValue, Is.EqualTo(groupByProject));
        }

        [Test]
        public void GetHighlightFileNameReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetHighlightFileName();

            // Assert

            Assert.IsTrue(value);
        }

        [Test]
        public void HighlightFileNameCanBeStoredAndRead()
        {
            // Arrange

            const bool highlightFileName = false;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetHighlightFileName(highlightFileName);

            // Assert

            var storedValue = repository.GetHighlightFileName();
            Assert.That(storedValue, Is.EqualTo(highlightFileName));
        }

        [Test]
        public void GetShowRecentUsageReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetShowRecentUsage();

            // Assert

            Assert.IsFalse(value);
        }

        [Test]
        public void ShowRecentUsageCanBeStoredAndRead()
        {
            // Arrange

            const bool showRecentUsage = true;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetShowRecentUsage(showRecentUsage);

            // Assert

            var storedValue = repository.GetShowRecentUsage();
            Assert.That(storedValue, Is.EqualTo(showRecentUsage));
        }

        [Test]
        public void GetAssignProjectColoursReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetAssignProjectColours();

            // Assert

            Assert.IsFalse(value);
        }

        [Test]
        public void AssignProjectColoursCanBeStoredAndRead()
        {
            // Arrange

            const bool assignProjectColours = true;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetAssignProjectColours(assignProjectColours);

            // Assert

            var storedValue = repository.GetAssignProjectColours();
            Assert.That(storedValue, Is.EqualTo(assignProjectColours));
        }

        [Test]
        public void GetShowFileTypeIconsReturnsDefaultValueIfNoStoredValueExists()
        {
            // Arrange

            var repository = CreateStoredSettingsRepository();

            // Act

            var value = repository.GetShowFileTypeIcons();

            // Assert

            Assert.IsTrue(value);
        }

        [Test]
        public void ShowFileTypeIconsCanBeStoredAndRead()
        {
            // Arrange

            const bool showFileTypeIcons = false;
            var repository = CreateStoredSettingsRepository();

            // Act

            repository.SetShowFileTypeIcons(showFileTypeIcons);

            // Assert

            var storedValue = repository.GetShowFileTypeIcons();
            Assert.That(storedValue, Is.EqualTo(showFileTypeIcons));
        }
    }
}
