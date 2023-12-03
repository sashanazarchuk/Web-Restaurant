using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces.ProductServices
{
    public interface IProductService<T>
    {
        Task<IEnumerable<T>> GetAllProduct();
        Task<T> GetById(int id);
    }
}
