using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TodoExtension.Classifier {
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "TodoClassifier")]
    [Name("TodoClassifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    public class TodoFormat : ClassificationFormatDefinition {
        public TodoFormat() {
            this.DisplayName = "TODO Comment";
            this.ForegroundColor = Colors.Orange;
        }
    }
}
