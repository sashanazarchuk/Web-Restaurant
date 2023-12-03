﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.ProductsDto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductDto> Product { get; set; }
    }
}
