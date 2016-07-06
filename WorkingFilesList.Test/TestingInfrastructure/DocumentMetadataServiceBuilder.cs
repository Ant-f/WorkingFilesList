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

namespace WorkingFilesList.Test.TestingInfrastructure
{
    internal class DocumentMetadataServiceBuilder
    {
        public Mock<IPathCasingRestorer> PathCasingRestorerMock { get; }
            = new Mock<IPathCasingRestorer>();

        public Mock<ITimeProvider> TimeProviderMock { get; }
            = new Mock<ITimeProvider>();

        /// <summary>
        /// Create and return a new <see cref="DocumentMetadataService"/>,
        /// configured with the properties available in this builder instance
        /// </summary>
        /// <param name="pathCasingRestorerReturnsInput">
        /// Adds a setup to <see cref="PathCasingRestorerMock"/> so that
        /// <see cref="PathCasingRestorer.RestoreCasing"/> returns its input
        /// parameter
        /// </param>
        /// <returns>
        /// A new <see cref="DocumentMetadataService"/> for use in unit tests
        /// </returns>
        public DocumentMetadataService CreateDocumentMetadataService(
            bool pathCasingRestorerReturnsInput)
        {
            if (pathCasingRestorerReturnsInput)
            {
                PathCasingRestorerMock
                    .Setup(p => p.RestoreCasing(It.IsAny<string>()))
                    .Returns<string>(str => str);
            }

            var service = new DocumentMetadataService(
                PathCasingRestorerMock.Object,
                TimeProviderMock.Object);

            return service;
        }
    }
}
