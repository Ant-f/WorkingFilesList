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

using Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.ViewModel.Command;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    [TestFixture]
    public class ToggleIsPinnedTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var manager = Mock.Of<IDocumentMetadataManager>();
            var command = new ToggleIsPinned(manager);

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteInvokesDocumentMetadataManagerTogglePinnedStatus()
        {
            // Arrange

            var manager = Mock.Of<IDocumentMetadataManager>();
            var command = new ToggleIsPinned(manager);

            var metadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Act

            command.Execute(metadata);

            // Assert

            Mock.Get(manager).Verify(m =>
                m.TogglePinnedStatus(metadata),
                Times.Once);
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWithNullParameter()
        {
            // Arrange

            var manager = Mock.Of<IDocumentMetadataManager>();
            var command = new ToggleIsPinned(manager);

            // Act, Assert

            Assert.DoesNotThrow(() => command.Execute(null));

            Mock.Get(manager).Verify(m =>
                m.TogglePinnedStatus(It.IsAny<DocumentMetadata>()),
                Times.Never);
        }
    }
}
