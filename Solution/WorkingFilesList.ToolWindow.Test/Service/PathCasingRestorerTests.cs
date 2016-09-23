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
using NUnit.Framework;
using System.IO;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
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
