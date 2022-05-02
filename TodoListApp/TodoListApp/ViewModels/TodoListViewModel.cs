using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TodoListApp.Models;
using TodoListApp.Services;
using System.Linq;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace TodoListApp.ViewModels
{
    public class TodoListViewModel : BaseViewModel {
        /// <summary>
        /// Główna lista Todo która jest wyświetlania w UI
        /// </summary>
        public ObservableRangeCollection<TodoList> List { get; set; }

        private bool _istoggledFilter = false;
        private bool _istoggled = false;
        

        //Właściwości

        public bool IsToggledFilter { get => _istoggledFilter; set {
                SetProperty(ref _istoggledFilter, value);
            } }
        public bool IsToggled { get => _istoggled; set { //Dla Filtrowania zadan ukończonych

                if (value)
                    FilterTheResult(async () => {

                        var query = from item in await TodoAppDb.GetList()
                                    where item.Status
                                    select item;
                        var NewResults = new ObservableRangeCollection<TodoList>();
                        NewResults.AddRange(query);
                        return NewResults;

                    });
                else
                    FilterTheResult(async () => {
                        var query = from item in await TodoAppDb.GetList()
                                    where !item.Status
                                    select item;
                        var FilteredResults = new ObservableRangeCollection<TodoList>();
                        FilteredResults.AddRange(query);
                        return FilteredResults;
                    });
                SetProperty(ref _istoggled, value);
            } }

        private DateTime _date = DateTime.Today;
        public DateTime DateSelected { get => _date; set { // Filtroawnie dat
                FilterTheResult(async () => {

                    var query = from item in await TodoAppDb.GetList()
                                where DateTime.Compare(item.Data, value) >= 0
                                select item;
                    var FilteredResults = new ObservableRangeCollection<TodoList>();
                    FilteredResults.AddRange(query);
                    return FilteredResults;

                });
                SetProperty(ref _date, value);

            } }

        //Komendy
        public AsyncCommand AddNewTODOCommand { get; }
        public AsyncCommand<TodoList> DeleteCommand { get; }
        
        public MvvmHelpers.Commands.Command<string> TagsFilterCommand { get; }
        public MvvmHelpers.Commands.Command<string> TitleOrDescriptonFilterCommand { get; }
        public MvvmHelpers.Commands.AsyncCommand RefreshCommand { get; }

        private bool _isBusy = false;
        public bool IsBusy { get => _isBusy; set => _isBusy = value; }
            
        public TodoListViewModel() {
            
            Add();

            AddNewTODOCommand = new AsyncCommand(AddNewTODO);
            DeleteCommand = new AsyncCommand<TodoList>(DeleteTODO);
            TagsFilterCommand = new MvvmHelpers.Commands.Command<string>(TagsFilter);
            TitleOrDescriptonFilterCommand = new MvvmHelpers.Commands.Command<string>(TitleOrDescriptionFilter);
            RefreshCommand = new AsyncCommand(Refresh);
            

        }



        private async void Add() {
            
            List = new ObservableRangeCollection<TodoList>();

            List.AddRange(await TodoAppDb.GetList());


            OnPropertyChanged("List");
        }

        //Dodaje nowy kafelek Todo
        private async Task AddNewTODO() {
            var tytul = await App.Current.MainPage.DisplayPromptAsync("Title", "Name the Title: ");
            var opis = await App.Current.MainPage.DisplayPromptAsync("Description", "Name the description: ");
            var tags = await App.Current.MainPage.DisplayPromptAsync("Tags", "Tags Represented are as (Tag, Tag2...) ", placeholder:"Seperate tags with (,)");
            TagFormater tagfor = new TagFormater();
            tagfor.FormatTags(tags);

            TodoList newTodo = new TodoList(tytul,opis,tagfor.ReturnCombinedString());
            await TodoAppDb.AddDateToDb(newTodo);
            List.Clear();
            List.AddRange(await TodoAppDb.GetList());
        }
        private async Task DeleteTODO(TodoList todo) {
            await TodoAppDb.Delete(todo.ID);
            List.Clear();
            List.AddRange(await TodoAppDb.GetList());
        }

        
        //Funkcja która wyświetla kafelki po filtrowaniu  (Każa filtrująca metoda z tej 'delegaty' korzysta)
        private async void FilterTheResult(Func<Task<ObservableRangeCollection<TodoList>>> func) {
            if (_istoggledFilter) {  // Jeżeli filtrowanie jest właczone to filtruj wyniki wraz z wyszukiwaniem
                var todoFilter = new ObservableRangeCollection<TodoList>();
                var fliteredTodoList = new ObservableRangeCollection<TodoList>();


                todoFilter = await func.Invoke();
                foreach (var t in todoFilter) {
                    if (List.Any(lisTodo => lisTodo.ID == t.ID))
                        fliteredTodoList.Add(t);
                }
                List.Clear();
                List.AddRange(fliteredTodoList);
                
            }
            else 
                List = await func.Invoke();
            OnPropertyChanged("List");
        }
        //Funkcja filtrująca tagi
        private void  TagsFilter(string text) {
            if (string.IsNullOrEmpty(text))
                return;

            TagFormater tagFormater = new TagFormater();
            var listOfUserTags =  tagFormater.ReturnCombidedListOfTagsWithPrexiex(text);
            tagFormater.Clear();
            FilterTheResult(async () =>
            {

                var filteredResult = new ObservableRangeCollection<TodoList>();
                
                foreach (var item in await TodoAppDb.GetList()) {
                    var tagsString = item.Tagi;
                    var tagsList = tagFormater.ReturnCombidedListOfTags(tagsString);
                    tagFormater.Clear();
                    foreach (var tag in listOfUserTags)
                        if (tagsList.Contains(tag)) {
                            filteredResult.Add(item);
                            break;
                        }
                }
                return filteredResult;
            });
        }

        //Funkja wyszukująca tytułu albo opisu
        private void TitleOrDescriptionFilter(string text) {
            if(string.IsNullOrEmpty(text))
                return;
            text = text.Trim();
            Regex rx = new Regex(@text, RegexOptions.IgnoreCase);
            FilterTheResult(async () =>
            {
                var filteredResult = new ObservableRangeCollection<TodoList>();
                var query = from item in await TodoAppDb.GetList()
                            where rx.IsMatch(item.Tytul) || rx.IsMatch(item.Opis)
                            select item;
                filteredResult.AddRange(query);
                return filteredResult;
            });
            
        }
        //Funkcja która resetuje UI
        private async Task Refresh() {
            IsBusy = true;
            var list = await TodoAppDb.GetList();
            var newList = new ObservableRangeCollection<TodoList>();
            newList.AddRange(list);
            this.List = newList;
            IsBusy = false;
            IsToggledFilter = false;
            OnPropertyChanged("List");
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("IsToggledFilter");

        }


    }
}
