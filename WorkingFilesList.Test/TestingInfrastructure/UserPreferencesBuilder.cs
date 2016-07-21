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
using WorkingFilesList.Interface;
using WorkingFilesList.ViewModel;

namespace WorkingFilesList.Test.TestingInfrastructure
{
    internal class UserPreferencesBuilder
    {
        /// <summary>
        /// The created <see cref="UserPreferences"/> instance will have its
        /// <see cref="IUserPreferences.PathSegmentCount"/> property value
        /// set to the value of this property
        /// </summary>
        public int PathSegmentCount { get; set; }

        /// <summary>
        /// The created <see cref="UserPreferences"/> instance will have its
        /// <see cref="IUserPreferences.SelectedSortOption"/> property value
        /// set to the value of this property
        /// </summary>
        public ISortOption SelectedSortOption { get; set; }

        public Mock<IStoredSettingsRepository> StoredSettingsRepositoryMock { get; }
            = new Mock<IStoredSettingsRepository>();

        /// <summary>
        /// Create and return a new <see cref="UserPreferences"/>, configured
        /// with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="UserPreferences"/> for use in unit tests
        /// </returns>
        public UserPreferences CreateUserPreferences()
        {
            var preferences = new UserPreferences(
                StoredSettingsRepositoryMock.Object)
            {
                PathSegmentCount = PathSegmentCount,
                SelectedSortOption = SelectedSortOption
            };

            return preferences;
        }
    }
}
