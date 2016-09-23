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
using NUnit.Framework;
using System.Threading;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service.Locator;

namespace WorkingFilesList.Test
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class WorkingFilesWindowTests
    {
        private const string DefaultCaption = "Working Files List";

        private ViewModelService CreateViewModelService(
            out Mock<ISolutionEventsService> solutionEventsServiceMock)
        {
            solutionEventsServiceMock = new Mock<ISolutionEventsService>();

            // Call constructor to assign service/view-model objects
            var viewModelService = new ViewModelService(
                Mock.Of<IAboutPanelService>(),
                Mock.Of<ICommands>(),
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IOptionsLists>(),
                solutionEventsServiceMock.Object,
                Mock.Of<IUserPreferences>());

            return viewModelService;
        }

        [Test]
        public void DefaultCaptionIsDisplayedOnWindowCreation()
        {
            // Arrange

            Mock<ISolutionEventsService> solutionEventsServiceMock;
            var service = CreateViewModelService(out solutionEventsServiceMock);

            // Act

            var window = new WorkingFilesWindow();

            // Assert

            Assert.That(window.Caption, Is.EqualTo(DefaultCaption));
        }

        [Test]
        public void CaptionIncludesSolutionNameWhenSolutionNameIsPresent()
        {
            // Arrange

            const string solutionName = "SolutionName";

            Mock<ISolutionEventsService> solutionEventsServiceMock;
            var service = CreateViewModelService(out solutionEventsServiceMock);
            var window = new WorkingFilesWindow();

            // Act

            solutionEventsServiceMock.Raise(s =>
                s.SolutionNameChanged += null,
                null,
                new SolutionNameChangedEventArgs(solutionName));

            // Assert

            var expectedCaption = $"{solutionName} - {DefaultCaption}";
            Assert.That(window.Caption, Is.EqualTo(expectedCaption));
        }

        [Test]
        public void DefaultCaptionIsDisplayedWhenSolutionNameIsEmptyString()
        {
            // Arrange

            Mock<ISolutionEventsService> solutionEventsServiceMock;
            var service = CreateViewModelService(out solutionEventsServiceMock);
            var window = new WorkingFilesWindow();

            solutionEventsServiceMock.Raise(s =>
                s.SolutionNameChanged += null,
                new SolutionNameChangedEventArgs("SolutionName"));

            // Act

            solutionEventsServiceMock.Raise(s =>
                s.SolutionNameChanged += null,
                null,
                new SolutionNameChangedEventArgs(string.Empty));

            // Assert

            Assert.That(window.Caption, Is.EqualTo(DefaultCaption));
        }
    }
}
