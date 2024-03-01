using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoExtension.Core {
    public class CommentCollector : CSharpSyntaxWalker {
        public List<SyntaxTrivia> Comments { get; } = new List<SyntaxTrivia>();

        public CommentCollector() : base(SyntaxWalkerDepth.Trivia) {
        }

        public override void VisitTrivia(SyntaxTrivia trivia) {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
                Comments.Add(trivia);

            // Continue traversing the syntax tree
            base.VisitTrivia(trivia);
        }
    }
}
