using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace suit
{
    public partial class InformationPage : ContentPage
    {
        public InformationPage()
        {
            InitializeComponent();
        }

        async void GoToTasks(object sender, EventArgs args)
        {
            /*App.Current.MainPage = new MainPage
            {
                Detail = new NavigationPage(new TasksPage())
            };*/

            await Navigation.PushAsync(new TasksPage());

        }
    }
}
