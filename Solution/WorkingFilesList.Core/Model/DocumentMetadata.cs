﻿// Working Files List
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

using EnvDTE;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorkingFilesList.Core.Interface;

namespace WorkingFilesList.Core.Model
{
    public class DocumentMetadata : PropertyChangedNotifier
    {
        /// <summary>
        /// Value for <see cref="PinOrder"/> denoting that this item is not pinned
        /// </summary>
        public const int UnpinnedOrderValue = -2;

        private bool _hasWindow;
        private bool _isActive;
        private bool _isPinned;
        private bool _isReordering;
        private Brush _projectBrush;
        private Direction _reorderingDirection;
        private double _usageOrder;
        private int _pinOrder;
        private string _displayNameHighlight;
        private string _displayNamePostHighlight;
        private string _displayNamePreHighlight;

        /// <summary>
        /// Icon depicting a document's file type
        /// </summary>
        public BitmapSource Icon { get; set; }

        /// <summary>
        /// Used when <see cref="IUserPreferences.ShowRecentUsage"/> and/or
        /// <see cref="IUserPreferences.AssignProjectColours"/> are enabled
        /// </summary>
        public Brush ProjectBrush
        {
            get
            {
                return _projectBrush ?? Brushes.Transparent;
            }

            set
            {
                if (!ProjectBrush.Equals(value))
                {
                    _projectBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Time when the document this metadata corresponds to was activated,
        /// i.e. the time that the document window received focus
        /// </summary>
        public DateTime ActivatedAt { get; set; }

        /// <summary>
        /// Full path and name of document file, used for display purposes
        /// </summary>
        public string CorrectedFullName { get; }
        
        /// <summary>
        /// Shows the direction that an item will be placed when re-ordering
        /// items via drag-and-drop
        /// </summary>
        public Direction ReorderingDirection
        {
            get
            {
                return _reorderingDirection;
            }

            set
            {
                if (_reorderingDirection != value)
                {
                    _reorderingDirection = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Substring of <see cref="CorrectedFullName"/> that is actually displayed
        /// </summary>
        public string DisplayName
        {
            get
            {
                var displayName =
                    DisplayNamePreHighlight +
                    DisplayNameHighlight +
                    DisplayNamePostHighlight;

                return displayName;
            }
        }

        /// <summary>
        /// Substring of <see cref="DisplayName"/> to be highlighted
        /// </summary>
        public string DisplayNameHighlight
        {
            get
            {
                return _displayNameHighlight;
            }

            set
            {
                if (_displayNameHighlight != value)
                {
                    _displayNameHighlight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Substring of <see cref="DisplayName"/> to the right of
        /// <see cref="DisplayNameHighlight"/>
        /// </summary>
        public string DisplayNamePostHighlight
        {
            get
            {
                return _displayNamePostHighlight;
            }

            set
            {
                if (_displayNamePostHighlight != value)
                {
                    _displayNamePostHighlight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Substring of <see cref="DisplayName"/> to the left of
        /// <see cref="DisplayNameHighlight"/>
        /// </summary>
        public string DisplayNamePreHighlight
        {
            get
            {
                return _displayNamePreHighlight;
            }

            set
            {
                if (_displayNamePreHighlight != value)
                {
                    _displayNamePreHighlight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Full path and name of document file, as reported by the <see cref="DTE"/>
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Indicates whether this document has an open, though not necessarily
        /// active, editor window
        /// </summary>
        public bool HasWindow
        {
            get
            {
                return _hasWindow;
            }

            set
            {
                if (_hasWindow != value)
                {
                    _hasWindow = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether this document is active/has focus
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Indicates whether this item is having its <see cref="PinOrder"/>
        /// updated via drag-and-drop
        /// </summary>
        public bool IsReordering
        {
            get
            {
                return _isReordering;
            }

            set
            {
                if (_isReordering != value)
                {
                    _isReordering = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPinned
        {
            get
            {
                return _isPinned;
            }
            private set
            {
                if (_isPinned != value)
                {
                    _isPinned = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Value representing relative ordering of pinned document in displayed
        /// list. The actual value (when above <see cref="UnpinnedOrderValue"/>)
        /// is not important; only its relative value, when compared with other
        /// <see cref="DocumentMetadata"/> instances, is
        /// </summary>
        public int PinOrder
        {
            get
            {
                return _pinOrder;
            }

            set
            {
                if (_pinOrder != value)
                {
                    _pinOrder = value;
                    OnPropertyChanged();
                }

                IsPinned = EvaluateIsPinned(_pinOrder);
            }
        }

        /// <summary>
        /// Indicates the position of this <see cref="DocumentMetadata"/>
        /// instance in relation to others when ordered by
        /// <see cref="ActivatedAt"/>. Represented by a value between 0 and 1
        /// inclusive
        /// </summary>
        public double UsageOrder
        {
            get
            {
                return _usageOrder;
            }

            set
            {
                if (_usageOrder != value)
                {
                    _usageOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProjectNameData ProjectNames { get; }

        public DocumentMetadata(
            DocumentMetadataInfo info,
            string correctedFullName,
            BitmapSource icon)
        {
            CorrectedFullName = correctedFullName;
            FullName = info.FullName;
            Icon = icon;
            PinOrder = UnpinnedOrderValue;

            ProjectNames = new ProjectNameData(
                info.ProjectDisplayName,
                info.ProjectFullName);
        }

        private static bool EvaluateIsPinned(int pinOrder)
        {
            return pinOrder > UnpinnedOrderValue;
        }
    }
}
