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

using NUnit.Framework;
using WorkingFilesList.Core.Model;
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

            var metadata = new DocumentMetadata(info, null, null);
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
            }, null, null);

            var service = new DocumentMetadataEqualityService();

            // Act

            var isEqual = service.Compare(info, metadata);

            // Assert

            Assert.IsFalse(isEqual);
        }
    }
}
