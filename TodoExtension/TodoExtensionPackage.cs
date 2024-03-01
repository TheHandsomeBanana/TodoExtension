using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TodoExtension.ToolWindows;
using TodoExtension.ToolWindows.Commands;
using Task = System.Threading.Tasks.Task;

namespace TodoExtension {
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
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(TodoExtensionPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ToolWindows.TodoItemWindow))]
    public sealed class TodoExtensionPackage : AsyncPackage, IVsSelectionEvents {
        /// <summary>
        /// TodoExtensionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "f7b85fdc-3d10-4391-b4fb-b39b05d1d388";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await TodoExtension.Commands.LoadProjectTodoItemsCommand.InitializeAsync(this);
            await TodoExtension.Commands.LoadSolutionTodoItemsCommand.InitializeAsync(this);
            await TodoExtension.Commands.OpenTodoItemWindowCommand.InitializeAsync(this);

            monitorSelection = await GetServiceAsync(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            if (monitorSelection != null)
                monitorSelection.AdviseSelectionEvents(this, out selectionEventsCookie);
        }


        private uint selectionEventsCookie;
        private IVsMonitorSelection monitorSelection;
        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew) {
            //TodoItemWindow window = FindToolWindow(typeof(TodoItemWindow), 0, true) as TodoItemWindow;
            //if(window == null || !(window.Content is FrameworkElement frameworkElement))
            //    return VSConstants.E_ABORT;

            //TodoItemViewModel viewModel = frameworkElement.DataContext as TodoItemViewModel;
            //if (viewModel == null)
            //    return VSConstants.E_ABORT;

            //AsyncRelayCommand command = viewModel.ScopeSelectionChangedCommand;

            //if(command == null || command.CanExecute(null))
            //    return VSConstants.E_ABORT;

            //JoinableTaskFactory.RunAsync(async () => {
            //    await command.ExecuteAsync(null);
            //});

            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew) {
            return VSConstants.S_OK;
        }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive) {
            return VSConstants.S_OK;
        }

        #endregion
    }
}
