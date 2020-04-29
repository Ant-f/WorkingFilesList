// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 - 2020 Anthony Fung

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
using System.ComponentModel;
using System.Windows;
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

            const string displayName = "DisplayName";

            var documentSort = new TestingSortOption(
                displayName,
                string.Empty,
                ListSortDirection.Ascending,
                ProjectItemType.Document);

            var projectSort = new TestingSortOption(
                displayName,
                string.Empty,
                ListSortDirection.Ascending,
                ProjectItemType.Project);

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new ISortOption[]
                {
                    documentSort,
                    projectSort
                }
            };

            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.ProjectSortOptionName = displayName;

            // Verify

            Assert.That(preferences.ProjectSortOption, Is.EqualTo(projectSort));
        }

        [Test]
        public void ProjectSortOptionNameIsRestoredOnInstanceCreation()
        {
            // Arrange

            const string displayName = "DisplayName";

            var builder = new UserPreferencesBuilder();

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.ProjectSortOptionName = displayName;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            const string displayName = "DisplayName";

            var documentSort = new TestingSortOption(
                displayName,
                string.Empty,
                ListSortDirection.Ascending,
                ProjectItemType.Document);

            var projectSort = new TestingSortOption(
                displayName,
                string.Empty,
                ListSortDirection.Ascending,
                ProjectItemType.Project);

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new ISortOption[]
                {
                    projectSort,
                    documentSort
                }
            };

            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.DocumentSortOptionName = displayName;

            // Verify

            Assert.That(preferences.DocumentSortOption, Is.EqualTo(documentSort));
        }

        [Test]
        public void DocumentSortOptionNameIsRestoredOnInstanceCreation()
        {
            // Arrange

            const string displayName = "DisplayName";

            var builder = new UserPreferencesBuilder();

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.DocumentSortOptionName = displayName;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.GroupByProject = groupByProject;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.ShowRecentUsage = showRecentUsage;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.AssignProjectColours = assignProjectColours;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.ShowFileTypeIcons = showFileTypeIcons;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.PathSegmentCount = value;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

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

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.HighlightFileName = highlightFileName;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetHighlightFileName(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.HighlightFileName, Is.EqualTo(highlightFileName));
        }

        [Test]
        public void SettingUnityRefreshDelayStoresNewValueInRepository()
        {
            // Arrange

            const int value = 50;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.UnityRefreshDelay = value;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetUnityRefreshDelay(value));
        }

        [Test]
        public void UnityRefreshDelayValueIsRestoredOnInstanceCreation()
        {
            // Arrange

            const int value = 100;

            var builder = new UserPreferencesBuilder();

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.UnityRefreshDelay = value;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetUnityRefreshDelay(
                    It.IsAny<int>()),
                    Times.Never);

            Assert.That(preferences.UnityRefreshDelay, Is.EqualTo(value));
        }

        [Test]
        public void SettingShowConfigurationBarStoresNewValueInRepository()
        {
            // Arrange

            const bool showConfigurationBar = true;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();
            preferences.ShowConfigurationBar = false;

            // Act

            preferences.ShowConfigurationBar = showConfigurationBar;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetShowConfigurationBar(showConfigurationBar));
        }

        [TestCase(true, Visibility.Visible)]
        [TestCase(false, Visibility.Collapsed)]
        public void SettingShowConfigurationBarSetsVisibility(bool value, Visibility expected)
        {
            // Arrange

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();
            preferences.ShowConfigurationBar = !value;

            // Act

            preferences.ShowConfigurationBar = value;

            // Verify

            Assert.That(preferences.ConfigurationBarVisibility, Is.EqualTo(expected));
        }

        [Test]
        public void ShowConfigurationBarIsRestoredOnInstanceCreation()
        {
            // Arrange

            const bool showConfigurationBar = true;

            var builder = new UserPreferencesBuilder();

            builder.UserPreferencesModelRepositoryMock
                .Setup(u => u.LoadInto(It.IsAny<IUserPreferencesModel>()))
                .Callback<IUserPreferencesModel>(u =>
                {
                    u.ShowConfigurationBar = showConfigurationBar;
                });

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.UserPreferencesModelRepositoryMock
                .Verify(u => u.LoadInto(preferences),
                    Times.Once());

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.SetShowConfigurationBar(
                    It.IsAny<bool>()),
                    Times.Never);

            Assert.That(preferences.ShowConfigurationBar, Is.EqualTo(showConfigurationBar));
        }
    }
}
