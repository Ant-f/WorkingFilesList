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

using NUnit.Framework;
using System.Threading;
using WorkingFilesList.OptionsDialoguePage.Factory;
using WorkingFilesList.OptionsDialoguePage.View;

namespace WorkingFilesList.OptionsDialoguePage.Test.Factory
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsPageControlFactoryTests
    {
        [Test]
        public void CreateControlReturnsOptionsPageControl()
        {
            // Arrange

            var factory = new OptionsPageControlFactory();

            // Act

            var preferences = factory.CreateControl();

            // Assert

            Assert.That(preferences, Is.TypeOf<OptionsPageControl>());
        }
    }
}
