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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.View.Controls.Command;

namespace WorkingFilesList.ToolWindow.View.Controls
{
    /// <summary>
    /// Control for displaying/editing int values. Style and template is located
    /// in /View/Theme/NumericUpDown.xaml
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
