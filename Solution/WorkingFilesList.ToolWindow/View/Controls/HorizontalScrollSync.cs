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
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.View.Controls
{
    /// <summary>
    /// Pinned and unpinned items are displayed in different ScrollViewer
    /// instances; this is so that pinned items can always be in view, even
    /// when there are many unpinned items and vertical scrolling is necessary.
    /// This class allows the two ScrollViewer instances to scroll horizontally
    /// as one
    /// </summary>
    public class HorizontalScrollSync : DependencyObject
    {
        private static ScrollViewer _primaryScrollViewer;
        private static ScrollViewer _replicaScrollViewer;

        public static readonly DependencyProperty RoleProperty =
            DependencyProperty.RegisterAttached(
                "Role",
                typeof(HorizontalScrollSyncRole),
                typeof(HorizontalScrollSync),
                new PropertyMetadata(RolePropertyChanged));

        public static void SetRole(DependencyObject obj, HorizontalScrollSyncRole role)
        {
            obj.SetValue(RoleProperty, role);
        }

        public static HorizontalScrollSyncRole GetRole(DependencyObject obj)
        {
            return (HorizontalScrollSyncRole) obj.GetValue(RoleProperty);
        }

        private static void RolePropertyChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var newValue = (HorizontalScrollSyncRole) args.NewValue;
            var scrollViewer = (ScrollViewer) obj;

            switch (newValue)
            {
                case HorizontalScrollSyncRole.Primary:
                {
                    if (_primaryScrollViewer != null)
                    {
                        _primaryScrollViewer.ScrollChanged -= PrimaryScrollViewerScrollChanged;
                    }

                    _primaryScrollViewer = scrollViewer;

                    if (_primaryScrollViewer != null)
                    {
                        _primaryScrollViewer.ScrollChanged += PrimaryScrollViewerScrollChanged;
                    }

                    break;
                }

                case HorizontalScrollSyncRole.Replica:
                {
                    _replicaScrollViewer = scrollViewer;

                    break;
                }

                default:
                {
                    throw new NotSupportedException();
                }
            }
        }

        private static void PrimaryScrollViewerScrollChanged(
            object sender,
            ScrollChangedEventArgs e)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            // Any non-zero values should update scroll offset

            var updateHorizontalOffset =
                _replicaScrollViewer != null &&
                e.HorizontalChange != 0 &&
                _replicaScrollViewer.HorizontalOffset != e.HorizontalOffset;

            // ReSharper restore CompareOfFloatsByEqualityOperator

            if (updateHorizontalOffset)
            {
                _replicaScrollViewer.ScrollToHorizontalOffset(
                    e.HorizontalOffset);
            }
        }
    }
}
