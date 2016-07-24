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
using WorkingFilesList.ToolWindow.Factory;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class DocumentMetadataFactoryBuilder
    {
        public Mock<IPathCasingRestorer> PathCasingRestorerMock { get; }
            = new Mock<IPathCasingRestorer>();

        public Mock<ITimeProvider> TimeProviderMock { get; }

        public DocumentMetadataFactoryBuilder()
        {
            TimeProviderMock = new Mock<ITimeProvider>();
        }

        public DocumentMetadataFactoryBuilder(Mock<ITimeProvider> timeProviderMock)
        {
            TimeProviderMock = timeProviderMock;
        }

        /// <summary>
        /// Create and return a new <see cref="DocumentMetadataFactory"/>,
        /// configured with the properties available in this builder instance
        /// </summary>
        /// <param name="pathCasingRestorerReturnsInput">
        /// true to add a setup to <see cref="PathCasingRestorerMock"/> so that
        /// <see cref="PathCasingRestorer.RestoreCasing"/> returns its input
        /// parameter; false otherwise
        /// </param>
        /// <returns>
        /// A new <see cref="DocumentMetadataFactory"/> for use in unit tests
        /// </returns>
        public DocumentMetadataFactory CreateDocumentMetadataFactory(
            bool pathCasingRestorerReturnsInput)
        {
            if (pathCasingRestorerReturnsInput)
            {
                PathCasingRestorerMock
                    .Setup(p => p.RestoreCasing(It.IsAny<string>()))
                    .Returns<string>(str => str);
            }

            var factory = new DocumentMetadataFactory(
                PathCasingRestorerMock.Object,
                TimeProviderMock.Object);

            return factory;
        }
    }
}
