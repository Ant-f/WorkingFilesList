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
using System.Collections.Generic;
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.AssignProjectColours == assignProjectColours);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.GroupByProject == groupByProject);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.HighlightFileName == highlightFileName);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.ShowFileTypeIcons == showFileTypeIcons);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.ShowRecentUsage == showRecentUsage);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
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
                Mock.Of<IOptionsLists>(),
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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.PathSegmentCount == pathSegmentCount);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var preferencesModelRepository = new UserPreferencesModelRepository(
                Mock.Of<IOptionsLists>(),
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetPathSegmentCount(pathSegmentCount));
        }

        [Test]
        public void SelectedDocumentSortIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const string documentSort = "DocumentSort";

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetSelectedDocumentSortType() == documentSort);

            var optionsLists = Mock.Of<IOptionsLists>(o =>
                o.DocumentSortOptions == new List<ISortOption>
                {Mock.Of<ISortOption>(s => s.DisplayName == documentSort)});

            var preferencesModelRepository = new UserPreferencesModelRepository(
                optionsLists,
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetSelectedDocumentSortType());

            Assert.That(preferences.SelectedDocumentSortOption.DisplayName,
                Is.EqualTo(documentSort));
        }

        [Test]
        public void SelectedDocumentSortIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const string documentSortName = "DocumentSortName";

            var documentSort = Mock.Of<ISortOption>(s =>
                s.DisplayName == documentSortName);

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.SelectedDocumentSortOption == documentSort);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var optionsLists = Mock.Of<IOptionsLists>(o =>
                o.DocumentSortOptions == new List<ISortOption> {documentSort});

            var preferencesModelRepository = new UserPreferencesModelRepository(
                optionsLists,
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetSelectedDocumentSortType(documentSortName));
        }

        [Test]
        public void SelectedProjectSortIsRestoredWhenLoadingUserPreferencesModel()
        {
            // Arrange

            const string projectSort = "ProjectSort";

            var settingsRepository = Mock.Of<IStoredSettingsRepository>(s =>
                s.GetSelectedProjectSortType() == projectSort);

            var optionsLists = Mock.Of<IOptionsLists>(o =>
                o.ProjectSortOptions == new List<ISortOption>
                {Mock.Of<ISortOption>(s => s.DisplayName == projectSort)});

            var preferencesModelRepository = new UserPreferencesModelRepository(
                optionsLists,
                settingsRepository);

            var preferences = new UserPreferencesModel();

            // Act

            preferencesModelRepository.LoadInto(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s => s.GetSelectedProjectSortType());

            Assert.That(preferences.SelectedProjectSortOption.DisplayName,
                Is.EqualTo(projectSort));
        }

        [Test]
        public void SelectedProjectSortIsStoredWhenSavingUserPreferencesModel()
        {
            // Arrange

            const string projectSortName = "ProjectSortName";

            var projectSort = Mock.Of<ISortOption>(s =>
                s.DisplayName == projectSortName);

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.SelectedProjectSortOption == projectSort);

            var settingsRepository = Mock.Of<IStoredSettingsRepository>();

            var optionsLists = Mock.Of<IOptionsLists>(o =>
                o.ProjectSortOptions == new List<ISortOption> {projectSort});

            var preferencesModelRepository = new UserPreferencesModelRepository(
                optionsLists,
                settingsRepository);

            // Act

            preferencesModelRepository.SaveModel(preferences);

            // Assert

            Mock.Get(settingsRepository).Verify(s =>
                s.SetSelectedProjectSortType(projectSortName));
        }
    }
}
