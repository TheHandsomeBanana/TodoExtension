using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TodoExtension.Core;

namespace TodoExtension.Classifier {
    public class TodoClassifier : IClassifier {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("TodoClassifier")]
        internal static ClassificationTypeDefinition TodoCommentClassificationType = null;

        private readonly IClassificationType classificationType;
        public TodoClassifier(IClassificationTypeRegistryService registry) {
            classificationType = registry.GetClassificationType("TodoClassifier");
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span) {
            List<ClassificationSpan> classifications = new List<ClassificationSpan>();

            MatchCollection matches = TodoRegex.DefaultTodoRegex.Matches(span.GetText());

            foreach (Match match in matches) {
                var matchSpan = new SnapshotSpan(span.Snapshot, new Span(span.Start + match.Index, match.Length));
                var classificationSpan = new ClassificationSpan(matchSpan, classificationType);
                classifications.Add(classificationSpan);
            }

            return classifications;
        }
    }
}
