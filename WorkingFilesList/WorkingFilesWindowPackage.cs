// WorkingFilesList
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Ninject;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using WorkingFilesList.Ioc;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.Locator;

namespace WorkingFilesList
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(WorkingFilesWindow))]
    [Guid(WorkingFilesWindowPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class WorkingFilesWindowPackage : Package
    {
        /// <summary>
        /// WorkingFilesWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "26bf5782-13d2-4168-b979-bfcde9bd63dd";

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingFilesWindow"/> class.
        /// </summary>
        public WorkingFilesWindowPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            WorkingFilesWindowCommand.Initialize(this);
            base.Initialize();

            var dte2 = (DTE2) GetService(typeof(DTE));
            NinjectContainer.InitializeKernel(dte2);

            var events2 = (Events2) dte2.Events;
            var subscriber = NinjectContainer.Kernel.Get<IDteEventsSubscriber>();
            subscriber.SubscribeTo(events2);

            // Inject properties by resolving a ViewModelService instance
            NinjectContainer.Kernel.Get<ViewModelService>();
        }

        #endregion
    }
}
