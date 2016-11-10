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
using System.Windows;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.OptionsDialoguePage.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)] // Uses static properties in ViewModelService
    [Apartment(ApartmentState.STA)]
    public class OptionsPageTests
    {
        private class TestingOptionsPage : OptionsPage
        {
            public FrameworkElement ChildFrameworkElement => (FrameworkElement) Child;
        }

        [Test]
        public void OptionsPageIsInitialized()
        {
            // Arrange

            var optionsPageControl = Mock.Of<FrameworkElement>();
            var preferencesModel = Mock.Of<UserPreferencesModel>();

            var optionsPageControlFactory = Mock.Of<IOptionsPageControlFactory>(o =>
                o.CreateControl() == optionsPageControl);

            var factory = Mock.Of<IUserPreferencesModelFactory>(u =>
                u.CreateModel() == preferencesModel);

            var service = new ViewModelService(
                Mock.Of<IAboutPanelService>(),
                Mock.Of<ICommands>(),
                Mock.Of<IDocumentMetadataManager>(),
                Mock.Of<IOptionsLists>(),
                optionsPageControlFactory,
                Mock.Of<ISolutionEventsService>(),
                Mock.Of<IUserPreferences>(),
                factory);

            // Act

            var optionsPage = new TestingOptionsPage();

            // Assert

            Mock.Get(ViewModelService.OptionsPageControlFactory)
                .Verify(o => o.CreateControl());

            Mock.Get(ViewModelService.UserPreferencesModelFactory)
                .Verify(o => o.CreateModel());

            // Dialogue controls should be set
            Assert.That(optionsPage.ChildFrameworkElement,
                Is.EqualTo(optionsPageControl));

            // DataContext for dialogue controls should be UserPreferencesModel
            // instance
            Assert.That(optionsPage.ChildFrameworkElement.DataContext,
                Is.EqualTo(preferencesModel));
        }
    }
}
