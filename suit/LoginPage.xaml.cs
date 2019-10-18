using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace suit
{
    public partial class LoginPage : ContentPage
    {
        
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void SubmitLogin(object sender, EventArgs args)
        {
            App.Current.MainPage = new MainPage {
                Detail = new NavigationPage(new ListenPage())
            };          

        }
    }
}
