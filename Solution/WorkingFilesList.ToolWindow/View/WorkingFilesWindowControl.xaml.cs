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

using System.Windows;

namespace WorkingFilesList.ToolWindow.View
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WorkingFilesWindowControl.
    /// </summary>
    public partial class WorkingFilesWindowControl : UserControl
    {
        private ScrollViewer _pinnedDocumentsScrollViewer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingFilesWindowControl"/> class.
        /// </summary>
        public WorkingFilesWindowControl()
        {
            this.InitializeComponent();
        }

        private void ActiveDocumentsScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Pinned and unpinned items are displayed in different ScrollViewer
            // instances. This is so that pinned items can always be in view even
            // when there are many unpinned items and vertical scrolling is
            // necessary.

            // The two ScrollViewer instances should scroll horizontally as one.
            // This event handler updates the horizontal scroll of the pinned items
            // ScrollViewer, to the offset of the unpinned items ScrollViewer.

            // ReSharper disable CompareOfFloatsByEqualityOperator
            // Any non-zero values should update scroll offset

            var updateHorizontalOffset =
                _pinnedDocumentsScrollViewer != null &&
                e.HorizontalChange != 0 &&
                _pinnedDocumentsScrollViewer.HorizontalOffset != e.HorizontalOffset;

            // ReSharper restore CompareOfFloatsByEqualityOperator

            if (updateHorizontalOffset)
            {
                _pinnedDocumentsScrollViewer.ScrollToHorizontalOffset(
                    e.HorizontalOffset);
            }
        }

        private void PinnedDocumentsScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            _pinnedDocumentsScrollViewer = (ScrollViewer) sender;
        }
    }
}
