using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // În realitate, ar trebui să fie hash-uită
        public string Role { get; set; }

        public string Team { get; set; } // Echipa din care face parte utilizatorul

    }
}
