using System;
using System.Collections.Generic;
using System.Linq;
using App.Abstraction;
using Domain.Entities;
using Infrastructure.Repositories;

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
                // Dacă este manager, verifică starea
                if (user.Role == "Manager")
                {
                    // Caută angajatul după nume
                    var managerRepo = new ManagerHrRepository();
                    var employee = managerRepo.LoadAll().FirstOrDefault(e => e.Name == user.Username && e.Role == "Manager");
                    if (employee != null && employee.StateName == "În concediu")
                    {
                        // Refuză autentificarea
                        return false;
                    }
                }

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