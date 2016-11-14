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
using WorkingFilesList.OptionsDialoguePage.Test.TestingInfrastructure;

namespace WorkingFilesList.OptionsDialoguePage.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)] // Uses static properties in ViewModelService
    [Apartment(ApartmentState.STA)]
    public class OptionsPageTests
    {
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

            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                optionsPageControlFactory: optionsPageControlFactory,
                userPreferencesModelFactory: factory);

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

        [Test]
        public void SettingsAreRetrievedFromRepositoryWhenLoadingFromStorage()
        {
            // Arrange

            var preferencesModel = Mock.Of<UserPreferencesModel>();

            var factory = Mock.Of<IUserPreferencesModelFactory>(u =>
                u.CreateModel() == preferencesModel);

            var repository = Mock.Of<IUserPreferencesModelRepository>();
            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferencesModelFactory: factory,
                userPreferencesModelRepository: repository);

            var optionsPage = new OptionsPage();

            // Act

            optionsPage.LoadSettingsFromStorage();

            // Assert

            Mock.Get(repository).Verify(r => r.LoadInto(preferencesModel));
        }

        [Test]
        public void SettingsAreSetInRepositoryWhenSavingToStorage()
        {
            // Arrange

            var preferencesModel = Mock.Of<UserPreferencesModel>();

            var factory = Mock.Of<IUserPreferencesModelFactory>(u =>
                u.CreateModel() == preferencesModel);

            var repository = Mock.Of<IUserPreferencesModelRepository>();
            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferencesModelFactory: factory,
                userPreferencesModelRepository: repository);

            var optionsPage = new OptionsPage();

            // Act

            optionsPage.SaveSettingsToStorage();

            // Assert

            Mock.Get(repository).Verify(r => r.SaveModel(preferencesModel));
        }

        [Test]
        public void SettingsAreNotLoadedIfDialogueIsActivatedWithoutClosing()
        {
            // Arrange

            var repository = Mock.Of<IUserPreferencesModelRepository>();
            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferencesModelRepository: repository);

            var optionsPage = new TestingOptionsPage();

            // Act

            optionsPage.SimulateActivate();

            // Assert

            Mock.Get(repository).Verify(r =>
                r.LoadInto(It.IsAny<IUserPreferences>()),
                Times.Never);
        }

        [Test]
        public void SettingsAreLoadedIfDialogueIsActivatedAfterClosing()
        {
            // Arrange

            var repository = Mock.Of<IUserPreferencesModelRepository>();
            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferencesModelRepository: repository);

            var optionsPage = new TestingOptionsPage();
            optionsPage.SimulateClose();

            // Act

            optionsPage.SimulateActivate();

            // Assert

            Mock.Get(repository).Verify(r =>
                r.LoadInto(It.IsAny<IUserPreferences>()));
        }

        [Test]
        public void SettingsAreLoadedIntoGlobalUserPreferencesWhenSavingToStorage()
        {
            // Arrange

            var repository = Mock.Of<IUserPreferencesModelRepository>();
            var globalPreferences = Mock.Of<IUserPreferences>();
            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferences: globalPreferences,
                userPreferencesModelRepository: repository);

            var optionsPage = new OptionsPage();

            // Act

            optionsPage.SaveSettingsToStorage();

            // Assert

            Mock.Get(repository).Verify(r => r.LoadInto(globalPreferences));
        }

        [Test]
        public void AutomationObjectIsSet()
        {
            // Arrange

            var preferencesModel = Mock.Of<UserPreferencesModel>();

            var factory = Mock.Of<IUserPreferencesModelFactory>(u =>
                u.CreateModel() == preferencesModel);

            var initializer = new ViewModelServiceInitializer();

            var service = initializer.InitializeViewModelService(
                userPreferencesModelFactory: factory);

            // Act

            var optionsPage = new OptionsPage();

            // Assert

            Mock.Get(ViewModelService.UserPreferencesModelFactory)
                .Verify(o => o.CreateModel());

            Assert.That(optionsPage.AutomationObject, Is.EqualTo(preferencesModel));
        }
    }
}
