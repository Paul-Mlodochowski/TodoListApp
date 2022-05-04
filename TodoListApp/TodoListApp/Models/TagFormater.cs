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
            Tags.ForEach(item => { sb.Append(item);sb.Append(' '); });
            return sb.ToString();
        }
        public string ReturnCombinedString(List<string> Tags) {
            StringBuilder sb = new StringBuilder();
            Tags.ForEach(item => {
                if (item != "") {
                    string itemToAdd = item.Remove(0, 1);
                    sb.Append(itemToAdd);
                    sb.Append(',');
                }
            });
            sb.Remove(sb.Length-1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// Zwraca Listę tagów z hasztagiem
        /// </summary>

        public List<string> ReturnCombidedListOfTagsWithPrexiex(string value) {
            var ReturnList = new List<string>(value.Split(','));
            ReturnList.ForEach(item => { Tags.Add(item.Trim().Insert(0, "#").ToLower()); });
            return new List<string>(this.Tags);
            

        }
        /// <summary>
        /// Zwraca Listę tagów BEZ hasztaków
        /// </summary>
        public List<string> ReturnCombidedListOfTags(string value) {
            var ReturnList = new List<string>(value.Split(' '));
            ReturnList.ForEach(item => { Tags.Add(item.Trim().ToLower()); });
            return new List<string>(this.Tags);


        }
        public void Clear() {
            Tags.Clear();
        }

    }
}
