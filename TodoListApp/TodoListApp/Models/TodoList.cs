﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

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

        public TodoList(string tytul, string opis) {
            this.Tytul = tytul;
            this.Data = DateTime.Now;
            this.Opis = opis;
        }
        public TodoList() { }
    }
}
