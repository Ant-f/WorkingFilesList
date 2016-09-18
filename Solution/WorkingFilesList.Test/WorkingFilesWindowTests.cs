// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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
