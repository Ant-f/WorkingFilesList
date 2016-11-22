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

using System.IO;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class FilePathService : IFilePathService
    {
        private readonly char[] _pathSeparators =
        {
            Path.AltDirectorySeparatorChar,
            Path.DirectorySeparatorChar
        };

        /// <summary>
        /// Returns a version of <see cref="fullName"/> containing the specified
        /// number of path segments, i.e. file/directory names. The returned
        /// path always includes the file name; the number of its parent
        /// directories depends on <see cref="pathSegmentCount"/>
        /// </summary>
        /// <param name="fullName">Full path and name of a file</param>
        /// <param name="pathSegmentCount">
        /// The combined number of file/directory names to include
        /// </param>
        /// <returns>
        /// A substring of <see cref="fullName"/> if <see cref="pathSegmentCount"/>
        /// is less than the number of file/directory names in <see cref="fullName"/>;
        /// <see cref="fullName"/> otherwise
        /// </returns>
        public string ReducePath(string fullName, int pathSegmentCount)
        {
            var maxIndex = fullName.Length - 1;

            // Start search at the last position of the string
            var startIndex = maxIndex;

            for (int i = 0;
                i < pathSegmentCount && startIndex > 0;
                i++)
            {
                var index = fullName.LastIndexOfAny(
                    _pathSeparators,
                    startIndex);

                // Decrement index: subsequent calls to LastIndexOfAny will
                // return the same index otherwise

                startIndex = index - 1;
            }

            int substringStartIndex;
            if (startIndex < 0 || startIndex == maxIndex)
            {
                substringStartIndex = 0;
            }
            else
            {
                // startIndex points to a character before a directory separator.
                // Increment startIndex by two:
                // - one to point at a directory separator, which we don't want
                // - one to point at the first character after the directory separator

                substringStartIndex = startIndex + 2;
            }

            var displayName = fullName.Substring(substringStartIndex);
            return displayName;
        }
    }
}
