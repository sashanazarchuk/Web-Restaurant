using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "You must enter a Username")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "You must enter a Username")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "You must enter a phone number")]
        [MinLength(12)]
        [MaxLength(12)]
        public string Phone { get; set; }


        [Required(ErrorMessage = "You must enter an email address")]
        [EmailAddress(ErrorMessage = "Email format is incorrect")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password must be entered")]
        [MinLength(6, ErrorMessage = "The minimum password length is 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

}
