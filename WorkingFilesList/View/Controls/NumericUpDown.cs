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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkingFilesList.Interface;
using WorkingFilesList.View.Controls.Command;

namespace WorkingFilesList.View.Controls
{
    /// <summary>
    /// Control for displaying/editing int values. Style and template is located
    /// in XAML of <see cref="WorkingFilesWindowControl"/>, where this control
    /// is used; it doesn't seem possible to use extenal resource dictionary
    /// files with Visual Studio extension tool windows.
    /// </summary>
    public class NumericUpDown : Control, IIntValueControl
    {
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(int),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.None,
                    MinimumPropertyChangedCallback));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.None,
                    null,
                    CoerceValueCallback));

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public ICommand DecrementValue { get; }
            = new DecrementNumericUpDownValue();

        public ICommand IncrementValue { get; }
            = new IncrementNumericUpDownValue();

        public NumericUpDown()
        {
            DefaultStyleKey = typeof(NumericUpDown);
        }

        /// <summary>
        /// Adjusts the value of <see cref="Value"/> if necessary so that it is
        /// not less then <see cref="Minimum"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="DependencyObject"/> that this callback is associated with
        /// </param>
        /// <param name="args">Data related to the property change</param>
        private static void MinimumPropertyChangedCallback(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var upDown = (NumericUpDown) obj;
            var intMinimum = (int) args.NewValue;

            if (upDown.Value < intMinimum)
            {
                upDown.Value = intMinimum;
            }
        }

        /// <summary>
        /// Adjusts the value of <see cref="Value"/> if necessary so that it is
        /// not less then <see cref="Minimum"/>
        /// </summary>
        /// <param name="obj">
        /// The <see cref="DependencyObject"/> that this callback is associated with
        /// </param>
        /// <param name="value">The desired value for <see cref="Value"/></param>
        /// <returns>
        /// The higher of <see cref="Value"/> and <see cref="Minimum"/>
        /// </returns>
        private static object CoerceValueCallback(DependencyObject obj, object value)
        {
            var upDown = (NumericUpDown) obj;
            var intValue = (int)value;

            int returnValue;

            if (intValue < upDown.Minimum)
            {
                returnValue = upDown.Minimum;
            }
            else
            {
                returnValue = intValue;
            }

            return returnValue;
        }
    }
}
