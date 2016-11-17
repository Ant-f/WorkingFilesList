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

using NUnit.Framework;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.ToolWindow.Repository;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Repository
{
    [TestFixture, Explicit]
    public class StoredSettingsRepositoryTests
    {
        /// <summary>
        /// Reload stored Settings data. This method is implementation-specific,
        /// as it doesn't seem possible to simulate accessing the stored value
        /// across multiple sessions otherwise
        /// </summary>
        private static void ReloadData()
        {
            WorkingFilesList.ToolWindow.Properties.Settings.Default.Reload();
        }

        [SetUp, OneTimeTearDown]
        public void ResetStoredData()
        {
            CommonMethods.ResetStoredRepositoryData();
        }

        [Test]
        public void PathSegmentCountCanBeReset()
        {
            // Arrange

            const int defaultValue = 1;

            var repository = new StoredSettingsRepository();
            repository.SetPathSegmentCount(7);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(defaultValue));
        }

        [Test]
        public void PathSegmentCountCanBeStoredAndRead()
        {
            // Arrange

            const int pathSegmentCount = 7;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetPathSegmentCount(pathSegmentCount);

            // Assert

            ReloadData();

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(pathSegmentCount));
        }

        [Test]
        public void SelectedDocumentSortTypeCanBeReset()
        {
            // Arrange

            var defaultValue = new AlphabeticalSort().DisplayName;

            var repository = new StoredSettingsRepository();
            repository.SetDocumentSortOptionName("Testing.SortingOption");

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetDocumentSortOptionName();
            Assert.That(storedValue, Is.EqualTo(defaultValue));
        }

        [Test]
        public void SelectedDocumentSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetDocumentSortOptionName(name);

            // Assert

            ReloadData();

            var storedValue = repository.GetDocumentSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void SelectedProjectSortTypeCanBeReset()
        {
            // Arrange

            var defaultValue = new DisableSorting().DisplayName;

            var repository = new StoredSettingsRepository();
            repository.SetProjectSortOptionName("Testing.SortOption");

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetProjectSortOptionName();
            Assert.That(storedValue, Is.EqualTo(defaultValue));
        }

        [Test]
        public void SelectedProjectSortTypeCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetProjectSortOptionName(name);

            // Assert

            ReloadData();

            var storedValue = repository.GetProjectSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }

        [Test]
        public void GroupByProjectCanBeReset()
        {
            // Arrange

            const bool defaultGroupByProject = false;

            var repository = new StoredSettingsRepository();
            repository.SetGroupByProject(true);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetGroupByProject();
            Assert.That(storedValue, Is.EqualTo(defaultGroupByProject));
        }

        [Test]
        public void GroupByProjectCanBeStoredAndRead()
        {
            // Arrange

            const bool groupByProject = true;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetGroupByProject(groupByProject);

            // Assert

            ReloadData();

            var storedValue = repository.GetGroupByProject();
            Assert.That(storedValue, Is.EqualTo(groupByProject));
        }

        [Test]
        public void ShowRecentUsageCanBeReset()
        {
            // Arrange

            const bool defaultShowRecentUsage = false;

            var repository = new StoredSettingsRepository();
            repository.SetShowRecentUsage(true);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetShowRecentUsage();
            Assert.That(storedValue, Is.EqualTo(defaultShowRecentUsage));
        }

        [Test]
        public void ShowRecentUsageCanBeStoredAndRead()
        {
            // Arrange

            const bool showRecentUsage = true;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetShowRecentUsage(showRecentUsage);

            // Assert

            ReloadData();

            var storedValue = repository.GetShowRecentUsage();
            Assert.That(storedValue, Is.EqualTo(showRecentUsage));
        }

        [Test]
        public void AssignProjectColoursCanBeReset()
        {
            // Arrange

            const bool defaultAssignProjectColours = false;

            var repository = new StoredSettingsRepository();
            repository.SetAssignProjectColours(true);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetAssignProjectColours();
            Assert.That(storedValue, Is.EqualTo(defaultAssignProjectColours));
        }

        [Test]
        public void AssignProjectColoursCanBeStoredAndRead()
        {
            // Arrange

            const bool assignProjectColours = true;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetAssignProjectColours(assignProjectColours);

            // Assert

            ReloadData();

            var storedValue = repository.GetAssignProjectColours();
            Assert.That(storedValue, Is.EqualTo(assignProjectColours));
        }

        [Test]
        public void ShowFileTypeIconsCanBeReset()
        {
            // Arrange

            const bool defaultShowFileTypeIcons = true;

            var repository = new StoredSettingsRepository();
            repository.SetShowFileTypeIcons(false);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetShowFileTypeIcons();
            Assert.That(storedValue, Is.EqualTo(defaultShowFileTypeIcons));
        }

        [Test]
        public void ShowFileTypeIconsCanBeStoredAndRead()
        {
            // Arrange

            const bool showFileTypeIcons = true;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetShowFileTypeIcons(showFileTypeIcons);

            // Assert

            ReloadData();

            var storedValue = repository.GetShowFileTypeIcons();
            Assert.That(storedValue, Is.EqualTo(showFileTypeIcons));
        }
    }
}
