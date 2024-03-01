using HBLibrary.VisualStudio.UI;
using HBLibrary.VisualStudio.Workspace;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoExtension.Core;
using TodoExtension.Models;
using TodoExtension.ToolWindows.Commands;

namespace TodoExtension.ToolWindows {
    public class TodoItemViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TodoItem> TodoItems { get; set; } = TodoItemContainer.Items;
        private void NotifyPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Scope selectedScope = Scope.Solution;
        public Scope SelectedScope {
            get { return selectedScope; }
            set {
                if (selectedScope == value)
                    return;

                selectedScope = value;
                NotifyPropertyChanged(nameof(SelectedScope));
            }
        }

        private TodoItem selectedTodoItem;
        public TodoItem SelectedTodoItem {
            get { return selectedTodoItem; }
            set {
                if (value == null || value == selectedTodoItem)
                    return;

                selectedTodoItem = value;
                NotifyPropertyChanged(nameof(SelectedTodoItem));

                if (value != null)
                    SolutionHelper.NavigateToFileAndLine(value.FileName, value.LineNumber);
            }
        }

        public AsyncRelayCommand ScopeSelectionChangedCommand { get; }

        public TodoItemViewModel() {
            TodoItemContainer.TodoItemsChanged += (_, e) => {
                SelectedScope = e.Scope;
                SelectedTodoItem = null;
            };

            ScopeSelectionChangedCommand = new AsyncRelayCommand(ExecuteScopeSelectionChangedAsync, e => true, OnException);
        }

        private void OnException(Exception exception) {
            UIHelper.ShowError(exception.ToString());
        }

        private async Task ExecuteScopeSelectionChangedAsync() {
            TodoItem[] todoItems = null;
            switch (this.SelectedScope) {
                case Scope.Solution:
                    todoItems = await TodoItemLoader.GetItemsInSolutionAsync(WorkspaceHelper.GetCurrentCASolution());
                    break;
                case Scope.Project:
                    todoItems = await TodoItemLoader.GetItemsInProjectAsync(SolutionHelper.GetCurrentCAProject());
                    break;
            }

            if (todoItems == null)
                return;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            TodoItemContainer.ReplaceItems(todoItems, this.SelectedScope);
        }
    }

    public enum Scope {
        Solution,
        Project
    }
}
