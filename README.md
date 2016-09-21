# Working Files List

Copyright © 2016 Anthony Fung

Working Files List is a Visual Studio 2015 extension. It adds a tool window
that shows a vertically stacked list of documents that have been opened in the
editor, with the ability to group and sort entries.

## Installation

After building the project (using the Release build configuration as
appropriate), navigate to the output directory and launch
`WorkingFilesList.vsix` to add the extension to Visual Studio. Within Visual
Studio, select [**View**] -> [**Other Windows**] -> [**Working Files List**] on
the menu bar.

## Resources

[User Guide](https://github.com/Ant-f/WorkingFilesList/wiki/User-Guide)

[Wiki](https://github.com/Ant-f/WorkingFilesList/wiki)

## License

Working Files List is free software: you can redistribute it and/or modify it
under the terms of the GNU Lesser General Public License as published by the
Free Software Foundation, either version 3 of the License, or (at your option)
any later version.

Working Files List is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for
more details.

You should have received a copy of the GNU Lesser General Public License along
with Working Files List. If not, see <http://www.gnu.org/licenses/>

Working Files List uses the third party open source software Ninject under the
terms and conditions of the Apache License Version 2.0. You may obtain a copy
of the License at <http://www.apache.org/licenses/LICENSE-2.0>

## Launching in the Visual Studio Experimental Instance

To launch this extension in the Visual Studio Experimental Instance, select the
**Debug** tab in the properties page for `WorkingFilesList`. Select **Start
external program** as the start action and enter the path to Visual Studio, e.g.

`C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe`

Set `/rootsuffix Exp` as the start options **Command line arguments**

## Uninstallation

Select [**Tools**] -> [**Extensions and Updates...**] on the menu bar. There
should be an option to uninstall the extension.