// WorkingFilesList
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.ViewModel
{
    [TestFixture]
    public class UserPreferencesTests
    {
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
        public void SettingSelectedDocumentSortOptionStoresNewValueDisplayNameInRepository()
        {
            // Arrange

            var option = new ChronologicalSort();

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.SelectedDocumentSortOption = option;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetSelectedDocumentSortOptionName(option.DisplayName));
        }

        [Test]
        public void SettingSelectedProjectSortOptionStoresNewValueDisplayNameInRepository()
        {
            // Arrange

            var option = new AlphabeticalSort();

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            // Act

            preferences.SelectedProjectSortOption = option;

            // Verify

            builder.StoredSettingsRepositoryMock
                .Verify(r => r.SetSelectedProjectSortOptionName(option.DisplayName));
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

            Assert.That(preferences.PathSegmentCount, Is.EqualTo(value));
        }

        [Test]
        public void SelectedDocumentSortOptionDisplayNameIsRestoredOnInstanceCreation()
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
                        ProjectItemType.Document | ProjectItemType.Project)
                }
            };

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetSelectedDocumentSortOptionName())
                .Returns(displayName);

            // Setup GetSelectedProjectSortOptionName so exception is not
            // thrown in UserPreferences constructor

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetSelectedProjectSortOptionName())
                .Returns(displayName);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetSelectedDocumentSortOptionName());

            Assert.That(
                preferences.SelectedDocumentSortOption.DisplayName,
                Is.EqualTo(displayName));
        }

        [Test]
        public void SelectedProjectSortOptionDisplayNameIsRestoredOnInstanceCreation()
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
                        ProjectItemType.Document | ProjectItemType.Project)
                }
            };

            // Setup GetSelectedDocumentSortOptionName so exception is not
            // thrown in UserPreferences constructor

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetSelectedDocumentSortOptionName())
                .Returns(displayName);

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetSelectedProjectSortOptionName())
                .Returns(displayName);

            // Act

            var preferences = builder.CreateUserPreferences();

            // Assert

            builder.StoredSettingsRepositoryMock
                .Verify(s => s.GetSelectedProjectSortOptionName());

            Assert.That(
                preferences.SelectedProjectSortOption.DisplayName,
                Is.EqualTo(displayName));
        }

        [Test]
        public void SettingPathSegmentCountToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const int pathSegmentCount = 7;
            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PathSegmentCount = pathSegmentCount;
            preferences.PropertyChanged += handler;

            // Act

            preferences.PathSegmentCount = pathSegmentCount;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingPathSegmentCountToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.PathSegmentCount = 7;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedDocumentSortOptionToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();
            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.SelectedDocumentSortOption = alphabeticalSort;
            preferences.PropertyChanged += handler;

            // Act

            preferences.SelectedDocumentSortOption = alphabeticalSort;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedDocumentSortOptionToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.SelectedDocumentSortOption = new AlphabeticalSort();
            preferences.PropertyChanged += handler;

            // Act

            preferences.SelectedDocumentSortOption = new ChronologicalSort();
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedProjectSortOptionToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();
            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.SelectedProjectSortOption = alphabeticalSort;
            preferences.PropertyChanged += handler;

            // Act

            preferences.SelectedProjectSortOption = alphabeticalSort;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedProjectSortOptionToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.SelectedProjectSortOption = new AlphabeticalSort();
            preferences.PropertyChanged += handler;

            // Act

            preferences.SelectedProjectSortOption = new ReverseAlphabeticalSort();
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
