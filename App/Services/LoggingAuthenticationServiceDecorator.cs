using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Services
{
    public class LoggingAuthenticationServiceDecorator : IAuthenticationService
    {
        private readonly IAuthenticationService _inner;

        public LoggingAuthenticationServiceDecorator(IAuthenticationService inner)
        {
            _inner = inner;
        }

        public bool Login(string username, string password)
        {
            Console.WriteLine($"[LOG] Attempt login for user: {username}");
            var result = _inner.Login(username, password);
            Console.WriteLine(result
                ? $"[LOG] Login SUCCESS for user: {username}"
                : $"[LOG] Login FAILED for user: {username}");
            return result;
        }

        public User GetCurrentUser()
        {
            return _inner.GetCurrentUser();
        }
    }
}
