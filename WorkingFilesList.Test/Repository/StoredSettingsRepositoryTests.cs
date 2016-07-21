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
using WorkingFilesList.Repository;
using WorkingFilesList.Test.TestingInfrastructure;

namespace WorkingFilesList.Test.Repository
{
    [TestFixture, Explicit]
    public class StoredSettingsRepositoryTests
    {
        /// <summary>
        /// Reload stored Settings data. This method is implementation-specific,
        /// as it doesn't seem possible to simulate accessing the stored value
        /// across multiple sessions otherwise
        /// </summary>
        private static void ReloadData()
        {
            WorkingFilesList.Properties.Settings.Default.Reload();
        }

        [SetUp, OneTimeTearDown]
        public void ResetStoredData()
        {
            CommonMethods.ResetStoredRepositoryData();
        }

        [Test]
        public void PathSegmentCountCanBeReset()
        {
            // Arrange

            const int defaultValue = 1;

            var repository = new StoredSettingsRepository();
            repository.SetPathSegmentCount(7);

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(defaultValue));
        }

        [Test]
        public void PathSegmentCountCanBeStoredAndRead()
        {
            // Arrange

            const int pathSegmentCount = 7;
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetPathSegmentCount(pathSegmentCount);

            // Assert

            ReloadData();

            var storedValue = repository.GetPathSegmentCount();
            Assert.That(storedValue, Is.EqualTo(pathSegmentCount));
        }

        [Test]
        public void SelectedSortOptionNameCanBeReset()
        {
            // Arrange

            const string defaultValue = "A-Z";

            var repository = new StoredSettingsRepository();
            repository.SetSelectedSortOptionName("Sort Option");

            // Act

            repository.Reset();

            // Assert

            var storedValue = repository.GetSelectedSortOptionName();
            Assert.That(storedValue, Is.EqualTo(defaultValue));
        }

        [Test]
        public void SelectedSortOptionNameCanBeStoredAndRead()
        {
            // Arrange

            const string name = "Name";
            var repository = new StoredSettingsRepository();

            // Act

            repository.SetSelectedSortOptionName(name);

            // Assert

            ReloadData();

            var storedValue = repository.GetSelectedSortOptionName();
            Assert.That(storedValue, Is.EqualTo(name));
        }
    }
}
