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
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class DisplayNameHighlightEvaluator : IDisplayNameHighlightEvaluator
    {
        public string GetPreHighlight(string path, bool highlightFileName)
        {
            if (!highlightFileName)
            {
                return string.Empty;
            }

            var directory = Path.GetDirectoryName(path);

            var preHighlight = string.IsNullOrWhiteSpace(directory)
                ? string.Empty
                : directory + Path.DirectorySeparatorChar;

            return preHighlight;
        }

        public string GetHighlight(string path, bool highlightFileName)
        {
            if (!highlightFileName)
            {
                return path;
            }

            var highlight = Path.GetFileNameWithoutExtension(path);
            return highlight;
        }

        public string GetPostHighlight(string path, bool highlightFileName)
        {
            if (!highlightFileName)
            {
                return string.Empty;
            }

            var postHighlight = Path.GetExtension(path);
            return postHighlight;
        }
    }
}
