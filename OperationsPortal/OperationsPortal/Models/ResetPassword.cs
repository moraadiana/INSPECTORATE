using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsPortal.Models
{
	public class ResetPassword
	{
        public string customerNo { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}