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
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class PathSegmentCountReactionTests
    {
        [Test]
        public void CollectionUpdateUsesFilePathService()
        {
            // Arrange

            const int pathSegmentCount = 7;
            const string correctedName = "CorrectedName";

            var filePathServiceMock = new Mock<IFilePathService>();

            var preferences = Mock.Of<IUserPreferences>(u =>
                u.PathSegmentCount == pathSegmentCount);

            var updateReaction = new PathSegmentCountReaction(
                filePathServiceMock.Object);

            var info = new DocumentMetadataInfo();
            var metadataList = new List<DocumentMetadata>
            {
                new DocumentMetadata(info, correctedName)
            };

            var view = new ListCollectionView(metadataList);

            // Act

            updateReaction.UpdateCollection(view, preferences);

            // Assert

            filePathServiceMock.Verify(f => f.ReducePath(
                correctedName,
                pathSegmentCount));
        }

        [Test]
        public void CollectionUpdateRefreshesCollectionView()
        {
            // Arrange

            var updateReaction = new PathSegmentCountReaction(
                Mock.Of<IFilePathService>());

            var collectionViewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            // Act

            updateReaction.UpdateCollection(
                collectionViewMock.Object,
                Mock.Of<IUserPreferences>());

            // Assert

            collectionViewMock.Verify(c => c.Refresh());
        }
    }
}
