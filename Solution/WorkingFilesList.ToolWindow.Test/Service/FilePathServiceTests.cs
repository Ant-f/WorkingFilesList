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
