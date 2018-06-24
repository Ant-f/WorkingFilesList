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

using System.IO;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class IOService : IIOService
    {
        public TextReader GetReader(string path)
        {
            var reader = new StreamReader(path);
            return reader;
        }

        public TextWriter GetWriter(string path)
        {
            var writer = new StreamWriter(path);
            return writer;
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
