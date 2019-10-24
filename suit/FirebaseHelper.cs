using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace suit
{
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://nutresa-33531.firebaseio.com/");
        public async Task<List<User>> GetAllUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  Name = item.Object.Name,
                  UserId = item.Object.UserId,
                  Email = item.Object.Email,
                  Username = item.Object.Username,
                  Password = item.Object.Password,
              }).ToList();
        }
        public async Task AddUser(int userId, string name)
        {

            await firebase
              .Child("Users")
              .PostAsync(new User() { UserId = userId, Name = name });
        }
        public async Task<User> GetUser(int userId)
        {
            var allUsers = await GetAllUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(a => a.UserId == userId).FirstOrDefault();
        }
        public async Task<User> GetUserByName(String username)
        {
            var allUsers = await GetAllUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(a => a.Username == username).FirstOrDefault();
        }
        public async Task UpdateUser(int userId, string name)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new User() { UserId = userId, Name = name });
        }
       
        public async Task DeleteUser(int userId)
        {
            var toDeleteUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault();
            await firebase.Child("Users").Child(toDeleteUser.Key).DeleteAsync();

        }
        public async Task<List<Location>> GetAllLocations()
        {
            return (await firebase
              .Child("Locations")
              .OnceAsync<Location>()).Select(item => new Location
              {
                  Address = item.Object.Address,
                  ID = item.Object.ID,
                  Neighborhood = item.Object.Neighborhood,
                  PDV = item.Object.PDV,
                  Type = item.Object.Type,
                  Tasks = item.Object.Tasks
              }).ToList();
        }
        public async Task<Location> GetLocation(String locationId)
        {
            var allLocations = await GetAllLocations();
            await firebase
              .Child("Locations")
              .OnceAsync<Location>();

            return allLocations.Where(a => a.ID == locationId).FirstOrDefault();
        }
        public async Task UpdateTaskStatus(string locationId, List<Tasks> taskName)
        {
            var toUpdateTaskStatus = (await firebase
              .Child("Locations")
              .OnceAsync<Location>()).Where(a => a.Object.ID == locationId).FirstOrDefault();

            await firebase
              .Child("Locations")
              .Child(toUpdateTaskStatus.Key)
              .Child("Tasks")
              .PutAsync(new List<Tasks>(taskName));
        }
    }
}
