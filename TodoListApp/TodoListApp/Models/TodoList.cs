using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using TodoListApp.Services;

namespace TodoListApp.Models
{
    public class TodoList {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Data { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }

        public string Tagi{get;set ;}
        public bool Status { get; set; }

        public TodoList(string tytul, string opis, string tagi) {
           this.Tytul = tytul;
           this.Data = DateTime.Now;
           this.Opis = opis;
           this.Tagi = tagi;
           
        }

        
        public TodoList() { }

        // Do zmieniania statusu po wciśnięciu swicha
        public bool ChangeStatus { get => Status;  set {
                if (Status != value) {
                    this.Status = value;
                    _ = TodoAppDb.UpdateStatus(this.Status, this.ID);
                }
            } }

    }
}
