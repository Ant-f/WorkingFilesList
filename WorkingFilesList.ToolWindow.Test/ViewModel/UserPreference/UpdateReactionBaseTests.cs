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

using Moq;
using NUnit.Framework;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class UpdateReactionBaseTests
    {
        [Test]
        public void MatchingNameInPropertyChangedEventTriggersCollectionUpdate()
        {
            // Arrange

            const string propertyName = "PropertyName";
            var updateOccurred = false;

            var userPreferencesMock = new Mock<IUserPreferences>();

            var updateReaction = new TestingUpdateReaction(
                userPreferencesMock.Object,
                propertyName, () =>
                {
                    updateOccurred = true;
                });

            var view = new ListCollectionView(new ArrayList());
            updateReaction.Initialize(view);

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            Assert.IsTrue(updateOccurred);
        }

        [Test]
        public void NonMatchingNameInPropertyChangedEventDoesNotTriggerCollectionUpdate()
        {
            // Arrange

            var updateOccurred = false;

            var userPreferencesMock = new Mock<IUserPreferences>();

            var updateReaction = new TestingUpdateReaction(
                userPreferencesMock.Object,
                "PropertyName", () =>
                {
                    updateOccurred = true;
                });

            var view = new ListCollectionView(new ArrayList());
            updateReaction.Initialize(view);

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs("AnotherPropertyName"));

            // Assert

            Assert.IsFalse(updateOccurred);
        }

        [Test]
        public void CollectionUpdateIsNotTriggeredIfCollectionIsNull()
        {
            // Arrange

            const string propertyName = "PropertyName";
            var updateOccurred = false;

            var userPreferencesMock = new Mock<IUserPreferences>();

            var updateReaction = new TestingUpdateReaction(
                userPreferencesMock.Object,
                propertyName, () =>
                {
                    updateOccurred = true;
                });

            updateReaction.Initialize(null);

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            Assert.IsFalse(updateOccurred);
        }
    }
}
