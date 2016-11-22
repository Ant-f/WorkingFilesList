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

using EnvDTE;
using Moq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class DteEventsSubscriberBuilder
    {
        public Mock<IProjectItemsEventsService> ProjectItemsEventsServiceMock { get; }
            = new Mock<IProjectItemsEventsService>();

        public Mock<ISolutionEventsService> SolutionEventsServiceMock { get; }
            = new Mock<ISolutionEventsService>();

        public Mock<IWindowEventsService> WindowEventsServiceMock { get; }
            = new Mock<IWindowEventsService>();

        /// <summary>
        /// Create and return a new <see cref="DteEventsSubscriber"/>, configured
        /// with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="DteEventsSubscriber"/> for use in unit tests
        /// </returns>
        public DteEventsSubscriber CreateDteEventsSubscriber()
        {
            WindowEventsServiceMock.Setup(w => w
                .WindowActivated(
                    It.IsAny<Window>(),
                    It.IsAny<Window>()));

            WindowEventsServiceMock.Setup(w => w
                .WindowClosing(It.IsAny<Window>()));

            WindowEventsServiceMock.Setup(w => w
                .WindowCreated(It.IsAny<Window>()));

            var subscriber = new DteEventsSubscriber(
                ProjectItemsEventsServiceMock.Object,
                SolutionEventsServiceMock.Object,
                WindowEventsServiceMock.Object);

            return subscriber;
        }
    }
}
