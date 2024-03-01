using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExtension.Models;
using TodoExtension.ToolWindows;

namespace TodoExtension.Core {
    public static class TodoItemContainer {
        public static ObservableCollection<TodoItem> Items { get; } = new ObservableCollection<TodoItem>();

        public static EventHandler<TodoItemsChangedEventArgs> TodoItemsChanged;
        public static void OnTodoItemsChanged(Scope scope) {
            TodoItemsChanged?.Invoke(null, new TodoItemsChangedEventArgs() { Scope = scope});
        }

        public static void ReplaceItems(IEnumerable<TodoItem> newItems, Scope scope) {
            Items.Clear();
            foreach (var item in newItems)
                Items.Add(item);

            OnTodoItemsChanged(scope);
        }
    }
}
