using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace suit
{
    public partial class LoginPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void SubmitLogin(object sender, EventArgs args)
        {

            if (loading.IsRunning)
            {
                return;
            }

            loading.IsRunning = true;

            String Password;
            String Username;
            if (string.IsNullOrEmpty(userEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un usuario", "Aceptar");
                userEntry.Focus();
                return;
            }
            if (string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar una clave", "Aceptar");
                userEntry.Focus();
                return;
            }
            try
            {
                //waitActivityIndicator.IsRunning = true;
                enterButton.IsEnabled = false;

                Password = Convert.ToString(passwordEntry.Text);
                Username = Convert.ToString(userEntry.Text);

                var user = await firebaseHelper.GetUserByName(Username);
                System.Diagnostics.Trace.WriteLine(user.Password);
                if (user != null && user.Password == Password)
                {
                    
                    App.Current.Properties["name"] = user.Name;
                    App.Current.Properties["email"] = user.Email;
                    App.Current.Properties["username"] = user.Username;
                    App.Current.Properties["userid"] = user.UserId;
                    App.Current.Properties["IsLoggedIn"] = true;
                    loading.IsRunning = false;
                    App.Current.MainPage = new MainPage
                    {
                        Detail = new NavigationPage(new ListenPage())
                    };

                }
                else
                {
                    await DisplayAlert("Alerta", "Usuario o contraseña incorrectas", "OK");
                    loading.IsRunning = false;
                    passwordEntry.Text = string.Empty;
                    passwordEntry.Focus();
                    enterButton.IsEnabled = true;
                    return;
                }
                //waitActivityIndicator.IsRunning = false;
                enterButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No hay conexión, intente más tarde", "Aceptar");
                enterButton.IsEnabled = true;
                //waitActivityIndicator.IsRunning = false;
                return;
            }

        }
    }
}
