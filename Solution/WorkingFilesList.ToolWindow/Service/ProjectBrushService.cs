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

using System.Collections.Generic;
using System.Windows.Media;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class ProjectBrushService : IProjectBrushService
    {
        private readonly IProjectBrushes _projectBrushes;

        /// <summary>
        /// Used to associate project names with <see cref="Brush"/> instances.
        /// Entries should not be removed, except by calling
        /// <see cref="ClearBrushIdCollection"/>. This is so that if a project
        /// is either removed or renamed, the same <see cref="Brush"/> can be
        /// used again if it is re-added or renamed to its original name
        /// </summary>
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

        /// <summary>
        /// Reset the association between id values and returned
        /// <see cref="Brush"/> instances when calling <see cref="GetBrush"/>
        /// </summary>
        public void ClearBrushIdCollection()
        {
            _brushAssignment.Clear();
        }

        /// <summary>
        /// Associates the <see cref="Brush"/> instance already associated with
        /// <paramref name="oldId"/> with <paramref name="newId"/>. Does
        /// nothing if <paramref name="newId"/> was previously used as an argument
        /// with <see cref="GetBrush"/>
        /// </summary>
        /// <param name="oldId">
        /// An id that <see cref="GetBrush"/> has previously been called with
        /// </param>
        /// <param name="newId">
        /// An id to associate the same <see cref="Brush"/> instance as that for
        /// <paramref name="oldId"/> when calling <see cref="GetBrush"/>
        /// </param>
        public void UpdateBrushId(string oldId, string newId)
        {
            var newIdIsValid =
                !string.IsNullOrWhiteSpace(newId) &&
                !_brushAssignment.ContainsKey(newId);

            if (!newIdIsValid)
            {
                return;
            }

            var oldIdIsValid = _brushAssignment.ContainsKey(oldId);
            if (!oldIdIsValid)
            {
                return;
            }

            var existingBrush = _brushAssignment[oldId];
            _brushAssignment.Add(newId, existingBrush);
        }
    }
}
