using System;
using System.Collections.Generic;
using suit.models;
using Xamarin.Forms;

namespace suit
{
    public partial class TasksPage : ContentPage
    {
        public TasksPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var tasks = new List<Tasks>();

            tasks.Add(new Tasks {
                Count = 1,
                Title = "Actualizar información",
                Icon = "user"
            });

            tasks.Add(new Tasks
            {
                Count = 2,
                Title = "Revisar estantería",
                Icon = "clipboard"
            });

            tasks.Add(new Tasks
            {
                Count = 3,
                Title = "Buscar descuentos",
                Icon = "magnifier"
            });

            tasks.Add(new Tasks
            {
                Count = 4,
                Title = "Consultar pedidos",
                Icon = "blackboard"
            });


            listView.ItemsSource = tasks;
        }
    } 
}
