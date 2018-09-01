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
using System.ComponentModel;
using WorkingFilesList.Core.Interface;
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

            updateReactionMock.Invocations.Clear();

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

            updateReactionMock1.Invocations.Clear();
            updateReactionMock2.Invocations.Clear();

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

            updateReactionMock1.Invocations.Clear();
            updateReactionMock2.Invocations.Clear();

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

            updateReactionMock.Invocations.Clear();

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

            updateReactionMock.Invocations.Clear();

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
