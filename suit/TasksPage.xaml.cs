﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
            listView.ItemTapped += OnAlertYesNoClicked;
        }
         private async void getMyTasks() {
            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            Location newLocation = location;
            Tasks myTask = new Tasks();

            //var task = new List<Task>();

            ObservableCollection<Task> task = new ObservableCollection<Task>();

            //List<Task> myTasks = new List<Task>();


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

        async void OnAlertYesNoClicked(object sender, ItemTappedEventArgs args)
        {
            if (loading.IsRunning)
            {
                return;       
            }

            loading.IsRunning = true;

            var selectedItem = args.Item as Task;            

            var location = await firebaseHelper.GetLocation(Convert.ToString(App.Current.Properties["locationID"]));
            List<Tasks> myTaskList = location.Tasks;
            List<Tasks> taskToChange;
            var myTask = listView.SelectedItem as Task;

            var answer = await DisplayActionSheet("¿ Ya realizó esta tarea ?", "Cancelar", null, "Si", "No");

            Console.WriteLine("answer for alert "+answer);

            if (answer == "Si")
            {
                taskToChange = changeTaskStatusYes(myTaskList, myTask.Title);
                await firebaseHelper.UpdateTaskStatus(Convert.ToString(App.Current.Properties["locationID"]), taskToChange);
                this.OnPropertyChanged("Content");
                selectedItem.Icon = "check";
                selectedItem.OnPropertyChanged();
                loading.IsRunning = false;
            }
            if (answer == "No")
            {
                taskToChange = changeTaskStatusNot(myTaskList, myTask.Title);
                await firebaseHelper.UpdateTaskStatus(Convert.ToString(App.Current.Properties["locationID"]), taskToChange);
                this.OnPropertyChanged("Content");
                selectedItem.Icon = "Uncheck";
                selectedItem.OnPropertyChanged();
                loading.IsRunning = false;
            }

            loading.IsRunning = false;

        }
        private List<Tasks> changeTaskStatusYes(List<Tasks> locationTasks, String taskName)
        {
            List<Tasks> myNewTasks = locationTasks;
            for (int count = 0; count < myNewTasks.Count; count++)
            {
                if (myNewTasks[count].TaskName == taskName)
                {
                    myNewTasks[count].TaskStatus = "done";
                    myNewTasks[count].userId = App.Current.Properties["userid"].ToString();
                    myNewTasks[count].hour = DateTime.Now.ToString();
                }
            }
            return myNewTasks;
        }

        private List<Tasks> changeTaskStatusNot(List<Tasks> locationTasks, String taskName)
        {
            List<Tasks> myNewTasks = locationTasks;
            for (int count = 0; count < myNewTasks.Count; count++)
            {
                if (myNewTasks[count].TaskName == taskName)
                {
                    myNewTasks[count].TaskStatus = "pending";
                    myNewTasks[count].userId = null;
                    myNewTasks[count].hour = null;
                }
            }
            return myNewTasks;
        }
    } 
}
