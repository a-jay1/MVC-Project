using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class UserEmail
    {
        string email = "";
        public UserEmail(string email)
        {
            this.email = email;
        }

        public string getEmail()
        {
            return this.email;
        }
    }
}