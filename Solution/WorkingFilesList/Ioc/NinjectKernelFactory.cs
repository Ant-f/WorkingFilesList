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

using EnvDTE80;
using Ninject;
using WorkingFilesList.Ioc.Modules;

namespace WorkingFilesList.Ioc
{
    public class NinjectKernelFactory
    {
        public IKernel CreateKernel(DTE2 dte2)
        {
            var kernel = new StandardKernel(
                new CommandModule(),
                new FactoryModule(),
                new RepositoryModule(),
                new ServiceModule(),
                new ViewModelModule());

            kernel.Bind<DTE2>().ToConstant(dte2);
            return kernel;
        }
    }
}
