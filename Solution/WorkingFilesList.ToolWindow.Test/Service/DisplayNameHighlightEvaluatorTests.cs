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
    public class DisplayNameHighlightEvaluatorTests
    {
        [Test]
        public void PreHighlightIsDirectoryNameWithDirectorySeparator()
        {
            // Arrange

            const string directoryName = @"DirectoryName\";

            var input = $@"{directoryName}\fileName.extension";
            var evaluator = new DisplayNameHighlightEvaluator();

            // Act

            var result = evaluator.GetPreHighlight(input);

            // Arrange

            Assert.That(result, Is.EqualTo(directoryName));
        }

        [Test]
        public void PreHighlightDoesNotContainDirectorySeparatorWhenDirectoryIsEmpty()
        {
            // Arrange

            const string input = @"fileName.extension";
            var evaluator = new DisplayNameHighlightEvaluator();

            // Act

            var result = evaluator.GetPreHighlight(input);

            // Arrange

            Assert.IsEmpty(result);
        }

        [Test]
        public void HighlightIsFileName()
        {
            // Arrange

            const string fileName = "FileName";

            var input = $@"DirectoryName\{fileName}.extension";
            var evaluator = new DisplayNameHighlightEvaluator();

            // Act

            var result = evaluator.GetHighlight(input);

            // Arrange

            Assert.That(result, Is.EqualTo(fileName));
        }

        [Test]
        public void PostHighlightIsExtension()
        {
            // Arrange

            const string extension = ".extension";

            var input = $@"DirectoryName\FileName{extension}";
            var evaluator = new DisplayNameHighlightEvaluator();

            // Act

            var result = evaluator.GetPostHighlight(input);

            // Arrange

            Assert.That(result, Is.EqualTo(extension));
        }
    }
}
