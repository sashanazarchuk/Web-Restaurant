using Entities.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces.IAuthServices
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(User user);
    }
}
