// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using Moq;
using System;
using System.Collections.Generic;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class OptionsListsBuilder
    {
        public IList<ISortOption> SortOptions { get; set; }

        public Type[] SortOptionsDisplayOrder { get; set; }

        public ISortOptionsService SortOptionsService { get; set; }

        public OptionsLists CreateOptionsLists()
        {
            var container = Mock.Of<IDisplayOrderContainer>(d =>
                d.DisplayOrder == (SortOptionsDisplayOrder ?? new Type[0]));

            var lists = new OptionsLists(
                SortOptions,
                container,
                SortOptionsService ?? Mock.Of<ISortOptionsService>());

            return lists;
        }
    }
}
