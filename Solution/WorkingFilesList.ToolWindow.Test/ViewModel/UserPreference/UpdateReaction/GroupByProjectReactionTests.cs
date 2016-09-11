// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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
