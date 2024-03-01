using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoExtension.Classifier {
    [Export(typeof(IClassifierProvider))]
    [ContentType("text")]
    public class TodoClassifierProvider : IClassifierProvider {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        public IClassifier GetClassifier(ITextBuffer textBuffer) {
            return textBuffer.Properties.GetOrCreateSingletonProperty(() =>
            new TodoClassifier(ClassificationRegistry));
        }
    }
}
