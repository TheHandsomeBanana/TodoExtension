using HBLibrary.VisualStudio.Commands;
using HBLibrary.VisualStudio.Workspace;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExtension.Core;
using Microsoft.VisualStudio.Shell.Interop;
using TodoExtension.ToolWindows;
using System.Threading;

namespace TodoExtension.Commands {
    public class OpenTodoItemWindowCommand : AsyncCommandBase {
        protected override int CommandId => 0x0300;
        protected override Guid CommandSet => new Guid("a1afaeeb-79e0-471c-b190-385f615a1d07");


        private OpenTodoItemWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
            : base(package, commandService, OnException) { }

        private static void OnException(Exception exception) {
            throw exception;
        }

        public static OpenTodoItemWindowCommand Instance {
            get;
            private set;
        }

        public static async Task InitializeAsync(AsyncPackage package) {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new OpenTodoItemWindowCommand(package, commandService);
        }

        protected override async Task ExecuteAsync(object sender, EventArgs e) {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            ToolWindowPane window = await Package.FindToolWindowAsync(typeof(TodoItemWindow), 0, true, CancellationToken.None);
            if (window == null || window.Frame == null) 
                throw new NotSupportedException("Cannot create tool window");
            
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
