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

using System.IO;
using System.Reflection;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class AboutPanelService : IAboutPanelService
    {
        public string AboutText { get; }

        public AboutPanelService()
        {
            const string aboutTextResourceName =
                "WorkingFilesList.ToolWindow.Service.AboutText.txt";

            AboutText = GetLicenseText(aboutTextResourceName);
        }

        /// <summary>
        /// Retrieve text from an embedded resource
        /// </summary>
        /// <param name="resourceName">Name of the resource to read</param>
        /// <returns>The entire contents of the named resource</returns>
        private static string GetLicenseText(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string licenseText;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    licenseText = streamReader.ReadToEnd();
                }
            }

            return licenseText;
        }
    }
}
