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
