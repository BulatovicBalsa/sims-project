using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class Profile
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        public Profile(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
