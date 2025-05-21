using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductMS.Domain.Entities;

namespace ProductMS.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid userId);
        Task<Product?> GetByCategoryAsync(string email);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product user);
        Task DeleteAsync(Guid userId);
        Task UpdateAsync(Product user);
    }
}
