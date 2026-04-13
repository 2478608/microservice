using ProductService.Core.DTOs;
using ProductService.Core.Entities;
using ProductService.Core.Interfaces.Repositories;
using ProductService.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Infra.Services
{
    public  class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public ProductDto GetProductById(int id)
        {
            var Prod = _repo.GetById(id) ?? throw new Exception("Prod not found");

            return new ProductDto
            {
                Id = Prod.Id,
                Name = Prod.Name,
                Price = Prod.Price,
            };
        }
    }
}
