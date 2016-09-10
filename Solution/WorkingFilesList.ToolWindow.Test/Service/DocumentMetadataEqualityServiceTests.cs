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
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class DocumentMetadataEqualityServiceTests
    {
        [Test]
        public void CompareReturnsTrueIfAllInfoPropertiesMatch()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "FullName",
                ProjectDisplayName = "ProjectDisplayName",
                ProjectFullName = "ProjectFullName"
            };

            var metadata = new DocumentMetadata(info, null);
            var service = new DocumentMetadataEqualityService();

            // Act

            var isEqual = service.Compare(info, metadata);

            // Assert

            Assert.IsTrue(isEqual);
        }

        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, false)]
        [TestCase(false, true, true)]
        [TestCase(false, false, true)]
        [TestCase(true, false, true)]
        public void CompareReturnsFalseIfAnyInfoPropertyDoesNotMatch(
            bool useMatchingFullName,
            bool useMatchingProjectDisplayName,
            bool useMatchingProjectFullName)
        {
            // Arrange

            const string infoFullName = "FullName";
            const string infoProjectDisplayName = "ProjectDisplayName";
            const string infoProjectFullName = "ProjectFullName";

            const string prefix = "Different";

            var fullNamePrefix = useMatchingFullName
                ? string.Empty
                : prefix;

            var projectDisplayNamePrefix = useMatchingProjectDisplayName
                ? string.Empty
                : prefix;

            var projectFullNamePrefix = useMatchingProjectFullName
                ? string.Empty
                : prefix;

            var metadataFullName =
                fullNamePrefix +
                infoFullName;

            var metadataProjectDisplayName =
                projectDisplayNamePrefix +
                infoProjectDisplayName;

            var metadataProjectFullName =
                projectFullNamePrefix +
                infoProjectFullName;

            var info = new DocumentMetadataInfo
            {
                FullName = infoFullName,
                ProjectDisplayName = infoProjectDisplayName,
                ProjectFullName = infoProjectFullName
            };

            var metadata = new DocumentMetadata(new DocumentMetadataInfo
            {
                FullName = metadataFullName,
                ProjectDisplayName = metadataProjectDisplayName,
                ProjectFullName = metadataProjectFullName
            }, null);

            var service = new DocumentMetadataEqualityService();

            // Act

            var isEqual = service.Compare(info, metadata);

            // Assert

            Assert.IsFalse(isEqual);
        }
    }
}
