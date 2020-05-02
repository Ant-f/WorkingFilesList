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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Repository;

namespace WorkingFilesList.ToolWindow.Test.Repository
{
    [TestFixture]
    public class UserPreferencesModelRepositoryTests
    {
        [Test]
        public void AssignProjectColoursIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool assignProjectColours = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetAssignProjectColours() == assignProjectColours);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetAssignProjectColours());
            Assert.That(preferences.AssignProjectColours, Is.EqualTo(assignProjectColours));
        }

        [Test]
        public void AssignProjectColoursIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool assignProjectColours = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.AssignProjectColours == assignProjectColours);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetAssignProjectColours(assignProjectColours));
        }

        [Test]
        public void GroupByProjectIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool groupByProject = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetGroupByProject() == groupByProject);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetGroupByProject());
            Assert.That(preferences.GroupByProject, Is.EqualTo(groupByProject));
        }

        [Test]
        public void GroupByProjectIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool groupByProject = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.GroupByProject == groupByProject);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetGroupByProject(groupByProject));
        }

        [Test]
        public void HighlightFileNameIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool highlightFileName = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetHighlightFileName() == highlightFileName);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetHighlightFileName());
            Assert.That(preferences.HighlightFileName, Is.EqualTo(highlightFileName));
        }

        [Test]
        public void HighlightFileNameIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool highlightFileName = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.HighlightFileName == highlightFileName);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetHighlightFileName(highlightFileName));
        }

        [Test]
        public void ShowFileTypeIconsIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool showFileTypeIcons = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetShowFileTypeIcons() == showFileTypeIcons);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetShowFileTypeIcons());
            Assert.That(preferences.ShowFileTypeIcons, Is.EqualTo(showFileTypeIcons));
        }

        [Test]
        public void ShowFileTypeIconsIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool showFileTypeIcons = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.ShowFileTypeIcons == showFileTypeIcons);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetShowFileTypeIcons(showFileTypeIcons));
        }

        [Test]
        public void ShowRecentUsageIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool showRecentUsage = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetShowRecentUsage() == showRecentUsage);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetShowRecentUsage());
            Assert.That(preferences.ShowRecentUsage, Is.EqualTo(showRecentUsage));
        }

        [Test]
        public void ShowRecentUsageIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool showRecentUsage = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.ShowRecentUsage == showRecentUsage);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetShowRecentUsage(showRecentUsage));
        }

        [Test]
        public void PathSegmentCountIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const int pathSegmentCount = 1;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetPathSegmentCount() == pathSegmentCount);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetPathSegmentCount());
            Assert.That(preferences.PathSegmentCount, Is.EqualTo(pathSegmentCount));
        }

        [Test]
        public void PathSegmentCountIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const int pathSegmentCount = 1;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.PathSegmentCount == pathSegmentCount);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetPathSegmentCount(pathSegmentCount));
        }

        [Test]
        public void UnityRefreshDelayIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const int unityRefreshDelay = 100;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetUnityRefreshDelay() == unityRefreshDelay);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetUnityRefreshDelay());
            Assert.That(preferences.UnityRefreshDelay, Is.EqualTo(unityRefreshDelay));
        }

        [Test]
        public void UnityRefreshDelayIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const int unityRefreshDelay = 125;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.UnityRefreshDelay == unityRefreshDelay);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetUnityRefreshDelay(unityRefreshDelay));
        }

        [Test]
        public void DocumentSortOptionNameIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const string documentSortName = "DocumentSortName";

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetDocumentSortOptionName() == documentSortName);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetDocumentSortOptionName());
            Assert.That(preferences.DocumentSortOptionName, Is.EqualTo(documentSortName));
        }

        [Test]
        public void DocumentSortOptionNameIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const string documentSortName = "DocumentSortName";

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.DocumentSortOptionName == documentSortName);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetDocumentSortOptionName(documentSortName));
        }

        [Test]
        public void SelectedProjectSortIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const string projectSortName = "ProjectSortName";

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetProjectSortOptionName() == projectSortName);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetProjectSortOptionName());
            Assert.That(preferences.ProjectSortOptionName, Is.EqualTo(projectSortName));
        }

        [Test]
        public void SelectedProjectSortIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const string projectSortName = "ProjectSortName";

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.ProjectSortOptionName == projectSortName);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetProjectSortOptionName(projectSortName));
        }

        [Test]
        public void ShowConfigurationBarIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool showConfigurationBar = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.ShowConfigurationBar == showConfigurationBar);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetShowConfigurationBar(showConfigurationBar));
        }

        [Test]
        public void ShowConfigurationBarIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool showConfigurationBar = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetShowConfigurationBar() == showConfigurationBar);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetShowConfigurationBar());
            Assert.That(preferences.ShowConfigurationBar, Is.EqualTo(showConfigurationBar));
        }

        [Test]
        public void ShowSearchBarIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const bool showSearchBar = true;

            var preferences = Mock.Of<IUserPreferencesModel>(p =>
                p.ShowSearchBar == showSearchBar);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetShowSearchBar(showSearchBar));
        }

        [Test]
        public void ShowSearchBarIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const bool showSearchBar = true;

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetShowSearchBar() == showSearchBar);

            var preferencesModelRepository = new UserPreferencesModelRepository(
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetShowSearchBar());
            Assert.That(preferences.ShowSearchBar, Is.EqualTo(showSearchBar));
        }
    }
}
