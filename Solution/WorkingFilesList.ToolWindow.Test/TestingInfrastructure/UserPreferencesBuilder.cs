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
using System.Collections.Generic;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class UserPreferencesBuilder
    {
        public IList<ISortOption> SortOptions { get; set; }

        public Mock<IStoredSettingsRepository> StoredSettingsRepositoryMock { get; }
            = new Mock<IStoredSettingsRepository>();

        public Mock<IUserPreferencesModelRepository> UserPreferencesModelRepositoryMock { get; }
            = new Mock<IUserPreferencesModelRepository>();

        /// <summary>
        /// Create and return a new <see cref="UserPreferences"/>, configured
        /// with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="UserPreferences"/> for use in unit tests
        /// </returns>
        public UserPreferences CreateUserPreferences()
        {
            if (SortOptions == null)
            {
                SortOptions = new List<ISortOption>();
            }

            var preferences = new UserPreferences(
                StoredSettingsRepositoryMock.Object,
                SortOptions,
                UserPreferencesModelRepositoryMock.Object);

            return preferences;
        }
    }
}
