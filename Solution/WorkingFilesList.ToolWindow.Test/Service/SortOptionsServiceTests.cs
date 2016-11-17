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
using System;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.ToolWindow.Service;

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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.DocumentSortOption == alphabeticalSort &&
                p.ProjectSortOption == Mock.Of<ISortOption>(s =>
                    s.HasSortDescription == false));

            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            // Returned collection should only contain sort option for
            // DocumentSortOption: ProjectSortOption.HasSortDescription is set
            // to return false

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

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.DocumentSortOption == Mock.Of<ISortOption>(s =>
                    s.HasSortDescription == false) &&
                p.ProjectSortOption == alphabeticalSort);

            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            // Returned collection should only contain sort option for
            // ProjectSortOption: DocumentSortOption.HasSortDescription is set
            // to return false

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

        [Test]
        public void ProjectSortIsReturnedBeforeDocumentSort()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();
            var projectReverseAlphabeticalSort = new ProjectReverseAlphabeticalSort();

            var preferences = Mock.Of<IUserPreferences>(p =>
                p.DocumentSortOption == alphabeticalSort &&
                p.ProjectSortOption == projectReverseAlphabeticalSort);

            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            Assert.That(appliedSortOptions.Length, Is.EqualTo(2));

            Assert.That(
                appliedSortOptions[0].Direction,
                Is.EqualTo(projectReverseAlphabeticalSort.SortDirection));

            Assert.That(
                appliedSortOptions[0].PropertyName,
                Is.EqualTo(projectReverseAlphabeticalSort.PropertyName));

            Assert.That(
                appliedSortOptions[1].Direction,
                Is.EqualTo(alphabeticalSort.SortDirection));

            Assert.That(
                appliedSortOptions[1].PropertyName,
                Is.EqualTo(alphabeticalSort.PropertyName));

            Assert.That(
                appliedSortOptions[0].Direction,
                Is.Not.EqualTo(appliedSortOptions[1].Direction));

            Assert.That(
                appliedSortOptions[0].PropertyName,
                Is.Not.EqualTo(appliedSortOptions[1].PropertyName));
        }
    }
}
