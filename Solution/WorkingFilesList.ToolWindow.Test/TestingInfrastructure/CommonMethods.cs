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
using Moq;
using System.Collections.Generic;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Repository;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal static class CommonMethods
    {
        public static Document CreateDocumentWithInfo(
            DocumentMetadataInfo info,
            bool nullActiveWindow = false)
        {
            var activeWindow = nullActiveWindow ? null : Mock.Of<Window>();

            var documentMock = Mock.Of<Document>(d =>
                d.ActiveWindow == activeWindow &&
                d.FullName == info.FullName &&
                d.ProjectItem.ContainingProject.Name == info.ProjectDisplayName &&
                d.ProjectItem.ContainingProject.FullName == info.ProjectFullName);

            return documentMock;
        }

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
