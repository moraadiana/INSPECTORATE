using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INSPECTORATETenders.Models
{
    public class ResetPassword
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}