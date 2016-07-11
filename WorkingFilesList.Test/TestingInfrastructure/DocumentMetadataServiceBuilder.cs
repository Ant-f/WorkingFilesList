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
using WorkingFilesList.Factory;
using WorkingFilesList.Interface;
using WorkingFilesList.Model;
using WorkingFilesList.Service;
using WorkingFilesList.ViewModel;

namespace WorkingFilesList.Test.TestingInfrastructure
{
    internal class DocumentMetadataServiceBuilder
    {
        /// <summary>
        /// Factory to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataService"/> instances. A mock factory that
        /// outputs its input parameter is created if this property is left null.
        /// </summary>
        public IDocumentMetadataFactory DocumentMetadataFactory { get; set; }

        public Mock<ITimeProvider> TimeProviderMock { get; }
            = new Mock<ITimeProvider>();

        public IUserPreferences UserPreferences { get; set; }
            = new UserPreferences();

        /// <summary>
        /// Create and return a new <see cref="DocumentMetadataService"/>,
        /// configured with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="DocumentMetadataService"/> for use in unit tests
        /// </returns>
        public DocumentMetadataService CreateDocumentMetadataService()
        {
            if (DocumentMetadataFactory == null)
            {
                var builder = new DocumentMetadataFactoryBuilder(TimeProviderMock);
                DocumentMetadataFactory = builder.CreateDocumentMetadataFactory(true);
            }

            var service = new DocumentMetadataService(
                DocumentMetadataFactory,
                TimeProviderMock.Object,
                UserPreferences);

            return service;
        }
    }
}
