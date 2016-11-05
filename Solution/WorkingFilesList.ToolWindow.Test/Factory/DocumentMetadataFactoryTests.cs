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

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Factory
{
    [TestFixture]
    public class DocumentMetadataFactoryTests
    {
        [Test]
        public void CreateReturnsNewDocumentMetadata()
        {
            // Arrange

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var info = new DocumentMetadataInfo();

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata, Is.Not.Null);
        }

        [Test]
        public void FullNameEqualsMetadataInfoValue()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName"
            };

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.FullName, Is.EqualTo(info.FullName));
        }

        [Test]
        public void ProjectDisplayNameEqualsMetadataInfoValue()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                ProjectDisplayName = "ProjectDisplayName"
            };

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(
                metadata.ProjectNames.DisplayName,
                Is.EqualTo(info.ProjectDisplayName));
        }

        [Test]
        public void ProjectFullNameEqualsMetadataInfoValue()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                ProjectFullName = "ProjectFullName"
            };

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(
                metadata.ProjectNames.FullName,
                Is.EqualTo(info.ProjectFullName));
        }

        [Test]
        public void CorrectedFullNameEqualsPathCasingRestorerOutput()
        {
            // Arrange

            const string correctedCasing = "FullName";
            const string lowerCase = "fullname";

            var builder = new DocumentMetadataFactoryBuilder();

            builder.PathCasingRestorerMock
                .Setup(p => p.RestoreCasing(lowerCase))
                .Returns(correctedCasing);

            var factory = builder.CreateDocumentMetadataFactory(false);

            var info = new DocumentMetadataInfo
            {
                FullName = lowerCase
            };

            // Act

            var metadata = factory.Create(info);

            // Assert

            builder.PathCasingRestorerMock.Verify(p => p.RestoreCasing(lowerCase));
            Assert.That(metadata.CorrectedFullName, Is.EqualTo(correctedCasing));
        }

        [Test]
        public void CreateWithInfoUsesFilePathServiceToSetDisplayName()
        {
            // Arrange

            const string filePathServiceOutput = "Output";
            const string preHighlightSuffix = "[PreHighlight]";
            const string highlightSuffix = "[Highlight]";
            const string postHighlightSuffix = "[PostHighlight]";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.FilePathServiceMock
                .Setup(f => f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(filePathServiceOutput);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPreHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + preHighlightSuffix);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + highlightSuffix);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPostHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + postHighlightSuffix);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetPreHighlight(filePathServiceOutput));

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetHighlight(filePathServiceOutput));

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetPostHighlight(filePathServiceOutput));

            var expected =
                $"{filePathServiceOutput}{preHighlightSuffix}" +
                $"{filePathServiceOutput}{highlightSuffix}" +
                $"{filePathServiceOutput}{postHighlightSuffix}";

            Assert.That(metadata.DisplayName, Is.EqualTo(expected));
        }

        [Test]
        public void CreateWithInfoAndActivatedTimeUsesFilePathServiceToSetDisplayName()
        {
            // Arrange

            const string filePathServiceOutput = "Output";
            const string preHighlightSuffix = "[PreHighlight]";
            const string highlightSuffix = "[Highlight]";
            const string postHighlightSuffix = "[PostHighlight]";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.FilePathServiceMock
                .Setup(f => f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(filePathServiceOutput);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPreHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + preHighlightSuffix);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + highlightSuffix);

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPostHighlight(
                    It.IsAny<string>()))
                .Returns<string>(str => str + postHighlightSuffix);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info, DateTime.UtcNow);

            // Assert

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetPreHighlight(filePathServiceOutput));

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetHighlight(filePathServiceOutput));

            builder.DisplayNameHighlightEvaluatorMock.Verify(d =>
                d.GetPostHighlight(filePathServiceOutput));

            var expected =
                $"{filePathServiceOutput}{preHighlightSuffix}" +
                $"{filePathServiceOutput}{highlightSuffix}" +
                $"{filePathServiceOutput}{postHighlightSuffix}";

            Assert.That(metadata.DisplayName, Is.EqualTo(expected));
        }

        [Test]
        public void CreatePassesPathSegmentCountFromUserPreferencesToFilePathService()
        {
            // Arrange

            const int pathSegmentCount = 7;

            var info = new DocumentMetadataInfo();

            var builder = new DocumentMetadataFactoryBuilder
            {
                UserPreferences = Mock.Of<IUserPreferences>(u =>
                    u.PathSegmentCount == pathSegmentCount)
            };

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            factory.Create(info);

            // Assert

            builder.FilePathServiceMock.Verify(f => f.ReducePath(
                It.IsAny<string>(),
                pathSegmentCount));
        }

        [Test]
        public void CreateWithInfoSetsProjectBrush()
        {
            // Arrange

            var projectBrush = Brushes.MidnightBlue;

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.ProjectBrushServiceMock
                .Setup(p => p.GetBrush(
                    It.IsAny<string>(),
                    It.IsAny<IUserPreferences>()))
                .Returns(projectBrush);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.ProjectBrush, Is.EqualTo(projectBrush));
        }

        [Test]
        public void CreateWithInfoAndActivatedTimeSetsProjectBrush()
        {
            // Arrange

            var projectBrush = Brushes.MidnightBlue;

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.ProjectBrushServiceMock
                .Setup(p => p.GetBrush(
                    It.IsAny<string>(),
                    It.IsAny<IUserPreferences>()))
                .Returns(projectBrush);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info, DateTime.UtcNow);

            // Assert

            Assert.That(metadata.ProjectBrush, Is.EqualTo(projectBrush));
        }

        [Test]
        public void CreateSetsMetadataIcon()
        {
            // Arrange

            const string extension = ".extension";

            var info = new DocumentMetadataInfo
            {
                FullName = $"FullName{extension}"
            };

            var builder = new DocumentMetadataFactoryBuilder();
            var icon = Mock.Of<BitmapSource>();

            builder.DocumentIconServiceMock
                .Setup(d => d.GetIcon(extension))
                .Returns(icon);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            builder.DocumentIconServiceMock.Verify(d =>
                d.GetIcon(extension));

            Assert.That(metadata.Icon, Is.EqualTo(icon));
        }

        [Test]
        public void CreateSetsDisplayNameHighlight()
        {
            // Arrange

            const string highlight = "Highlight";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetHighlight(
                    It.IsAny<string>()))
                .Returns(highlight);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.DisplayNameHighlight, Is.EqualTo(highlight));
        }

        [Test]
        public void CreateSetsDisplayNamePostHighlight()
        {
            // Arrange

            const string postHighlight = "PostHighlight";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPostHighlight(
                    It.IsAny<string>()))
                .Returns(postHighlight);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.DisplayNamePostHighlight, Is.EqualTo(postHighlight));
        }

        [Test]
        public void CreateSetsDisplayNamePreHighlight()
        {
            // Arrange

            const string preHighlight = "PreHighlight";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.DisplayNameHighlightEvaluatorMock
                .Setup(d => d.GetPreHighlight(
                    It.IsAny<string>()))
                .Returns(preHighlight);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.DisplayNamePreHighlight, Is.EqualTo(preHighlight));
        }
    }
}
