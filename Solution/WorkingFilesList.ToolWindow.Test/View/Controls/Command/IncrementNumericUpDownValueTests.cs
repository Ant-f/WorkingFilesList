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

using NUnit.Framework;
using System.Threading;
using WorkingFilesList.ToolWindow.View.Controls;
using WorkingFilesList.ToolWindow.View.Controls.Command;

namespace WorkingFilesList.ToolWindow.Test.View.Controls.Command
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class IncrementNumericUpDownValueTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new IncrementNumericUpDownValue();

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void CommandIncrementsNumericUpDownValue()
        {
            // Arrange

            var upDown = new NumericUpDown
            {
                Value = 0
            };

            var command = new IncrementNumericUpDownValue();

            // Act

            command.Execute(upDown);

            // Assert

            Assert.That(upDown.Value, Is.EqualTo(1));
        }

        [Test]
        public void ExceptionIsNotThrownIfParameterIsNotNumericUpDown()
        {
            // Arrange

            var command = new IncrementNumericUpDownValue();

            // Assert

            Assert.DoesNotThrow(() => command.Execute(new object()));
        }
    }
}
