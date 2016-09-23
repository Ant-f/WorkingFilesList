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
using WorkingFilesList.ToolWindow.View.Controls;
using WorkingFilesList.ToolWindow.View.Controls.Command;

namespace WorkingFilesList.ToolWindow.Test.View.Controls.Command
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DecrementNumericUpDownValueTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new DecrementNumericUpDownValue();

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void CommandDecrementsNumericUpDownValue()
        {
            // Arrange

            var upDown = new NumericUpDown
            {
                Minimum = 0,
                Value = 5
            };

            var command = new DecrementNumericUpDownValue();

            // Act

            command.Execute(upDown);

            // Assert

            Assert.That(upDown.Value, Is.EqualTo(4));
        }

        [Test]
        public void ExceptionIsNotThrownIfParameterIsNotNumericUpDown()
        {
            // Arrange

            var command = new DecrementNumericUpDownValue();

            // Assert

            Assert.DoesNotThrow(() => command.Execute(new object()));
        }

        [Test]
        public void NumericUpDownValueIsNotDecrementedBeyondMinimum()
        {
            // Arrange

            var controlMock = new Mock<IIntValueControl>();
            controlMock.Setup(c => c.Minimum).Returns(1);

            var command = new DecrementNumericUpDownValue();

            // Act

            command.Execute(controlMock.Object);

            // Assert

            controlMock.VerifySet(c => c.Value--, Times.Never);
        }
    }
}
