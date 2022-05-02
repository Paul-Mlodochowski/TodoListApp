using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TodoListApp.Models;
using Xamarin.Essentials;

namespace TodoListApp.Services
{
    public static class TodoAppDb
    {
        public static SQLiteAsyncConnection db;
        public static async Task Init() {
            if (db != null)
                return;


            var PathToDb = Path.Combine(FileSystem.AppDataDirectory, "TodoListAppDatabase.db");
            db = new SQLiteAsyncConnection(PathToDb);
            await db.CreateTableAsync<TodoList>();
        }
        public static async Task<IEnumerable<TodoList>> GetList() {
            await Init();
        
            var Records = await db.Table<TodoList>().ToListAsync();
            return Records;

        }
        public static async Task AddDateToDb(TodoList item) {

            await Init();
            var ItemToPut = new TodoList()
            {
                Tytul = item.Tytul,
                Opis = item.Opis,
                Data = item.Data,
                Status = item.Status,
                Tagi = item.Tagi

            };
            

            await db.InsertAsync(ItemToPut);
        }
        public static async Task Delete(int id) {
            await Init();

            await db.DeleteAsync<TodoList>(id);
           
        }
        public static async Task UpdateStatus(bool val,int ID) {
            await Init();
            var todo = await db.Table<TodoList>().ToListAsync();
            var guy = todo.Find(t => t.ID == ID);
            var ItemToPut = new TodoList()
            {
                ID = ID,
                Tytul = guy.Tytul,
                Opis = guy.Opis,
                Data = guy.Data,
                Status = val,
                Tagi = guy.Tagi

            };

            var rows = db.UpdateAsync(ItemToPut);
            
        }
        
       
    }
}
