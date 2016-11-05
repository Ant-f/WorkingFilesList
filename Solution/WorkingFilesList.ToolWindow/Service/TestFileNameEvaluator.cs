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
using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class TestFileNameEvaluator : ITestFileNameEvaluator
    {
        /// <summary>
        /// Evaluates the most likely name for a file containing tests, based
        /// on the provided file name
        /// </summary>
        /// <param name="fileName">
        /// Name of file containing logic to have tests written for
        /// </param>
        /// <returns>
        /// <paramref name="fileName"/> with the suffix 'Tests' added before
        /// the file extension
        /// </returns>
        public string EvaluateTestFileName(string fileName)
        {
            var inputFileName = Path.GetFileNameWithoutExtension(fileName);
            var testFileName = inputFileName + "Tests";

            var extension = Path.GetExtension(fileName);

            var combined = $"{testFileName}{extension}";
            return combined;
        }
    }
}
