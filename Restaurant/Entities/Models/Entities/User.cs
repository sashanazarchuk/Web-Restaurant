using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
