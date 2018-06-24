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
using System.IO;
using System.Text;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class TestingTextWriter : TextWriter
    {
        public bool DisposeInvoked { get; private set; }
        public string WrittenData { get; private set; }

        public override Encoding Encoding { get; }

        protected override void Dispose(bool disposing)
        {
            DisposeInvoked = true;
            base.Dispose(disposing);
        }

        public override void WriteLine(string value)
        {
            WrittenData += value;
            WrittenData += Environment.NewLine;
        }
    }
}
