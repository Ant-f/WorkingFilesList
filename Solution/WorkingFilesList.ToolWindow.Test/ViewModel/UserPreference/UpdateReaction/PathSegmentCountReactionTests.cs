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
