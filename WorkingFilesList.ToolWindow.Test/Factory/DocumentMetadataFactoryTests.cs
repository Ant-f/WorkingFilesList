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

using NUnit.Framework;
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

            const string fullName = "FullName";

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(fullName);

            // Assert

            Assert.That(metadata, Is.Not.Null);
        }

        [Test]
        public void FullNameEqualsParameterValue()
        {
            // Arrange

            const string fullName = "FullName";

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);

            // Act

            var metadata = factory.Create(fullName);

            // Assert

            Assert.That(metadata.FullName, Is.EqualTo(fullName));
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

            // Act

            var metadata = factory.Create(lowerCase);

            // Assert

            builder.PathCasingRestorerMock.Verify(p => p.RestoreCasing(lowerCase));
            Assert.That(metadata.CorrectedFullName, Is.EqualTo(correctedCasing));
        }
    }
}
