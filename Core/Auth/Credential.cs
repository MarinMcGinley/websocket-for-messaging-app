using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Auth
{
    public class Credential
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Credential(string Email, string Password) {
            this.Email = Email;
            this.Password = Password;
        }
    }
}