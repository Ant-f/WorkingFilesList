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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
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
                Mock.Of<IDisplayNameHighlightEvaluator>(),
                filePathServiceMock.Object);

            var info = new DocumentMetadataInfo();
            var metadataList = new List<DocumentMetadata>
            {
                new DocumentMetadata(info, correctedName, null)
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
                Mock.Of<IDisplayNameHighlightEvaluator>(),
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

        [Test]
        public void CollectionUpdateSetsDisplayNameHighlight()
        {
            // Arrange

            const string highlight = "Highlight";
            const string reducedPath = "ReducedPath";

            var filePathService = Mock.Of<IFilePathService>(f =>
                f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()) == reducedPath);

            var highlightEvaluatorMock = new Mock<IDisplayNameHighlightEvaluator>();

            highlightEvaluatorMock
                .Setup(h => h.GetHighlight(
                    reducedPath,
                    It.IsAny<bool>()))
                .Returns(highlight);

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, "CorrectedName", null);

            var metadataList = new List<DocumentMetadata>
            {
                metadata
            };

            var view = new ListCollectionView(metadataList);

            var updateReaction = new PathSegmentCountReaction(
                highlightEvaluatorMock.Object,
                filePathService);

            // Act

            updateReaction.UpdateCollection(view, Mock.Of<IUserPreferences>());

            // Assert

            highlightEvaluatorMock.Verify(h => h.GetHighlight(
                reducedPath,
                It.IsAny<bool>()));

            Assert.That(metadata.DisplayNameHighlight, Is.EqualTo(highlight));
        }

        [Test]
        public void CollectionUpdateSetsDisplayNamePostHighlight()
        {
            // Arrange

            const string postHighlight = "PostHighlight";
            const string reducedPath = "ReducedPath";

            var filePathService = Mock.Of<IFilePathService>(f =>
                f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()) == reducedPath);

            var highlightEvaluatorMock = new Mock<IDisplayNameHighlightEvaluator>();

            highlightEvaluatorMock
                .Setup(h => h.GetPostHighlight(
                    reducedPath,
                    It.IsAny<bool>()))
                .Returns(postHighlight);

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, "CorrectedName", null);

            var metadataList = new List<DocumentMetadata>
            {
                metadata
            };

            var view = new ListCollectionView(metadataList);

            var updateReaction = new PathSegmentCountReaction(
                highlightEvaluatorMock.Object,
                filePathService);

            // Act

            updateReaction.UpdateCollection(view, Mock.Of<IUserPreferences>());

            // Assert

            highlightEvaluatorMock.Verify(h => h.GetPostHighlight(
                reducedPath,
                It.IsAny<bool>()));

            Assert.That(metadata.DisplayNamePostHighlight, Is.EqualTo(postHighlight));
        }

        [Test]
        public void CollectionUpdateSetsDisplayNamePreHighlight()
        {
            // Arrange

            const string preHighlight = "PreHighlight";
            const string reducedPath = "ReducedPath";

            var filePathService = Mock.Of<IFilePathService>(f =>
                f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()) == reducedPath);

            var highlightEvaluatorMock = new Mock<IDisplayNameHighlightEvaluator>();

            highlightEvaluatorMock
                .Setup(h => h.GetPreHighlight(
                    reducedPath,
                    It.IsAny<bool>()))
                .Returns(preHighlight);

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, "CorrectedName", null);

            var metadataList = new List<DocumentMetadata>
            {
                metadata
            };

            var view = new ListCollectionView(metadataList);

            var updateReaction = new PathSegmentCountReaction(
                highlightEvaluatorMock.Object,
                filePathService);

            // Act

            updateReaction.UpdateCollection(view, Mock.Of<IUserPreferences>());

            // Assert

            highlightEvaluatorMock.Verify(h => h.GetPreHighlight(
                reducedPath,
                It.IsAny<bool>()));

            Assert.That(metadata.DisplayNamePreHighlight, Is.EqualTo(preHighlight));
        }
    }
}
