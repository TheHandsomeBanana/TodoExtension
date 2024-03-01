using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExtension.Models;

namespace TodoExtension.Core {
    public static class TodoItemLoader {
        public static async Task<TodoItem[]> GetItemsInSolutionAsync(Solution solution) {
            List<Task<TodoItem[]>> todoTasks = new List<Task<TodoItem[]>>();
            foreach(Project project in solution.Projects) {
                todoTasks.Add(GetItemsInProjectAsync(project));
            }

            return (await Task.WhenAll(todoTasks)).SelectMany(e => e).ToArray();
        }

        public static async Task<TodoItem[]> GetItemsInProjectAsync(Project project) {
            CommentCollector commentCollector = new CommentCollector();

            foreach (Document document in project.Documents) {
                SyntaxNode root = await document.GetSyntaxRootAsync();
                commentCollector.Visit(root);
            }

            IEnumerable<SyntaxTrivia> todoItemComments = FilterComments(commentCollector.Comments);

            return todoItemComments.Select(f => new TodoItem() {
                Project = project.Name,
                FileName = f.SyntaxTree.FilePath,
                LineNumber = f.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                Description = f.ToString()
            }).ToArray();
        }

        private static IEnumerable<SyntaxTrivia> FilterComments(IEnumerable<SyntaxTrivia> comments) {
            foreach (SyntaxTrivia comment in comments) {
                if (IsTodoComment(comment.ToString()))
                    yield return comment;
            }
        }

        public static bool IsTodoComment(string triviaString) {
            return triviaString.TrimStart(' ', '/', '*').StartsWith("todo", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
