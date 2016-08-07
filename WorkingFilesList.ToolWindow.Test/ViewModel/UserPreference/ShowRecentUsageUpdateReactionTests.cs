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
using System.Collections.Generic;
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class ShowRecentUsageUpdateReactionTests
    {
        [Test]
        public void UpdateCollectionUsesNormalizedUseOrderService()
        {
            // Arrange

            var serviceMock = new Mock<INormalizedUseOrderService>();

            var updateReaction = (IUpdateReaction)new ShowRecentUsageUpdateReaction(
                serviceMock.Object);

            var preferences = Mock.Of<IUserPreferences>();

            var collection = new List<DocumentMetadata>();
            var view = new ListCollectionView(collection);

            // Act

            updateReaction.UpdateCollection(view, preferences);

            // Assert

            serviceMock.Verify(s => s.SetUseOrder(collection, preferences));
        }
    }
}
