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

using EnvDTE;
using System.IO;
using WorkingFilesList.ToolWindow.Interface;

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
