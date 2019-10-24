using System;
using System.Collections.Generic;
using suit.models;

using Xamarin.Forms;
using Xamarin.Forms;

namespace suit
{
    public partial class TasksPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public TasksPage()
        { 
            InitializeComponent();
            getMyTasks();
            listView.ItemSelected += OnAlertYesNoClicked;
        }
         private async void getMyTasks() {
            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            Location newLocation = location;
            Tasks myTask = new Tasks();
            var task = new List<Task>();
            List<Task> myTasks = new List<Task>();
            int LabelCount = 0;
            if (location != null)
            {
                for (int count = 0; count < newLocation.Tasks.Count; count++)
                {
                    if (newLocation.Tasks[count].TaskStatus == "pending")
                    {
                        LabelCount++;
                        task.Add(new Task
                        {
                            Count = LabelCount,
                            Title = newLocation.Tasks[count].TaskName,
                            Icon = "Uncheck"
                        });
                    }
                    else 
                    {
                        LabelCount++;
                        task.Add(new Task
                        {
                            Count = LabelCount,
                            Title = newLocation.Tasks[count].TaskName,
                            Icon = "check"
                        });
                    }
                }
                listView.ItemsSource = task;
            }
            else
            {
                await DisplayAlert("EXITO", "No hay tareas asignadas a la ubicación", "OK");
            }
        }
        async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            List<Tasks> myTaskList = location.Tasks;
            List<Tasks> taskToChange;
            var myTask = listView.SelectedItem as Task;
            bool answer = await DisplayAlert("Pregunta", "¿Esta tarea ha sido ya hecha?", "Si", "No");
            if (answer == true)
            {
                taskToChange = changeTaskStatus(myTaskList, myTask.Title);
                await firebaseHelper.UpdateTaskStatus(Convert.ToString(App.Current.Properties["locationID"]), taskToChange);
            }
        }
        private List<Tasks> changeTaskStatus(List<Tasks> locationTasks, String taskName)
        {
            List<Tasks> myNewTasks = locationTasks;
            for (int count = 0; count < myNewTasks.Count; count++)
            {
                if (myNewTasks[count].TaskName == taskName)
                    myNewTasks[count].TaskStatus = "done";
            }
            return myNewTasks;
        }
    } 
}
