using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoExtension.Models {
    public class TodoItem : IEquatable<TodoItem> {
        public string Description { get; set; }
        public string Project { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }

        public bool Equals(TodoItem other) {
            return Description == other.Description 
                && Project == other.Project 
                && FileName == other.FileName 
                && LineNumber == other.LineNumber;
        }

        public override bool Equals(object obj) {
            return obj is TodoItem i && Equals(i);
        }

        public override int GetHashCode() {
            return Description.GetHashCode() 
                + Project.GetHashCode() 
                + FileName.GetHashCode() 
                + LineNumber.GetHashCode();
        }
    }
}
