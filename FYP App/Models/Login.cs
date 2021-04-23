using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_App.Models
{
    public class Login
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string LoginErrorMessage { get; set; }
    }
   
}