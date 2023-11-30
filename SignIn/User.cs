using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignIn
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is User user)
            {
                if (user.Id == Id)
                    return true;
                return false;
            }
            return false;
        }
    }
}
