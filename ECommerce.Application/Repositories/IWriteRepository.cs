﻿using ECommerce.Domain.Entities.Common;

namespace ECommerce.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        bool Remove(T model);
        Task<bool> RemoveAsync(string id); // Asenkron bir işlem olduğu için Task ile işaretlendi.
        bool RemoveRange(List<T> datas);

        bool Update(T model);
        Task<int> SaveAsync(); // Yapılan işlemlerde SaveChanges'ı kullanabilmek için bu fonksiyonu kullanacağız.




    }
}
