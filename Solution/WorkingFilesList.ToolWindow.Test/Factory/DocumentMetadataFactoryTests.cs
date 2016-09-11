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

using System;
using System.Windows.Media;
using Moq;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
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

            const string filePathServiceOutput = "FilePathServiceOutput";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.FilePathServiceMock
                .Setup(f => f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(filePathServiceOutput);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info);

            // Assert

            Assert.That(metadata.DisplayName, Is.EqualTo(filePathServiceOutput));
        }

        [Test]
        public void CreateWithInfoAndActivatedTimeUsesFilePathServiceToSetDisplayName()
        {
            // Arrange

            const string filePathServiceOutput = "FilePathServiceOutput";

            var info = new DocumentMetadataInfo();
            var builder = new DocumentMetadataFactoryBuilder();

            builder.FilePathServiceMock
                .Setup(f => f.ReducePath(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(filePathServiceOutput);

            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(info, DateTime.UtcNow);

            // Assert

            Assert.That(metadata.DisplayName, Is.EqualTo(filePathServiceOutput));
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
    }
}
