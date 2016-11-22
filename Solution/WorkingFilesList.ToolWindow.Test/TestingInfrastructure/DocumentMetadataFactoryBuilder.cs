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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Factory;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class DocumentMetadataFactoryBuilder
    {
        public Mock<IDisplayNameHighlightEvaluator> DisplayNameHighlightEvaluatorMock { get; }
            = new Mock<IDisplayNameHighlightEvaluator>();

        public Mock<IDocumentIconService> DocumentIconServiceMock { get; }
            = new Mock<IDocumentIconService>();

        public Mock<IFilePathService> FilePathServiceMock { get; }
            = new Mock<IFilePathService>();

        public Mock<IPathCasingRestorer> PathCasingRestorerMock { get; }
            = new Mock<IPathCasingRestorer>();

        public Mock<IProjectBrushService> ProjectBrushServiceMock { get; }
            = new Mock<IProjectBrushService>();

        public Mock<ITimeProvider> TimeProviderMock { get; }

        public IUserPreferences UserPreferences { get; set; }

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
                DisplayNameHighlightEvaluatorMock.Object,
                DocumentIconServiceMock.Object,
                FilePathServiceMock.Object,
                PathCasingRestorerMock.Object,
                ProjectBrushServiceMock.Object,
                TimeProviderMock.Object,
                UserPreferences ?? Mock.Of<IUserPreferences>());

            return factory;
        }
    }
}
