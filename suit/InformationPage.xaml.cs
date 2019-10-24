using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace suit
{
    public partial class InformationPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        DateTime thisDay = DateTime.Today;
        
        public InformationPage()
        {
            InitializeComponent();

            labelAddress.Text = Convert.ToString(App.Current.Properties["LocationAddress"]);
            labelLocationName.Text = Convert.ToString(App.Current.Properties["LocationPDV"]);
            labelUserName.Text = Convert.ToString(App.Current.Properties["name"]);
            labelDate.Text = thisDay.ToString("g");
        }

        async void GoToTasks(object sender, EventArgs args)
        {
            /*App.Current.MainPage = new MainPage
            {
                Detail = new NavigationPage(new TasksPage())
            };*/
            GetLocationTasks();
            await Navigation.PushAsync(new TasksPage());

        }
        async void GetLocationTasks()
        {
            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            App.Current.Properties["LocationTasks"] = location.Tasks;
        }
    }
}
