using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApp.Models
{
    public class TagFormater
    {
        public List<string> Tags { get; set; } = new List<string>();

        public void FormatTags(string tag) {
            var tagarray = tag.Split(',');
            foreach (var item in tagarray) 
                Tags.Add(item.Trim().Insert(0, "#"));
            
        }
        public string ReturnCombinedString() {
            StringBuilder sb = new StringBuilder();
            Tags.ForEach(item => { sb.Append(item + " "); });
            return sb.ToString();
        }

    }
}
