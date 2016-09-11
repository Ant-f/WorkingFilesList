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
using System.Globalization;
using System.Windows.Data;

namespace WorkingFilesList.ToolWindow.View.Converter
{
    public class MappedValuesConverter : IValueConverter
    {
        /// <summary>
        /// Converts objects according to a provided value mapping
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="targetType">This parameter is not used</param>
        /// <param name="parameter">
        /// This should be an instance of <see cref="ConverterMappingCollection"/>,
        /// specifying appropriate return values
        /// </param>
        /// <param name="culture">This parameter is not used</param>
        /// <returns>
        /// The appropriate value as specified by <see cref="parameter"/>, or
        /// <see cref="value"/> if a corresponding value cannot be found
        /// </returns>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var returnValue = ConvertValue(value, parameter);
            return returnValue;
        }

        /// <summary>
        /// Converts objects according to a provided value mapping
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="targetType">This parameter is not used</param>
        /// <param name="parameter">
        /// This should be an instance of <see cref="ConverterMappingCollection"/>,
        /// specifying appropriate return values
        /// </param>
        /// <param name="culture">This parameter is not used</param>
        /// <returns>
        /// The appropriate value as specified by <see cref="parameter"/>, or
        /// <see cref="value"/> if a corresponding value cannot be found
        /// </returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var returnValue = ConvertValue(value, parameter);
            return returnValue;
        }

        private static object ConvertValue(object value, object parameter)
        {
            var collection = parameter as ConverterMappingCollection;
            if (collection == null)
            {
                return value;
            }

            var exists = collection.MappedValuesDictionary.ContainsKey(value);
            if (!exists)
            {
                return value;
            }

            var returnValue = collection.MappedValuesDictionary[value];
            return returnValue;
        }
    }
}
