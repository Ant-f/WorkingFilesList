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
using WorkingFilesList.Core.Controls;

namespace WorkingFilesList.Core.Test.Controls
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class NumericUpDownTests
    {
        [Test]
        public void ValueCanNotBeLessThanMinumum()
        {
            // Arrange

            const int minimum = 0;

            var upDown = new NumericUpDown
            {
                Minimum = minimum
            };

            // Act

            upDown.Value = -5;

            // Assert

            Assert.That(upDown.Value, Is.EqualTo(minimum));
        }

        [Test]
        public void SettingMinimumHigherThanValueUpdatesValue()
        {
            // Arrange

            const int minimum = 5;

            // Act

            var upDown = new NumericUpDown
            {
                Minimum = 5
            };

            // Assert

            Assert.That(upDown.Value, Is.EqualTo(minimum));
        }
    }
}
