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
using EnvDTE80;
using Moq;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.Test
{
    [TestFixture]
    public class WorkingFilesWindowPackageTests
    {
        [Test]
        public void InitializeServicesSubscribesToDte2Events()
        {
            // Arrange

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var events2 = Mock.Of<Events2>();

            var dte2 = Mock.Of<DTE2>(d => d.Events == events2);
            kernel.Bind<DTE2>().ToConstant(dte2);

            var subscriberMock = new Mock<IDteEventsSubscriber>();
            kernel.Bind<IDteEventsSubscriber>().ToConstant(subscriberMock.Object);

            // Act

            package.InitializeServices(kernel);

            // Assert

            subscriberMock.Verify(s => s.SubscribeTo(events2));
        }

        /// <summary>
        /// Properties in <see cref="ViewModelService"/> are injected by
        /// resolving an instance, and can be used in subsequent instances
        /// created by XAML
        /// </summary>
        [Test]
        public void InitializeServicesResolvesViewModelServiceInstance()
        {
            // Arrange

            var serviceResolved = false;

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var events2 = Mock.Of<Events2>();

            var dte2 = Mock.Of<DTE2>(d => d.Events == events2);
            kernel.Bind<DTE2>().ToConstant(dte2);

            kernel.Bind<ViewModelService>().ToMethod(context =>
            {
                serviceResolved = true;
                return new ViewModelService();
            });

            // Act

            package.InitializeServices(kernel);

            // Assert

            Assert.IsTrue(serviceResolved);
        }

        [Test]
        public void InitializeServicesSynchronizesDocuments()
        {
            // Arrange

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var documents = Mock.Of<Documents>();

            var dte2 = Mock.Of<DTE2>(d =>
                d.Events == Mock.Of<Events2>() &&
                d.Documents == documents);

            kernel.Bind<DTE2>().ToConstant(dte2);

            var subscriberMock = new Mock<IDteEventsSubscriber>();
            kernel.Bind<IDteEventsSubscriber>().ToConstant(subscriberMock.Object);

            var managerMock = new Mock<IDocumentMetadataManager>();
            kernel.Bind<IDocumentMetadataManager>().ToConstant(managerMock.Object);

            // Act

            package.InitializeServices(kernel);

            // Assert

            managerMock.Verify(m => m.Synchronize(documents, true));
        }
    }
}
