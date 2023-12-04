using Entities.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces.IAuthService
{
    public interface IRegisterService<T>
    {
        Task<T> Register(RegisterViewModel model);
    }
}
