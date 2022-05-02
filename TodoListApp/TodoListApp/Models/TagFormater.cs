using System;
using System.Collections.Generic;
using System.Text;

namespace TodoListApp.Models
{
    public class TagFormater
    {
        private List<string> Tags { get; set; } = new List<string>();

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
       public List<string> ReturnCombidedListOfTagsWithPrexiex(string value) {
            var ReturnList = new List<string>(value.Split(','));
            ReturnList.ForEach(item => { Tags.Add(item.Trim().Insert(0, "#")); });
            return new List<string>(this.Tags);
            

        }
        public List<string> ReturnCombidedListOfTags(string value) {
            var ReturnList = new List<string>(value.Split(' '));
            ReturnList.ForEach(item => { Tags.Add(item.Trim()); });
            return new List<string>(this.Tags);


        }
        public void Clear() {
            Tags.Clear();
        }

    }
}
