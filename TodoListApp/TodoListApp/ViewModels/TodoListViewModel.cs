using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.ViewModels
{
    public class TodoListViewModel : BaseViewModel
    {
        public ObservableRangeCollection<TodoList> List { get; set; }
       
        public AsyncCommand AddNewTODOCommand { get; }
        public AsyncCommand<TodoList> DeleteCommand { get; }
        private bool _istoggled = false;
        public bool IsToggledFilter { get => _istoggled; set { 
            SetProperty(ref _istoggled, value);
            } }
        public TodoListViewModel() {

            Add();
            AddNewTODOCommand = new AsyncCommand(AddNewTODO);
            DeleteCommand = new AsyncCommand<TodoList>(DeleteTODO);
            
        }
        async void Add() {
           
            List = new ObservableRangeCollection<TodoList>();

            List.AddRange(await TodoAppDb.GetList());


            OnPropertyChanged("List");
        }
        
        async Task AddNewTODO() {
            var tytul = await App.Current.MainPage.DisplayPromptAsync("Title", "Name the Title: ");
            var opis = await App.Current.MainPage.DisplayPromptAsync("Description", "Name the description: ");
            var tags = await App.Current.MainPage.DisplayPromptAsync("Tags", "Tags Represented are as (Tag, Tag2...) ");
            TagFormater tagfor = new TagFormater();
            tagfor.FormatTags(tags);

            TodoList newTodo = new TodoList(tytul,opis,tagfor.ReturnCombinedString());
            await TodoAppDb.AddDateToDb(newTodo);
            List.Clear();
            List.AddRange(await TodoAppDb.GetList());
        }
        async Task DeleteTODO(TodoList todo) {
            await TodoAppDb.Delete(todo.ID);
            List.Clear();
            List.AddRange(await TodoAppDb.GetList());
        }
        void ToggleFilters(bool value) {
            App.Current.MainPage.DisplayAlert("RA",value.ToString(),"fff");
        }
    }
}
