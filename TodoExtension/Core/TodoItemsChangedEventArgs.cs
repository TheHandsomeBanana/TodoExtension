using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExtension.ToolWindows;

namespace TodoExtension.Core {
    public class TodoItemsChangedEventArgs : EventArgs {
        public Scope Scope { get; set; }
    }
}
