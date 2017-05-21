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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.View.Controls
{
    public class DragDropButton : Button
    {
        public static readonly DependencyProperty AllowDragAndDropProperty =
            DependencyProperty.Register(
                "AllowDragAndDrop",
                typeof(bool),
                typeof(DragDropButton), new PropertyMetadata(
                    false, (obj, args) =>
                    {
                        ((DragDropButton) obj).AllowDrop = (bool) args.NewValue;
                    }));

        /// <summary>
        /// Specifies whether drag-and-drop operations are allowed
        /// </summary>
        public bool AllowDragAndDrop
        {
            get => (bool)GetValue(AllowDragAndDropProperty);
            set => SetValue(AllowDragAndDropProperty, value);
        }

        public static readonly DependencyProperty PinnedMetadataManagerProperty =
            DependencyProperty.Register(
                "PinnedMetadataManager",
                typeof(IPinnedMetadataManager),
                typeof(DragDropButton));

        public IPinnedMetadataManager PinnedMetadataManager
        {
            get => (IPinnedMetadataManager)GetValue(PinnedMetadataManagerProperty);
            set => SetValue(PinnedMetadataManagerProperty, value);
        }

        /// <summary>
        /// Used to ensure that a drag operation is not triggered as part of an
        /// unrelated drag-and-drop operation that moves across this button
        /// </summary>
        private bool _isDragSource = false;

        private void SetDragDropMoveEffect(DragEventArgs e)
        {
            if (AllowDragAndDrop)
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
            SetDragDropMoveEffect(e);
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
            SetDragDropMoveEffect(e);
        }

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            base.OnPreviewDragOver(e);
            SetDragDropMoveEffect(e);
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
            var dragSource = e.Data.GetData(typeof(DocumentMetadata));

            PinnedMetadataManager?.MovePinnedItem(
                (DocumentMetadata) dragSource,
                (DocumentMetadata) DataContext);

            e.Handled = true;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (AllowDragAndDrop)
            {
                _isDragSource = true;
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (AllowDragAndDrop &&
                _isDragSource &&
                e.LeftButton == MouseButtonState.Pressed)
            {
                var metadata = (DocumentMetadata) DataContext;

                if (metadata.IsPinned)
                {
                    DragDrop.DoDragDrop(this, metadata, DragDropEffects.Move);
                    e.Handled = true;
                }
            }

            _isDragSource = false;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            _isDragSource = false;
        }
    }
}
