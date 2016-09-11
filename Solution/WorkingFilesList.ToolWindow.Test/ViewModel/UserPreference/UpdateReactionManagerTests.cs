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

using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class UpdateReactionManagerTests
    {
        [Test]
        public void ReactionInMappingIsTriggeredOnMatchingPropertyUpdate()
        {
            // Arrange

            const string propertyName = "PropertyName";
            var updateReactionMock = new Mock<IUpdateReaction>();

            updateReactionMock
                .Setup(u => u.UpdateCollection(
                    It.IsAny<ICollectionView>(),
                    It.IsAny<IUserPreferences>()));

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                [propertyName] = new[] {updateReactionMock.Object}
            };

            var mapping = new TestingUpdateReactionMapping(mappingTable);
            var userPreferencesMock = new Mock<IUserPreferences>();
            var collectionView = Mock.Of<ICollectionView>();

            var manager = new UpdateReactionManager(mapping, userPreferencesMock.Object);
            manager.Initialize(collectionView);

            // Calling Initialize will invoke IUpdateReaction.UpdateCollection
            // Reset calls so that it is possible to verify the number of times
            // it is called as a result of raising PropertyChanged

            updateReactionMock.ResetCalls();

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            updateReactionMock.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Once);
        }

        [Test]
        public void AllReactionsInMappingAreTriggeredOnMatchingPropertyUpdates()
        {
            // Arrange

            const string propertyName1 = "PropertyName1";
            const string propertyName2 = "PropertyName2";

            var updateReactionMock1 = new Mock<IUpdateReaction>();
            var updateReactionMock2 = new Mock<IUpdateReaction>();

            updateReactionMock1.Setup(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()));

            updateReactionMock2.Setup(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()));

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                [propertyName1] = new[] { updateReactionMock1.Object },
                [propertyName2] = new[] { updateReactionMock2.Object }
            };

            var mapping = new TestingUpdateReactionMapping(mappingTable);
            var userPreferencesMock = new Mock<IUserPreferences>();
            var collectionView = Mock.Of<ICollectionView>();

            var manager = new UpdateReactionManager(mapping, userPreferencesMock.Object);
            manager.Initialize(collectionView);

            // Calling Initialize will invoke IUpdateReaction.UpdateCollection
            // Reset calls so that it is possible to verify the number of times
            // it is called as a result of raising PropertyChanged

            updateReactionMock1.ResetCalls();
            updateReactionMock2.ResetCalls();

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName1));

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName2));

            // Assert

            updateReactionMock1.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Exactly(1));

            updateReactionMock2.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Exactly(1));
        }

        [Test]
        public void AllReactionsMappedToPropertyNameAreTriggeredOnMatchingPropertyUpdate()
        {
            // Arrange

            const string propertyName = "PropertyName";
            var updateReactionMock1 = new Mock<IUpdateReaction>();
            var updateReactionMock2 = new Mock<IUpdateReaction>();

            updateReactionMock1.Setup(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()));

            updateReactionMock2.Setup(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()));

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                [propertyName] = new[]
                {
                    updateReactionMock1.Object,
                    updateReactionMock2.Object
                }
            };

            var mapping = new TestingUpdateReactionMapping(mappingTable);
            var userPreferencesMock = new Mock<IUserPreferences>();
            var collectionView = Mock.Of<ICollectionView>();

            var manager = new UpdateReactionManager(mapping, userPreferencesMock.Object);
            manager.Initialize(collectionView);

            // Calling Initialize will invoke IUpdateReaction.UpdateCollection
            // Reset calls so that it is possible to verify the number of times
            // it is called as a result of raising PropertyChanged

            updateReactionMock1.ResetCalls();
            updateReactionMock2.ResetCalls();

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            updateReactionMock1.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Once);

            updateReactionMock2.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Once);
        }

        [Test]
        public void ReactionInMappingIsNotTriggeredOnNonMatchingPropertyUpdate()
        {
            // Arrange

            var updateReactionMock = new Mock<IUpdateReaction>();

            updateReactionMock
                .Setup(u => u.UpdateCollection(
                    It.IsAny<ICollectionView>(),
                    It.IsAny<IUserPreferences>()));

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                ["MappedProperty"] = new[] {updateReactionMock.Object}
            };

            var mapping = new TestingUpdateReactionMapping(mappingTable);
            var userPreferencesMock = new Mock<IUserPreferences>();
            var collectionView = Mock.Of<ICollectionView>();

            var manager = new UpdateReactionManager(mapping, userPreferencesMock.Object);
            manager.Initialize(collectionView);

            // Calling Initialize will invoke IUpdateReaction.UpdateCollection
            // Reset calls so that it is possible to verify the number of times
            // it is called as a result of raising PropertyChanged

            updateReactionMock.ResetCalls();

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs("UnmappedPropertyName"));

            // Assert

            updateReactionMock.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Never);
        }

        [Test]
        public void ReactionIsNotTriggeredIfCollectionViewIsNull()
        {
            // Arrange

            const string propertyName = "PropertyName";
            var updateReactionMock = new Mock<IUpdateReaction>();

            updateReactionMock
                .Setup(u => u.UpdateCollection(
                    It.IsAny<ICollectionView>(),
                    It.IsAny<IUserPreferences>()));

            var mappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                [propertyName] = new[] { updateReactionMock.Object }
            };

            var mapping = new TestingUpdateReactionMapping(mappingTable);
            var userPreferencesMock = new Mock<IUserPreferences>();

            var manager = new UpdateReactionManager(mapping, userPreferencesMock.Object);
            manager.Initialize(null);

            // Calling Initialize will invoke IUpdateReaction.UpdateCollection
            // Reset calls so that it is possible to verify the number of times
            // it is called as a result of raising PropertyChanged

            updateReactionMock.ResetCalls();

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            updateReactionMock.Verify(u => u.UpdateCollection(
                It.IsAny<ICollectionView>(),
                It.IsAny<IUserPreferences>()),
                Times.Never);
        }
    }
}
