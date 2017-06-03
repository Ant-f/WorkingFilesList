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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;

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

        private static Point? _mouseDownPoint;
        private static DragDropButton _mouseDownOrigin;
        private static bool _dragDropStaged = false;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (AllowDragAndDrop &&
                e.LeftButton == MouseButtonState.Released)
            {
                _mouseDownPoint = null;
                _dragDropStaged = false;
            }
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
            SetDragDropMoveEffectAndHandled(e);
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            base.OnPreviewDragLeave(e);
            SetDragDropMoveEffectAndHandled(e);
        }

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            base.OnPreviewDragOver(e);
            SetDragDropMoveEffectAndHandled(e);
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

            if (AllowDragAndDrop &&
                e.ChangedButton == MouseButton.Left)
            {
                _mouseDownOrigin = this;
                _mouseDownPoint = e.GetPosition(_mouseDownOrigin);
                _dragDropStaged = true;
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            var canDragDrop =
                AllowDragAndDrop &&
                _dragDropStaged &&
                e.LeftButton == MouseButtonState.Pressed &&
                _mouseDownPoint.HasValue;

            if (!canDragDrop)
            {
                return;
            }

            // Begin drag-and-drop if dragging over another object
            var beginDragDrop = this != _mouseDownOrigin;

            // ... or after dragging a certain distance
            if (!beginDragDrop)
            {
                const int distanceToBeginDragDrop = 5;
                
                var delta = _mouseDownPoint.Value - e.GetPosition(_mouseDownOrigin);
                var absoluteLength = Math.Abs(delta.Length);
                beginDragDrop = absoluteLength > distanceToBeginDragDrop;
            }

            if (beginDragDrop)
            {
                var metadata = (DocumentMetadata) _mouseDownOrigin.DataContext;

                if (metadata.IsPinned)
                {
                    _dragDropStaged = false;

                    DragDrop.DoDragDrop(
                        _mouseDownOrigin,
                        metadata,
                        DragDropEffects.Move);

                    e.Handled = true;
                }
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            _mouseDownPoint = null;
            _dragDropStaged = false;

            if (AllowDragAndDrop &&
                e.ChangedButton == MouseButton.Left &&
                this == _mouseDownOrigin)
            {
                var mouseBinding = InputBindings
                    .Cast<MouseBinding>()
                    .Single(b => b.MouseAction == MouseAction.LeftClick);

                mouseBinding.Command.Execute(
                    mouseBinding.CommandParameter);
            }
        }

        private void SetDragDropMoveEffectAndHandled(DragEventArgs e)
        {
            if (AllowDragAndDrop)
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}
