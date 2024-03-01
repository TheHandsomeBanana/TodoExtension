﻿using System;
using System.Threading.Tasks;

namespace TodoExtension.ToolWindows.Commands {
    public class AsyncRelayCommand : AsyncCommandBase {
        private readonly Func<Task> callback;
        private readonly Predicate<object> canExecute;
        public AsyncRelayCommand(Func<Task> callback, Predicate<object> canExecute, Action<Exception> onException) : base(onException) {
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return canExecute != null ? canExecute(parameter) && base.CanExecute(parameter) : base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter) => await callback();

    }
}
