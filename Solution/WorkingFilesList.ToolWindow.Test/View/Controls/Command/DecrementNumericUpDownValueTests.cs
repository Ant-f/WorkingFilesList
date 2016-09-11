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
