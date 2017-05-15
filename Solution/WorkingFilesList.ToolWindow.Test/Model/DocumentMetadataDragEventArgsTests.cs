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
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Test.Model
{
    [TestFixture]
    public class DocumentMetadataDragEventArgsTests
    {
        [Test]
        public void DropTargetIsAssigned()
        {
            // Arrange

            var dropTarget = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Act

            var args = new DocumentMetadataDragEventArgs(dropTarget, null);

            // Assert

            Assert.That(args.DropTarget, Is.EqualTo(dropTarget));
        }

        [Test]
        public void DragSourceIsAssigned()
        {
            // Arrange

            var dragSource = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Act

            var args = new DocumentMetadataDragEventArgs(null, dragSource);

            // Assert

            Assert.That(args.DragSource, Is.EqualTo(dragSource));
        }
    }
}
