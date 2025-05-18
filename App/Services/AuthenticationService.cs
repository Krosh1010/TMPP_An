using System;
using System.Collections.Generic;
using System.Linq;
using App.Abstraction;
using Domain.Entities;

namespace App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private List<User> users;
        private User currentUser;

        public AuthenticationService(IManagerHrRepository managerRepo)
        {
            users = new List<User>();

            var persons = managerRepo.LoadAll();

            foreach (var person in persons)
            {
                users.Add(new User
                {
                    Username = person.Name,
                    Password = person.Role == "HR" ? "123" : "456", 
                    Role = person.Role,
                    Team = person.Team
                });
            }
        }

        public bool Login(string username, string password)
        {
            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.Ordinal) &&
                u.Password == password);
            if (user != null)
            {
                currentUser = user;
                return true;
            }
            return false;
        }

        public User GetCurrentUser()
        {
            return currentUser;
        }
    }
}