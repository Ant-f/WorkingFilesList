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
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class FilePathServiceTests
    {
        [Test]
        public void OnePathSegmentIncludesFileOnly()
        {
            // Arrange

            var service = new FilePathService();

            // Act

            var path = service.ReducePath(@"C:\Three\Two\One.txt", 1);

            // Assert

            Assert.That(path, Is.EqualTo(@"One.txt"));
        }

        [Test]
        public void TwoPathSegmentsIncludeFileAndOneParentDirectory()
        {
            // Arrange

            var service = new FilePathService();

            // Act

            var path = service.ReducePath(@"C:\Three\Two\One.txt", 2);

            // Assert

            Assert.That(path, Is.EqualTo(@"Two\One.txt"));
        }

        [Test]
        public void FullNameIsReturnedIfPathSegmentCountIsGreaterThanSegmentsInFullName()
        {
            // Arrange

            const string fullName = @"C:\Three\Two\One.txt";

            var service = new FilePathService();

            // Act

            var path = service.ReducePath(fullName, 9);

            // Assert

            Assert.That(path, Is.EqualTo(fullName));
        }
    }
}
