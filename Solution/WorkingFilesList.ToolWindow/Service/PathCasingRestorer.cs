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

using EnvDTE;
using System.IO;
using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    /// <summary>
    /// There are certain conditions where <see cref="Document"/> provides
    /// <see cref="Document.FullName"/> in lower case. This class contains
    /// logic to return the path to a file with the same casing as written on
    /// the file system
    /// </summary>
    public class PathCasingRestorer : IPathCasingRestorer
    {
        /// <summary>
        /// Corrects the casing of file and directory names
        /// </summary>
        /// <param name="fullName">Full path and name of a file</param>
        /// <returns>
        /// Full name of the file referenced by <see cref="fullName"/> with
        /// the same casing as written on the file system
        /// </returns>
        public string RestoreCasing(string fullName)
        {
            // The name of a file/directory will have the correct casing in a
            // directory's contents listing. Find the parent directory of each
            // file/directory in the path and within that parent directory,
            // search for the file/directory

            var fileInfo = new FileInfo(fullName);
            var parentDirInfo = fileInfo.Directory;
            var fileName = parentDirInfo.GetFiles(fileInfo.Name)[0].Name;

            var returnValue = fileName;

            while (parentDirInfo != null)
            {
                var grandParentDirInfo = parentDirInfo.Parent;

                if (grandParentDirInfo == null)
                {
                    returnValue = Path.Combine(
                        parentDirInfo.Name.ToUpperInvariant(),
                        returnValue);
                }
                else
                {
                    var parentDirName = grandParentDirInfo
                        .GetDirectories(parentDirInfo.Name)[0]
                        .Name;

                    returnValue = Path.Combine(parentDirName, returnValue);
                }

                parentDirInfo = grandParentDirInfo;

            }

            return returnValue;
        }
    }
}
