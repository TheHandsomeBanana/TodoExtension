using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TodoExtension.Core {
    public static class TodoRegex {
        public static readonly Regex DefaultTodoRegex = new Regex(@"//\s*Todo:.*|/\*+\s*Todo:.*|\*+\s*Todo:.*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex BetterTodoRegex = new Regex(@"//\s*Todo:.*|/\*\s*Todo:[^*]*\*(?:[^/][^*]*\*)*/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
