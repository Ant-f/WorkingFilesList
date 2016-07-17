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

using EnvDTE;
using EnvDTE80;
using Moq;
using NUnit.Framework;
using WorkingFilesList.Test.TestingInfrastructure;

namespace WorkingFilesList.Test.Service.EventRelay
{
    [TestFixture]
    public class DteEventsSubscriberTests
    {
        private class TestingMocks
        {
            public Mock<Events2> Events2Mock { get; set; }
            public Mock<ProjectItemsEvents> ProjectItemsEventsMock { get; set; }
            public Mock<WindowEvents> WindowEventsMock { get; set; }
        }

        private static Mock<WindowEvents> CreateWindowEventsMock()
        {
            var windowEventsMock = new Mock<WindowEvents>();

            // Exception will be thrown when subscribing to events if they are
            // not raised at least once; do so here as part of setup

            windowEventsMock.Raise(w =>
                w.WindowActivated += null,
                Mock.Of<Window>(),
                Mock.Of<Window>());

            windowEventsMock.Raise(w =>
                w.WindowClosing += null,
                Mock.Of<Window>());

            windowEventsMock.Raise(w =>
                w.WindowCreated += null,
                Mock.Of<Window>());

            return windowEventsMock;
        }

        private static Mock<ProjectItemsEvents> CreateProjectItemsEventsMock()
        {
            var projectItemsEventsMock = new Mock<ProjectItemsEvents>();

            // Exception will be thrown when subscribing to events if they are
            // not raised at least once; do so here as part of setup

            projectItemsEventsMock.Raise(w =>
                w.ItemRenamed += null,
                Mock.Of<ProjectItem>(),
                string.Empty);

            return projectItemsEventsMock;
        }

        private static TestingMocks CreateTestingMocks()
        {
            var mocks = new TestingMocks
            {
                Events2Mock = new Mock<Events2>(),
                ProjectItemsEventsMock = CreateProjectItemsEventsMock(),
                WindowEventsMock = CreateWindowEventsMock()
            };

            mocks.Events2Mock
                .Setup(e => e.ProjectItemsEvents)
                .Returns(mocks.ProjectItemsEventsMock.Object);

            mocks.Events2Mock
                .Setup(e => e.get_WindowEvents(It.IsAny<Window>()))
                .Returns(mocks.WindowEventsMock.Object);

            return mocks;
        }

        [Test]
        public void SubscribesToWindowActivated()
        {
            // Arrange

            var mocks = CreateTestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowActivated += null,
                null, null);

            // Assert

            builder.WindowEventsServiceMock.Verify(w => w
                .WindowActivated(
                    It.IsAny<Window>(),
                    It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToWindowCreated()
        {
            // Arrange

            var mocks = CreateTestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowCreated += null,
                Mock.Of<Window>());

            // Assert

            builder.WindowEventsServiceMock.Verify(f => f
                .WindowCreated(It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToWindowClosing()
        {
            // Arrange

            var mocks = CreateTestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowClosing += null,
                Mock.Of<Window>());

            // Assert

            builder.WindowEventsServiceMock.Verify(f => f
                .WindowClosing(It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToItemRenamed()
        {
            // Arrange

            var mocks = CreateTestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.ProjectItemsEventsMock.Raise(w =>
                w.ItemRenamed += null,
                Mock.Of<ProjectItem>(), string.Empty);

            // Assert

            builder.ProjectItemsEventsServiceMock.Verify(f => f
                .ItemRenamed(
                    It.IsAny<ProjectItem>(),
                    It.IsAny<string>()));
        }
    }
}
