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
using System.Windows.Data;
using System.Windows.Media;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class AssignProjectColoursReactionTests
    {
        private static DocumentMetadata CreateDocumentMetadata()
        {
            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null);
            return metadata;
        }

        [Test]
        public void UpdateCollectionAssignsBrushToEachItemInCollectionViewSource()
        {
            // Arrange

            var brush = Brushes.MidnightBlue;

            var service = Mock.Of<IProjectBrushService>(p =>
                p.GetBrush(
                    It.IsAny<string>(),
                    It.IsAny<IUserPreferences>()) == brush);

            var preferences = Mock.Of<IUserPreferences>();
            var updateReaction = new AssignProjectColoursReaction(service);

            var metadataCollection = new[]
            {
                CreateDocumentMetadata(),
                CreateDocumentMetadata(),
                CreateDocumentMetadata()
            };

            var view = new ListCollectionView(metadataCollection)
            {
                Filter = obj =>
                {
                    // Use IsActive as indication of being included in filter
                    // in this test

                    var metadata = (DocumentMetadata)obj;
                    metadata.IsActive = metadata == metadataCollection[0];
                    return metadata.IsActive;
                }
            };

            // Act

            updateReaction.UpdateCollection(view, preferences);

            // Assert

            Assert.That(metadataCollection[0].ProjectBrush, Is.EqualTo(brush));
            Assert.That(metadataCollection[1].ProjectBrush, Is.EqualTo(brush));
            Assert.That(metadataCollection[2].ProjectBrush, Is.EqualTo(brush));

            Assert.IsTrue(metadataCollection[0].IsActive);
            Assert.IsFalse(metadataCollection[1].IsActive);
            Assert.IsFalse(metadataCollection[2].IsActive);
        }
    }
}
