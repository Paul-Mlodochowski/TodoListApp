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
        private bool _istoggledFilter = false;
        private bool _istoggled = false;
        public bool IsToggledFilter { get => _istoggledFilter; set { 
            SetProperty(ref _istoggledFilter, value);
            } }
        public bool IsToggled { get => _istoggled; set { //Dla Filtrowania zadan ukończonych

                if(value)
                FilterTheResult(async () => {
                    var FiltredResults = new ObservableRangeCollection<TodoList>();
                    foreach (var item in await TodoAppDb.GetList())
                        if(item.Status)
                            FiltredResults.Add(item);
                    return FiltredResults;
                 });
                else
                    FilterTheResult(async () => {
                        var FiltredResults = new ObservableRangeCollection<TodoList>();
                        foreach (var item in await TodoAppDb.GetList())
                            if (!item.Status)
                                FiltredResults.Add(item);
                        return FiltredResults;
                    });
                SetProperty(ref _istoggled, value);
            } }
        public Command<TodoList> IsToggledCommand{ get; }
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
       async void FilterTheResult(Func<Task<ObservableRangeCollection<TodoList>>> func) {
            List = await func.Invoke();
            OnPropertyChanged("List");
        }
        
    }
}
