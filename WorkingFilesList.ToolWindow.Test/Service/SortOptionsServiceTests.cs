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
using System;
using System.Collections.Generic;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class SortOptionsServiceTests
    {
        [Test]
        public void SortDescriptionIsReturnedForSelectedDocumentSortOption()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();
            var builder = new UserPreferencesBuilder();

            var preferences = builder.CreateUserPreferences();
            preferences.SelectedDocumentSortOption = alphabeticalSort;

            preferences.SelectedProjectSortOption = Mock.Of<ISortOption>(s =>
                s.HasSortDescription == false);

            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            // Returned collection should only contain sort option for
            // SelectedDocumentSortOption: the ISortOption of
            // SelectedProjectSortOption has its HasSortDescription property
            // set to return false

            Assert.That(appliedSortOptions.Length, Is.EqualTo(1));

            Assert.That(
                appliedSortOptions[0].Direction,
                Is.EqualTo(alphabeticalSort.SortDirection));

            Assert.That(
                appliedSortOptions[0].PropertyName,
                Is.EqualTo(alphabeticalSort.PropertyName));
        }

        [Test]
        public void SortDescriptionIsReturnedForApplicableSelectedProjectSortOption()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();

            preferences.SelectedDocumentSortOption = Mock.Of<ISortOption>(s =>
                s.HasSortDescription == false);

            preferences.SelectedProjectSortOption = alphabeticalSort;

            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            // Returned collection should only contain sort option for
            // SelectedProjectSortOption: the ISortOption of
            // SelectedDocumentSortOption has its HasSortDescription property
            // set to return false

            Assert.That(appliedSortOptions.Length, Is.EqualTo(1));

            Assert.That(
                appliedSortOptions[0].Direction,
                Is.EqualTo(alphabeticalSort.SortDirection));

            Assert.That(
                appliedSortOptions[0].PropertyName,
                Is.EqualTo(alphabeticalSort.PropertyName));
        }

        [Test]
        public void AssignedDisplayOrderMatchesSpecifiedOrder()
        {
            // Arrange

            const int disableSortingIndex = 0;
            const int chronologicalSortIndex = 1;
            const int alphabeticalSortIndex = 2;

            var displayOrder = new Type[3];
            displayOrder[disableSortingIndex] = typeof(DisableSorting);
            displayOrder[chronologicalSortIndex] = typeof(ChronologicalSort);
            displayOrder[alphabeticalSortIndex] = typeof(AlphabeticalSort);

            var alphabeticalSort = new AlphabeticalSort();
            var chronologicalSort = new ChronologicalSort();
            var disableSorting = new DisableSorting();

            var sortOptions = new ISortOption[]
            {
                // Order should be different from indexes specified above
                alphabeticalSort,
                chronologicalSort,
                disableSorting
            };

            var service = new SortOptionsService();

            // Act

            service.AssignDisplayOrder(displayOrder, sortOptions);

            // Assert

            Assert.That(
                disableSorting.DisplayIndex,
                Is.EqualTo(disableSortingIndex));

            Assert.That(
                chronologicalSort.DisplayIndex,
                Is.EqualTo(chronologicalSortIndex));

            Assert.That(
                alphabeticalSort.DisplayIndex,
                Is.EqualTo(alphabeticalSortIndex));
        }
    }
}
