﻿using System;
using System.Threading.Tasks;

namespace TodoExtension.ToolWindows.Commands {
    public abstract class AsyncCommandBase : CommandBase {
        private bool isExecuting;
        private readonly Action<Exception> onException;

        public bool IsExecuting {
            get { return isExecuting; }
            set {
                isExecuting = value;
                OnCanExecuteChanged();
            }
        }

        public AsyncCommandBase(Action<Exception> onException) {
            this.onException = onException;
        }

        public override bool CanExecute(object parameter) {
            return !IsExecuting;
        }

        public override async void Execute(object parameter) {
            IsExecuting = true;
            try {
                await ExecuteAsync(parameter);
            }
            catch (Exception e) {
                onException?.Invoke(e);
            }

            IsExecuting = false;
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}
