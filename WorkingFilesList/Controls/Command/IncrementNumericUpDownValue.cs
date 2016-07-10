﻿// WorkingFilesList
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

using System;
using System.Windows.Input;

namespace WorkingFilesList.Controls.Command
{
    /// <summary>
    /// <see cref="ICommand"/> that increments the <see cref="NumericUpDown.Value"/>
    /// property of the <see cref="NumericUpDown"/> passed in as a parameter. This
    /// class should be bound to the appropriate button in a
    /// <see cref="NumericUpDown"/> control template
    /// </summary>
    public class IncrementNumericUpDownValue : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var upDown = parameter as NumericUpDown;

            if (upDown != null)
            {
                upDown.Value++;
            }
        }
    }
}
