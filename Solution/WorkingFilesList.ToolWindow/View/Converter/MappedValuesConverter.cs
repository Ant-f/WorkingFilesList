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
