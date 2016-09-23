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
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class GroupByProjectReactionTests
    {
        [Test]
        public void PropertyGroupDescriptionForProjectNamesIsAddedWhenGroupByProjectIsTrue()
        {
            // Arrange

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();
            preferences.GroupByProject = true;

            var collection = new DocumentMetadata[0];
            var view = new ListCollectionView(collection);

            var reaction = new GroupByProjectReaction();

            // Act

            reaction.UpdateCollection(view, preferences);

            // Assert

            Assert.That(view.GroupDescriptions.Count, Is.EqualTo(1));

            var description = (PropertyGroupDescription) view.GroupDescriptions[0];
            const string propertyName = nameof(DocumentMetadata.ProjectNames);
            Assert.That(description.PropertyName, Is.EqualTo(propertyName));
        }

        [Test]
        public void GroupDescriptionsAreEmptyWhenGroupByProjectIsFalse()
        {
            // Arrange

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();
            preferences.GroupByProject = false;

            var collection = new DocumentMetadata[0];
            var view = new ListCollectionView(collection);

            var reaction = new GroupByProjectReaction();

            // Act

            reaction.UpdateCollection(view, preferences);

            // Assert

            Assert.That(view.GroupDescriptions, Is.Empty);
        }
    }
}
