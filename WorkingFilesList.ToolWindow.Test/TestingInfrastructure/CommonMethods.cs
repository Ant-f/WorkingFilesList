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
using Moq;
using System.Collections.Generic;
using WorkingFilesList.ToolWindow.Repository;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal static class CommonMethods
    {
        public static Documents CreateDocuments(IList<Document> documentsToReturn)
        {
            var documentsMock = new Mock<Documents>();

            documentsMock.Setup(d => d.GetEnumerator())
                .Returns(documentsToReturn.GetEnumerator());

            return documentsMock.Object;
        }

        public static EnvDTE.Windows CreateWindows(IList<Window> windowsToReturn)
        {
            var windowsMock = new Mock<EnvDTE.Windows>();

            windowsMock.Setup(d => d.GetEnumerator())
                .Returns(windowsToReturn.GetEnumerator());

            return windowsMock.Object;
        }

        /// <summary>
        /// Implementation-specific method of resetting data previously stored
        /// by a <see cref="StoredSettingsRepository"/>. For use in setup/tear
        /// down methods for tests that rely on a default state for stored data.
        /// </summary>
        public static void ResetStoredRepositoryData()
        {
            WorkingFilesList.ToolWindow.Properties.Settings.Default.Reset();
        }
    }
}
