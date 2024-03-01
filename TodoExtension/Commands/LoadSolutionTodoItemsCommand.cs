using HBLibrary.VisualStudio.Commands;
using HBLibrary.VisualStudio.Workspace;
using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoExtension.Core;
using TodoExtension.Models;

namespace TodoExtension.Commands {
    internal sealed class LoadSolutionTodoItemsCommand : AsyncCommandBase {
        protected override int CommandId => 0x0200;
        protected override Guid CommandSet => new Guid("a1afaeeb-79e0-471c-b190-385f615a1d07");


        private LoadSolutionTodoItemsCommand(AsyncPackage package, OleMenuCommandService commandService) 
            : base(package, commandService, OnException) { }

        private static void OnException(Exception exception) {
            // Todo: Implement logging
        }

        public static LoadSolutionTodoItemsCommand Instance {
            get;
            private set;
        }

        public static async Task InitializeAsync(AsyncPackage package) {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new LoadSolutionTodoItemsCommand(package, commandService);
        }

        protected override async Task ExecuteAsync(object sender, EventArgs e) {
            Solution solution = WorkspaceHelper.GetCurrentCASolution();
            TodoItem[] todoItems = await TodoItemLoader.GetItemsInSolutionAsync(solution);

            await this.Package.JoinableTaskFactory.SwitchToMainThreadAsync();

            TodoItemContainer.ReplaceItems(todoItems, ToolWindows.Scope.Solution);
        }
    }
}
