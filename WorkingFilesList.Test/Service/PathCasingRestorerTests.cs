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

using System;
using NUnit.Framework;
using System.IO;
using WorkingFilesList.Service;

namespace WorkingFilesList.Test.Service
{
    [TestFixture]
    public class PathCasingRestorerTests
    {
        [Test]
        public void PathCasingIsRestored()
        {
            // Arrange

            var runningDir = AppDomain.CurrentDomain.BaseDirectory;

            var relativePath = Path.Combine(
                "TestingData",
                "PathCasingRestorerTestFile.txt");

            var fullName = Path.Combine(runningDir, relativePath);
            var lowerCase = fullName.ToLowerInvariant();

            var restorer = new PathCasingRestorer();

            // Act

            // Full name is converted to be entirely lower case; this should be
            // corrected by the call to RestoreCasing
            var output = restorer.RestoreCasing(lowerCase);

            // Assert

            // String comparison is case sensitive; only assert that casing is
            // correct on the relative path so the test is not affect by
            // relocating the project
            Assert.That(output, Contains.Substring(relativePath));
        }

        [Test]
        public void FirstCharIsUpperCase()
        {
            // Arrange

            var runningDir = AppDomain.CurrentDomain.BaseDirectory;

            var relativePath = Path.Combine(
                "TestingData",
                "PathCasingRestorerTestFile.txt");

            var fullName = Path.Combine(runningDir, relativePath);

            var restorer = new PathCasingRestorer();

            // Act

            var output = restorer.RestoreCasing(fullName);

            // Assert

            var isUpperCase = char.IsUpper(output[0]);
            Assert.IsTrue(isUpperCase);
        }
    }
}
