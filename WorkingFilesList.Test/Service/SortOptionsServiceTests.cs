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

using NUnit.Framework;
using WorkingFilesList.Model.SortOption;
using WorkingFilesList.Service;
using WorkingFilesList.Test.TestingInfrastructure;

namespace WorkingFilesList.Test.Service
{
    [TestFixture]
    public class SortOptionsServiceTests
    {
        [Test]
        public void SortDescriptionIsReturnedForSelectedSortOption()
        {
            // Arrange

            var alphabeticalSort = new AlphabeticalSort();

            var builder = new UserPreferencesBuilder
            {
                SortOptions = new[] {alphabeticalSort}
            };

            builder.StoredSettingsRepositoryMock
                .Setup(s => s.GetSelectedSortOptionName())
                .Returns(alphabeticalSort.DisplayName);

            var preferences = builder.CreateUserPreferences();
            var service = new SortOptionsService();

            // Act

            var appliedSortOptions = service.EvaluateAppliedSortDescriptions(
                preferences);

            // Assert

            Assert.That(appliedSortOptions.Length, Is.EqualTo(1));

            Assert.That(
                appliedSortOptions[0].Direction,
                Is.EqualTo(alphabeticalSort.SortDirection));

            Assert.That(
                appliedSortOptions[0].PropertyName,
                Is.EqualTo(alphabeticalSort.PropertyName));
        }
    }
}
