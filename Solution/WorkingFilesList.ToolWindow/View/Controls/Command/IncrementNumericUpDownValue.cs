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

using System;
using System.Windows.Input;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.View.Controls.Command
{
    /// <summary>
    /// <see cref="ICommand"/> that increments the <see cref="IIntValueControl.Value"/>
    /// property of the <see cref="IIntValueControl"/> passed in as a parameter. This
    /// class should be bound to the appropriate button in a
    /// <see cref="NumericUpDown"/> control template
    /// </summary>
    public class IncrementNumericUpDownValue : ICommand
    {
        #pragma warning disable 67
        // ICommand interface member is only used by XAML
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 67

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var control = parameter as IIntValueControl;

            if (control != null)
            {
                control.Value++;
            }
        }
    }
}
