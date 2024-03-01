using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.ComponentModelHost;

namespace TodoExtension.Listener {
    //[Export(typeof(IWpfTextViewConnectionListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal class TodoTextViewConnectionListener : IWpfTextViewConnectionListener {
        private ErrorListProvider errorListProvider;
        private readonly VisualStudioWorkspace workspace;

        [ImportingConstructor]
        public TodoTextViewConnectionListener([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider) {
            IComponentModel componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            this.workspace = componentModel.GetService<VisualStudioWorkspace>();
        }

        public void SubjectBuffersConnected(IWpfTextView textView, ConnectionReason reason, Collection<ITextBuffer> subjectBuffers) {
            foreach (ITextBuffer buffer in subjectBuffers)
                buffer.Changed += BufferChanged;
        }

        public void SubjectBuffersDisconnected(IWpfTextView textView, ConnectionReason reason, Collection<ITextBuffer> subjectBuffers) {
            foreach (ITextBuffer buffer in subjectBuffers) {
                buffer.Changed -= BufferChanged;
            }
        }

        private void BufferChanged(object sender, TextContentChangedEventArgs e) {
            RefreshTodoItems();
        }

        private void RefreshTodoItems() {

            UpdateErrorList();
        }

        private void UpdateErrorList() {
            errorListProvider.Tasks.Clear();

        }
    }
}
