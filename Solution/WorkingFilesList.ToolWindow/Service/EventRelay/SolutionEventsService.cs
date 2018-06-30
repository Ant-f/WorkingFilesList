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
using EnvDTE80;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service.EventRelay
{
    public class SolutionEventsService : ISolutionEventsService
    {
        private readonly DTE2 _dte2;
        private readonly IDocumentMetadataManager _documentMetadataManager;
        private readonly IPinnedItemStorageService _pinnedItemStorageService;
        private readonly IProjectBrushService _projectBrushService;
        private readonly IUserPreferences _userPreferences;

        public event EventHandler<SolutionNameChangedEventArgs> SolutionNameChanged;

        public SolutionEventsService(
            DTE2 dte2,
            IDocumentMetadataManager documentMetadataManager,
            IPinnedItemStorageService pinnedItemStorageService,
            IProjectBrushService projectBrushService,
            IUserPreferences userPreferences)
        {
            _dte2 = dte2;
            _documentMetadataManager = documentMetadataManager;
            _pinnedItemStorageService = pinnedItemStorageService;
            _projectBrushService = projectBrushService;
            _userPreferences = userPreferences;
        }

        public void AfterClosing()
        {
            _documentMetadataManager.Clear();
            _projectBrushService.ClearBrushIdCollection();
            RaiseSolutionNameChanged(string.Empty);
        }

        public void BeforeClosing()
        {
            if (string.IsNullOrWhiteSpace(_dte2.Solution.FullName))
            {
                return;
            }

            var data = _documentMetadataManager
                .PinnedDocumentMetadata
                .Cast<DocumentMetadata>()
                .ToArray();

            if (data.Length == 0)
            {
                return;
            }

            _pinnedItemStorageService.Write(data, _dte2.Solution.FullName);
        }

        public void Opened()
        {
            var name = Path.GetFileNameWithoutExtension(_dte2.Solution.FullName);
            RaiseSolutionNameChanged(name);

            if (string.IsNullOrWhiteSpace(_dte2.Solution.FullName))
            {
                return;
            }

            var metadata = _pinnedItemStorageService.Read(_dte2.Solution.FullName);

            if (metadata == null)
            {
                return;
            }

            foreach (var info in metadata)
            {
                _documentMetadataManager.AddPinned(info);
            }
        }

        public async Task ProjectAdded(Project project)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(project.FullName);
            var nodes = xmlDocument.GetElementsByTagName("ProjectTypeGuids");
            var guids = nodes[0]?.InnerText;

            var isUnityProject =
                guids != null &&
                guids.Contains("{E097FAD1-6243-4DAD-9C02-E9B9EFC3FFC1}");

            if (isUnityProject && _userPreferences.UnityRefreshDelay != 0)
            {
                await Task.Delay(_userPreferences.UnityRefreshDelay);
                _documentMetadataManager.Synchronize(project.DTE.Documents, true);
            }
        }

        public void ProjectRenamed(Project project, string oldName)
        {
            _projectBrushService.UpdateBrushId(oldName, project.FullName);

            // Synchronize after updating brush ID so the project continues to
            // use the same brush
            _documentMetadataManager.Synchronize(project.DTE.Documents, true);
        }

        private void RaiseSolutionNameChanged(string name)
        {
            SolutionNameChanged?.Invoke(
                this,
                new SolutionNameChangedEventArgs(name));
        }
    }
}
