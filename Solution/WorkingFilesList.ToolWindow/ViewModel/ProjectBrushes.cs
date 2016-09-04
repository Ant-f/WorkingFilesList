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

using System.Windows.Media;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel
{
    public class ProjectBrushes : IProjectBrushes
    {
        /// <summary>
        /// <see cref="Brush"/> to return when not using project specific colours
        /// </summary>
        public Brush GenericBrush { get; } = Brushes.DarkGray;

        /// <summary>
        /// Available <see cref="Brush"/> instances for assigning to projects
        /// </summary>
        public Brush[] ProjectSpecificBrushes { get; } =
            {
                Brushes.CornflowerBlue,
                Brushes.GreenYellow,
                Brushes.DarkOrange,
                Brushes.Pink,
                Brushes.BlanchedAlmond,
                Brushes.MediumSlateBlue,
                Brushes.DarkSeaGreen,
                Brushes.Goldenrod,
                Brushes.Sienna,
                Brushes.Salmon
            };
    }
}
