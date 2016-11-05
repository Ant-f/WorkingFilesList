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

using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class DocumentIconService : IDocumentIconService
    {
        private readonly Dictionary<string, BitmapImage> _documentIcons;
        private readonly BitmapImage _defaultDocumentIcon;

        public DocumentIconService()
        {
            _defaultDocumentIcon = CreateBitmapImage("Document_16x.png");

            _documentIcons = new Dictionary<string, BitmapImage>(
                StringComparer.OrdinalIgnoreCase)
            {
                [".config"] = CreateBitmapImage("ConfigurationFile_16x.png"),
                [".cpp"] = CreateBitmapImage("CPP_16x.png"),
                [".cs"] = CreateBitmapImage("CS_16x.png"),
                [".fs"] = CreateBitmapImage("FS_16x.png"),
                [".js"] = CreateBitmapImage("JS_16x.png"),
                [".txt"] = CreateBitmapImage("TextFile_16x.png"),
                [".ts"] = CreateBitmapImage("TS_16x.png"),
                [".vb"] = CreateBitmapImage("VB_16x.png"),
                [".xaml"] = CreateBitmapImage("WPFPage_16x.png")
            };
        }

        public BitmapSource GetIcon(string fileExtension)
        {
            var exists = _documentIcons.ContainsKey(fileExtension);

            var icon = exists
                ? _documentIcons[fileExtension]
                : _defaultDocumentIcon;

            return icon;
        }

        private static BitmapImage CreateBitmapImage(string fileName)
        {
            var uriString =
                $"pack://application:,,,/WorkingFilesList.ToolWindow;Component/Service/DocumentIcon/{fileName}";

            var uri = new Uri(uriString);
            var bitmap = new BitmapImage(uri);
            return bitmap;
        }
    }
}
