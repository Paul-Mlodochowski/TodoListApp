using System;
using TodoListApp.Services;
using TodoListApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoListApp
{
    public partial class App : Application
    {

        public App() {
            InitializeComponent();

            MainPage = new TodoListPage();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
