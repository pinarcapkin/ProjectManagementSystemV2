using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementSystemV2.Repositories;

public interface IRepository<T> where T : class// "şu işlemleri yapıcak" "<T>  içine ne koyarsan o olur user ise user olur yani bir karkater temsil eder
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}