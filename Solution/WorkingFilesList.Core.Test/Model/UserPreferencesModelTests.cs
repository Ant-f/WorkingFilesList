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

using NUnit.Framework;
using System.ComponentModel;
using WorkingFilesList.Core.Model;

namespace WorkingFilesList.Core.Test.Model
{
    [TestFixture]
    public class UserPreferencesModelTests
    {
        [Test]
        public void SettingSelectedProjectSortOptionToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string sortOptionName = "SortOptionName";
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.ProjectSortOptionName = sortOptionName;
            preferences.PropertyChanged += handler;

            // Act

            preferences.ProjectSortOptionName = sortOptionName;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedProjectSortOptionToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.ProjectSortOptionName = "SortOptionName";
            preferences.PropertyChanged += handler;

            // Act

            preferences.ProjectSortOptionName = "AnotherSortOptionName";
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedDocumentSortOptionToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string sortOptionName = "SortOptionName";
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.DocumentSortOptionName = sortOptionName;
            preferences.PropertyChanged += handler;

            // Act

            preferences.DocumentSortOptionName = sortOptionName;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingSelectedDocumentSortOptionToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.DocumentSortOptionName = "SortOptionName";
            preferences.PropertyChanged += handler;

            // Act

            preferences.DocumentSortOptionName = "AnotherSortOptionName";
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingGroupByProjectToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool groupByProject = true;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.GroupByProject = groupByProject;
            preferences.PropertyChanged += handler;

            // Act

            preferences.GroupByProject = groupByProject;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingGroupByProjectToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.GroupByProject = true;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingShowRecentUsageToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool showRecentUsage = true;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.ShowRecentUsage = showRecentUsage;
            preferences.PropertyChanged += handler;

            // Act

            preferences.ShowRecentUsage = showRecentUsage;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingShowRecentUsageToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.ShowRecentUsage = true;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingAssignProjectColoursToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool assignProjectColours = true;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.AssignProjectColours = assignProjectColours;
            preferences.PropertyChanged += handler;

            // Act

            preferences.AssignProjectColours = assignProjectColours;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingAssignProjectColoursToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.AssignProjectColours = true;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingShowFileTypeIconsToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool showFileTypeIcons = true;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.ShowFileTypeIcons = showFileTypeIcons;
            preferences.PropertyChanged += handler;

            // Act

            preferences.ShowFileTypeIcons = showFileTypeIcons;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingShowFileTypeIconsToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.ShowFileTypeIcons = true;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingPathSegmentCountToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const int pathSegmentCount = 7;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

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

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

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
        public void SettingHighlightFileNameToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool highlightFileName = true;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.HighlightFileName = highlightFileName;
            preferences.PropertyChanged += handler;

            // Act

            preferences.HighlightFileName = highlightFileName;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingHighlightFileNameToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.HighlightFileName = true;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingUnityRefreshDelayToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const int unityRefreshDelay = 100;
            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.UnityRefreshDelay = unityRefreshDelay;
            preferences.PropertyChanged += handler;

            // Act

            preferences.UnityRefreshDelay = unityRefreshDelay;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingUnityRefreshDelayToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var preferences = new UserPreferencesModel();
            var propertyChangedRaised = false;

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            preferences.PropertyChanged += handler;

            // Act

            preferences.UnityRefreshDelay = 100;
            preferences.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
