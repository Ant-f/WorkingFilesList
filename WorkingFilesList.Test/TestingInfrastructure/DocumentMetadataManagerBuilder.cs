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
using WorkingFilesList.Service;
using WorkingFilesList.ViewModel;

namespace WorkingFilesList.Test.TestingInfrastructure
{
    internal class DocumentMetadataManagerBuilder
    {
        /// <summary>
        /// Generator to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataManager"/> instances. A
        /// <see cref="WorkingFilesList.Service.CollectionViewGenerator"/>
        /// instance will be created if this property is left null.
        /// </summary>
        public ICollectionViewGenerator CollectionViewGenerator { get; set; }

        /// <summary>
        /// Factory to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataManager"/> instances. A mock factory that
        /// outputs its input parameter is created if this property is left null.
        /// </summary>
        public IDocumentMetadataFactory DocumentMetadataFactory { get; set; }

        public Mock<ITimeProvider> TimeProviderMock { get; }
            = new Mock<ITimeProvider>();

        public ISortOptionsService SortOptionsService { get; set; }
            = new SortOptionsService();

        public IUserPreferences UserPreferences { get; set; }

        /// <summary>
        /// Creates a new <see cref="WorkingFilesList.ViewModel.UserPreferences"/>
        /// instance when <see cref="UserPreferences"/> is null.
        /// </summary>
        public UserPreferencesBuilder UserPreferencesBuilder { get; }
            = new UserPreferencesBuilder();

        /// <summary>
        /// Create and return a new <see cref="DocumentMetadataManager"/>,
        /// configured with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="DocumentMetadataManager"/> for use in unit tests
        /// </returns>
        public DocumentMetadataManager CreateDocumentMetadataManager()
        {
            if (DocumentMetadataFactory == null)
            {
                var builder = new DocumentMetadataFactoryBuilder(TimeProviderMock);
                DocumentMetadataFactory = builder.CreateDocumentMetadataFactory(true);
            }

            if (UserPreferences == null)
            {
                UserPreferences = UserPreferencesBuilder.CreateUserPreferences();
            }

            var service = new DocumentMetadataManager(
                CollectionViewGenerator ?? new CollectionViewGenerator(),
                DocumentMetadataFactory,
                SortOptionsService,
                TimeProviderMock.Object,
                UserPreferences);

            return service;
        }
    }
}
