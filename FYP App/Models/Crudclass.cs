using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_App.Models
{
    public class Crudclass
    {
        public int ID { get; set; }
        [RegularExpression(@"^[a-zA-Z].*[\s\.]*$", ErrorMessage = "Name is not in Format."), MaxLength(20), Required]

        public string Name { get; set; }
        public string Password { get; set; }
        public string Status{ get; set; }

        [Required(ErrorMessage = "Email is Requird"), MaxLength(30)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is Requird"), MaxLength(14)]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Phone is not in Format.")]
        public string Phone { get; set; }


        //public string Status { get; set; }
    }
}