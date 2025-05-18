using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Services
{
    public class ProxyAuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationService realService;
        private bool isAuthenticated = false;

        public ProxyAuthenticationService(IManagerHrRepository managerRepov)
        {
            realService = new AuthenticationService(managerRepov);
        }

        public bool Login(string username, string password)
        {
            isAuthenticated = realService.Login(username, password);
            return isAuthenticated;
        }

        public User GetCurrentUser()
        {
            if (!isAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated!");
            }

            return realService.GetCurrentUser();
        }
    }
}