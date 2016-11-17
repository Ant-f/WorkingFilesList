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
using System.Collections.Generic;
using System.ComponentModel;
using Moq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.Core.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.ViewModel
{
    [TestFixture]
    public class UserPreferencesTests
    {
        [Test]
        public void SettingProjectSortOptionNameStoresNewValueInRepository()
        {
            // Arrange

            const string sortOptionName = "SortOptionName";

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.ProjectSortOptionName = sortOptionName;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetProjectSortOptionName(sortOptionName));
        }

        [Test]
        public void SettingProjectSortOptionNameSetsProjectSortOption()
        {
            // Arrange

            var option = new AlphabeticalSort();

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new ISortOption[] {option}
            };

            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.ProjectSortOptionName = option.DisplayName;

            // Verify

            Assert.That(preferences.ProjectSortOption, Is.EqualTo(option));
        }

        [Test]
        public void ProjectSortOptionNameIsRestoredOnInstanceCreation()
        {
            // Arrange

            const string displayName = "DisplayName";

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new List<ISortOption>
                {
                    new TestingSortOption(
                        displayName,
                        null,
                        ListSortDirection.Ascending,
                        ProjectItemType.Project)
                }
            };

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetProjectSortOptionName())
                .Returns(displayName);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetProjectSortOptionName());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetProjectSortOptionName(
                    It.IsAny<string>()),
                    Times.Never);

            Assert.That(preferences.ProjectSortOptionName, Is.EqualTo(displayName));
        }

        [Test]
        public void SettingDocumentSortOptionNameStoresNewValueInRepository()
        {
            // Arrange

            const string sortOptionName = "SortOptionName";

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.DocumentSortOptionName = sortOptionName;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetDocumentSortOptionName(sortOptionName));
        }

        [Test]
        public void SettingDocumentSortOptionNameSetsDocumentSortOption()
        {
            // Arrange

            var option = new ChronologicalSort();

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new ISortOption[] {option}
            };

            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.DocumentSortOptionName = option.DisplayName;

            // Verify

            Assert.That(preferences.DocumentSortOption, Is.EqualTo(option));
        }

        [Test]
        public void DocumentSortOptionNameIsRestoredOnInstanceCreation()
        {
            // Arrange

            const string displayName = "DisplayName";

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new List<ISortOption>
                {
                    new TestingSortOption(
                        displayName,
                        null,
                        ListSortDirection.Ascending,
                        ProjectItemType.Document)
                }
            };

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetDocumentSortOptionName())
                .Returns(displayName);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetDocumentSortOptionName());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetDocumentSortOptionName(
                    It.IsAny<string>()),
                    Times.Never);

            Assert.That(preferences.DocumentSortOptionName, Is.EqualTo(displayName));
        }

        [Test]
        public void SettingGroupByProjectStoresNewValueInRepository()
        {
            // Arrange

            const bool groupByProject = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.GroupByProject = groupByProject;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetGroupByProject(groupByProject));
        }

        [Test]
        public void GroupByProjectValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool groupByProject = true;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetGroupByProject())
                .Returns(groupByProject);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetGroupByProject());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetGroupByProject(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.GroupByProject, Is.EqualTo(groupByProject));
        }

        [Test]
        public void SettingShowRecentUsageStoresNewValueInRepository()
        {
            // Arrange

            const bool showRecentUsage = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.ShowRecentUsage = showRecentUsage;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetShowRecentUsage(showRecentUsage));
        }

        [Test]
        public void ShowRecentUsageValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool showRecentUsage = true;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetShowRecentUsage())
                .Returns(showRecentUsage);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetShowRecentUsage());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetShowRecentUsage(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.ShowRecentUsage, Is.EqualTo(showRecentUsage));
        }

        [Test]
        public void SettingAssignProjectColoursStoresNewValueInRepository()
        {
            // Arrange

            const bool assignProjectColours = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.AssignProjectColours = assignProjectColours;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetAssignProjectColours(assignProjectColours));
        }

        [Test]
        public void AssignProjectColoursValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool assignProjectColours = true;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetAssignProjectColours())
                .Returns(assignProjectColours);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetAssignProjectColours());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetAssignProjectColours(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.AssignProjectColours, Is.EqualTo(assignProjectColours));
        }

        [Test]
        public void SettingShowFileTypeIconsStoresNewValueInRepository()
        {
            // Arrange

            const bool showFileTypeIcons = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.ShowFileTypeIcons = showFileTypeIcons;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetShowFileTypeIcons(showFileTypeIcons));
        }

        [Test]
        public void ShowFileTypeIconsValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool showFileTypeIcons = true;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetShowFileTypeIcons())
                .Returns(showFileTypeIcons);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetShowFileTypeIcons());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetShowFileTypeIcons(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.ShowFileTypeIcons, Is.EqualTo(showFileTypeIcons));
        }

        [Test]
        public void SettingPathSegmentCountStoresNewValueInRepository()
        {
            // Arrange

            const int value = 5;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.PathSegmentCount = value;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetPathSegmentCount(value));
        }

        [Test]
        public void PathSegmentCountValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const int value = 47;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetPathSegmentCount())
                .Returns(value);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetPathSegmentCount());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetPathSegmentCount(
                    It.IsAny<int>()),
                    Times.Never);

            Assert.That(preferences.PathSegmentCount, Is.EqualTo(value));
        }

        [Test]
        public void SettingHighlightFileNameStoresNewValueInRepository()
        {
            // Arrange

            const bool highlightFileName = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.HighlightFileName = highlightFileName;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetHighlightFileName(highlightFileName));
        }

        [Test]
        public void HighlightFileNameValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool highlightFileName = true;

            var builder = new UserPreferencesBuilder();
            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetHighlightFileName())
                .Returns(highlightFileName);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetHighlightFileName());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetHighlightFileName(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.HighlightFileName, Is.EqualTo(highlightFileName));
        }
    }
}
