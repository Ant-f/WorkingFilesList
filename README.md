# Working Files List

Copyright © 2016 Anthony Fung

Working Files List is a Visual Studio 2015/2017 extension. It adds a tool window
that shows a vertically stacked list of documents that have been opened in the
editor, with the ability to group and sort entries.

## Installation

After building the project (using the Release build configuration as
appropriate), navigate to the output directory and launch
`WorkingFilesList.vsix` to add the extension to Visual Studio.

Alternatively, you can download a copy from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=Ant-f.WorkingFilesList);
or from within Visual Studio by searching for `working files list` in
[**Tools**] -> [**Extensions and Updates**]

Within Visual Studio, select [**View**] -> [**Other Windows**] -> [**Working Files List**] on
the menu bar.

## Resources

[User Guide](https://github.com/Ant-f/WorkingFilesList/wiki/User-Guide)

[Wiki](https://github.com/Ant-f/WorkingFilesList/wiki)

[Visual Studio Marketplace Page](https://marketplace.visualstudio.com/items?itemName=Ant-f.WorkingFilesList)

## License

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
   
<http://www.apache.org/licenses/LICENSE-2.0>
   
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

## Launching in the Visual Studio Experimental Instance

To launch this extension in the Visual Studio Experimental Instance, select the
**Debug** tab in the properties page for `WorkingFilesList`. Select **Start
external program** as the start action and enter the path to Visual Studio, e.g.

`C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe`

Set `/rootsuffix Exp` as the start options **Command line arguments**

## Pinned Files

When closing a solution or exiting Visual Studio with items pinned in Working
Files List, information about the pinned items will be written to disk so that
they can be restored when the solution is next opened. JSON files are written to
the `WorkingFilesList` folder, located within the logged in user’s Documents
folder. The names of the written files include a hash of the corresponding
solution’s path, meaning that if a solution is moved to another location, any
previously pinned items for that solution will not be restored the next time
that it is opened.

The JSON files created by Working Files List can be safely deleted if the pinned
status of files is no longer required.

## Uninstallation

* Select [**Tools**] -> [**Extensions and Updates...**] on the menu bar. There
should be an option to uninstall the extension.

* The `WorkingFilesList` folder and its contents, located in the logged in
user's Documents folder can safely be deleted.

## Known Issues

### Automatic project reloading (Visual Studio Tools for Unity)

Working Files List relies on certain internal event notifications from Visual
Studio to maintain synchronisation between itself and any open document editors
within Visual Studio. When _Visual Studio Tools for Unity_ is installed and
_Automatic project reloading_ is enabled, it does not seem possible to precisely
track open files within a Unity generated project when the project is
updated/overwritten by an external source. See
[here](https://github.com/Ant-f/WorkingFilesList/issues/14) for more information.
