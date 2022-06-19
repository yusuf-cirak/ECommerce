using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Application.Repositories
{
    public interface IReadRepository<T>:IRepository<T> where T:BaseEntity
    {
        /* Select işlemleri için repository */
        /* IQueryable kullanıyoruz çünkü IEnumerable veya List şeklinde dönersek veriler memory'ye atılır. IQueryable ise veri tabanına sorguyu atar. Yükü programa vermemiş oluruz. */

        /* Task sınıfı awaitable yani asenkron işlemler için bir sınıftır. */

        IQueryable<T> GetAll(bool tracking=true);
        IQueryable<T> GetWhere(Expression<Func<T,bool>> method,bool tracking=true);
        Task<T> GetSingleAsync(Expression<Func<T,bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(string id,bool tracking=true);
    }
}
