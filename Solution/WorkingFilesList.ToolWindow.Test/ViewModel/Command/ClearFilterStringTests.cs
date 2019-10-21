// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2019 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.ViewModel.Command;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    [TestFixture]
    public class ClearFilterStringTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var manager = Mock.Of<IDocumentMetadataManager>();
            var command = new ClearFilterString(manager);

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteSetsDocumentMetadataManagerFilterStringToEmptyString()
        {
            // Arrange

            var manager = Mock.Of<IDocumentMetadataManager>();
            var command = new ClearFilterString(manager);

            // Act

            command.Execute(null);

            // Assert

            Mock.Get(manager).VerifySet(m =>
                m.FilterString = string.Empty,
                Times.Once);
        }
    }
}
