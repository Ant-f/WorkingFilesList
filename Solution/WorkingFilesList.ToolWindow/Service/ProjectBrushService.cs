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

using System.Collections.Generic;
using System.Windows.Media;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Service
{
    public class ProjectBrushService : IProjectBrushService
    {
        private readonly IProjectBrushes _projectBrushes;
        private readonly IDictionary<string, Brush> _brushAssignment;

        /// <summary>
        /// Contains methods to return <see cref="Brush"/> instances
        /// </summary>
        /// <param name="projectBrushes">
        /// The range of <see cref="Brush"/> instances that can be returned
        /// </param>
        public ProjectBrushService(IProjectBrushes projectBrushes)
        {
            _projectBrushes = projectBrushes;
            _brushAssignment = new Dictionary<string, Brush>();
        }

        /// <summary>
        /// Returns a <see cref="Brush"/> instance to use as a background for
        /// each entry on the <see cref="DocumentMetadata"/> list
        /// </summary>
        /// <param name="id">
        /// A value to be associated with a <see cref="Brush"/>. When
        /// <see cref="IUserPreferences.AssignProjectColours"/> is true,
        /// passing the same id on subsequent invocations of this method will
        /// return the same <see cref="Brush"/>
        /// </param>
        /// <param name="userPreferences">
        /// Determines whether the returned <see cref="Brush"/> is
        /// project-specific, generic, or transparent
        /// </param>
        /// <returns>
        /// A <see cref="Brush"/> appropriate to both <see cref="id"/> and the
        /// property values of <see cref="userPreferences"/>
        /// </returns>
        public Brush GetBrush(string id, IUserPreferences userPreferences)
        {
            Brush returnBrush;

            if (userPreferences.AssignProjectColours)
            {
                var entryExists = _brushAssignment.ContainsKey(id);

                if (!entryExists)
                {
                    var index =
                        _brushAssignment.Count%
                        _projectBrushes.ProjectSpecificBrushes.Length;

                    _brushAssignment[id] = _projectBrushes
                        .ProjectSpecificBrushes[index];
                }

                returnBrush = _brushAssignment[id];
            }
            else
            {
                returnBrush = userPreferences.ShowRecentUsage
                    ? _projectBrushes.GenericBrush
                    : Brushes.Transparent;
            }

            return returnBrush;
        }
    }
}
