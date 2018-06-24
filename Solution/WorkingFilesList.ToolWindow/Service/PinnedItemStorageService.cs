// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2018 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class PinnedItemStorageService : IPinnedItemStorageService
    {
        private readonly IIOService _ioService;

        private readonly string _outputDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "WorkingFilesList");

        public PinnedItemStorageService(IIOService ioService)
        {
            _ioService = ioService;
        }

        public void Write(IEnumerable<DocumentMetadata> metadata, string fullName)
        {
            var invalid = string.IsNullOrWhiteSpace(fullName);

            if (invalid)
            {
                throw new ArgumentException($"'{fullName}' is not valid");
            }

            _ioService.CreateDirectory(_outputDirectory);

            var path = GetDerivedPath(fullName);
            var info = metadata.Select(m => new DocumentMetadataInfo
            {
                FullName = m.FullName,
                ProjectDisplayName = m.ProjectNames.DisplayName,
                ProjectFullName = m.ProjectNames.FullName
            });
            
            var json = JsonConvert.SerializeObject(info);
            using (var writer = _ioService.GetWriter(path))
            {
                writer.WriteLine(json);
            }
        }

        private string GetDerivedPath(string fullName)
        {
            const string version = "1.3";

            var bytes = Encoding.UTF8.GetBytes(fullName);
            var sb = new StringBuilder();

            using (var sha1 = new SHA1Managed())
            {
                var hashBytes = sha1.ComputeHash(bytes);

                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
            }

            var hashString = sb.ToString();

            var path = Path.Combine(
                _outputDirectory,
                $"{version}_{hashString}.json");

            return path;
        }
    }
}
