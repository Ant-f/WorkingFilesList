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
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class SelectedSortOptionUpdateReactionTests
    {
        [TestCase(nameof(IUserPreferences.SelectedDocumentSortOption))]
        [TestCase(nameof(IUserPreferences.SelectedProjectSortOption))]
        public void PropertyNamesIncludes(string propertyName)
        {
            // Arrange

            var sortOptionsServiceMock = new Mock<ISortOptionsService>();

            sortOptionsServiceMock
                .Setup(s => s.EvaluateAppliedSortDescriptions(
                    It.IsAny<IUserPreferences>()))
                .Returns(new SortDescription[0]);

            var userPreferencesMock = new Mock<IUserPreferences>();

            var updateReaction = new SelectedSortOptionUpdateReaction(
                sortOptionsServiceMock.Object,
                userPreferencesMock.Object);

            var collectionViewMock = new Mock<ICollectionView>
            {
                DefaultValue = DefaultValue.Mock
            };

            updateReaction.Initialize(collectionViewMock.Object);

            // Act

            userPreferencesMock.Raise(u => u.PropertyChanged += null,
                new PropertyChangedEventArgs(propertyName));

            // Assert

            sortOptionsServiceMock
                .Verify(s => s.EvaluateAppliedSortDescriptions(
                    userPreferencesMock.Object));
        }
    }
}
