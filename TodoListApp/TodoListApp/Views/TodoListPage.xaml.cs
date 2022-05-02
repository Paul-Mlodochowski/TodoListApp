using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoListApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage() {
            InitializeComponent();


        }

        private void Switch_Toggled(object sender, ToggledEventArgs e) {
            var frame = (sender as Switch).Parent.Parent.Parent as Frame;
            _ = (bool)e.Value ? frame.BackgroundColor = Color.FromHex("#77D191") : frame.BackgroundColor = Color.Default;
        }

        
    }
}
