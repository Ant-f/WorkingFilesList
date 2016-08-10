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

using Moq;
using NUnit.Framework;
using System.Windows.Data;
using System.Windows.Media;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class AssignProjectColoursReactionTests
    {
        private static DocumentMetadata CreateDocumentMetadata()
        {
            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty);
            return metadata;
        }

        [Test]
        public void UpdateCollectionAssignsBrushToEachItem()
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

            var view = new ListCollectionView(metadataCollection);

            // Act

            updateReaction.UpdateCollection(view, preferences);

            // Assert

            Assert.That(metadataCollection[0].ProjectBrush, Is.EqualTo(brush));
            Assert.That(metadataCollection[1].ProjectBrush, Is.EqualTo(brush));
            Assert.That(metadataCollection[2].ProjectBrush, Is.EqualTo(brush));
        }
    }
}
